namespace adventCode21
{
    public static class InputConverter
    {
        public static string addCharacterEveryXPosition(string input, char character, int X)
        {
            var newString = new List<char>();
            for (int i = 0; i < input.Length; i= i+X)
            {
                newString.AddRange(input.Substring(i, X).ToCharArray());
                newString.Add(character);
            }

            return String.Join(String.Empty,newString);
        }

        public static int[,] get2dArray(string[] input, char seperator='\0')
        {
            if(seperator == '\0')
            {
                input = input.Select(s => addCharacterEveryXPosition(s, ',', 1)).ToArray();
                seperator = ',';
            }

            var readings = new int[input.Length, input[0].Where(s => s == seperator).Count()+1];

            for (int x = 0; x < input.Length; x++)
            {
                var newRow = input[x].Split(seperator).Where(s => ! string.IsNullOrWhiteSpace(s)).Select(s => int.Parse(s)).ToArray();
                readings = replaceRow(readings,newRow, x);
            }

            return readings;
        }

        public static int[] get1dArray(string[] input)
        {
            return input.Select(x => int.Parse(x)).ToArray();
        }

        public static T[,] SetAllValues<T>(this T[,] array, T value) where T : struct
        {
            for (int i = 0; i <= array.GetUpperBound(0); i++)
            {
                for (int l = 0; l <= array.GetUpperBound(1); l++)
                {
                    array[i,l] = value;
                }
            }
            return array;
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