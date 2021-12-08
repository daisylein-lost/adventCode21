namespace adventCode21
{
    public class day3
    {
        private bool real = false;

        public void execute()
        {
            do1();
        }

        private void do1()
        {
            var file = real ? "day3_input1.txt" : "day3_inputTest.txt";
            var input = System.IO.File.ReadAllLines(Path.Join(Directory.GetCurrentDirectory(), "files" , file));

            var matrix = new int[input.Length,input.OrderByDescending(s => s.Length).First().Length];

            for (int i = 0; i < matrix.GetUpperBound(0); i++)
            {   
                var number = input[i];
                for (int l = 0; l < matrix.GetUpperBound(1); l++)
                {
                    matrix[i,l] = number[l] - '0';
                }
            }

            for (int i = 0; i < matrix.GetUpperBound(1); i++)
            {
                var mostCommon = matrix;
            }

        }
    }
}