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
    }
}
