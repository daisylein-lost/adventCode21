namespace adventCode21
{
    public class day10
    {
        private static bool real = true;
        private string file = real ? "day10_input.txt" : "day10_inputTest.txt";

        private List<char> closingCharacters = new List<char>{ ')', ']', '}', '>'};
        private Dictionary<char, char> fittingOpeningCharacter = new Dictionary<char, char> { {')', '('}, {']', '['}, {'}', '{'}, {'>', '<'}};
        private Dictionary<char, int> errorScoreTable = new Dictionary<char, int> { {')', 3}, {']', 57}, {'}', 1197}, {'>', 25137}};

        private Dictionary<char, int> incompleteScoreTable = new Dictionary<char, int> { {')', 1}, {']', 2}, {'}', 3}, {'>', 4}};

        public void execute()
        {
            do1();
            do2();
        }

        private void do2()
        {
            var input = InputConverter.getInput(file);

            var errorScore = new List<long>();

            foreach (var line in input)
            {
                var cleanLine = CleanUpLine(line);

                if(!cleanLine.Any(c => closingCharacters.Contains(c)))
                {
                    var missingPart = cleanLine.Reverse().Select(c => fittingOpeningCharacter.FirstOrDefault(x => x.Value == c).Key);
                    errorScore.Add(calculateIncompleteScore(missingPart.ToList()));
                }
            }

            errorScore.Sort();
            Console.WriteLine("IncompleteErrorScore: {0}", errorScore[(errorScore.Count-1)/2]);
        }

        private long calculateIncompleteScore(List<char> missingPart)
        {
            long score = 0;

            foreach (var character in missingPart)
            {
                score *= 5;
                score += incompleteScoreTable[character];
            }

            return score;
        }

        private void do1()
        {
            var input = InputConverter.getInput(file);

            var syntaxErrors = new List<int>();

            foreach (var line in input)
            {
                var cleanLine = CleanUpLine(line);

                if(cleanLine.Any(c => closingCharacters.Contains(c)))
                {
                    var firstClosingCharacter = cleanLine.Where(c => closingCharacters.Contains(c)).First();
                    syntaxErrors.Add(errorScoreTable[firstClosingCharacter]);
                }
            }

            Console.WriteLine("SyntaxErrorScore: {0}",syntaxErrors.Sum());
        }

        private string CleanUpLine(string line)
        {
            int oldLength, newLength;
            var cleanerLine = line;

            do
            {
                oldLength = cleanerLine.Count();
                cleanerLine = RemoveEmptySubChunk(cleanerLine);
                newLength = cleanerLine.Count();

            } while (oldLength != newLength);

            return cleanerLine;
        }

        private string RemoveEmptySubChunk(string line)
        {
            var indexes = new List<int>();
            for (int i = 0; i < line.Count()-1; i++)
            {
                if( fittingOpeningCharacter.ContainsKey(line[i+1]) && line[i] == fittingOpeningCharacter[line[i+1]])
                {
                    indexes.Add(i);
                }
            }

            indexes.Reverse();

            var newLine = line.ToList();

            foreach (var index in indexes)
            {
                newLine.RemoveRange(index, 2);
            }

            return String.Join(String.Empty,newLine);
        }

    }
}