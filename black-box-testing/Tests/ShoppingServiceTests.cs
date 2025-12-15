using black_box_testing.Context;
using black_box_testing.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace black_box_testing.Tests
{
    public class ShoppingServiceTests
    {
        private readonly ITestOutputHelper _testOutput;
        private readonly ShoppingService shoppingService;
        //her test çalıştığında yeniden oluşturulur...
        public ShoppingServiceTests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
            InitializeShoppingService();
        }

        public void InitializeShoppingService()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
            var appdbcontext = new AppDbContext(options);

            shoppingService = new ShoppingService(appdbcontext);
            seedDatabase();
        }

        public void seedDatabase()
        {
            shoppingService.AddUser("Ahmet Standart Kullanıcı", false);// Id=1 olacak
            shoppingService.AddUser("Mehmet Premium Kullanıcı", true); // Id=2 Olacak
        }



        [Theory]
        [InlineData(1,99.9,false)]
        [InlineData(1,100.00,true)]
        [InlineData(2,100.01,true)]
        public void IsOrderEligibleForDiscount_ShouldReturnCorrectResult(int userId, int totalAmount, bool expectedResult)
        {
            shoppingService.AddOrder(userId, totalAmount);

            int orderId = shoppingService.

        }




    }
}
