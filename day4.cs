using System.Text.RegularExpressions;

namespace adventCode21
{
    public class day4
    {
        private static bool real = false;
        private string file = real ? "day4_input.txt" : "day4_inputTest.txt";

        public void execute()
        {
            do1();
            do2();
        }

        private void do2()
        {
            
        }

        private void do1()
        {
            var input = InputConverter.getInput(file).ToList();

            var allNumbers = input[0].Split(',').Select(x => int.Parse(x.ToString())).ToList();

            input = input.ToList().GetRange(1, input.Count()-1);

            var boards = getBoards(input);


            for (int i = 1; i < allNumbers.Count; i++)
            {
                var winner = CheckWin(boards, allNumbers.GetRange(0, i));

                if( winner != -1)
                {
                    var drawnNumbers = allNumbers.GetRange(0, i);
                    var sumUnmarkedNumbers = getAllValues(boards[winner]).Where(n => !drawnNumbers.Contains(n)).Sum();
                    Console.WriteLine("Board {0} wins!", winner);
                    Console.WriteLine("Sum of unmarked Number: {0}", sumUnmarkedNumbers);
                    Console.WriteLine("Final Score: {0}", sumUnmarkedNumbers * drawnNumbers.Last());
                    break;
                }
            }
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

        private int CheckWin(List<int[,]> boards, List<int> numbers)
        {
            for (int b= 0; b < boards.Count(); b++)
            {
                for (int i = 0; i < boards[b].GetUpperBound(0); i++)
                {
                    var collum = InputConverter.getCollum(boards[b], i);
                    var row = InputConverter.getRow(boards[b], i);

                    if(collum.All( c => numbers.Contains(c)) || row.All(r => numbers.Contains(r)))
                    {
                        return b;
                    }
                }
            }
            return -1;
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