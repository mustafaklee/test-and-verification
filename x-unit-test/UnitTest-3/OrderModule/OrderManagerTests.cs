using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest_3.OrderModule
{
    public class OrderManagerTests
    {
        //İçindeki hangi metodun hangi değerlerle çalışacağını, hangi metodun nasıl bir değer döndürmesi gerektiğini iddia edeceğimiz veya belirtebileceğimiz servisin Mocklanmasını istiyoruz. Bu yüzden 
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly OrderManager _orderManager; //OrderManager sınıfındaki ProcessOrder metodunun farklı senaryolar için test edeceğiz.


        public OrderManagerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _orderManager = new OrderManager(_orderServiceMock.Object);
        }

        //Amaç: Bu testte amacımız, Kullanıcının bir order nesnesiyle success parametresi true olacak şekilde çağrılmasını/bildirim gönderilmesini test ediyoruz.
        [Fact]
        public async Task ProcessOrder_ValidOrder_SuccessfulSend_NotifyUserCalledWithTrue()
        {
            //arrange
            var order = new Order { Id = 1, ProductName = "Laptop", Quantity = 2 };

            _orderServiceMock.Setup(x => x.ValidateOrder(order)).Returns(true); // alttaki  await _orderManager.ProcessOrder(order); satır çalıştığında ProcessOrder metodunda eğer ValidateOrder çalışırsa true döndürdüğünü varsayıyoruz.
            _orderServiceMock.Setup(x => x.SendOrderAsync(order)).ReturnsAsync(true);


            //Act
            await _orderManager.ProcessOrder(order);


            //Assert
            //Aşağıdaki ASsertion'ın anlamı şudur: yukardkai _orderManager.ProcessOrder metodu çalıştığında o metodda NotifyUser(order,true) şeklinde 1 Defa(Times.Once) çağrılacaktır anlamına geliyor.
            _orderServiceMock.Verify(X => X.NotifyUser(order, true), Times.Once);
            _orderServiceMock.Verify(X => X.NotifyUser(It.Is<Order>(o => o.Id == order.Id), true), Times.Once); // NotifyUser metodu belirtilen order.ID değeriyle 1 kere çağrılacaktır.
        }


        //Amaç:Bu testte invalid(geçersiz) bir order(sipariş) kaydı geldiğinde SendOrderAsync() ve NotifyUser() metodlarının çağrılmamasını test etmektedir.
        [Fact]
        public async Task ProcessOrder_InvalidOrder_SendOrderNotCalled()
        {
            //arrange
            var order = new Order { Id = 1, ProductName = "Laptop", Quantity = 0 };
            _orderServiceMock.Setup(x => x.ValidateOrder(order)).Returns(false);

            //act
            await _orderManager.ProcessOrder(order);

            //assert
            _orderServiceMock.Verify(x => x.SendOrderAsync(It.IsAny<Order>()), Times.Never);//It.IsAny<Order>()=> Order tipinden ama içeriğinin bir önemi olmadığını belirtir. Zaten Times.Never dediğimiz için yani hiç çağrılmayacağını düşündüğümüzden dolayı Order için Any(herhangi bir) ifadesini koyuyoruz
            _orderServiceMock.Verify(x => x.NotifyUser(It.IsAny<Order>(), It.IsAny<bool>()), Times.Never);

        }

        //Amaç: Geçerli bir sipariş verisi gelmiştir. Sipariş kaydını oluştururken SendOrderAsync metodu fail olarak çalışmıştır. Bu yüzden Kullanıcıya bildirimin sipariş failure olacak şekilde bildirimin gitmiş olduğunu test ederiz
        [Fact]
        public async Task ProcessOrder_ValidOrder_FailureInSend_NotifyUserCalledWithFalse()
        {
            //arrange
            var order = new Order { Id = 1, ProductName = "Laptop", Quantity = 1 };
            _orderServiceMock.Setup(x => x.ValidateOrder(order)).Returns(true); //ValidateOrder metodu aracılığıyla 
            _orderServiceMock.Setup(x => x.SendOrderAsync(order)).ReturnsAsync(false);  //SendOrderAsync metodu bir sebeple fail oldu varsayıyoruz.

            //act
            await _orderManager.ProcessOrder(order);

            //assert
            //NotifyUser belirtilen Order parametresiyle çağrılacak ve ikinci parametre olarak ona false gönderilmiş olacak.
            _orderServiceMock.Verify(x => x.NotifyUser(It.Is<Order>(o => o.Id == order.Id), false), Times.Once); //NotifyUser() metodunun ilgili order ile 2. parametre olarak (success) false parametreleriyle birlikte çalışacağını test ediyoruz.

            //yukardaki kodda It.Is yapısını NotifyUserdan çıkarmak istiyorsak aşağıdaki gibi daha sade halde yapabiliriz.
            _orderServiceMock.Verify(x => x.NotifyUser(order, false), Times.Once());
        }
    }
}
