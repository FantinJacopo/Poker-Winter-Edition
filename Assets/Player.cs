using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class Player
    {
        public int id { get; set; }
        public List<Card> cards { get; set; }
        public List<CardPosition> positions { get; set; }
        public Player(int id) { this.id = id; cards = new List<Card>(); positions = new List<CardPosition>(); }
    }
}
