using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class Hand
    {
        public List<Card> hand { get; set; }
        int Cuori, Quadri, Fiori, Picche;
        public Hand(List<Card> hand) { this.hand = hand; }
        public int getNumberOfSuits()
        {
            foreach (Card card in hand)
            {
                switch (card.suit)
                {
                    case "Cuori":
                        Cuori++;
                        break;
                    case "Quadri":
                        Quadri++;
                        break;
                    case "Fiori":
                        Fiori++;
                        break;
                    case "Picche":
                        Picche++;
                        break;
                }
            }
            return Math.Max(Cuori, Math.Max(Fiori, Math.Max(Quadri, Picche)));
        }
    }
}
