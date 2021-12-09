namespace adventCode21
{
    public class day3
    {
        private bool real = true;

        public void execute()
        {
            do1();
            do2();
        }

        private void do2()
        {
            var readings = getInput();
            var oxygenGeneratorRating = calculateRateForLifeSupport(readings, getMostCommon);
            var co2scrubberRating = calculateRateForLifeSupport(readings, getleastCommon);

            Console.WriteLine(oxygenGeneratorRating*co2scrubberRating);
        }

        private int calculateRateForLifeSupport(int[,] readings, Func<int[], int> strategy)
        {
            var arrayList = new List<int[]>();

            for (int i = 0; i < readings.GetUpperBound(0)+1; i++)
            {
                arrayList.Add(getRow(readings, i));
            }

            for (int i = 0; i < readings.GetUpperBound(1)+1; i++)
            {
                var bitCritera = strategy(getCollum(arrayList, i));
            
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
            var readings = getInput();
            var gamma = calculateRateForPowerConsumption(readings, getMostCommon);
            var epsilon = calculateRateForPowerConsumption(readings, getleastCommon);

            Console.WriteLine(gamma*epsilon);
        }

        private int calculateRateForPowerConsumption(int[,] readings, Func<int[], int> strategy)
        {
            var rate = new int[readings.GetUpperBound(1)+1];

            for (int i = 0; i < rate.Length; i++) 
            {
                rate[i] = strategy(getCollum(readings, i));
            }

            return Convert.ToInt32(String.Join(String.Empty, rate), 2);
        }

        private int[] getCollum(int[,] array, int index)
        {
            var collum = new int[array.GetUpperBound(0)+1];
            
            for (int i = 0; i <= array.GetUpperBound(0); i++)
            {
                collum[i] = array[i, index];
            }

            return collum;
        }

        private int[] getCollum(List<int[]> list, int index)
        {
            var collum = new int[list.Count];
            
            for (int i = 0; i < list.Count; i++)
            {
                collum[i] = list[i][index];
            }

            return collum;
        }

        private int[] getRow(int[,] array, int index)
        {
            var row = new int[array.GetUpperBound(1)+1];
            
            for (int i = 0; i <= array.GetUpperBound(1); i++)
            {
                row[i] = array[index, i];
            }

            return row;
        }

        private int[,] getInput()
        {
            
            var file = real ? "day3_input.txt" : "day3_inputTest.txt";
            var input = File.ReadAllLines(Path.Join(Directory.GetCurrentDirectory(), "files" , file));

            var readings = new int[input.Length, input[0].Length];

            for (int x = 0; x < input.Length; x++)
            {
                readings = replaceRow(readings,input[x].ToCharArray().Select(x => int.Parse(x.ToString())).ToArray(), x);
            }

            return readings;
        }

        private int[,] replaceRow(int[,] array, int[] newRow, int index)
        {
            var newArray = array;

            for (int y = 0; y < newRow.Length; y++)
            {
                newArray[index, y] = newRow[y];
            }

            return newArray;
        }

        private int getMostCommon(int[] array)
        {
            var ones = array.Where(x => x == 1).Count();
            var zeros = array.Where(x => x==0).Count();
            return array.Where(x => x == 1).Count() >= array.Where(x => x==0).Count() ? 1 : 0;
        }

                private int getleastCommon(int[] array)
        {
            return array.Where(x => x == 1).Count() >= array.Where(x => x==0).Count() ? 0 : 1;
        }
    }
}