namespace SRP
{
    using System;

    // Class responsible for employee data only
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }

        // Other properties related to employee data can be added here
    }

    // Class responsible for calculating hours worked
    public class TimeSheet
    {
        public decimal CalculateHoursWorked(Employee employee, DateTime startDate, DateTime endDate)
        {
            // Code to calculate hours worked based on the employee's time records
            // This class should only deal with calculating hours worked, not salary
            return 40; // Placeholder value
        }
    }

    // Class responsible for calculating salary
    public class SalaryCalculator
    {
        private const decimal HourlyRate = 10; // Example hourly rate

        public decimal CalculateSalary(Employee employee, decimal hoursWorked)
        {
            // Code to calculate salary based on hours worked and other factors
            // This class should only deal with calculating salary, not hours worked
            return hoursWorked * HourlyRate; // Placeholder value
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Employee employee = new Employee
            {
                EmployeeId = 1,
                Name = "John Doe",
                Position = "Software Developer"
                // Other employee data can be initialized here
            };

            TimeSheet timeSheet = new TimeSheet();
            SalaryCalculator salaryCalculator = new SalaryCalculator();

            DateTime startDate = DateTime.Today.AddDays(-7); // Example start date
            DateTime endDate = DateTime.Today; // Example end date

            // Calculate hours worked
            decimal hoursWorked = timeSheet.CalculateHoursWorked(employee, startDate, endDate);

            // Calculate salary based on hours worked
            decimal salary = salaryCalculator.CalculateSalary(employee, hoursWorked);

            Console.WriteLine($"Employee {employee.Name} worked {hoursWorked} hours and earned ${salary}.");
        }
    }
}
