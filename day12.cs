namespace adventCode21
{
    public class day12
    {
        private static bool real = true;
        private string file = real ? "day12_input.txt" : "day12_inputTest.txt";
        public void execute()
        {
            do1();
            do2();
        }

        private void do1()
        {
            var input = InputConverter.getInput(file);
            var caveList = getCaveList(input);

            var startCave = caveList.Where(c => c.Name.Equals("start")).First();

            var paths = new List<List<Cave>>(){ new List<Cave>{ startCave}};

            paths = findPaths(paths, CanVisitPart1);

            Console.WriteLine("Total Count of paths: {0}", paths.Count());

        }

        private void do2()
        {
            var input = InputConverter.getInput(file);
            var caveList = getCaveList(input);

            var startCave = caveList.Where(c => c.Name.Equals("start")).First();

            var paths = new List<List<Cave>>(){ new List<Cave>{ startCave}};

            paths = findPaths(paths, CanVisitPart2);

            Console.WriteLine("Total Count of paths: {0}", paths.Count());
        }

        private List<List<Cave>> findPaths(List<List<Cave>> paths, Func<List<Cave>, Cave, bool> CanVisit)
        {
            var grownPaths = new List<List<Cave>> ();

            foreach (var path in paths)
            {
                var lastCave = path.Last();

                if(!lastCave.Name.Equals("end"))
                {
                    foreach (var nextConnection in lastCave.ConnectedCaves)
                    {
                        if(CanVisit(path, nextConnection))
                        {
                        var newPath = path.Select( c=> c).ToList(); //deep copy
                        newPath.Add(nextConnection);
                        grownPaths.Add(newPath);
                        }
                    }
                }
                else
                {
                    grownPaths.Add(path);
                }
            }

            if(grownPaths.Select(l => l.Last()).All(c => c.Name.Equals("end")))
            {
                return grownPaths;
            }

            return findPaths(grownPaths, CanVisit);
        }

        private bool CanVisitPart2(List<Cave> path, Cave nextConnection)
        {   
            if( nextConnection.Name.Equals("end")) return true;

            if( nextConnection.Name.Equals("start")) return false;

            if (nextConnection.IsLarge) return true;

            if (!path.Contains(nextConnection)) return true;

            var duplicates = path.Where(s => !s.IsLarge).GroupBy(c => c.Name).Where(g => g.Count() > 1).Select(m => m.Key).ToList();

            if(duplicates.Count() == 0) 
            {
                return true;
            }

            return false;
        }

        private bool CanVisitPart1(List<Cave> path, Cave nextConnection)
        {   
            return nextConnection.IsLarge || !path.Any(s => s.Name.Equals(nextConnection.Name));
        }

        private List<Cave> addAllConnections(List<Cave> caveList, string[] input)
        {
            var splitInput = input.Select(l => l.Split('-')).ToList();
            foreach (var cave in caveList)
            {
                var connections = splitInput.Where(c => c[0].Equals(cave.Name)).Select(s => s[1]).ToList();
                var newBackwardConnections = splitInput.Where(c => c[1].Equals(cave.Name)).Select(s => s[0]).ToList();
                connections.AddRange(newBackwardConnections);
                
                caveList.Where(c => connections.Contains(c.Name)).ToList().ForEach(c => cave.AddConnection(c));
            }

            return caveList;
        }

        private List<Cave> getCaveList(string[] input)
        {
            var firstCaveNames = input.Select(s => s.Split('-')[0]).ToList();
            var secondCavesNames = input.Select(s => s.Split('-')[1]);
            firstCaveNames.AddRange(secondCavesNames);
            var caveNames = firstCaveNames.Distinct().ToList();

            var caves = new List<Cave>();

            foreach (var cave in caveNames)
            {
                caves.Add(new Cave(cave));
            }

            caves = addAllConnections(caves, input);

            return caves;

        }

        private class Cave
        {
            public string Name;

            public List<Cave> ConnectedCaves;

            public bool IsLarge;

            public Cave(string name)
            {
                Name = name;
                ConnectedCaves = new List<Cave>();
                IsLarge = name.All(Char.IsUpper);
            }

            public void AddConnection(Cave connection)
            {
                ConnectedCaves.Add(connection);
                ConnectedCaves.Distinct();
            }

            public override string ToString()
            {
                return Name;
            }

            
            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                if (!(obj is Cave))
                {
                    return false;
                }
                return (this.Name == ((Cave)obj).Name);
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }
        }
    }

}