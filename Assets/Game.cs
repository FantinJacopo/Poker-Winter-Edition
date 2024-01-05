using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms;

namespace Assets
{
    public class Game
    {
        public List<Player> Players { get; set; }
        public List<Card> Cards { get; set; }
        public List<Card> commonCards { get; set; }
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
        public string CalculateHandStrength(Player player)
        {
            // Combine player's own cards and common cards
            var hand = new List<Card>(player.cards);
            hand.AddRange(commonCards);

            // Sort the hand in descending order by value
            hand.Sort((a, b) => b.value.CompareTo(a.value));

            // Group cards by value and count the number of cards in each group
            var groupCounts = hand.GroupBy(c => c.value).Select(g => g.Count()).ToList();

            // Check for Royal Flush
            if (groupCounts.Contains(1) && groupCounts.Contains(5))
            {
                var royalGroup = hand.Where(c => c.value >= 10).GroupBy(c => c.suit).FirstOrDefault(g => g.Count() == 5);
                if (royalGroup != null)
                {
                    return "Hand: Royal Flush";
                }
            }

            //Check for Straight Flush
            if (groupCounts.Contains(1) && groupCounts.Contains(5))
            {
                var straightGroup = hand.Where(c => c.value >= 10).GroupBy(c => c.suit).FirstOrDefault(g => g.Count() == 5);
                if (straightGroup != null)
                {
                    return "Hand: Straight Flush";
                }
            }
            //check for Four of a Kind
            if (groupCounts.Contains(4))
            {
                return "Hand: Four of a Kind";
            }
            //check for Full House
            if (groupCounts.Contains(3) && groupCounts.Contains(2))
            {
                return "Hand: Full House";
            }
            //check for Flush
            if (groupCounts.Contains(5))
            {
                return "Hand: Flush";
            }
            //check for Straight
            if (groupCounts.Contains(1))
            {
                return "Hand: Straight";
            }
            //check for Three of a Kind
            if (groupCounts.Contains(3))
            {
                return "Hand: Three of a Kind";
            }
            //check for Two Pair
            if (groupCounts.Contains(2) && groupCounts.Contains(2))
            {
                return "Hand: Two Pair";
            }

            // If no match found, return high card
            return "Hand: High Card";
        }

    }
}
