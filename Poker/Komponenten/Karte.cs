using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poker.SpielerLogik;

namespace Poker
{
    public class Karte
    {
        //Lokale Variablen für den Namen, den Wert, und der pfad zum Bild der Karte
        public string Name { get; private set; }
        public int Wert { get; private set; }
        public Uri Path { get; private set; }

        /// <summary>
        /// Konstruktor der Klasse Karte.
        /// Speichert sich Werte lokal und bildet den Pfad zum Bild
        /// </summary>
        /// <param name="_name">Name der Karte</param>
        /// <param name="_wert">Wert der Karte</param>
        public Karte(string _name, int _wert) 
        {
            //Speichere dir Name und Wert der Karte lokal
            Name = _name;
            Wert = _wert;

            //Bilde dir String für Pfad und speicher ihn auch
            Path = new Uri($"img/cards/{_name}.png", UriKind.Relative);
        }
    }
}
