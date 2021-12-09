namespace adventCode21
{
    public static class InputConverter
    {
        public static int[,] get2dArray(string[] input)
        {
            var readings = new int[input.Length, input[0].Length];

            for (int x = 0; x < input.Length; x++)
            {
                readings = replaceRow(readings,input[x].ToCharArray().Select(x => int.Parse(x.ToString())).ToArray(), x);
            }

            return readings;
        }

        public static int[] get1dArray(string[] input)
        {
            return input.Select(x => int.Parse(x)).ToArray();
        }

        public static string[] getInput(string file)
        {
            return File.ReadAllLines(Path.Join(Directory.GetCurrentDirectory(), "files" , file));
        }

        public static int[,] replaceRow(int[,] array, int[] newRow, int index)
        {
            var newArray = array;

            for (int y = 0; y < newRow.Length; y++)
            {
                newArray[index, y] = newRow[y];
            }

            return newArray;
        }

        public static int[] getCollum(List<int[]> list, int index)
        {
            var collum = new int[list.Count];
            
            for (int i = 0; i < list.Count; i++)
            {
                collum[i] = list[i][index];
            }

            return collum;
        }

        public static int[] getCollum(int[,] array, int index)
        {
            var collum = new int[array.GetUpperBound(0)+1];
            
            for (int i = 0; i <= array.GetUpperBound(0); i++)
            {
                collum[i] = array[i, index];
            }

            return collum;
        }

        
        public static int[] getRow(int[,] array, int index)
        {
            var row = new int[array.GetUpperBound(1)+1];
            
            for (int i = 0; i <= array.GetUpperBound(1); i++)
            {
                row[i] = array[index, i];
            }

            return row;
        }
    }
}