using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Poker
{
    public class Tisch
    {
        MainWindow mw;              //Instanz des MW um auf die UI-Elemte zuzugreifen
        List<Spieler> spieler;      //Instanz aller Spieler am Tisch
        List<Karte> tablecards;     //Instanz der Tischkarten

        int Dealerindex = 0;    //Index des Spieler der grade Dealer ist (anfängt)
        int potmoney = 0;            //Geld in der Mitte
        int rundenstatus = 0;   //Status in welcher Phase die Runde ist (1.-4. Setzrunde )
        
        Kartendeck kartendeck;                      //Kartendeck mit dem gespielt wird
        List<(Image k1, Image k2)> img_handkarten;  //Liste mit Bilder, der Handkarten der Spieler
        List<Image> img_tablecards;                 //Liste mit Bilder, der Tischkarten
        List<Button> btn_actionbuttons;             //Liste der Actionsbuttons des Benutzers
        List<Label> lab_einsatz;                    //Liste mit den Label für die Einsätze der Bots
        Label labPot;                               //Label für den Pot

        /// <summary>
        /// Konstruktor der Klasse Tisch.
        /// Initialisiert das Kartendeck, sämtliche Listen mit Bildern/Buttons/Labels und startet die erste Runde
        /// </summary>
        public Tisch(MainWindow mw)
        {
            this.mw = mw;                   //Merke die die Instanz des MW
            kartendeck = new Kartendeck();  //Initialisiere Kartendeck

            InitPlayersAndHandcards();      //Initialisiere Spieler direkt mit Handkarten

            //Initialisiere Liste und Variablen, welche den Elemten aus der UI entsprechen
            img_handkarten = new List<(Image k1, Image k2)>()
            {
                (mw.player1_1, mw.player1_2),
                (mw.gegner1_1, mw.gegner1_2),
                (mw.gegner2_1, mw.gegner2_2),
                (mw.gegner3_1, mw.gegner3_2),
                (mw.gegner4_1, mw.gegner4_2),
            };
            img_tablecards = new List<Image>()
            {
                mw.tablecard1,
                mw.tablecard2,
                mw.tablecard3,
                mw.tablecard4,
                mw.tablecard5,
            };
            btn_actionbuttons = new List<Button>()
            {
                mw.btnFold,
                mw.btnCall,
                mw.btnRaise,
                mw.btnCheck
            };
            lab_einsatz = new List<Label>()
            {
                mw.labEinsatz1,
                mw.labEinsatz2,
                mw.labEinsatz3,
                mw.labEinsatz4
            };
            //labPot = mw.labPot;

            NewRound();
        }
        /// <summary>
        /// Methode wird aufgerufen um Instanzen für Spieler und Gegner 
        /// zu initialisieren oder zu überschreiben (Runde beginnt von vorne)
        /// </summary>
        private void InitPlayersAndHandcards()
        {
            //Spieler initialisieren
            spieler = new List<Spieler>()
            {
                new Spieler("Paul"),
                new Spieler("Gudrun"),
                new Spieler("Karl"),
                new Spieler("Johanna"),
                new Spieler("Günter"),
            };
        }

        /// <summary>
        /// MEthode wird aufgerufen wenn die neue Runde startet
        /// </summary>
        private void NewRound()
        {
            //Setze Pot zurück und den RUndenstatus
            potmoney = 0;
            rundenstatus = 0;
            
            //Mische alle Karten neu durch  
            kartendeck.ResetDeck();

            //Gib jedem die 1. Handkarte
            foreach (Spieler g in spieler)
            {
                g.NewHandcards(kartendeck, 1);
            }

            //Gib jedem die 2. Handkarte
            foreach (Spieler g in spieler)
            {
                g.NewHandcards(kartendeck, 2);
            }

            //Teile die Tischkarten aus, alle verdeckt
            GetTablecards();

            //Jeder hat Karten, kann anfangen
            StartGame();
            ShowCards();
        }

        /// <summary>
        /// Methode beinhaltet den GameLoop für das Poker
        /// </summary>
        private void StartGame()
        { 
        }

        /// <summary>
        /// Methode teile die Gemeinschaftskarten auf dem Tisch aus
        /// </summary>
        private void GetTablecards()
        {
            Karte burning = kartendeck.GetNextCard();   //1. Karte verbrennen

            tablecards = new List<Karte>(){
                kartendeck.GetNextCard(),
                kartendeck.GetNextCard(),
            };

            burning = kartendeck.GetNextCard();         //2. Karte verbrennen#

            tablecards.Add(kartendeck.GetNextCard());   //4. Gemeinschaftskarte

            burning = kartendeck.GetNextCard();         //3. Karte verbrennen#

            tablecards.Add(kartendeck.GetNextCard());   //5. Gemeinschaftskarte
        }

        /// <summary>
        /// Methode wird aufgerufen um die Handkarten zu zeigen.
        /// Für den Spieler werden sie offen angezeigt, für den PC verdeckt
        /// </summary>
        private void ShowCards()
        {
            bool isPlayer = true;

            foreach ((Image card1, Image card2) in img_handkarten)
            {
                if (isPlayer)
                {
                    //Zeige die Karten vom Spieler offen an
                    card1.Source = new BitmapImage(new Uri(spieler[0].Hand[0].Path.ToString(), UriKind.RelativeOrAbsolute));
                    card2.Source = new BitmapImage(new Uri(spieler[0].Hand[1].Path.ToString(), UriKind.RelativeOrAbsolute));
                    
                    
                    isPlayer = false;
                    continue;
                }

                card1.Source = new BitmapImage(new Uri("img/cards/placeholder.png", UriKind.RelativeOrAbsolute));
                card2.Source = new BitmapImage(new Uri("img/cards/placeholder.png", UriKind.RelativeOrAbsolute));
            }

            //Je nach Phase zeige auch Gemeinschaftskarten
            switch (rundenstatus)
            {
                //Liegen verdeckt
                case 0:
                    ShowGemeinschaftscard(new List<int>());
                    break;
                //Ersten 3
                case 1:
                    ShowGemeinschaftscard(new List<int>() { 0, 1, 2 });
                    break;
                //noch die 4.
                case 2:
                    ShowGemeinschaftscard(new List<int>() { 0, 1, 2, 3 });
                    break;
                //und die 5.
                case 3:
                    ShowGemeinschaftscard(new List<int>() { 0, 1, 2, 3, 4 });
                    break;
            }
        }

        /// <summary>
        /// Methode stellt die aufgedeckten Gemeinschaftskarten dar
        /// </summary>
        /// <param name="indexAufgedeckt">Index aller Karten die mit Bild angezeigt werden sollen</param>
        private void ShowGemeinschaftscard(List<int> indexAufgedeckt)
        {
            int counter = 0;    //Zähler  bei welchen index er aktuell ist

            //Gehe jede Gemeinschaftskarte durch
            foreach (Image img in img_tablecards)
            {
                //Wenn die Karte sichbar werden soll, dann mach printe die Karte rein
                if (indexAufgedeckt.Contains(counter))
                {
                    img.Source = new BitmapImage(tablecards[counter].Path);
                   
                }
                //Ansonsten printe den Platzhalter rein
                else
                {
                    img.Source = new BitmapImage(new Uri("img/cards/placeholder.png", UriKind.RelativeOrAbsolute));
                }

                counter++;
            }
            
        }
    }
}
