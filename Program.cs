// See https://aka.ms/new-console-template for more information



namespace adventCode21
{
    class Program
    {
        static void Main(string[] args)
        {
          var day = new day5();
          day.execute();
        }
    }

    public class Point
    {
      public int xCoordinate;

      public int yCoordinate;

      public Point (int x, int y)
      {
        xCoordinate = x;
        yCoordinate = y;
      }
      public Point (string point)
      {
        var points = point.Trim().Split(',');
        xCoordinate = int.Parse(points[0]);
        yCoordinate = int.Parse(points[1]);
      }

      public override string ToString()
      {
        return $"({xCoordinate},{yCoordinate})";
      }
    }
}