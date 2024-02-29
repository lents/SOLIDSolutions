class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 1) return;

        int userInput;
        if (!int.TryParse(args[0], out userInput)) return;

        DetermineNumberType(userInput);

        DisplayIndexType(userInput);

        if (args.Length > 1)
        {
            string secondInput = args[1];
            DisplayReversedString(secondInput);
        }
    }

    static void DetermineNumberType(int number)
    {
        if (number % 2 == 0)
        {
            Console.WriteLine($"Input number {number} is even.");
        }
        else
        {
            Console.WriteLine($"Input number {number} is odd.");
        }
    }

    static void DisplayIndexType(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (i % 2 == 0)
            {
                Console.WriteLine("Index is even.");
            }
            else
            {
                Console.WriteLine("Index is odd.");
            }
        }
    }

    static void DisplayReversedString(string input)
    {
        Console.Write("Reversed second input parameter: ");
        for (int i = input.Length - 1; i >= 0; i--)
        {
            Console.Write(input[i]);
        }
        Console.WriteLine();
    }
}