namespace adventCode21
{
    public class day7
    {
        private int[] positions = new int[] { 16,1,2,0,4,2,7,1,2,14};

        public void execute()
        {
            do1();
            do2();
        }

        private int do1()
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

        private int do2()
        {
          var totalfuelToPosition = new int[positions.Max() - positions.Min()];

          for (int i = 0 ; i < totalfuelToPosition.Length; i++)
          {
              foreach (var pos in positions)
              {
                  var steps = Math.Abs(i - pos);
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