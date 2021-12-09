namespace adventCode21
{
    public class day1
    {
        private static bool real = true;
        private string file = real ? "day1_input1.txt" : "day1_inputTest.txt";

        public void execute()
        {
            Console.WriteLine("Part 1:");
            calculateIncreases(InputConverter.get1dArray(InputConverter.getInput(file)));
            Console.WriteLine("--------------------");
            Console.WriteLine("Part2:");
            calculateIncreases(convertInput(InputConverter.get1dArray(InputConverter.getInput(file))));
        }

        private int[] convertInput(int[] input)
        { 
            var measurements = new int[input.Length - 3 + 1];

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
                increaseTimes = measures[i-1]< measures[i] ? ++increaseTimes : increaseTimes;
            }

            Console.WriteLine("{0} times increased", increaseTimes);
        }
    }
}