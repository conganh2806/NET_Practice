using System;
using System.Runtime.InteropServices;

namespace Excercise2
{
    public interface ICommission
    {
        decimal CalculateCommission();
    }

    public class Staff
    {
        private const decimal BONUS_THRESHOLD = 10000m;
        private const decimal BASE_SALARY_MULTIPLIER = 40m;
        private const decimal ADDITIONAL_SALARY_RATE = 0.01m;
        private const decimal BONUS_AMOUNT = 100m;
        private const decimal PENALTY_AMOUNT = 30m;

        private string _name;
        private decimal _salaryCoefficient;
        private List<Insurance> _soldInsurances = new List<Insurance>();
        public string Name => _name;
        public decimal SalaryCoefficient => _salaryCoefficient;

        public Staff(string name, decimal salaryCoefficient)
        {
            _name = name;
            _salaryCoefficient = salaryCoefficient;

        }

        public void AddInsurance(Insurance insurance)
        { 
            _soldInsurances.Add(insurance);
        }
        
        public decimal CalculateTotalCommission()
        {
            return _soldInsurances.Sum(insurance => (insurance as ICommission)?.CalculateCommission() ?? 0);
        }
        
        public decimal CalculateTotalInsuranceAmount() 
        {
            return _soldInsurances.Sum(insurance => insurance.Amount);
        }

        public bool GetBonus() 
        { 
            return _soldInsurances.Any(insurance => insurance.Amount > BONUS_THRESHOLD);
        }


        public bool GetPenalty()
        { 
            return CalculateTotalInsuranceAmount() < BONUS_THRESHOLD;
        }

        public decimal CalculateSalary() 
        { 
            decimal baseSalary = BASE_SALARY_MULTIPLIER * _salaryCoefficient;
            decimal totalInsuranceAmount = CalculateTotalInsuranceAmount();
            decimal commission = CalculateTotalCommission();
            decimal additionalSalary = ADDITIONAL_SALARY_RATE * (totalInsuranceAmount - commission);
            
            decimal salary = baseSalary + additionalSalary;

            if(GetBonus())
            {
                salary += BONUS_AMOUNT;
            }
            if(GetPenalty())
            {
                salary -= PENALTY_AMOUNT;
            }

            return salary;
        }

        public override string ToString()
        {
            string result = $"Nhân viên: {_name}\n";
            result += $"Hệ số lương: {_salaryCoefficient}\n";
            result += $"Tổng số bảo hiểm: {_soldInsurances.Count}\n";
            result += $"Tổng tiền bảo hiểm: {CalculateTotalInsuranceAmount()}\n";
            result += $"Tổng hoa hồng: {CalculateTotalCommission()}\n";
            
            if (GetBonus())
                result += "Được thưởng: 100 USD\n";
            if (GetPenalty())
                result += "Bị phạt: 30 USD\n";
                
            result += $"Lương: {CalculateSalary()}\n";
            
            if (_soldInsurances.Count > 0)
            {
                result += "Danh sách bảo hiểm đã bán:\n";
                foreach (var insurance in _soldInsurances)
                {
                    result += $"- {insurance}\n";
                }
            }
            
            return result;
        }
    }

    public enum InsuranceType
    {
        SHORT_TERM,
        LONG_TERM
    }

    public abstract class Insurance
    {
        public string BuyerName { get; set; }
        public int Term { get; set; }
        public decimal Amount { get; set; }
        public Insurance(string buyerName, int term, decimal amount)
        {
            BuyerName = buyerName;
            Term = term;
            Amount = amount;
        }
        public override string ToString()
        {
            return $"Người mua: {BuyerName}, Thời hạn: {Term}, Số tiền: {Amount}";
        }
    }

    public class ShortTermInsurance : Insurance, ICommission
    {
        private const decimal COMMISSION_RATE = 0.05m;

        public ShortTermInsurance(string buyerName, int term, decimal amount)
            : base(buyerName, term, amount)
        {}
        
        public decimal CalculateCommission()
        {
            return Amount * COMMISSION_RATE;
        }

        public override string ToString()
        {
            return $"Bảo hiểm ngắn hạn - {base.ToString()} - Hoa hồng: {CalculateCommission()}";
        }
    }

    public class LongTermInsurance : Insurance, ICommission
    {
        public decimal MonthlyPayment { get; set; }
        public LongTermInsurance(string buyerName, int term, decimal amount, decimal monthlyPayment)
            : base(buyerName, term, amount)
        {
            if(term <= 12)
            { 
                throw new ArgumentException("Bảo hiểm phải có thời hạn trên 12 tháng");
            }

            MonthlyPayment = monthlyPayment;
        }

        public decimal CalculateCommission() 
        { 
            return Amount * 0.5m;
        }

        public override string ToString()
        {
            return $"Bảo hiểm dài hạn - {base.ToString()} - Hoa hồng: {CalculateCommission()}";
        }
    }

    public class InsuranceCompany
    {
        public List<Staff> Staffs { get; private set; }
        
        public InsuranceCompany()
        {
            Staffs = new List<Staff>();
        }

        public void AddStaff(Staff staff)
        {
            Staffs.Add(staff);
        }

        public void DisplayAllStaffs() 
        { 
            Console.WriteLine("==========DANH SÁCH TẤT CẢ NHÂN VIÊN==========");
            foreach(var staff in Staffs)
            {
                Console.WriteLine(staff);
                Console.WriteLine("====================================================");
            }
        }

        public List<Staff> GetAllStaffsWithCommissionGreaterThan(decimal amount)
        {
            return Staffs.Where(staff => staff.CalculateTotalCommission() > amount).ToList(); 
        }

        public List<Staff> GetStaffsWithPenalty()
        { 
            return Staffs.Where(staff => staff.GetPenalty() == true).ToList();
        }

        public List<Staff> GetStaffsWithBonus()
        {
            return Staffs.Where(staff => staff.GetBonus()).ToList();
        }

        public void DisplayEmployeesWithHighCommission(decimal amount)
        {
            var staffs = GetAllStaffsWithCommissionGreaterThan(amount);

            Console.WriteLine($"=== NHÂN VIÊN CÓ HOA HỒNG > {amount} ===");
            foreach (var staff in staffs)
            {
                Console.WriteLine($"Nhân viên: {staff.Name}, Hoa hồng: {staff.CalculateTotalCommission():C}");
            }
            Console.WriteLine("--------------------------------");
        }

        public void DisplayAllStaffsWithPenalty() 
        { 
            var staffs = GetStaffsWithPenalty();
            Console.WriteLine($"=== NHÂN VIÊN BỊ PHẠT ===");

            foreach (var staff in staffs)
            {
                Console.WriteLine(staff);
            }
            Console.WriteLine("--------------------------------");
        }

        public void DisplayAllStaffsWithBonus() 
        { 
            var staffs = GetStaffsWithBonus();
            Console.WriteLine($"=== NHÂN VIÊN ĐƯỢC THƯỞNG 100USD ===");

            foreach (var staff in staffs)
            {
                Console.WriteLine(staff);
            }
            Console.WriteLine("--------------------------------");
        }
    }

    public class Excercise2
    {
        public static void Main(string[] args)
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany();

            Staff staff1 = new Staff("Nguyễn Văn A", 3.5m);
            staff1.AddInsurance(new ShortTermInsurance("Trần Thị B", 10, 8000m));
            staff1.AddInsurance(new LongTermInsurance("Lê Văn C", 24, 12000m, 500m));
            insuranceCompany.AddStaff(staff1);

            Staff staff2 = new Staff("Nguyễn Văn B", 3.5m);
            staff2.AddInsurance(new ShortTermInsurance("Hoàng Văn E", 3, 5000m));
            staff2.AddInsurance(new ShortTermInsurance("Ngô Thị F", 9, 3000m));
            insuranceCompany.AddStaff(staff2);
            
            Staff staff3 = new Staff("Vũ Văn G", 5.0m);
            staff3.AddInsurance(new LongTermInsurance("Đặng Thị H", 36, 15000m, 420m));
            staff3.AddInsurance(new ShortTermInsurance("Lê Văn C", 6, 11000m));
            insuranceCompany.AddStaff(staff3);

            Staff staff4 = new Staff("Đoàn Văn H", 2.0m);
            insuranceCompany.AddStaff(staff4);

            insuranceCompany.DisplayAllStaffs();
            insuranceCompany.DisplayEmployeesWithHighCommission(6000);
            insuranceCompany.DisplayAllStaffsWithPenalty();
            insuranceCompany.DisplayAllStaffsWithBonus();

        }
    }
}