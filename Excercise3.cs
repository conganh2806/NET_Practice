using System;

namespace Excercise3
{
    public interface ISalary
    {
        decimal CalculateSalary();
    }

    public abstract class Staff
    {
        private string _name;
        private DateTime _dateOfBirth;
        private string _address;
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set => _dateOfBirth = value;
        }
        public string Address
        {
            get => _address;
            set => _address = value;
        }

        public Staff(string name, DateTime dateOfBirth, string address)
        {
            _name = name;
            _dateOfBirth = dateOfBirth;
            _address = address;
        }

        public override string ToString()
        {
            return $"Name: {_name}\nDate of birth: {_dateOfBirth}\nAddress: {_address}";
        }
    }

    public class ProductionStaff : Staff, ISalary
    {
        private const decimal PRODUCTION_RATE = 20000;
        private int _productCount;
        public int ProductCount
        {
            get => _productCount;
            set => _productCount = value;
        }

        public ProductionStaff(string name, DateTime dateOfBirth, string address, int productCount)
            :
            base(name, dateOfBirth, address)
        {
            _productCount = productCount;
        }

        public decimal CalculateSalary()
        {
            return _productCount * PRODUCTION_RATE;
        }
        public override string ToString()
        {
            return $"{base.ToString()}\nStaff type: Production staff\nProduct count: {_productCount}";
        }
    }

    public class DailyWorker : Staff, ISalary
    {
        private int _workDays;
        private const decimal DAILY_RATE = 50000;
        public int WorkDays
        {
            get => _workDays;
            set => _workDays = value;
        }
        public DailyWorker(string name, DateTime dateOfBirth, string address, int workDays)
            :
            base(name, dateOfBirth, address)
        {
            _workDays = workDays;
        }
        public decimal CalculateSalary()
        {
            return _workDays * DAILY_RATE;
        }
        public override string ToString()
        {
            return $"{base.ToString()}\nStaff type: Daily worker\nWork day: {_workDays}";
        }
    }

    public class Manager : Staff, ISalary
    {
        private decimal _baseSalary;
        private decimal _salaryCoefficient;

        public Manager(string name, DateTime dateOfBirth, string address, decimal
                        baseSalary, decimal salaryCoefficient)
            :
            base(name, dateOfBirth, address)
        {
            _baseSalary = baseSalary;
            _salaryCoefficient = salaryCoefficient;
        }

        public decimal CalculateSalary()
        {
            return _baseSalary * _salaryCoefficient;
        }

        public override string ToString()
        {
            return $@"
                    {base.ToString()}
                    Staff type: Manager
                    Base Salary: {_baseSalary}
                    Salary Coefficient: {_salaryCoefficient}
                    ";
        }
    }


    public class CompanyABC
    {
        private List<Staff> _staffs;

        public CompanyABC()
        {
            _staffs = new List<Staff>();
        }

        public void Add(Staff staff)
        {
            _staffs.Add(staff);
        }

        public void Add(List<Staff> staffs)
        { 
            staffs.ForEach(_staffs.Add);
        }

        public Staff FindHighestPaidStaff() 
        { 
            if(_staffs.Count == 0)
            { 
                return null;
            }

            return _staffs.OrderByDescending(
                s => (s as ISalary)?.CalculateSalary() ?? 0
            ).First();
        }

        public Staff FindLowestPaidStaff() 
        { 
            if(_staffs.Count == 0)
            { 
                return null;
            }

            return _staffs.OrderBy(
                s => (s as ISalary)?.CalculateSalary() ?? 0
            ).First();
        }
    }

    public class StaffManagementSystem
    {
        private CompanyABC _companyABC;
        public StaffManagementSystem()
        {
            _companyABC = new CompanyABC();
        }

        public void RunAddStaff()
        {
            Console.WriteLine("Enter staff type (1-Production, 2-Daily Worker, 3-Manager): ");
            if (!int.TryParse(Console.ReadLine(), out int type) || type < 1 || type > 3)
            {
                Console.WriteLine("Invalid type!");
                return;
            }

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Date of Birth (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateOfBirth))
            {
                Console.WriteLine("Invalid date!");
                return;
            }

            Console.Write("Enter Address: ");
            string address = Console.ReadLine();

            Staff newStaff = null;

            switch (type)
            {
                case 1: // Production Staff
                    Console.Write("Enter Product Count: ");
                    if (int.TryParse(Console.ReadLine(), out int productCount))
                        newStaff = new ProductionStaff(name, dateOfBirth, address, productCount);
                    break;

                case 2: // Daily Worker
                    Console.Write("Enter Work Days: ");
                    if (int.TryParse(Console.ReadLine(), out int workDays))
                        newStaff = new DailyWorker(name, dateOfBirth, address, workDays);
                    break;

                case 3: // Manager
                    Console.Write("Enter Base Salary: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal baseSalary))
                    {
                        Console.WriteLine("Invalid input!");
                        return;
                    }

                    Console.Write("Enter Salary Coefficient: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal coefficient))
                    {
                        Console.WriteLine("Invalid input!");
                        return;
                    }

                    newStaff = new Manager(name, dateOfBirth, address, baseSalary, coefficient);
                    break;
            }

            if (newStaff != null)
            {
                _companyABC.Add(newStaff);
                Console.WriteLine("Staff added successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add staff!");
            }
        }

        public void RunMainMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n========ABC COMPANY STAFF MANAGEMENT SYSTEM========");
                Console.WriteLine("1. Add a new staff member");
                Console.WriteLine("2. Add multiple staff members");
                Console.WriteLine("3. Display all staff");
                Console.WriteLine("4. Calculate total salary");
                Console.WriteLine("5. Find highest paid staff");
                Console.WriteLine("6. Find lowest paid staff");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input .. Please try again");
                    continue;
                }

                switch (choice)
                {
                    case 0:
                        exit = true;
                        Console.WriteLine("Exiting Program. Goodbye!");
                        break;
                    case 1:
                        RunAddStaff();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            StaffManagementSystem system = new StaffManagementSystem();
            system.RunMainMenu();

        }
    }
}