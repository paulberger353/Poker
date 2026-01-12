using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poker.SpielerLogik;

namespace Poker
{
    public class Spieler : Gegner
    {
        private string name;
        public int guthaben;
        public int einsatz;

        public bool hasFold;
        public int role;

        public Karte[] Hand { get; private set; } = new Karte[2];       //Handkarten des Spielers/Gegners

        /// <summary>
        /// Konstruktor der Klasse SPielerGegner
        ///Merkt sich Namen lokal
        /// </summary>
        /// <param name="_name">Name des Spielers</param>
        public Spieler(string _name)
        {
            //SPeichere Namen, setze Startguthaben und teil startkarten aus
            name = _name;
            guthaben = 1000;
        }

        /// <summary>
        /// Methode wird aufgerufen um dem Spieler/Gegner 2 neue Handkarten zu geben
        /// </summary>
        /// <param name="k">Instanz des Kartendecks</param>
        /// <param name="c">Parameter, ob 1. oder 2. Handkarte</param>
        public void NewHandcards(Kartendeck k, int c)
        {
            if (c == 1)
            {
                Hand[0] = k.GetNextCard();
            }
            else
            {
                Hand[1] = k.GetNextCard();
            }
        }
    }
}
