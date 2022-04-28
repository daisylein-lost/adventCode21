namespace adventCode21
{
    public class day21
    {
        private static bool real = true;

        private int startPlayer1 = real ? 4 : 4;

        private int startPlayer2 = real ? 2 : 8;


        public void execute()
        {
            //do1();
            do2();
        }

        private void do2()
        {
            var allPossibleRolls = new List<int[]>();

            for (int firstRoll = 1; firstRoll <= 3; firstRoll++)
            {
                for (int secondRoll = 1; secondRoll <= 3; secondRoll++)
                {
                    for (int thirdRoll = 1; thirdRoll <= 3; thirdRoll++)
                    {
                        allPossibleRolls.Add(new int[] {firstRoll, secondRoll, thirdRoll});
                    }
                }
            }

            var ongoingGames = new List<Game>{ new Game(new Player(startPlayer1), new Player(startPlayer2), 1)};

            var winners = new Dictionary<string, long>();
            winners.Add("player1", 0);
            winners.Add("player2", 0);

            while (ongoingGames.Count > 0)
            {
                var newGames = new List<Game>();
                foreach (var game in ongoingGames)
                {
                    foreach (var roll in allPossibleRolls)
                    {
                        var newGame = new Game(game.player1, game.player2, game.nextPlayer);
                        newGame.rollDices(roll);
                        if(newGame.ongoing)
                        {
                        newGames.Add(newGame);
                        }
                        else
                        {
                            if(newGame.nextPlayer == 1)
                            {
                                winners["player2"]++;
                            }
                            else
                            {
                                winners["player1"]++;
                            }
                        }

                    }
                }
                ongoingGames = newGames;
            }

        }

        private class Game
        {
            public Player player1;

            public Player player2;

            public bool ongoing = true;

            public int nextPlayer = 1;

            public Game (Player first, Player second, int nextPlayer)
            {
                player1 = new Player(first.position, first.score);
                player2 = new Player(second.position, second.score);
                this.nextPlayer = nextPlayer;
            }

            public void rollDices(int[] rolls)
            {
                if(nextPlayer == 1)
                {
                    player1.move(rolls);
                    nextPlayer = 2;
                }
                else
                {
                    player2.move(rolls);
                    nextPlayer = 1;
                }

                if(player2.score >= 21 || player1.score >= 21)
                {
                    ongoing = false;
                }
            }
        }

        private void do1()
        {
            var dice = new dice();

            var player1 = new Player(startPlayer1);
            var player2 = new Player(startPlayer2);

            while (true)
            {
                player1.move(dice.rollDice());
                if(player1.score >= 1000) break;
                player2.move(dice.rollDice());
                if(player2.score >= 1000) break;
            }

            var loosingScore = player1.score >= 1000 ? player2.score : player1.score;

            Console.WriteLine("Loosing Score time dice rolls: {0}", loosingScore * dice.totalRolls);
        }

        private class dice
        {
            List<int> rolls = Enumerable.Range(1, 100).ToList();

            int rollCount = 0;

            public int totalRolls = 0;

            public int[] rollDice()
            {
                int[] numbers;
                if(rollCount <= 96)
                {
                    numbers = rolls.GetRange(rollCount, 3).ToArray();
                    rollCount = rollCount + 3;
                }
                else if(rollCount == 97)
                {
                    numbers = rolls.GetRange(rollCount, 3).ToArray();
                    rollCount = 0;
                }
                else if(rollCount == 98)
                {
                    var numbersList = rolls.GetRange(rollCount, 2);
                    numbersList.Add(rolls[0]);
                    numbers = numbersList.ToArray();
                    rollCount = 1;
                }
                else
                {
                    var numbersList = rolls.GetRange(rollCount, 1);
                    numbersList.AddRange(rolls.GetRange(0,2));
                    numbers = numbersList.ToArray();
                    rollCount = 2;
                }

                totalRolls += 3;

                return numbers;
            }
        }

        private class Player
        {
            public int position;

            public long score;

            public Player(int position)
            {
                this.position = position;
                score = 0;
            }

            public Player(int position, long score)
            {
                this.position = position;
                this.score = score;
            }

            public void move(int[] rolls)
            {
                foreach (var roll in rolls)
                {
                    var newPosition = (position + roll)% 10;
                    position = newPosition == 0 ? 10 : newPosition;
                }

                score += position;
            }
        }

    }
}