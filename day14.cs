using System.Text;

namespace adventCode21
{
    public class day14
    {
        private static bool real = true;
        private string file = real ? "day14_input.txt" : "day14_inputTest.txt";
        public void execute()
        {
            //do1();
            do2();
            
        }

        private void do2()
        {
            var input = InputConverter.getInput(file).ToList();
            var seperationLineIndex = input.FindIndex(0, s => String.IsNullOrEmpty(s));

            var polyTemp = input.GetRange(0, seperationLineIndex).First();
            var rawPairInsertions = input.GetRange(seperationLineIndex+1, input.Count()-seperationLineIndex-1);
            var pairInsertions = new Dictionary<string, char>();
            rawPairInsertions.Select(s => s.Split("->")).ToList()
                            .ForEach(p => pairInsertions.
                            Add(p[0].Trim(), p[1].Trim()[0]));
            var steps = 40;

            var pairCount = new Dictionary<string, long>();
            var pairReplacment = new Dictionary<string, List<string>>();
            var polyCount = new Dictionary<char, long>();
            Enumerable.Range('A', 26).Select(x => (char)x).ToList().ForEach(c => polyCount.Add(c,0));

            
            foreach (var insertion in pairInsertions)
            {
                var pair = insertion.Key;
                var newpairs = new List<string>();
                newpairs.Add(String.Join(String.Empty,pair[0], insertion.Value));
                newpairs.Add(String.Join(String.Empty,insertion.Value, pair[1]));
                pairReplacment.Add(pair, newpairs);
                pairCount.Add(pair, 0);
            }

            for (int i = 0; i < polyTemp.Length-1; i++)
            {
                pairCount[polyTemp.Substring(i,2)]++;
            }
            
            for (int i = 0; i < steps; i++)
            {
                pairCount = calculatePairs(pairCount, pairReplacment);
            }

            foreach (var pair in pairCount)
            {
                polyCount[pair.Key[0]] += pair.Value;
            }

            polyCount[polyTemp.Last()]++;

            var ordered = polyCount.OrderByDescending(p => p.Value);

            var mostCommonCount = ordered.First().Value;
            var leastCommonCount = ordered.Last(x => x.Value > 0).Value;

            Console.WriteLine("Most Common - Least Common = {0}", mostCommonCount - leastCommonCount);
        }

        private Dictionary<string, long> calculatePairs(Dictionary<string, long> pairCount, Dictionary<string, List<string>> pairReplacment)
        {
            var newPairCount = pairCount.ToDictionary(entry => entry.Key, entry => entry.Value);
            foreach (var pair in pairCount)
            {
                if(pair.Value == 0) continue;
                newPairCount[pair.Key] -= pair.Value;
                pairReplacment[pair.Key].ForEach(p => newPairCount[p] += pair.Value);
            }

            return newPairCount;
        }

        private void do1Recursive()
        {
            var input = InputConverter.getInput(file).ToList();
            var seperationLineIndex = input.FindIndex(0, s => String.IsNullOrEmpty(s));

            var polyTemp = input.GetRange(0, seperationLineIndex).First();
            var rawPairInsertions = input.GetRange(seperationLineIndex+1, input.Count()-seperationLineIndex-1);
            var pairInsertions = new Dictionary<string, char>();
            rawPairInsertions.Select(s => s.Split("->")).ToList()
                            .ForEach(p => pairInsertions.
                            Add(p[0].Trim(), p[1].Trim()[0]));
            var steps = 2;

            var polyCount = new Dictionary<char, long>();
            Enumerable.Range('A', 26).Select(x => (char)x).ToList().ForEach(c => polyCount.Add(c,0));

            countPolymeresRecursive(polyTemp, steps, pairInsertions, polyCount);
            polyCount[polyTemp.Last()]++;

            var ordered = polyCount.OrderByDescending(p => p.Value);

            var mostCommonCount = ordered.First().Value;
            var leastCommonCount = ordered.Last(x => x.Value > 0).Value;

            Console.WriteLine("Most Common - Least Common = {0}", mostCommonCount - leastCommonCount);

        }

        private void countPolymeresRecursive(string startingPolymere, int steps, Dictionary<string, char> pairInsertions, Dictionary<char, long> polyCount)
        {
            var polymere = startingPolymere;
            for(int s = 0; s < steps; s++)
            {
                if(polymere.Length >= 3)
                {
                    var first = polymere.Substring(0, polymere.Length/2+1);
                    countPolymeresRecursive(first, steps-s, pairInsertions, polyCount);
                    polymere = polymere.Substring(polymere.Length/2, polymere.Length-(polymere.Length/2));
                }

                polymere = String.Join(String.Empty, insertPolymeres(polymere, pairInsertions, 1));
            }

            polymere.Substring(0, polymere.Length-1).ToList().ForEach(c => polyCount[c]++);
        }
        private void do1WithStream()
        {
            var input = InputConverter.getInput(file).ToList();
            var seperationLineIndex = input.FindIndex(0, s => String.IsNullOrEmpty(s));

            var polyTemp = input.GetRange(0, seperationLineIndex).First();
            var rawPairInsertions = input.GetRange(seperationLineIndex+1, input.Count()-seperationLineIndex-1);
            var pairInsertions = new Dictionary<string, char>();
            var steps = 40;

            rawPairInsertions.Select(s => s.Split("->")).ToList()
                            .ForEach(p => pairInsertions.
                            Add(p[0].Trim(), p[1].Trim()[0]));

            var path = insertPolymereWithFileStream(pairInsertions, steps, polyTemp);

            var characterCount = new Dictionary<char, int>();

            using(var st = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            {
                byte[] buffer = new byte[1];
                while (st.Position != st.Length)
                {
                    st.Read(buffer, 0, 1);
                    var letter = Convert.ToChar(buffer[0]);
                    if(characterCount.ContainsKey(letter))
                    {
                        ++characterCount[letter];
                    }
                    else
                    {
                        characterCount.Add(letter, 1);
                    }
                }
            }

            var ordered = characterCount.OrderByDescending(p => p.Value);

            var mostCommonCount = ordered.First().Value;
            var leastCommonCount = ordered.Last().Value;

            Console.WriteLine("Most Common - Least Common = {0}", mostCommonCount - leastCommonCount);
        }

        private (string path, long length, long space ) getInitialPolymereWithFileStream(string startingTemplate, int steps)
        {
            long length = startingTemplate.Length;
            long space = 0;
            for (int i = 0; i < steps; i++)
            {
                length = (length*2)-1;
                space = (space*2)+1;
            }

            string path = @"c:\temp\MyTest.txt";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using(var st = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] info = new UTF8Encoding(true).GetBytes('?'.ToString());
                for (int i = 0; i < startingTemplate.Length; i++)
                {
                    info = new UTF8Encoding(true).GetBytes(startingTemplate[i].ToString());
                    st.Write(info,0, info.Length);
                    st.Seek(space, SeekOrigin.Current);
                }
            }

            return (path, length, space);
        }

        private string insertPolymereWithFileStream(Dictionary<string, char> pairInsertions, int steps, string initialPolymer)
        {
            (var path, var length, var space) = getInitialPolymereWithFileStream(initialPolymer, steps);

            for (int i = 0; i < steps; i++)
            {
                using(var st = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
                {
                    byte[] buffer = new byte[2];
                    while (st.Position != length-1)
                    {
                        st.Read(buffer, 0, 1);
                        st.Seek(space, SeekOrigin.Current);
                        st.Read(buffer, 1, 1);
                        var pair = String.Join(String.Empty,buffer.Select(b => Convert.ToChar(b)));
                        byte[] info = new UTF8Encoding(true).GetBytes(pairInsertions[pair].ToString());
                        st.Seek(-(((space-1)/2)+2),SeekOrigin.Current);
                        st.Write(info, 0, info.Length);
                        st.Seek((space-1)/2, SeekOrigin.Current);
                    }
                }
                space = (space-1)/2;
            }

            return path;
        }

        private void do1()
        {
            var input = InputConverter.getInput(file).ToList();
            var seperationLineIndex = input.FindIndex(0, s => String.IsNullOrEmpty(s));

            var polyTemp = input.GetRange(0, seperationLineIndex).First();
            var rawPairInsertions = input.GetRange(seperationLineIndex+1, input.Count()-seperationLineIndex-1);
            var pairInsertions = new Dictionary<string, char>();
            var steps = 3;

            rawPairInsertions.Select(s => s.Split("->")).ToList()
                            .ForEach(p => pairInsertions.
                            Add(p[0].Trim(), p[1].Trim()[0]));

            var newPolyTemp = insertPolymeres(polyTemp, pairInsertions, steps);

            var grouped = newPolyTemp.GroupBy(c => c).OrderByDescending(g => g.Count());

            var mostCommonCount = grouped.First().Count();
            var leastCommonCount = grouped.Last().Count();

            Console.WriteLine("Most Common - Least Common = {0}", mostCommonCount - leastCommonCount);
        }

        private (char[], int) getInitialPolymereWithEmptySpaces(string startingTemplate, int steps)
        {
            var length = startingTemplate.Length;
            int space = 0;
            for (int i = 0; i < steps; i++)
            {
                length = (length*2)-1;
                space = (space*2)+1;
            }
            var polymere = new char[length];
            
            for (int i = 0; i < startingTemplate.Count(); i++)
            {
                polymere[(i+i*(space))] = startingTemplate[i];
            }

            return (polymere, space);
        }

        private char[] insertPolymeres(string startingTemplate, Dictionary<string, char> pairInsertions, int steps)
        {
            (var endPolymere, int space) = getInitialPolymereWithEmptySpaces(startingTemplate, steps);
            
            var currentFilled = endPolymere.Where(c => c != '\0').Count();

            for (int i = 1; i <= steps; i++)
            {
                for (int p = 0; p < currentFilled-1; p++)
                {
                    var first = p+p*(space);
                    var second = first+(space+1);
                    var newPair = String.Join(String.Empty, endPolymere[first], endPolymere[second]);
                    endPolymere[first+(space/2)+1] = pairInsertions[newPair];
                }
                space = (space)/2;
                currentFilled = endPolymere.Where(c => c != '\0').Count();
            }
            return endPolymere;
        }
    }
}