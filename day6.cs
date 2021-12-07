namespace adventCode21
{
    public class day6
    {
        private static List<int> real = new List<int>() {1,1,1,1,1,5,1,1,1,5,1,1,3,1,5,1,4,1,5,1,2,5,1,1,1,1,3,1,4,5,1,1,2,1,1,1,2,4,3,2,1,1,2,1,5,4,4,1,4,1,1,1,4,1,3,1,1,1,2,1,1,1,1,1,1,1,5,4,4,2,4,5,2,1,5,3,1,3,3,1,1,5,4,1,1,3,5,1,1,1,4,4,2,4,1,1,4,1,1,2,1,1,1,2,1,5,2,5,1,1,1,4,1,2,1,1,1,2,2,1,3,1,4,4,1,1,3,1,4,1,1,1,2,5,5,1,4,1,4,4,1,4,1,2,4,1,1,4,1,3,4,4,1,1,5,3,1,1,5,1,3,4,2,1,3,1,3,1,1,1,1,1,1,1,1,1,4,5,1,1,1,1,3,1,1,5,1,1,4,1,1,3,1,1,5,2,1,4,4,1,4,1,2,1,1,1,1,2,1,4,1,1,2,5,1,4,4,1,1,1,4,1,1,1,5,3,1,4,1,4,1,1,3,5,3,5,5,5,1,5,1,1,1,1,1,1,1,1,2,3,3,3,3,4,2,1,1,4,5,3,1,1,5,5,1,1,2,1,4,1,3,5,1,1,1,5,2,2,1,4,2,1,1,4,1,3,1,1,1,3,1,5,1,5,1,1,4,1,2,1};
        private static List<int> test = new List<int>() {3,4,3,1,2};

        private List<int> lanternfish = real;
        public void execute()
        {
            do1();
            do2();
        }

        private void do2()
        {
            Console.WriteLine("Part 2:");

            var count = populationAfterDays2(256);

            Console.WriteLine("Amount of lanternfish: {0}", count);
        }

        private void do1()
        {
            Console.WriteLine("Part 1:");

            var count = populationAfterDays(80);

            Console.WriteLine("Amount of lanternfish: {0}", count);

        }

        private long populationAfterDays2(int days)
        {
            var fishCounts = new Dictionary<int, long>()
            {
                {0, 0},
                {1, 0},
                {2, 0},
                {3, 0},
                {4, 0},
                {5, 0},
                {6, 0},
                {7, 0},
                {8, 0}
            };
            lanternfish.ForEach(x => fishCounts[x]++);

            for (int i = 1; i <= days; i++)
            {
                var newFish = fishCounts[0];
                
                for (int l = 0; l < 8; l++)
                {
                    fishCounts[l] = fishCounts[l+1];
                }
                
                fishCounts[8] = newFish;
                fishCounts[6] = fishCounts[6] + newFish;
            }

            return fishCounts.Sum(x => x.Value);
        }

        private int populationAfterDays(int days)
        {
            var fishList = lanternfish;

            for (int i = 1; i <= days; i++)
            {
                var due = fishList.Count( x => x == 0);
                var newFish = Enumerable.Repeat(8, due).ToList();
                fishList = fishList.Select(x => (x == 0 ? 6 : x - 1)).ToList();
                fishList.AddRange(newFish);

            }

            return fishList.Count;
        }
    }
}