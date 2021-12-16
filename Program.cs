// See https://aka.ms/new-console-template for more information



namespace adventCode21
{
    class Program
    {
        static void Main(string[] args)
        {
          var day = new day16();
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

      public override bool Equals(object obj)
      {
        if (obj == null)
        {
            return false;
        }
        if (!(obj is Point))
        {
            return false;
        }
        return (this.xCoordinate == ((Point)obj).xCoordinate)
            && (this.yCoordinate == ((Point)obj).yCoordinate);
      }

      public override int GetHashCode()
      {
          return xCoordinate.GetHashCode() ^ yCoordinate.GetHashCode();
      }
    }
}