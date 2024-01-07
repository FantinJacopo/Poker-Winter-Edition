using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class Bot : Player
    {
        public Bot(int id) : base(id) { }
        public Bot(int id, int money) : base(id, money) { }
        public bool MakeMove(Game game, out int bet, out bool allIn, out string move)
        {
            bet = 0;
            allIn = false;
            move = "Fold";
            string handStrength = game.CalculateHandStrength(this);

            int betAmount;
            switch (handStrength)
            {
                case "Hand: High Card":
                    betAmount = (int)(money * 0.1);
                    break;
                case "Hand: One Pair":
                    betAmount = (int)(money * 0.2);
                    break;
                case "Hand: Two Pairs":
                    betAmount = (int)(money * 0.3);
                    break;
                case "Hand: Three of a Kind":
                    betAmount = (int)(money * 0.5);
                    break;
                case "Hand: Straight":
                    betAmount = (int)(money * 0.75);
                    break;
                case "Hand: Flush":
                    betAmount = (int)(money * 0.9);
                    break;
                case "Hand: Full House":
                    betAmount = (int)(money * 0.95);
                    break;
                case "Hand: Four of a Kind":
                    betAmount = (int)(money * 1.0);
                    break;
                case "Hand: Straight Flush":
                    betAmount = (int)(money * 1.0);
                    break;
                case "Hand: Royal Flush":
                    betAmount = (int)(money * 1.0);
                    break;
                default:
                    betAmount = (int)(money * 0.1);
                    break;
            }

            if (betAmount >= money)
            {
                bet = money;
                move = "AllIn";
                allIn = true;
                this.bet += betAmount;
                return true;
            }
            if (betAmount == 0)
            {
                if (game.actualBet == 0)
                {
                    move = "Check";
                    this.bet = game.actualBet;
                    return true;
                }
                return false;
            }

            // bluff
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int a = random.Next(0, 10);
            if (a == 0)
            {
                betAmount = (int)(betAmount * (float)(random.Next(1, 5) / 10)); // da x 0.1 a x 0.5
            }
            else if (a == 1)
            {
                betAmount = (int)(betAmount * (float)(random.Next(10, 20) / 10)); //da x 1 a x 2
            }
            else if (a == 2)
            {
                betAmount = (int)(betAmount * (float)(random.Next(20, 50) / 10)); //da x 2 a x 5
            }
            else if (a == 3)
            {
                betAmount = (int)(betAmount * (float)(random.Next(50, 100) / 10)); //da x 5 a x 10
            }

            if (betAmount >= money)
            {
                bet = money;
                allIn = true;
                move = "AllIn";
                this.bet += bet;
                return true;
            }
            else if (betAmount == 0)
            {
                if (game.actualBet == 0)
                {
                    move = "Check";
                    return true;
                }
                move = "Fold";
                return false;
            }
            else
            {
                if (game.actualBet - betAmount < 0 && game.raiserIndex != game.playerTurn)
                {
                    bet = game.actualBet;
                    this.bet += bet;
                    move = "Call";
                }
                else
                {
                    if(game.raiseCounter < game.maxRaise)
                    {
                        bet = betAmount;
                        this.bet += bet;
                        move = "Raise";
                    }
                    else
                    {
                        bet = game.actualBet;
                        this.bet += bet;
                        move = "Call";
                    }
                }
            }
            return true;
        }
    }
}
