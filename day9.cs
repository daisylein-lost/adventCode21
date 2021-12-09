
namespace adventCode21
{
    public class day9
    {
        private static bool real = true;
        private string file = real ? "day9_input1.txt" : "day9_inputTest.txt";

        public void execute()
        {
            do1();
            do2();
        }

        private void do2()
        {
            var heatmap = InputConverter.get2dArray(InputConverter.getInput(file));
            var lowestPoints = GetLowestPoints(heatmap);
            var basinSizeList = new List<int>();

            foreach (var point in lowestPoints)
            {
                basinSizeList.Add(getBasinSize(heatmap, point));
            }

            basinSizeList.Sort();
            basinSizeList.Reverse();
            var biggest3BasinSizes = basinSizeList.GetRange(0, 3);

            Console.WriteLine(biggest3BasinSizes[0]* biggest3BasinSizes[1] * biggest3BasinSizes[2]);

        }

        private int getBasinSize(int[,] heatmap, (int, int) position)
        {
            var basin = new List<(int, int)>() {position};
            int oldLength;

            do
            { 
                oldLength = basin.Count;
                var potentialCandidates = new List<(int, int)>();

                foreach (var point in basin)
                {
                    potentialCandidates.AddRange(getNeighbors(heatmap, point));
                }

                foreach (var item in potentialCandidates.Distinct())
                {
                    if(heatmap[item.Item1, item.Item2] < 9)
                    {
                        basin.Add(item);
                    }
                }
                basin = basin.Distinct().ToList();
            } while (basin.Count > oldLength);

            return basin.Count();
        }

        private void do1()
        {
            var heatmap = InputConverter.get2dArray(InputConverter.getInput(file));

            var lowestPointValues = GetLowestPoints(heatmap).Select(x => heatmap[x.Item1, x.Item2]);

            var ristLevels = lowestPointValues.Select(x => x+1);

            Console.WriteLine(ristLevels.Sum());
        }

        private List<(int, int)> GetLowestPoints(int[,] heatmap)
        {
            var points = new List<(int, int)>();
            for (int x = 0; x < heatmap.GetLength(0); x++)
            {
                for (int y = 0; y < heatmap.GetLength(1); y++)
                {
                    var values = getNeighborValues(heatmap, (x,y));
                    if(values.All(v => v > heatmap[x,y]))
                    {
                        points.Add((x,y));
                    }
                }
            }

            return points;
        }

        private List<(int, int)> getNeighbors(int[,] array, (int, int) position)
        {
            var neighbors = new List<(int, int)>();

            if(position.Item1 > 0)
            {
                neighbors.Add((position.Item1-1, position.Item2));
            }
            if(position.Item2 > 0)
            {
                neighbors.Add((position.Item1, position.Item2-1));
            }
            if(position.Item1 < array.GetLength(0)-1)
            {
                neighbors.Add((position.Item1+1, position.Item2));
            }
            if(position.Item2 < array.GetLength(1)-1)
            {
                neighbors.Add((position.Item1, position.Item2+1));
            }

            return neighbors;
        }

        private List<int> getNeighborValues(int[,] array, (int, int) position)
        {
            return getNeighbors(array, position).Select(x => array[x.Item1,x.Item2]).ToList();
        }
    }
}