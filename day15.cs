using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;

namespace adventCode21
{
    public class day15
    {
        private static bool real = true;
        
        private string file = real ? "day15_input.txt" : "day15_inputTest.txt";
        

        public void execute()
        {
            //do1();
            do2();
            
        }

        private void do2()
        {
            var inputMap = InputConverter.get2dArray(InputConverter.getInput(file));

            var map = getSuperMap(inputMap);

            var result = otherDijkstra(map);

            Console.WriteLine("Lowest risk = {0}", result.Distance);
        }

        private int[,] getSuperMap(int[,] initialMap)
        {
            var map = new int[initialMap.GetLength(0)*5,initialMap.GetLength(1)*5];

            for (int i = 0; i < 5; i++)
            {
                var inputMap = initialMap.Copy();
                for (int s = 0; s < 5; s++)
                {
                    for (int y = 0; y <= inputMap.GetUpperBound(0); y++)
                    {
                        for (int x = 0; x <= inputMap.GetUpperBound(1); x++)
                        {
                            map[y + i*inputMap.GetLength(0),x + s*inputMap.GetLength(1)] = inputMap[y,x];
                            inputMap[y,x] = inputMap[y,x] >= 9 ? 1 : ++inputMap[y,x];
                        }
                    }
                }

                for (int y = 0; y <= initialMap.GetUpperBound(0); y++)
                {
                    for (int x = 0; x <= initialMap.GetUpperBound(1); x++)
                    {
                        initialMap[y,x] = initialMap[y,x] >= 9 ? 1 : ++initialMap[y,x];
                    }
                }
            }
                
            return map;
        }

        private ShortestPathResult otherDijkstra(int[,] map)
        {
            var graph = new Graph<int, string>();
            var pointsToNodes = new Dictionary<Point, uint>();
            uint nodeCount = 1;

            for (int y = 0; y <= map.GetUpperBound(0); y++)
            {
                for (int x = 0; x <= map.GetUpperBound(1); x++)
                {

                    graph.AddNode((int)nodeCount);
                    pointsToNodes.Add(new Point(x,y), nodeCount);
                    nodeCount++;
                }
            }

            foreach (var pTn in pointsToNodes)
            {
                var neighbors = InputConverter.get4Neighborhood(map, pTn.Key);

                foreach ( var neighbor in neighbors)
                {
                    graph.Connect(pTn.Value, pointsToNodes[neighbor], map[neighbor.yCoordinate, neighbor.xCoordinate], "fdsf");
                }
            }

            return graph.Dijkstra(pointsToNodes[new Point(0,0)], pointsToNodes[new Point(map.GetUpperBound(0),map.GetUpperBound(1))]);
        }

        private void do1()
        {
            var map = InputConverter.get2dArray(InputConverter.getInput(file));
            
            var cost = DijkstraAlgo(map, new Point(0,0));

            Console.WriteLine("Lowest risk = {0}", cost[cost.GetUpperBound(0), cost.GetUpperBound(1)]);
        }



        private int[,] DijkstraAlgo(int[,] map, Point startingPoint)
        {
            var cost = new int[map.GetLength(0), map.GetLength(1)];
            cost.SetAllValues(int.MaxValue);
            
            var hashSet = new HashSet<(Point point, int cost)>();
            hashSet.Add((startingPoint, 0));
            cost[startingPoint.yCoordinate, startingPoint.xCoordinate] = 0;
            
            while( hashSet.Count != 0)
            {
                hashSet.OrderBy(x => x.Item2);
                var point = hashSet.First();
                hashSet.Remove(point);

                var neighborhood = InputConverter.get4Neighborhood(map, point.point);

                foreach (var neighbor in neighborhood)
                {
                    if( cost[neighbor.yCoordinate, neighbor.xCoordinate] > 
                    cost[point.point.yCoordinate, point.point.xCoordinate] + map[neighbor.yCoordinate, neighbor.xCoordinate])
                    {
                        if(cost[neighbor.yCoordinate, neighbor.xCoordinate] != int.MaxValue)
                        {
                            hashSet.Remove((neighbor, cost[neighbor.yCoordinate, neighbor.xCoordinate]));
                        }

                        cost[neighbor.yCoordinate, neighbor.xCoordinate] = 
                        cost[point.point.yCoordinate, point.point.xCoordinate] + map[neighbor.yCoordinate, neighbor.xCoordinate];

                        hashSet.Add((neighbor, cost[neighbor.yCoordinate, neighbor.xCoordinate]));
                    }
                }
            }

            return cost;
        }
    }
}