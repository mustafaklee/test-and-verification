
namespace UnitTest.MyCalculator
{

    public class CalculatorTest2
    {
        //readonly olarak tanımlandığı için sadece metodlarını çağırıp kullanabilirsiniz. Herhangi bir property'sinin değerini (Calculator sınıfında yok ama olsaydı) değiştiremezdiniz.

        private readonly Calculator _calculator = new Calculator();

        [Fact]
        public void Add_TwoNumbers_ReturnsCorrectSum()
        {
            //Arrange
            int n1 = 10;
            int n2 = 15;

            // Act
            int result = _calculator.Add(n1, n2);

            //Assert
            Assert.Equal(n1 + n2, result);
        }
        [Fact]
        public void Subtract_TwoNumbers_ReturnsCorreftDifference()
        {
            //arrange
            int n1 = 10, n2 = 15;

            //Act
            int result = _calculator.Subtract(n1, n2);

            //Assert
            Assert.Equal(n1 - n2, result);
        }

        [Fact]
        public void Divide_DivideByZero_ThrowsDivideByZeroException()
        {
            int n1 = 10, n2 = 0;
            string expectedMessage = "Bir sayıyı sıfıra bölemezsin";
            Exception exc = Assert.Throws<DivideByZeroException>(() => _calculator.Divide(n1, n2));
            Assert.Equal(expectedMessage, exc.Message);
        }

        [Theory] //farklı veri kümesi ile test yürütmeyi sağlar.
        [InlineData(0)] //sıfır
        [InlineData(10)] //pozitif sayi
        [InlineData(-99)] //negatif sayi
        //InlineData'da belirtilen değerler aynı test metoduna a parametresi için sırasıyla yollanır. Böylece tek bir metod fark parametrelerle dinamik olarak çalıştırılır
        public void Abs_Number_ReturnsAbsoluteValue(int a)
        {
            Calculator _calculator = new Calculator();
            int result = _calculator.abs(a);
            Assert.Equal(Math.Abs(a), result);
        }


    }

}
