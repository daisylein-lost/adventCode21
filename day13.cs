namespace adventCode21
{
    public class day13
    {
        private static bool real = true;
        private string file = real ? "day13_input.txt" : "day13_inputTest.txt";
        public void execute()
        {
            do1();
            do2();
        }

        private int highestX;
        private int highestY;

        private void do1()
        {
            var input = InputConverter.getInput(file).ToList();
            var seperationLineIndex = input.FindIndex(0, s => String.IsNullOrEmpty(s));

            var dotList = input.GetRange(0, seperationLineIndex);
            var foldCommandList = input.GetRange(seperationLineIndex+1, input.Count()-seperationLineIndex-1);

            var dotPoints = GetInitialDotList(dotList);

            highestX = dotPoints.Select(p => p.xCoordinate).OrderByDescending(x => x).First();
            highestY = dotPoints.Select(p => p.yCoordinate).OrderByDescending(x => x).First();

            if(!real) printPointsOnMap(dotPoints);

            dotPoints = foldPaper(foldCommandList[0], dotPoints);
        }

        private void do2()
        {
            var input = InputConverter.getInput(file).ToList();
            var seperationLineIndex = input.FindIndex(0, s => String.IsNullOrEmpty(s));

            var dotList = input.GetRange(0, seperationLineIndex);
            var foldCommandList = input.GetRange(seperationLineIndex+1, input.Count()-seperationLineIndex-1);

            var dotPoints = GetInitialDotList(dotList);

            highestX = dotPoints.Select(p => p.xCoordinate).OrderByDescending(x => x).First();
            highestY = dotPoints.Select(p => p.yCoordinate).OrderByDescending(x => x).First();

            if(!real) printPointsOnMap(dotPoints);

            foreach (var command in foldCommandList)
            {
                dotPoints = foldPaper(command, dotPoints);
            }

            printPointsOnMap(dotPoints);
        }

        private List<Point> foldPaper(string foldingCommand, List<Point> dotPoints)
        {
            if(foldingCommand.Contains("x="))
            {
                var xFolding = int.Parse(String.Join(String.Empty, foldingCommand.Where(s => char.IsDigit(s))));
                dotPoints = foldLeft(dotPoints, xFolding);
            }
            else if(foldingCommand.Contains("y="))
            {
                var yFolding = int.Parse(String.Join(String.Empty, foldingCommand.Where(s => char.IsDigit(s))));
                dotPoints = foldUp(dotPoints, yFolding);
            }
            else throw new ArgumentException();

            if(!real)
            {
                Console.WriteLine("=============================");
                printPointsOnMap(dotPoints);
                Console.WriteLine("=============================");
            }
            Console.WriteLine("Visible Dots after folding: {0}", dotPoints.Count());

            return dotPoints;
        }
        private List<Point> GetInitialDotList(List<string> inputList)
        {
            var dotPoints = new List<Point>();

            foreach (var dot in inputList)
            {
                var values = dot.Trim().Split(',').Select(s => int.Parse(s)).ToArray();
                dotPoints.Add(new Point(values[0], values[1]));
            }

            return dotPoints;
        }

        private void printPointsOnMap(List<Point> pointList)
        {
            var map = new int[highestY+1, highestX+1];

            pointList.ForEach(p => map[p.yCoordinate, p.xCoordinate]= 1);

            map.PrintMap();
        }

        private List<Point> foldUp(List<Point> pointList, int foldingLine)
        {
            pointList.Where(p => p.yCoordinate > foldingLine).ToList()
            .ForEach(y => y.yCoordinate = foldingLine - Math.Abs(y.yCoordinate - foldingLine));
            pointList = pointList.Distinct().ToList();
            highestY = foldingLine-1;

            return pointList;
        }

        private List<Point> foldLeft(List<Point> pointList, int foldingLine)
        {
            pointList.Where(p => p.xCoordinate > foldingLine).ToList()
            .ForEach(x => x.xCoordinate = foldingLine - Math.Abs(x.xCoordinate - foldingLine));
            pointList = pointList.Distinct().ToList();
            highestX = foldingLine-1;

            return pointList;
        }
    }
}