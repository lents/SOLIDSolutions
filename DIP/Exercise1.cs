namespace DIP
{
    public interface ILogger
    {
        void Log(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"Logging: {message}");
        }
    }

    public class OrderService
    {
        private readonly ILogger _logger;

        // Dependency is injected through constructor
        public OrderService(ILogger logger)
        {
            _logger = logger;
        }

        public void PlaceOrder(string orderDetails)
        {
            // Business logic to place the order

            _logger.Log("Order placed successfully.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = new ConsoleLogger(); // Concrete implementation
            OrderService orderService = new OrderService(logger); // Dependency injection
            orderService.PlaceOrder("Sample order details");
        }
    }
}
