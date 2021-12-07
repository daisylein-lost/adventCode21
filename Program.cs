// See https://aka.ms/new-console-template for more information



namespace day7
{
    class Program
    {
        static void Main(string[] args)
        {
          var positions = new int[] { 16,1,2,0,4,2,7,1,2,14};
          
          var totalfuel1 = do1(positions);
          var totalfuel2 = do2(positions);
        }

        private static int do1(int[] positions)
        {
          var totalfuelToPosition = new int[positions.Length];

          for (int i = 0 ; i < positions.Length; i++)
          {
              foreach (var pos in positions)
              {
                  totalfuelToPosition[i] += Math.Abs(positions[i] - pos);
              }

            Console.WriteLine("{0} -> {1}", positions[i], totalfuelToPosition[i]);
          }

          var bestPos = positions[Array.IndexOf(totalfuelToPosition, totalfuelToPosition.Min())];
          Console.WriteLine("Best Position: {0} with {1} fuel", bestPos, totalfuelToPosition.Min());

          return totalfuelToPosition.Min();
        }

        private static int do2(int[] positions)
        {
          var totalfuelToPosition = new int[positions.Max() - positions.Min()];

          for (int i = 0 ; i < totalfuelToPosition.Length; i++)
          {
            var steps= 0;
              foreach (var pos in positions)
              {
                  steps = Math.Abs(i - pos);
                  totalfuelToPosition[i] += ((int)Math.Pow(steps, 2) + steps)/2 ;
              }

            Console.WriteLine("{0} -> {1}", i, totalfuelToPosition[i]);
          }

          var bestPos = Array.IndexOf(totalfuelToPosition, totalfuelToPosition.Min());
          Console.WriteLine("Best Position: {0} with {1} fuel", bestPos, totalfuelToPosition.Min());

          return totalfuelToPosition.Min();
        }
    }
}