using System;

public interface IShape
{
    double Area();
}

public class Circle : IShape
{
    public double Radius { get; set; }

    public double Area()
    {
        return Math.PI * Radius * Radius;
    }
}

public class Square : IShape
{
    public double Side { get; set; }

    public double Area()
    {
        return Side * Side;
    }
}

class Program
{
    static void Main(string[] args)
    {
        IShape circle = new Circle { Radius = 5 };
        IShape square = new Square { Side = 4 };

        Console.WriteLine("Area of circle: " + circle.Area());
        Console.WriteLine("Area of square: " + square.Area());
    }
}