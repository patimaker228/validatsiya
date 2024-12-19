using System.Globalization;

namespace валидация
{
    internal class Program
    {

    }
   
public class Deposit
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public decimal Cashback { get; set; }
        public int NonWithdrawableBalance { get; set; }
        public DateTime OpeningDate { get; set; }
        public int TermInDays { get; set; }

        public DateTime ExpirationDate => OpeningDate.AddDays(TermInDays);
    }

    public class DepositManager
    {
        private List<Deposit> deposits = new List<Deposit>();

        public void AddDeposit(Deposit deposit)
        {
            deposits.Add(deposit);
        }

        public void ShowDeposits()
        {
            Console.WriteLine("Вклады:");
            foreach (var deposit in deposits)
            {
                var color = Console.ForegroundColor;
                if (deposit.ExpirationDate < DateTime.Now)
                {
                    Console.ForegroundColor = ConsoleColor.Red; 
                }
                else if (deposit.ExpirationDate < DateTime.Now.AddDays(5))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow; 
                }

                Console.WriteLine($"Название: {deposit.Name}, Сумма: {deposit.Amount}, Процент: {deposit.InterestRate}%, " +
                                  $"Кэшбек: {deposit.Cashback}, Дата открытия: {deposit.OpeningDate.ToString("dd:MM:yyyy")}, " +
                                  $"Срок: {deposit.TermInDays} дней");

                Console.ForegroundColor = color; 
            }
        }

        public void AddFunds(string name, decimal amount)
        {
            var deposit = deposits.Find(d => d.Name == name);
            if (deposit != null)
            {
                deposit.Amount += amount;
                Console.WriteLine($"Добавлено {amount} к вкладу '{name}'. Новая сумма: {deposit.Amount}");
            }
            else
            {
                Console.WriteLine("Вклад не найден.");
            }
        }

        public void WithdrawFunds(string name, decimal amount)
        {
            var deposit = deposits.Find(d => d.Name == name);
            if (deposit != null)
            {
                if (deposit.Amount >= amount)
                {
                    deposit.Amount -= amount;
                    Console.WriteLine($"Снято {amount} с вклада '{name}'. Остаток: {deposit.Amount}");
                }
                else
                {
                    Console.WriteLine("Недостаточно средств на вкладе.");
                }
            }
            else
            {
                Console.WriteLine("Вклад не найден.");
            }
        }

        public void ApplyInterest()
        {
            foreach (var deposit in deposits)
            {
                if (deposit.ExpirationDate > DateTime.Now)
                {
                    deposit.Amount += deposit.Amount * deposit.InterestRate / 100;
                    Console.WriteLine($"Начислен процент по вкладу '{deposit.Name}'. Новая сумма: {deposit.Amount}");
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DepositManager manager = new DepositManager();

           
            manager.AddDeposit(new Deposit
            {
                Name = "Вклад 1",
                Amount = 1000,
                InterestRate = 5,
                Cashback = 50,
                NonWithdrawableBalance = 100,
                OpeningDate = DateTime.ParseExact("01:01:2023", "dd:MM:yyyy", CultureInfo.InvariantCulture),
                TermInDays = 30
            });

            manager.AddDeposit(new Deposit
            {
                Name = "Вклад 2",
                Amount = 2000,
                InterestRate = 3,
                Cashback = 0,
                NonWithdrawableBalance = 0,
                OpeningDate = DateTime.ParseExact("15:02:2023", "dd:MM:yyyy", CultureInfo.InvariantCulture),
                TermInDays = 20
            });

            
            manager.ShowDeposits();

           
            manager.ApplyInterest();

            manager.AddFunds("Вклад 1", 500);

         
            manager.WithdrawFunds("Вклад 2", 100);

            
            manager.ShowDeposits();
        }
    }
}

