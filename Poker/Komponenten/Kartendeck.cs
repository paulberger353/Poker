using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class Kartendeck
    {
        private List<Karte> FullDeck;       //Deck aller Karten
        private List<Karte> CurrentDeck;    //Deck aller übrigen Karten im Deck, gemischt

        /// <summary>
        /// Konstruktor der Klasse Kartendeck
        /// Initialisiert gemischtes und ungemischtes Kartendeck
        /// </summary>
        public Kartendeck()
        {
            FullDeck = new List<Karte>()
            {
                new Karte("HerzZwei", 2),
                new Karte("HerzDrei", 3),
                new Karte("HerzVier", 4),
                new Karte("HerzFuenf", 5),
                new Karte("HerzSechs", 6),
                new Karte("HerzSieben", 7),
                new Karte("HerzAcht", 8),
                new Karte("HerzNeun", 9),
                new Karte("HerzZehn", 10),
                new Karte("HerzBube", 11),
                new Karte("HerzDame", 12),
                new Karte("HerzKoenig", 13),
                new Karte("HerzAss", 14),

                new Karte("KaroZwei", 2),
                new Karte("KaroDrei", 3),
                new Karte("KaroVier", 4),
                new Karte("KaroFuenf", 5),
                new Karte("KaroSechs", 6),
                new Karte("KaroSieben", 7),
                new Karte("KaroAcht", 8),
                new Karte("KaroNeun", 9),
                new Karte("KaroZehn", 10),
                new Karte("KaroBube", 11),
                new Karte("KaroDame", 12),
                new Karte("KaroKoenig", 13),
                new Karte("KaroAss", 14),

                new Karte("PiekZwei", 2),
                new Karte("PiekDrei", 3),
                new Karte("PiekVier", 4),
                new Karte("PiekFuenf", 5),
                new Karte("PiekSechs", 6),
                new Karte("PiekSieben", 7),
                new Karte("PiekAcht", 8),
                new Karte("PiekNeun", 9),
                new Karte("PiekZehn", 10),
                new Karte("PiekBube", 11),
                new Karte("PiekDame", 12),
                new Karte("PiekKoenig", 13),
                new Karte("PiekAss", 14),

                new Karte("KreuzZwei", 2),
                new Karte("KreuzDrei", 3),
                new Karte("KreuzVier", 4),
                new Karte("KreuzFuenf", 5),
                new Karte("KreuzSechs", 6),
                new Karte("KreuzSieben", 7),
                new Karte("KreuzAcht", 8),
                new Karte("KreuzNeun", 9),
                new Karte("KreuzZehn", 10),
                new Karte("KreuzBube", 11),
                new Karte("KreuzDame", 12),
                new Karte("KreuzKoenig", 13),
                new Karte("KreuzAss", 14),
            };
            
            //Initialisiere vollständiges Deck
            ResetDeck();                        //Mische deck durch
        }

        /// <summary>
        /// Methode wird aufgerufen um die nächste Karte vom Stapel zu holen
        /// </summary>
        /// <returns>Nächste Karte des Stapels</returns>
        public Karte GetNextCard()
        {
            //Hol dir die oberste Karte im Deck
            Karte next = CurrentDeck[CurrentDeck.Count - 1];

            //Nehme die Karte aus dem Deck und händige sie dem Spieler aus
            CurrentDeck.Remove(next);

            return next;
        }

        /// <summary>
        /// Methode wird nach beenden einer Runde aufgerufen. Mischt alle Karten durch, 
        /// sodass in der nächsten Runde wieder ein vollständiges, gemischtes Deck benutzt wird
        /// </summary>
        public void ResetDeck()
        {
            //Hol dir alle Karten und mische sie. Danach bist du bereit für die nächste Runde
            CurrentDeck = ShuffleDeck(FullDeck); 
        }

        /// <summary>
        /// Methode wird aufgerufen um ein ungemischtes Kartendeck zu mischen
        /// </summary>
        /// <param name="unshuffled">Ungemischtes Kartendeck</param>
        /// <returns>Gemischtes Kartendeck</returns>
        private List<Karte> ShuffleDeck(List<Karte> unshuffled) 
        {
            Random rng = new Random();
            int n = unshuffled.Count;

            //Gehe rückwärts von der letzten Karte zur 2.
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);        //Wähle eine unberührte Karte aus

                //Verschieb die zufällige Karte an die aktuell letzte Position (berührter Teil)
                Karte temp = unshuffled[k];
                unshuffled[k] = unshuffled[n];
                unshuffled[n] = temp;
            }

            return unshuffled;  //Gib gemischtes Kartendeck zurück
        }
    }
}
