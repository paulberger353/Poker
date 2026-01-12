using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.SpielerLogik
{
    public class HandCalculator
    {
        public int HandcardValue;
        public bool ShouldBluff;

        /// <summary>
        /// Konstruktor der Klasse HandCalculator
        /// setzt die Werte lokal und berechnet HandkarteValue
        /// </summary>
        /// <param name="_hand">Handkarten</param>
        /// <param name="_table">Tischkarten</param>
        public HandCalculator(Karte[] _hand, List<Karte> _table)
        {
            //Wenn übergebene Werte falsch sind, sind Handkarten nichts wert und du kannst Klasse verlassen
            if (_hand.Length != 2 || _hand == null)
            {
                HandcardValue = 0;
                return;
            }

            //Stelle eine Liste mit allen Karten zusammen
            List<Karte> allCards = new List<Karte>(_hand);
            if (_table != null)
            {
                allCards.AddRange(_table);
            }

            //Berechne die HCV, je nach dem wieviele Karten du hast
            if (allCards.Count == 2)
            {
                HandcardValue = EvaluatePreflop(allCards[0], allCards[1]);
            }
            else
            {
                HandcardValue = EvaluatePostflop(allCards);
            }

            ShouldBluff = ShouldPlayerBluff(allCards);      //Überprüfe ob er Bluffen sollte
        }

        /// <summary>
        /// Methode wird aufgerufen um den HandcardValue zu berechnen, wenn noch keine Gemeinschaftskarten liegen
        /// </summary>
        /// <param name="k1">1. Handkarte</param>
        /// <param name="k2">2. Handkarte</param>
        /// <returns>Berechnetes HandcardValue</returns>
        private int EvaluatePreflop(Karte k1, Karte k2)
        {
            //Merke dir die Werte und überprüfe ob es ein Paar ist/ die selbe Farbe hat
            int val1 = k1.Wert;
            int val2 = k2.Wert;

            bool isSuited = GetFarbe(k1.Name) == GetFarbe(k2.Name);
            bool isPair = val1 == val2;
           
            if (isPair)
            {
                //Wenn es ein Paar ist, ist es schon recht hoch. Gib das so zurück
                return Math.Min(100, 58 + (val1 * 3));
            }

            //Bestimme die höchste/niedrigste Zahl und einen Basiswert der Hand
            int high = Math.Max(val1, val2);
            int low = Math.Min(val1, val2);
            int score = (int)(high * 2.5 + low);

            //10 Punkte mehr, wenn es die selbe farbe ist
            if (isSuited)
            {
                score += 10;
            }

            //Berechne den Gap (evtl. Straight)
            int gap = high - low;
            if (gap == 1)
            {
                score += 6;
            }
            else if (gap == 2)
            {
                score += 3;
            }

            return Math.Min(85, score);
        }

        /// <summary>
        /// Methode wird aufgerufen um den HandcardValue zu berechnen, wenn schon Gemeinschaftskarten liegen
        /// </summary>
        /// <param name="cards">Liste aller Karten (3-8 Stück)</param>
        /// <returns>Berechnetes HandcardValue</returns>
        private int EvaluatePostflop(List<Karte> cards)
        {
            //Stelle eine Liste nach Wert sortierte Liste der Karten zusammen, welche Wert und Farbe enthält
            var analyzedCards = cards.Select(c => new
            {
                Wert = c.Wert,
                Farbe = GetFarbe(c.Name)
            }).ToList();
            var sorted = analyzedCards.OrderByDescending(x => x.Wert).ToList();

            //Erstelle eine Liste gruppiert nach Wert und eine gruppert nach Farbe (Prüfe ob Flush)
            var valueGroups = sorted.GroupBy(x => x.Wert).OrderByDescending(g => g.Count()).ThenByDescending(g => g.Key).ToList();
            var flushGroup = analyzedCards.GroupBy(x => x.Farbe).FirstOrDefault(g => g.Count() >= 5);
            bool isFlush = flushGroup != null;

            //Stell eine Liste nur mit Werten zusammen, und überprüfe ob es eine Straße ist
            List<int> cardValues = sorted.Select(x => x.Wert).ToList();
            bool isStraight = CheckForStraight(cardValues);

            //Gehe Mögliche Hände durch und gib ihm darauf basierend Punkte
            if (isFlush && isStraight)
            {
                //Bei nen Straight/Royal Flush gibts 100 Punkte
                return 100;
            }
            else if (valueGroups.First().Count() == 4)
            {
                //Bei einem Vierling gib 90 + (Wert einer Karte des Vierlings / 2 ) zurüc
                return 90 + (int)(valueGroups.First().Key / 2.0);
            }           
            else if (valueGroups.Count >= 2 && valueGroups[0].Count() == 3 && valueGroups[1].Count() >= 2)
            {
                //Bei einem Fullhouse gib 85 zurück
                return 85;
            }
            else if (isFlush)
            {
                //Bei nem Flush gib 75 zurück
                return 75;
            }
            else if (isStraight)
            {
                //Bei ner Straße gib 65 zurück
                return 65;
            }
            else if (valueGroups.First().Count() == 3)
            {
                //Bei nem Drilling gib 50 + Wert einer Karte des Drillings zurück
                return 50 + valueGroups.First().Key;
            }
            else if (valueGroups.Count >= 2 && valueGroups[0].Count() == 2 && valueGroups[1].Count() == 2)
            {
                //Bei nem Zweipaaren gib 40 + Wert einer Karte des höchsten paares zurück
                return 40 + valueGroups[0].Key;
            }
            else if (valueGroups.First().Count() == 2)
            {
                //Bei nem Paar gib 20 + Wert einer Karte des Paares zurück
                return 20 + valueGroups.First().Key;
            }
            else
            {
                //Wenn du keine Kombi hast (nur Highkard) gib Wert der höchsten Karte zurück
                return sorted[0].Wert;
            }
        }

        /// <summary>
        /// Methode überprüft ob der Benutzer eine Straße legen kann.
        /// Sie unterscheidet aber nicht zwischen Royal-, Straight- oder Normalflush
        /// </summary>
        /// <param name="values">Werte der Tisch und Handkarten</param>
        /// <returns>true wenn Straße, false wenn nein</returns>
        private bool CheckForStraight(List<int> values)
        {
            //Entferne doppelte Values, sortiere und überprüfe ob immernoch 5 Karten vorhanden sind
            var distinct = values.Distinct().OrderByDescending(x => x).ToList();
            if (distinct.Count < 5)
            {
                //Falls nein, ist keine Straße mehr möglich
                return false;
            }

            //Überprüfe ob es 5 aufeinander folgenden Karten sind (Flush)
            int chain = 0;
            for (int i = 0; i < distinct.Count - 1; i++)
            {
                if (distinct[i] - distinct[i + 1] == 1)
                {
                    chain++;
                    if (chain >= 4) 
                    {
                        return true;
                    }
                }
                else
                {
                    chain = 0;
                }
            }

            //Überprüfe ob es ein Flush ist, der mit Ass anfängt
            if (distinct.Contains(14) && distinct.Contains(2) && distinct.Contains(3) &&
                distinct.Contains(4) && distinct.Contains(5))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Methode bestimmt anhand des Kartennamens die Farbe
        /// </summary>
        /// <param name="kartenName">Kartenname</param>
        /// <returns>Farbe der Karte</returns>
        private string GetFarbe(string kartenName)
        {
            if (string.IsNullOrEmpty(kartenName)) return "Unknown";
            if (kartenName.StartsWith("Herz")) return "Herz";
            if (kartenName.StartsWith("Karo")) return "Karo";
            if (kartenName.StartsWith("Pik")) return "Pik";
            if (kartenName.StartsWith("Kreuz")) return "Kreuz";
            return "Unknown";
        }

        /// <summary>
        /// Methode überprüft ob man bluffen sollte
        /// </summary>
        /// <param name="allCards">Alle Karten (Hand und ´Tisch)</param>
        /// <returns>true wenn ja, false wenn nein</returns>
        private bool ShouldPlayerBluff(List<Karte> allCards)
        {
            //Bluffen ist sinnvoll wenn Gemeinschaftskarten ausgeteilt sind oder er maximal HighCard hat
            if (allCards.Count <= 2 || HandcardValue >= 25)
            {
                return false;
            }

            //Gruppiere die Karten nach Farbe, und schau ob mindestens 4 einer Farbe vorhanden sind
            var colorGroups = allCards.GroupBy(x => GetFarbe(x.Name)).ToList();
            bool hasFourFlushDraw = colorGroups.Any(g => g.Count() == 4);

            //Überprüfe ob irgendwoe (Hand oder Tisch) ein König oder Ass liegt
            var sortedDistinctValues = allCards.Select(x => x.Wert).Distinct().OrderByDescending(x => x).ToList();
            bool hasTableAss = sortedDistinctValues.Any() && sortedDistinctValues[0] >= 13;

            //Bluffe nur wenn man fast einen Flush hat oder ein Ass auf dem Tisch liegt
            if (hasFourFlushDraw || hasTableAss)
            {
                //Bluffe in 30% der Fälle
                Random rnd = new Random();
                return rnd.Next(100) < 30;
            }

            return false;
        }
    }
}
