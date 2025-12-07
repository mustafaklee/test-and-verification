
namespace UnitTest_1.MBankAccount
{
    public class BankAccount
    {
        public string AccountHolder { get; }
        public decimal Balance { get; set; }
        public bool IsClosed { get;private set; }

        public BankAccount(string accountHolder, decimal balance )
        {
            if (string.IsNullOrEmpty(accountHolder))
            {
                throw new ArgumentException("Hesap sahibinin adı boş olamaz.");
            }
            if (balance < 0)
            {
                throw new ArgumentException("Başlangıc hesap bakiyesi negatif olamaz");
            }

            AccountHolder = accountHolder;
            Balance = balance;
            IsClosed = false;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Yatırılacak Miktar Pozitif Bir Değer Olmalıdır");

            if (IsClosed) throw new InvalidOperationException("Kapalı Bir Hesaba Para Yatıramazsın");

            Balance += amount;
        }


        public void WithDraw(decimal amount)
        {

            if (amount <= 0) throw new ArgumentException("Çekilecek Para Miktarı Pozitif Olmalıdır");

            if (IsClosed) throw new InvalidOperationException("Kapalı Bir Hesaptan Para Çekilemez");

            if (amount > Balance)
                throw new InvalidOperationException("Çekilecek Para Miktarı Hesaptaki Paradan Daha Fazla Olamaz");
            Balance -= amount;
        }


        public void CloseAccount()
        {
            if (IsClosed) throw new InvalidOperationException("Kapalı Bir Hesabı Yine Kapatamazsın");

            IsClosed = true;
        }



    }
}
