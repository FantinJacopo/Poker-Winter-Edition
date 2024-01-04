using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class Game
    {
        public List<Player> Players { get; set; }
        public List<Card> Cards { get; set; }
        public Game() { Players = new(); Cards = new(); }
        public Game(List<Player> players) { Players = players; Cards = new(); }
        public void StartGame()
        {
            //start a poker game

            //start a round
            StartRound();
        }
        public void StartRound()
        {
            //start a round

        }
    }
}
