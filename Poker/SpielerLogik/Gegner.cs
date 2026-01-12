using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.SpielerLogik
{
    public class Gegner
    {
        // Die HandcardValue Eigenschaft muss in dieser Klasse definiert sein (Wert von 1-100)
        private int HandcardValue { get; set; }
        private int LastBetAmount { get; set; } // Der Betrag, der für den Raise/Bet berechnet wurde

        // Rückgabewerte: 1 = Fold, 2 = Call (Mitgehen), 3 = Raise/Bet (Erhöhen/Setzen), 4 = Check (Schieben)

        /// <summary>
        /// Die Methode bestimmt welcher Zug gemacht werden muss (Fold, mitgehen, erhöhen, schieben) und gibt bei mitgehen/erhöhen ebenso zurück wieviel zum aktuellen einsatz ergänzt werden soll
        /// </summary>
        /// <param name="hand">Handkarten des Spielers</param>
        /// <param name="table">Gemeinschaftskarten aller Spieler</param>
        /// <param name="currentPotSize">Geld was aktuell im Pot liegt</param>
        /// <param name="playerStack">Noch verfügbares Geld des Spielers</param>
        /// <param name="currentBet">Betrag, den man schon gesetzt hat</param>
        /// <param name="currentBetToCall">Betrag den man drauf zahlen muss zum mitgehen</param>
        /// <param name="minRaiseAmount">Betrag um den man beim erhöhen mindestens erhöhen muss</param>
        /// <returns>1 für Fold, 2 für mitgehen, 3 für Rais/Bet und 4 für schieben</returns>
        public (int move, int money) DecideNextMove(
            Karte[] hand,
            List<Karte> table,
            int currentPotSize,
            int playerStack, 
            int currentBet,
            int currentBetToCall,
            int minRaiseAmount
        )
        {
            //Instanz von Handcalculator instanzieren und HCV merken
            HandCalculator hc = new HandCalculator(hand, table);
            HandcardValue = hc.HandcardValue;

            //Wenn noch nichts gewettet wurde und der HCV unter 70 liegt kannst du schieben
            if (currentBetToCall == 0 && HandcardValue <= 70)
            {
                return (4, 0);
            }
                        

            //Berechnet die minimale Gewinnchance der Hand, damit sich der Call im Verhältnis zum Pot lohn
            double potOddsCost = (currentPotSize > 0) ? (double)currentBetToCall / (currentPotSize + currentBetToCall) : 1.0;

            //Bei einer schwachen Hand und wenn wir zahlen müssten oder bei einer
            //schwachen Hand und einer verhältnismäßig hohen Summe zum setzen --> folden
            if (HandcardValue <= 20 && currentBetToCall > 0)
            {
                return (1,0);
            }
            else if (HandcardValue <= 40 && (potOddsCost >= 0.33 || currentBetToCall > playerStack * 0.15))
            {
                return (1, 0);
            }
        
            //Wenn er eine starke HCV hat oder bluffen sollte, dann wette/erhöhe
            if (HandcardValue > 70 || hc.ShouldBluff)
            {
                //Berechne wieviel er nun setzen soll
                int raiseAmount = GetRaise(currentPotSize, currentBetToCall, minRaiseAmount, playerStack, currentBet);

                //Wenn wir mehr errechent haben als wir eigentlich setzenb müssen, dann erhöhe
                if (raiseAmount > currentBetToCall)
                {
                    return (3, (raiseAmount - currentBet)); 
                }
            }

            //Wenn wir nicht folden/erhöhen und was setzen müssen, dann berechne hier wieviel
            if (currentBetToCall > 0)
            {
                //Wenn der Betrag zum mitgehen nicht hoch ist oder wie eine okay starke hand haben, dann geh mit
                if (HandcardValue >= 40 || currentBetToCall <= currentPotSize * 0.10)
                {
                    // Sicherstellen, dass wir Callen können (nicht All-In gehen, wenn wir es nicht wollen)
                    if (currentBetToCall < playerStack)
                    {
                        return (2, currentBetToCall); // Call
                    }
                    else if (currentBetToCall >= playerStack)
                    {
                        return (2, playerStack); 
                    }
                }
            }

            return (1, 0);  //Wenn alle Bedingungen fehlschalgen, dann folde einfach
        }

        /// <summary>
        /// Methode berechnet beim erhöhen, auf wieviel man erhöhen sollte.
        /// Gibt enweder den MindestBetrag zum erhöhen zurück, oder das strategisch klügste
        /// </summary>
        /// <param name="pot">Aktuelles Geld im Pot</param>
        /// <param name="playerStack">Aktuelles insgesamtes Geld des Spielers</param>
        /// <param name="currentBet">Aktuell schon gesetztes Geld</param>
        /// <param name="currentBetToCall">Geld um das erhöht werden muss zum mitgehen</param>
        /// <param name="minRaiseAmount">Mindestbetrag um den erhöht werden muss</param>
        /// <returns>Betrag auf den erhöht werden soll</returns>
        public int GetRaise(int pot, int currentBetToCall, int minRaiseAmount, int playerStack, int currentBet)
        {
            //Berechne den Mindestbetrag auf den erhöht werden muss
            int minimumTotalRaise = currentBet + currentBetToCall + minRaiseAmount;

            //Der effektive Pot erhält das Geld was grade im Pot ist und das um was wir erhöhen wollen
            int effectivePot = pot + currentBet + currentBetToCall;

            //Berechne an der Handstärke und der Potgröße auf was man am besten erhöhen sollte
            double desiredTotalRaise;
            if (HandcardValue > 90)
            {
                desiredTotalRaise = effectivePot * 1.5;
            }
            else if (HandcardValue > 70)
            {
                // Starke Hand: Setze eine große Value-Bet (z.B. 80% des Pots)
                desiredTotalRaise = effectivePot * 0.8;
            }
            else // (Für Bluffs oder niedrigere Value-Bets)
            {
                // Standard-Einsatzgröße (z.B. 60% des Pots)
                desiredTotalRaise = effectivePot * 0.6;
            }

            //Runde... Setze das was strategisch besser wäre, es sei denn es ist niedriger als das was du mindestens setzen musst
            int finalRaise = (int)Math.Round(Math.Max(desiredTotalRaise, (double)minimumTotalRaise));

            //Wenn es schlauer wäre mehr zu setzen als man hat, gehe All-in
            return Math.Min(finalRaise, playerStack);
        }
    }
}
