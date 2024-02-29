using System;

public interface IShape
{
    int Area();
}

public class Rectangle : IShape
{
    public int Width { get; set; }
    public int Height { get; set; }

    public int Area()
    {
        return Width * Height;
    }
}

public class Square : IShape
{
    public int Side { get; set; }

    public int Area()
    {
        return Side * Side;
    }
}

class Program
{
    static void Main(string[] args)
    {
        IShape rectangle = new Rectangle { Height = 5, Width = 4 };

        Console.WriteLine("Area of rectangle: " + rectangle.Area());

        IShape square = new Square { Side = 5 };

        Console.WriteLine("Area of square: " + square.Area());
    }
}