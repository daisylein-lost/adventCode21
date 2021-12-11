namespace adventCode21
{
    public class day11
    {
        private static bool real = true;
        private string file = real ? "day11_input.txt" : "day11_inputTest.txt";
        public void execute()
        {
            do1();
            do2();
        }

        private void do2()
        {
            var map = InputConverter.get2dArray(InputConverter.getInput(file));

            var valueCount = (map.GetLength(0))*(map.GetLength(1));
            var flashCount = 0;
            var stepCount = 0;

            do
            {
                map.IncreaseAllValues(1);

                var flashingPoints = getAllPointsGreaterValue(map, 9 );

                (map, flashCount)  = flashPoints(map, flashingPoints);
                stepCount++;

            } while (flashCount != valueCount);

            Console.WriteLine("First simultaneous flash after {0} steps.", stepCount);

        }

        private void do1()
        {
            var map = InputConverter.get2dArray(InputConverter.getInput(file));

            var flashCount = 0;

            for (int i = 0; i < 100; i++)
            {
                map.IncreaseAllValues(1);

                var flashingPoints = getAllPointsGreaterValue(map, 9 );

                (map, var newFlashes)  = flashPoints(map, flashingPoints);

                flashCount += newFlashes;

                if((i+1) % 10 == 0) 
                {
                    Console.WriteLine("==========================");
                    map.PrintMap();
                    Console.WriteLine("FlashingPoints: {0}", flashCount);
                }
            }

            Console.WriteLine("Total FlashingPoints: {0}", flashCount);

        }

        private (int[,] map, int flashCount) flashPoints(int[,] map, List<Point> points)
        {
            var newPoints = points;
            while(newPoints.Count() != 0)
            {
                foreach (var point in newPoints)
                {
                    var neighbors = InputConverter.get8Neighborhood(map, point);
                    neighbors.ForEach(p => map[p.yCoordinate, p.xCoordinate]++ );
                }
                var allPoints = getAllPointsGreaterValue(map, 9);
                newPoints = allPoints.Where(p => !points.Contains(p)).ToList();
                points = allPoints;
            }

            points.ForEach(p => map[p.yCoordinate, p.xCoordinate] = 0);

            return (map, points.Count());
        }

        private List<Point> getAllPointsGreaterValue(int[,] map, int value)
        {
            var points = new List<Point>();

            for (int i = 0; i <= map.GetUpperBound(0); i++)
            {
                for (int l = 0; l <= map.GetUpperBound(1); l++)
                {
                    if(map[i,l] > value)
                    {
                        points.Add(new Point(l,i));
                    }
                }
            }

            return points;
        }
    }
}