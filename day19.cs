namespace adventCode21
{
    public class day19
    {
        private static bool real = false;
        
        private string file = real ? "day19_input.txt" : "day19_inputTest.txt";


        public void execute()
        {
           do1();
           //do2();
        }

        private void do1()
        {
            var input = InputConverter.getInput(file);

            var scannerIndexes =  Enumerable.Range(0, input.Length).Where(i => input[i].StartsWith("--- scanner ")).ToList();

            var scannerList = new List<List<Point>>();

            foreach (var index in scannerIndexes)
            {
                var i = index+1;
                var beacons = new List<Point>();
                while (i < input.Length &&!String.Empty.Equals(input[i]))
                {
                    beacons.Add(new Point(input[i]));
                    i++;
                }

                scannerList.Add(beacons);
            }
        }
    }
}