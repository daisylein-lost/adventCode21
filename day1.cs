namespace adventCode21
{
    public class day1
    {
        private bool real = true;

        public void execute()
        {
            Console.WriteLine("Part 1:");
            calculateIncreases(readInput1());
            Console.WriteLine("--------------------");
            Console.WriteLine("Part2:");
            calculateIncreases(readInput2());
        }

        private int[] readInput1()
        { 
            var file = real ? "day1_input1.txt" : "day1_inputTest.txt";
            return System.IO.File.ReadAllLines(Path.Join(Directory.GetCurrentDirectory(), "files" , file)).Select(x => int.Parse(x)).ToArray();
        }

        private int[] readInput2()
        {
            var slidingWindow = 3;
            var file = real ? "day1_input2.txt" : "day1_inputTest.txt";
            var input = System.IO.File.ReadAllLines(Path.Join(Directory.GetCurrentDirectory(), "files" , file)).Select(x => int.Parse(x)).ToArray();
            var measurements = new int[input.Length - slidingWindow + 1];

            for (int i = 0; i < measurements.Length; i++)
            {
                measurements[i] = input[i] + input[i+1] + input[i+2];
            }

            return measurements;
        }

        private void calculateIncreases(int[] measures)
        {
            var increaseTimes = 0;
            
            for (int i = 1; i < measures.Length; i++)
            {
                var increased = measures[i-1]< measures[i];
                var message = increased ? "(increased)" : "(decreased)";
                increaseTimes = increased ? ++increaseTimes : increaseTimes;
            }

            Console.WriteLine("{0} times increased", increaseTimes);
        }
    }
}