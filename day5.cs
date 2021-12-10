namespace adventCode21
{
    public class day5
    {
        private static bool real = true;
        private string file = real ? "day5_input.txt" : "day5_inputTest.txt";

        public void execute()
        {
            calculateOverlappingPoints(false);
            calculateOverlappingPoints(true);
        }

        public void calculateOverlappingPoints(bool includeDiagonal)
        {
            var input = InputConverter.getInput(file);
            var vents = getAllVents(input);
            var map = getEmptyMapForVents(vents);

            foreach (var vent in vents)
            {
                map = AddVent(map, vent, includeDiagonal);
            }

            if(!real) map.PrintMap();

            var indexes = findAllDangerousPoints(map);

            Console.WriteLine("Overlapping Points: {0}", indexes.Count());
        }

        private List<(Point start,Point end)> getAllVents(string[] input)
        {
            var vents = new List<(Point start,Point end)>();

            foreach (var line in input)
            {
                var startPoint = new Point(line.Split("->")[0]);
                var endPoint = new Point(line.Split("->")[1]);

                vents.Add((startPoint, endPoint));
            }

            return vents;
        }

        private int[,] AddVent(int[,] map, (Point start, Point end) vent, bool includeDiagonal)
        {
            if( vent.start.xCoordinate == vent.end.xCoordinate)
            {
                return AddVerticalVent(map, vent);
            }

            if( vent.start.yCoordinate == vent.end.yCoordinate)
            {
                return AddHorizonalVent(map, vent);
            }

            if(!includeDiagonal) return map;

            return AddDiagonalVent(map, vent);
        }

        private int[,] AddDiagonalVent(int[,] array, (Point start, Point end) vent)
        {
            var start = vent.start.xCoordinate < vent.end.xCoordinate ? vent.start: vent.end;
            var end = vent.start== start ? vent.end : vent.start;

            var run = (end.xCoordinate - start.xCoordinate);
            var gradient = (int)Math.Floor((double)(end.yCoordinate - start.yCoordinate)/(end.xCoordinate - start.xCoordinate));

            for (int r = 0; r <= run; r++)
            {
                var newY = start.yCoordinate+(r*gradient);
                var newX = start.xCoordinate + r;
                array[newY, newX] = ++array[newY, newX];
            }
            return array;
        }

        private int[,] AddVerticalVent(int[,] array, (Point start, Point end) vent)
        {
            var start = vent.start.yCoordinate < vent.end.yCoordinate ? vent.start.yCoordinate : vent.end.yCoordinate;
            var end = vent.start.yCoordinate == start ? vent.end.yCoordinate : vent.start.yCoordinate;
            
            for (int y = start; y <= end; y++)
            {
                array[y, vent.start.xCoordinate] = ++array[y, vent.start.xCoordinate];
            }

            return array;
        }

        private int[,] AddHorizonalVent(int[,] array, (Point start, Point end) vent)
        {
            var start = vent.start.xCoordinate < vent.end.xCoordinate ? vent.start.xCoordinate : vent.end.xCoordinate;
            var end = vent.start.xCoordinate == start ? vent.end.xCoordinate : vent.start.xCoordinate;

            for (int x = start; x <= end; x++)
            {
                array[vent.start.yCoordinate, x] = ++array[vent.start.yCoordinate, x];
            }

            return array;
        }

        private List<Point> findAllDangerousPoints(int[,] array)
        {
            var indexes = new List<Point>();
            for (int i = 0; i <= array.GetUpperBound(0); i++)
            {
                for (int l = 0; l <= array.GetUpperBound(1); l++)
                {
                    if( array[i,l] >= 2)
                    {
                        indexes.Add(new Point(l,i));
                    }
                }
            }

            return indexes;
        }

        private int[,] getEmptyMapForVents(List<(Point, Point)> vents)
        {
            var allPoints = vents.Select(s => s.Item1);
            allPoints.ToList().AddRange(vents.Select(e => e.Item2));

            var highest = (allPoints.Select(x => x.xCoordinate).OrderBy(i => i).Last()+1, 
                            allPoints.Select(y => y.yCoordinate).OrderBy(l => l).Last()+1);

            return new int[highest.Item1+1, highest.Item2+1];
        }

    }
}