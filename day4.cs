using System.Text.RegularExpressions;

namespace adventCode21
{
    public class day4
    {
        private static bool real = true;
        private string file = real ? "day4_input.txt" : "day4_inputTest.txt";

        public void execute()
        {
            //do1();
            do2();
        }

        private void do2()
        {
            var data = getData();
            var allNumbers = data.Item1;
            var boards = data.Item2;
            var dummyboard = new int[boards[0].GetUpperBound(0)+1, boards[0].GetUpperBound(0)+1].SetAllValues(-1);
            var highScore = new List<(int,int)>();

            for (int i = 1; i < allNumbers.Count; i++)
            {
                var drawnNumbers = allNumbers.GetRange(0, i);
                var winners = CheckWin(boards, drawnNumbers);

                if(winners.Any())
                {
                    foreach (var winner in winners)
                    {
                        var unmarkedNumbers = getAllValues(boards[winner]).Where(n => !drawnNumbers.Contains(n)).ToList();
                        var sumUnmarkedNumbers = getAllValues(boards[winner]).Where(n => !drawnNumbers.Contains(n)).Sum();
                        highScore.Add((winner, sumUnmarkedNumbers * drawnNumbers.Last()));
                        boards[winner] = dummyboard;
                    }
                }
            }

        }

        private void do1()
        {
            var data = getData();
            var allNumbers = data.Item1;
            var boards = data.Item2;

            for (int i = 1; i < allNumbers.Count; i++)
            {
                var winner = CheckWin(boards, allNumbers.GetRange(0, i));

                if( winner.Any())
                {
                    var drawnNumbers = allNumbers.GetRange(0, i);
                    var sumUnmarkedNumbers = getAllValues(boards[winner.First()]).Where(n => !drawnNumbers.Contains(n)).Sum(); // first board wins
                    Console.WriteLine("Board {0} wins!", winner);
                    Console.WriteLine("Sum of unmarked Number: {0}", sumUnmarkedNumbers);
                    Console.WriteLine("Final Score: {0}", sumUnmarkedNumbers * drawnNumbers.Last());
                    break;
                }
            }
        }

        private (List<int>, List<int[,]>) getData()
        {
            var input = InputConverter.getInput(file).ToList();

            var allNumbers = input[0].Split(',').Select(x => int.Parse(x.ToString())).ToList();

            input = input.ToList().GetRange(1, input.Count()-1);

            var boards = getBoards(input);

            return (allNumbers, boards);
        }

        private List<int> getAllValues(int[,] array)
        {
            var numbers = new List<int>();
            for (int i = 0; i <= array.GetUpperBound(0); i++)
            {
                numbers.AddRange(InputConverter.getCollum(array, i));
            }
            return numbers;
        }

        private List<int> CheckWin(List<int[,]> boards, List<int> numbers)
        {
            var winners = new List<int>();
            for (int b= 0; b < boards.Count(); b++)
            {
                for (int i = 0; i <= boards[b].GetUpperBound(0); i++)
                {
                    var collum = InputConverter.getCollum(boards[b], i);
                    var row = InputConverter.getRow(boards[b], i);

                    if(collum.All( c => numbers.Contains(c)) || row.All(r => numbers.Contains(r)))
                    {
                        winners.Add(b);
                    }
                }
            }
            return winners.Distinct().ToList();
        }

        private List<int[,]> getBoards(List<string> input)
        {
            var indexes = Enumerable.Range(0, input.Count).Where(i => input[i].Equals("")).ToList();

            var boards = new List<int[,]>();

            foreach (var index in indexes)
            {
                var newBoard= input.GetRange(index+1, 5).Select(m => Regex.Replace(m, @"\s+", " "))
                                    .Select(s => s.Trim().Replace(" ", ",")).ToArray();
                boards.Add(InputConverter.get2dArray(newBoard, ','));
            }

            return boards;
        }
    }
}