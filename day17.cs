namespace adventCode21
{
    public class day17
    {
        private static bool real = true;
        
        private string rawTargetArea = real ? "target area: x=14..50, y=-267..-225" : "target area: x=20..30, y=-10..-5";


        public void execute()
        {
            var startingPoint = new Point(0,0);
            var targetArea = getTargetArea();

            var highestXInTargetArea = targetArea.Select(p => p.xCoordinate).OrderByDescending(x => x).First();
            var possibleXVelocities = Enumerable.Range(0, highestXInTargetArea+1);

            var velocityList = new List<((int, int) velocity, int highestY)>();

            for (int y = -500; y <= 500; y++) //guess
            {

                foreach (var x in possibleXVelocities)
                {
                    var velocity = (x, y);
                    var result = shootProbe(startingPoint, velocity, targetArea);
                    if(result.targetFound)
                    {
                        velocityList.Add((velocity, result.highestY));
                    }
                }
            }

            var ordered = velocityList.OrderByDescending(vl => vl.highestY);
            Console.WriteLine("Highest Y reached is {0}", ordered.First().highestY);
            Console.WriteLine("All possible velocities: {0}", ordered.Count());

        }


        private List<Point> getTargetArea()
        {
            var xPostions = rawTargetArea.Split(',')[0].Split('=')[1].Split("..").Select(s => int.Parse(s));
            var yPostions = rawTargetArea.Split(',')[1].Split('=')[1].Split("..").Select(s => int.Parse(s));

            var targetPoints = new List<Point>();

            for (int x = xPostions.First(); x <= xPostions.Last(); x++)
            {
                for (int y = yPostions.First(); y <= yPostions.Last(); y++)
                {
                    targetPoints.Add(new Point(x,y));
                }
            }

            return targetPoints;
        }

        private (bool targetFound, int highestY) shootProbe(Point startingPoint, (int x, int y) velocity, List<Point> targetArea)
        {
            var highestX = targetArea.Select(p => p.xCoordinate).OrderByDescending(x => x).First();
            var lowestY = targetArea.Select(p => p.yCoordinate).OrderBy(y => y).First();
            var bottomRight = new Point(highestX, lowestY);

            var currentPoint = new Point(startingPoint.xCoordinate, startingPoint.yCoordinate);
            var points = new List<Point>();

            do
            {
                points.Add(new Point(currentPoint.xCoordinate, currentPoint.yCoordinate));
                currentPoint.xCoordinate += velocity.x;
                currentPoint.yCoordinate += velocity.y;
                velocity.y--;
                if(velocity.x != 0)
                {
                    velocity.x = velocity.x > 0 ? --velocity.x : ++velocity.x;
                }
            }while(!targetArea.Contains(currentPoint) && !targetAreaMissed(bottomRight, currentPoint));

            var highestY = points.OrderByDescending(p => p.yCoordinate).First().yCoordinate;

            return (targetArea.Contains(currentPoint), highestY);
        }

        private bool targetAreaMissed(Point bottomRight, Point currentPoint)
        {
            return currentPoint.xCoordinate > bottomRight.xCoordinate || currentPoint.yCoordinate < bottomRight.yCoordinate;
        }

    }
}