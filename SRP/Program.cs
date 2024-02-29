public class Order
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string CustomerName { get; set; }
    public decimal TotalAmount { get; set; }
}

public class OrderRepository
{
    public void SaveOrderToDatabase(Order order)
    {
        // Code to save the order details to the database
    }
}

public class EmailService
{
    public void SendOrderConfirmationEmail(Order order)
    {
        // Code to send order confirmation email to the customer
    }
}

public class InvoiceService
{
    public void PrintOrderInvoice(Order order)
    {
        // Code to print the order invoice
    }
}