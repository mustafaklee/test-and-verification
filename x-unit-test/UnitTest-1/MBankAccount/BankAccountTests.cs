using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

namespace UnitTest_1.MBankAccount
{
    [Trait("ModuleName","BankAccount")]
    public class BankAccountTests
    {

        BankAccount bankAccount;
        ITestOutputHelper testOutput;
        public BankAccountTests(ITestOutputHelper _output)
        {
            //Alttaki her bir unit test metodu çalışmadan öne bu constructor çalışır
            bankAccount = new BankAccount("Mehmet Yılmaz", 500);
            testOutput = _output;
            _output.WriteLine("BankAccountTests Constructor Çalıştı ve Yeni Bir BankAccount Nesnesi Oluştu");

        }

        [Trait("Category","CreateAccount")]
        [Trait("Exception","False")]
        [Fact]
        public void CreateAccount_WithValidInitalBalance_SetsCorrectBalance()
        {
            //arrange
            string name = "Mehmet Yılmaz";
            decimal balance = 500;

            // act & assert
            Assert.Equal(name, bankAccount.AccountHolder);
            Assert.Equal(balance, bankAccount.Balance);
            Assert.False(bankAccount.IsClosed);
            Assert.True(bankAccount.Balance > 0);
        }



        [Trait("Category", "Deposit")]
        [Trait("Exception", "False")]
        [Fact]
        public void Deposit_WithValidAmount_IncreaseBalance()
        {
            //arrange 
            decimal increaseAmount = 500;
            decimal expectedResult = bankAccount.Balance + increaseAmount;
            //act
            bankAccount.Deposit(increaseAmount);
            //assert
            Assert.Equal(expectedResult, bankAccount.Balance);
        }


        [Trait("Category", "WithDraw")]
        [Trait("Exception", "False")]
        [Fact]
        public void WithDraw_WithSufficientBalance_DecreasesBalance()
        {
            //arrange
            decimal decreaseAmount = 100;
            decimal expectedResult = bankAccount.Balance - decreaseAmount;

            //act
            bankAccount.WithDraw(decreaseAmount);

            //assert
            Assert.Equal(expectedResult, bankAccount.Balance);
        }


        [Trait("Category", "Deposit")]
        [Trait("Exception", "True")]
        [Fact]
        public void Deposit_NegativeAmount_ThrowsArgumentException()
        {
            //arrange
            decimal increaseAmount = -200;
            decimal expectedResult = bankAccount.Balance + increaseAmount;
            //act & assert 
            Assert.Throws<ArgumentException>(() => bankAccount.Deposit(increaseAmount));
        }
        [Trait("Category", "Withdraw")]
        [Trait("Exception", "True")]
        [Fact]
        public void Withdraw_InsufficientFunds_ThrowsInvalidOperationException()
        {
            //arrange
            decimal decreaseAmount = 700;
            decimal expectedResult = bankAccount.Balance - decreaseAmount;

            //act & arrange
            Assert.Throws<InvalidOperationException>(() => bankAccount.WithDraw(decreaseAmount));
        }

        [Fact]
        public void Deposit_IntoClosedAccount_ThrowsInvalidOperationException()
        {
            //Arrange
            decimal amount = 100;
            bankAccount.CloseAccount(); // hesabı kapattık
            string expectedMessage = "Kapalı Bir Hesaba Para Yatıramazsın";

            var exc = Assert.Throws<InvalidOperationException>(() => bankAccount.Deposit(amount));
            Assert.Equal(expectedMessage, exc.Message);

        }

        [Fact]
        public void CloseAccount_AlreadyClosed_ThrowsInvalidOperationException()
        {
            //Zaten Kapalı olan bir hesabı kapatmaya çalışacağız. 
            bankAccount.CloseAccount();//Hesabı kapattık
            string expectedMessage = "Kapalı Bir Hesabı Yine Kapatamazsın";


            var exc = Assert.Throws<InvalidOperationException>(() => bankAccount.CloseAccount());
            Assert.Equal(expectedMessage, exc.Message);

        }


        public static IEnumerable<object[]> AmountData => new List<object[]>
        {
            new object[]{100.0,20.0,80.0},
            new object[]{200.0,50.0,150.0},
            new object[]{300.0,300.0,0.0}
        };

        [Trait("Category", "WithDraw")]
        [Trait("Exception", "False")]
        [Theory]
        [MemberData(nameof(AmountData))]
        public void WithDraw_MemberData_DecreasesBalance(decimal initialBalance, decimal amount, decimal expectedBalance)
        {
            BankAccount _acc = new BankAccount("Ahmet Yılmaz",initialBalance);

            _acc.WithDraw(amount);

            Assert.Equal(expectedBalance, _acc.Balance);
        }




        [Trait("Category", "Deposit")]
        [Trait("Exception", "False")]
        [Theory]
        [MemberData(nameof(AmountData))]
        public void Deposit_MemberData_IncreasesBalance(decimal initialBalance, decimal amount, decimal _)
        {
            BankAccount _acc = new BankAccount("Ahmet Yılmaz", initialBalance);
            decimal expectedBalance = _acc.Balance + amount;
            _acc.Deposit(amount);

            Assert.Equal(expectedBalance, _acc.Balance);

        }


    }
}
