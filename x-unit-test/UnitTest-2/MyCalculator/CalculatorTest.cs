using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest;

namespace UnitTest_1.MyCalculator
{
    [Trait("ModuleName", "CalculatorTest1")]
    public class CalculatorTest
    {
        [Fact]
        public void Add_TwoNumbers_ReturnsCorrectSum()
        {
            //arrange 
            Calculator _calculator = new Calculator();
            int s1 = 10;
            int s2 = 20;

            //act 
            int result = _calculator.Add(s1, s2);

            //assert
            Assert.Equal(s1 + s2, result);

        }


        [Fact]
        public void Subtract_TwoNumbers_ReturnsCorrectDifference()
        {
            //arrange
            Calculator _calculator = new Calculator();
            int s1=10;
            int s2 = 24;
            //act

            int result = _calculator.Subtract(s1, s2);

            //assert

            Assert.Equal(s1 - s2, result);
        }

        [Fact]
        public void Divide_DivideByZero_ThrowsDivideByZeroException()
        {
            //arrange
            Calculator _calculator = new Calculator();
            int s1 = 10;
            int s2 = 0;
            //Act
            string expectedMessage = "Bir sayıyı sıfıra bölemezsin";
            //assert
            Assert.Throws<DivideByZeroException>(() => _calculator.Divide(s1, s2));
        }

        //isimler aynı diye değiştirdim yoksa değişmez.
        [Fact]
        public void Divide_DivideByZero__ThrowsDivideByZeroException()
        {
            //arrange
            Calculator _calculator = new Calculator();
            int n1 = 10, n2 = 0;
            //Act 
            string expectedMessage = "Bir sayıyı sıfıra bölemezsin";
            //arrange
            Exception exc = Assert.Throws<DivideByZeroException>(() => _calculator.Divide(n1, n2));
            Assert.Equal(expectedMessage, exc.Message);
        }

    }
}
