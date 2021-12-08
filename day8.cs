namespace adventCode21
{
    public class day8
    {
        private bool real = true;

        public void execute()
        {
            do1();
            do2();
        }

        private void do2()
        {
            Console.WriteLine("Part 2:");
            var input = getInput(1);
            var sum = 0;

            foreach (var line in input)
            {
                var knownPatterns = new List<string>() {"?","?","?","?","?","?","?","?","?","?"};
                var data = splitInput(line);

                knownPatterns[1] = String.Join(String.Empty,findEasyDigitPatterns(1, data.Item2));
                knownPatterns[4] = String.Join(String.Empty,findEasyDigitPatterns(4, data.Item2));
                knownPatterns[7] = String.Join(String.Empty,findEasyDigitPatterns(7, data.Item2));
                knownPatterns[8] = String.Join(String.Empty,findEasyDigitPatterns(8, data.Item2));

                var possible096 = data.Item2.Where(s => s.Count() == 6).Select(s  => s.ToCharArray().ToList()).ToList();
                var possible235 = data.Item2.Where(s => s.Count() == 5).Select(s  => s.ToCharArray().ToList()).ToList();

                possible235.ForEach(x => x.Sort());
                possible096.ForEach(x => x.Sort());

                knownPatterns[3] = FindHardDigitPatterns(3, knownPatterns, possible235);
                knownPatterns[0] = FindHardDigitPatterns(0, knownPatterns, possible096);
                knownPatterns[9] = FindHardDigitPatterns(9, knownPatterns, possible096);
                knownPatterns[6] = FindHardDigitPatterns(6, knownPatterns, possible096);
                knownPatterns[5] = FindHardDigitPatterns(5, knownPatterns, possible235);
                knownPatterns[2] = FindHardDigitPatterns(2, knownPatterns, possible235);

                var digits = String.Join(String.Empty, data.Item1.Select(x => String.Join(String.Empty, x)).Select(d => knownPatterns.FindIndex(x => x.Equals(d))));

                Console.WriteLine(digits);
                sum += int.Parse(digits);
            }

            Console.WriteLine("Total amount: {0}", sum);

        }

        private string FindHardDigitPatterns(int digit, List<string> knownPatterns, List<List<char>> possibleCandidates)
        {

            switch (digit)
            {
                case 0:
                    return knownPatterns[8].Replace(knownPatterns[3].Intersect(knownPatterns[4]).Where(x => ! knownPatterns[1].Contains(x)).First().ToString(), String.Empty);
                case 3:
                    return possibleCandidates.Where(x => x.Contains(knownPatterns[1][0]) && x.Contains(knownPatterns[1][1])).Select(c => String.Join(String.Empty, c)).First();
                case 2:
                    return possibleCandidates.Select(x => String.Join(String.Empty,x)).Where(m => !m.Equals(knownPatterns[3]) && !m.Equals(knownPatterns[5])).First();
                case 5:
                    var ce = knownPatterns[8].Where(x => !knownPatterns[6].Intersect(knownPatterns[9]).Contains(x)).ToList();
                    return knownPatterns[8].Replace(ce[0].ToString(), String.Empty).Replace(ce[1].ToString(), String.Empty);
                case 6:
                    return possibleCandidates.Select(x => String.Join(String.Empty, x)).Where(m => !m.Equals(knownPatterns[0]) && !m.Equals(knownPatterns[9])).First();
                case 9:
                    return possibleCandidates.Select(x => String.Join(String.Empty, x)).Where(m => !m.Equals(knownPatterns[0]) && m.Contains(knownPatterns[1][0]) && m.Contains(knownPatterns[1][1])).First();
                default:
                    throw new ArgumentException();
            }
        }

        private void do1()
        {
            Console.WriteLine("Part 1:");
            var input = getInput(1);
            var amounts = new List<int>() {0,0,0,0,0,0,0,0,0,0};
            
            foreach (var line in input)
            {
                var data = splitInput(line);

                amounts[1] += countDigit(1, data.Item2, data.Item1);
                amounts[4] += countDigit(4, data.Item2, data.Item1);
                amounts[7] += countDigit(7, data.Item2, data.Item1);
                amounts[8] += countDigit(8, data.Item2, data.Item1);
            }

            Console.WriteLine("Amount of ones: {0}", amounts[1]);
            Console.WriteLine("Amount of fours: {0}", amounts[4]);
            Console.WriteLine("Amount of sevens: {0}", amounts[7]);
            Console.WriteLine("Amount of eights: {0}", amounts[8]);
            Console.WriteLine("Total amount of 1,4,7 or 8: {0}", amounts[1]+amounts[4]+amounts[7]+amounts[8]);
        }

        private (List<List<char>>, string[]) splitInput(string inputLine)
        {
            var allDigits = inputLine.Split('|')[0].Split(' ').Select(x => x.Trim()).Where(s => !String.IsNullOrEmpty(s)).ToArray();

            var fourDigits = inputLine.Split('|')[1].Split(' ').Select(x => x.Trim()).Where(s => !String.IsNullOrEmpty(s)).Select(c => c.ToCharArray().ToList()).ToList();
            fourDigits.ForEach(l => l.Sort());

            return (fourDigits, allDigits);
        }

        private string[] getInput(int part)
        {
            
            var file = real ? "day8_input1.txt" : "day8_inputTest" + part + ".txt";
            return File.ReadAllLines(Path.Join(Directory.GetCurrentDirectory(), "files" , file));
        }

        private int countDigit(int digit, string[] allDigits, List<List<char>> fourDigits)
        {
            var knownPatterns = findEasyDigitPatterns(digit, allDigits);

            return fourDigits.FindAll(s => s.SequenceEqual(knownPatterns)).Count();
        }

        private List<char> findEasyDigitPatterns(int digit, string[] allDigits)
        {
            List<char> list;
            switch (digit)
            {
                case 1:
                    list = allDigits.Where(s => s.Count() == 2).First().ToCharArray().ToList();
                    break;
                case 4:
                    list = allDigits.Where(s => s.Count() == 4).First().ToCharArray().ToList();
                    break;
                case 7:
                    list = allDigits.Where(s => s.Count() == 3).First().ToCharArray().ToList();
                    break;
                case 8:
                    list = allDigits.Where(s => s.Count() == 7).First().ToCharArray().ToList();
                    break;
                default:
                    throw new ArgumentException();
            }
            list.Sort();
            return list;
        }
    }
}