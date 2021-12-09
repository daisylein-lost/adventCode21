namespace adventCode21
{
    public class day3
    {
        private static bool real = true;
        private string file = real ? "day3_input.txt" : "day3_inputTest.txt";

        public void execute()
        {
            do1();
            do2();
        }

        private void do2()
        {
            var readings = InputConverter.get2dArray(InputConverter.getInput(file));
            var oxygenGeneratorRating = calculateRateForLifeSupport(readings, getMostCommon);
            var co2scrubberRating = calculateRateForLifeSupport(readings, getleastCommon);

            Console.WriteLine(oxygenGeneratorRating*co2scrubberRating);
        }

        private int calculateRateForLifeSupport(int[,] readings, Func<int[], int> strategy)
        {
            var arrayList = new List<int[]>();

            for (int i = 0; i < readings.GetUpperBound(0)+1; i++)
            {
                arrayList.Add(InputConverter.getRow(readings, i));
            }

            for (int i = 0; i < readings.GetUpperBound(1)+1; i++)
            {
                var bitCritera = strategy(InputConverter.getCollum(arrayList, i));
            
                arrayList = arrayList.Where(x => x[i] == bitCritera).ToList();

                if(arrayList.Count() <= 1)
                {
                    break;
                }
            }

            return Convert.ToInt32(String.Join(String.Empty, arrayList.First()), 2);
        }

        private void do1()
        {
            var readings = InputConverter.get2dArray(InputConverter.getInput(file));
            var gamma = calculateRateForPowerConsumption(readings, getMostCommon);
            var epsilon = calculateRateForPowerConsumption(readings, getleastCommon);

            Console.WriteLine(gamma*epsilon);
        }

        private int calculateRateForPowerConsumption(int[,] readings, Func<int[], int> strategy)
        {
            var rate = new int[readings.GetUpperBound(1)+1];

            for (int i = 0; i < rate.Length; i++) 
            {
                rate[i] = strategy(InputConverter.getCollum(readings, i));
            }

            return Convert.ToInt32(String.Join(String.Empty, rate), 2);
        }

        private int getMostCommon(int[] array)
        {
            return array.Where(x => x == 1).Count() >= array.Where(x => x==0).Count() ? 1 : 0;
        }

        private int getleastCommon(int[] array)
        {
            return array.Where(x => x == 1).Count() >= array.Where(x => x==0).Count() ? 0 : 1;
        }
    }
}