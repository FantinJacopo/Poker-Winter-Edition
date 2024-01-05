using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class Card
    {
        public string suit { get; set; }
        public int value { get; set; }
        public string name { get; set; }
        public Card(string suit, int value, string name)
        {
            this.suit = suit;
            this.value = value;
            this.name = name;
        }
        public Card(GameObject card)
        {
            suit = card.name.Split('_')[1];
            value = GetCardValue(card.name.Split('_')[0]);
            name = card.name;

        }
        private int GetCardValue(string val)
        {
            switch (val)
            {
                case "J":
                    return 11;
                case "Q":
                    return 12;
                case "K":
                    return 13;
                case "A":
                    return 14;
                default:
                    return int.Parse(val);
            }
        }
    }
}
