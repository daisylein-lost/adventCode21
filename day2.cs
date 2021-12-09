namespace adventCode21
{
    public class day2
    {
        private static bool real = true;

        private string file = real ? "day2_input1.txt" : "day2_inputTest.txt";

        public void execute()
        {
            do1();
            do2();
        }

        private void do2()
        {
            Console.WriteLine("Part 2:");
            var input = InputConverter.getInput(file);
            var position = (horizontal: 0, vertical: 0, aim: 0);
            
            foreach (var line in input)
            {
                position = navigate2(line, position);
            }
            Console.WriteLine("Final Position {0}", position);
            Console.WriteLine("Solution {0}", position.horizontal * position.vertical);
        }

        private (int, int, int) navigate2(string commandString,  (int horizontal, int vertical, int aim) position)
        {
            var newPosition = position;

            var value = int.Parse(new String(commandString.Where(s => Char.IsDigit(s)).ToArray()));
            var command = new String(commandString.Where(s =>!Char.IsDigit(s)).ToArray()).Trim();

            switch (command)
            {
                case "down": 
                    newPosition.aim = newPosition.aim + value;
                    break;
                case "up":
                    newPosition.aim = newPosition.aim - value;
                    break;
                case "forward":
                    newPosition.horizontal = newPosition.horizontal + value;
                    newPosition.vertical = newPosition.vertical + (value * newPosition.aim);
                    break;
                default: 
                    Console.WriteLine(command);
                    throw new ArgumentOutOfRangeException();
            }

            return newPosition;
        }

        private void do1()
        {
            Console.WriteLine("Part 1:");
            var input = InputConverter.getInput(file);
            var position = (0, 0);
            
            foreach (var line in input)
            {
                position = navigate1(line, position);
            }
            Console.WriteLine("Final Position {0}", position);
            Console.WriteLine("Solution {0}", position.Item1 * position.Item2);
        }

        private (int, int) navigate1(string commandString, (int horizontal, int vertical) position )
        {
            var newPosition = position;
            var value = int.Parse(new String(commandString.Where(s => Char.IsDigit(s)).ToArray()));
            var command = new String(commandString.Where(s =>!Char.IsDigit(s)).ToArray()).Trim();

            switch (command)
            {
                case "down": 
                    newPosition.vertical = newPosition.vertical + value;
                    break;
                case "up":
                    newPosition.vertical = newPosition.vertical - value;
                    break;
                case "forward":
                    newPosition.horizontal = newPosition.horizontal + value;
                    break;
                case "back":
                    newPosition.horizontal = newPosition.horizontal - value;
                    break;
                default: 
                    Console.WriteLine(command);
                    throw new ArgumentOutOfRangeException();
            }

            return newPosition;
        }

    }
}