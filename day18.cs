using System.Text.RegularExpressions;

namespace adventCode21
{
    public class day18
    {
        private static bool real = true;
        
        private string file = real ? "day18_input.txt" : "day18_inputTest.txt";


        public void execute()
        {
           //do1();
           do2();
        }

        private void do2()
        {
            var input = InputConverter.getInput(file);

            var listOfMagnitudes = new List<int>();

            foreach (var firstSnailNumber in input)
            {
                foreach (var secondSnailNumber in input)
                {
                    var addition = AddNumbers(firstSnailNumber, secondSnailNumber);
                    var magnitude = CalculateMagnitude(addition);
                    listOfMagnitudes.Add(magnitude);

                }
            }

            Console.WriteLine("Largest magnitude: {0}", listOfMagnitudes.OrderByDescending(x => x).First());
        }

        private void do1()
        {
            var input = InputConverter.getInput(file);
            string number = input[0];

            for (int i = 1; i < input.Length; i++)
            {
                number = AddNumbers(number, input[i]);
            }

            Console.WriteLine(number);

            Console.WriteLine("Magnitude: {0}", CalculateMagnitude(number));

        }

        private int CalculateMagnitude(string numberString)
        {
            var test = ReplaceSimplePairWithMagnitude(numberString);
            int magnitude;

            while (!int.TryParse(numberString, out magnitude))
            {
                numberString = ReplaceSimplePairWithMagnitude(numberString);
            }

            return magnitude;
        }

        private string ReplaceSimplePairWithMagnitude(string numberString)
        {
            var matches = Regex.Matches(numberString, @"\[(?'values'\d+,\d+)\]").Reverse();
            foreach (Match pair in matches)
            {
                var values = pair.Groups["values"].Value.Split(',').Select(s => int.Parse(s));
                var mag = values.First()*3 + values.Last()*2;
                numberString = numberString.Remove(pair.Index, pair.Length);
                numberString = numberString.Insert(pair.Index, mag.ToString());
            }

            return numberString;
        }

        private string ReduceNumber(string numberString)
        {
            var reducedNumber = numberString;
            bool explosion, split = false;

            do
            {
                (reducedNumber, explosion) = ExplodeNumber(reducedNumber);

                if(!explosion)
                {
                    (reducedNumber, split)  = SplitNumber(reducedNumber);
                }

            } while (explosion || split);

            return reducedNumber;
        }

        private (string newNumber, bool somethingHappend) SplitNumber(string numberString)
        {
            var split = numberString;
            var somethingHappend = false;
            var matches = Regex.Matches(numberString, "[0-9]{2,}");
            if(matches.Any())
            {
                somethingHappend = true;
                var tobeSplit = matches.OfType<Match>().First();
                var number = int.Parse(tobeSplit.Value);

                var newPair = "[" + Math.Floor((double)number/2) + "," + Math.Ceiling((double)number/2) + "]";

                split = split.Remove(tobeSplit.Index, tobeSplit.Length);
                split = split.Insert(tobeSplit.Index, newPair);
            }

            return (split, somethingHappend) ;
        }

        private (string newNumber, bool somethingHappend) ExplodeNumber(string number)
        {
            var exploded = number;
            var nestingLevel = 0;
            var somethingHappend = false;


            for (int i = 0; i < number.Length; i++)
            {
                if(number[i] == '[') nestingLevel++;
                if(number[i] == ']') nestingLevel--;

                if(nestingLevel > 4)
                {
                    somethingHappend = true;
                    var openingBracket = i;
                    var closingBracket = FindClosingBracket(number, openingBracket);
                    var tobeExploded = number.Substring(openingBracket+1, closingBracket - openingBracket-1);
                    var leftNumber = int.Parse(tobeExploded.Split(',')[0]);
                    var rightNumber =int.Parse(tobeExploded.Split(',')[1]);

                    exploded = exploded.Remove(openingBracket, closingBracket-openingBracket+1);
                    exploded = exploded.Insert(openingBracket, "0");

                    exploded = IncreaseNumberToTheLeft(exploded, leftNumber, openingBracket);
                    exploded = IncreaseNumberToTheRight(exploded, rightNumber, openingBracket+1);

                    break;
                }
            }

            return (exploded, somethingHappend);
        }

        private string IncreaseNumberToTheRight(string number, int rightNumber, int closingBracket)
        {
            if(number.Substring(closingBracket+1).Any(c => Char.IsDigit(c)))
            {
                var stringToRight = number.Substring(closingBracket+1);
                var matches = Regex.Matches(stringToRight, @"\d+");
                var firstNumber= matches.First();

                number = number.Remove(closingBracket+1+firstNumber.Index, firstNumber.Length);
                number = number.Insert(closingBracket+1+firstNumber.Index, (int.Parse(firstNumber.Value)+rightNumber).ToString());
            }

            return number;
        }

        private string IncreaseNumberToTheLeft(string number, int leftNumber, int openingBracket)
        {
            if(number.Substring(0,openingBracket).Any(c => Char.IsDigit(c)))
            {
                var stringToLeft = number.Substring(0, openingBracket);
                var matches = Regex.Matches(stringToLeft, @"\d+");
                var lastNumber= matches.Last();

                number = number.Remove(lastNumber.Index, lastNumber.Length);
                number = number.Insert(lastNumber.Index, (int.Parse(lastNumber.Value)+leftNumber).ToString());
            }

            return number;
        }

        private string AddNumbers(string number1, string number2)
        {
            var newNumber = "[" + number1 + "," + number2 + "]";
            newNumber = ReduceNumber(newNumber);
            return newNumber;
        }

        private int FindClosingBracket(string number, int indexOfOpeningBracket)
        {
            var openingBracketsCount = 1;
            for (int i = indexOfOpeningBracket+1; i < number.Length; i++)
            {
                if(number[i] == '[') openingBracketsCount++;
                if(number[i] == ']') openingBracketsCount--;

                if(openingBracketsCount == 0) return i;
            }

            return -1;
        }

    }
}