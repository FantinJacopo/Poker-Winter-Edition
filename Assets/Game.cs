﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms;
using UnityEngine.XR;

namespace Assets
{
    public class Game
    {
        public List<Player> Players { get; set; }
        public List<Card> Cards { get; set; }
        public List<Card> commonCards { get; set; }
        public int actualBet { get; set; }
        public int pot { get; set; }
        public int playerTurn { get; set; }
        public int maxRaise = 2;
        public int raiseCounter;
        public int raiserIndex;
        public List<bool> playersInGame;
        public bool allIn { get; set; }
        public Game() { 
            Players = new(); 
            Cards = new(); 
            commonCards = new();
            playersInGame = new();
            playerTurn = 0;
            allIn = false;
            actualBet = 0;
            pot = 0;
            raiseCounter = 0;
            raiserIndex = 0;
        }
        public Game(List<Player> players) { 
            Players = players;
            Cards = new();
            commonCards = new();
            playersInGame = new();
            foreach(var player in players) playersInGame.Add(true);
            playerTurn = 0;
            allIn = false;
            actualBet = 0;
            pot = 0;
            raiseCounter = 0;
            raiserIndex = 0;
        }
        
        public void AddPlayer(Player player)
        {
            Players.Add(player);
            playersInGame.Add(true);
        }

        public string CalculateHandStrength(Player player)
        {
            // Combine player's own cards and common cards
            List<Card> phand = player.cards;
            foreach (Card card in commonCards)
            {
                phand.Add(card);
            }

            // Sort the hand in ascending order by value
            phand.Sort((a, b) => a.value.CompareTo(b.value));
            Hand hand;
            hand = new Hand(phand);
            int bestSuit = hand.getNumberOfSuits();
            if (hand.hand[hand.hand.Count - 1].value == 14) // per gestire l'eccezione Asso, che può valere sia nelle scale A, 2, 3 .. che in quelle A, K, Q, ..
            {
                hand.hand.Add(new Card(hand.hand[hand.hand.Count - 1].suit, 1, hand.hand[hand.hand.Count - 1].name));
                hand.hand.Sort((a, b) => a.value.CompareTo(b.value));
            }

            // Check for Royal Flush
            if (hand.hand[hand.hand.Count - 1].value == 14 && bestSuit >= 5)
            {
                int count = 0;
                for (int i = hand.hand.Count - 1; i > 0; i--)
                {
                    if (hand.hand[i].value - hand.hand[i - 1].value == 1 && hand.hand[i].suit == hand.hand[i - 1].suit)
                    {
                        count++;
                        if (count == 4)
                            return "Hand: Royal Flush";
                    }
                    else if (hand.hand[i].value - hand.hand[i - 1].value > 1)
                        break;
                }
            }


            if (bestSuit >= 5)
            {
                int count = 0;
                for (int i = hand.hand.Count - 1; i > 0; i--)
                {
                    if (hand.hand[i].value - hand.hand[i - 1].value == 1)
                    {
                        count++;
                        if (count == 4)
                            if (hand.hand[hand.hand.Count - 1].value == 14) // per gestire l'eccezione Asso, che può valere sia nelle scale A, 2, 3 .. che in quelle A, K, Q, ..
                                hand.hand.RemoveAt(0);
                        return "Hand: Straight Flush";
                    }
                    else if (hand.hand[i].value - hand.hand[i - 1].value > 1)
                        break;
                }
            }
            if (hand.hand[hand.hand.Count - 1].value == 14) // per gestire l'eccezione Asso, che può valere sia nelle scale A, 2, 3 .. che in quelle A, K, Q, ..
                hand.hand.RemoveAt(0);
            //check for Four of a Kind
            Dictionary<int, int> valueCounts = new Dictionary<int, int>();
            foreach (Card card in hand.hand)
            {
                if (valueCounts.ContainsKey(card.value))
                    valueCounts[card.value]++;
                else
                    valueCounts[card.value] = 1;
            }
            if (valueCounts.ContainsValue(4))
                return "Hand: Poker";

            //check for Full House
            if (valueCounts.ContainsValue(3) && valueCounts.ContainsValue(2))
                return "Hand: Full House";

            //check for Flush
            if (bestSuit >= 5)
                return "Hand: Flush";
            //check for Straight
            int counter = 0;
            for (int i = hand.hand.Count - 1; i > 0; i--)
            {
                if (hand.hand[i].value - hand.hand[i - 1].value == 1)
                {
                    counter++;
                    if (counter == 4)
                        return "Hand: Straight";
                }
                else if (hand.hand[i].value - hand.hand[i - 1].value > 1)
                    break;

            }
            //check for Three of a Kind
            if (valueCounts.ContainsValue(3))
                return "Hand: Three of a Kind";
            //check for Pairs
            int k = 0;
            for (int i = 1; i < hand.hand.Count; i++)
            {
                if (hand.hand[i].value == hand.hand[i - 1].value)
                {
                    k++;
                    i++;
                }
            }

            if (k >= 2)
                return "Hand: Two Pairs";
            else if (k == 1)
                return "Hand: One Pair";
            return "Hand: High Card";
        }

        public void StartGame()
        {
            
        }
        public void PlayBotMoves()
        {
            playerTurn = 0;
            raiseCounter = 0;
            foreach (Player player in Players)
            {
                if (playersInGame[playerTurn])
                {
                    if(playerTurn != 0)
                    {
                        if((player as Bot).MakeMove(this, out int bet, out bool allIn, out string move))
                        {
                            if (allIn) this.allIn = true;
                            if(bet > actualBet)
                            {
                                raiseCounter++;
                                raiserIndex = playerTurn;
                                actualBet = bet;
                                
                            }
                            playerTurn++;
                        }
                        else
                        {
                            playersInGame[playerTurn] = false;
                            Players[playerTurn].money -= Players[playerTurn].bet;
                            playerTurn++;
                        }
                    }
                }
            }
        }

        public void PlayerCall()
        {
            Players[0].bet += actualBet - Players[0].bet;
            playerTurn++;
            PlayBotMoves();
        }
        public void PlayerRaise(int amount)
        {
            Players[0].bet += amount;
            playerTurn++;
            raiseCounter++;
            raiserIndex = 0;
            PlayBotMoves();
        }

        public void PlayerCheck()
        {
            playerTurn++;
            PlayBotMoves();
        }
        public void PlayerFold()
        {
            Players[0].money -= Players[0].bet;
            Players[0].bet = 0;
        }
    }
}
