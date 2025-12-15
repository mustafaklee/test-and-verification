using black_box_testing.Context;
using black_box_testing.Models;
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

        //Denklik Sınıflarına Ayırma Yöntemine Ait Unit Test

        private readonly ITestOutputHelper output;
        private ShoppingService shoppingService;
        public ShoppingServiceTests(ITestOutputHelper output)
        {
            this.output = output;
            InitializeShoppingService();
        }


        private void InitializeShoppingService()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                        .Options;
            var appDbContext = new AppDbContext(options);

            shoppingService = new ShoppingService(appDbContext);

            seedDatabase(); //shoppingService'in veritabanına 2 tane kullanıcı ekleyecek.

        }

        public void seedDatabase()
        {
            shoppingService.AddUser("Ahmet Standart Kullanıcı", false);// Id=1 olacak
            shoppingService.AddUser("Mehmet Premium Kullanıcı", true); // Id=2 Olacak.
        }


        [Theory]
        [InlineData(1, 50, false)]  // Standart kullanıcı, düşük totalAmount
        [InlineData(1, 150, true)] // Standart kullanıcı, yüksek totalAmount
        [InlineData(2, 50, true)]  // Premium kullanıcı, düşük totalAmount
        public void IsOrderEligibleForDiscount_ShouldReturnCorrectResult(int userId, decimal totalAmount, bool expected)
        {

            shoppingService.AddOrder(userId, totalAmount);

            int orderId = shoppingService._context.Orders.Last().Id;

            bool result = shoppingService.IsOrderEligibleForDiscount(orderId);

            Assert.Equal(expected, result);
        }


        // Sınır Değer Analizi Yöntemine Ait Unit Test 
        [Theory]
        [InlineData(1, 99.99, false)]
        [InlineData(1, 100.00, true)]
        [InlineData(2, 99, true)]
        public void IsOrderEligibleForDiscount_ShouldReturnCorrectResultAtBoundaries(int userId, decimal total, bool expected)
        {
            shoppingService.AddOrder(userId, total);

            var orderId = shoppingService._context.Orders.Last().Id;

            var result = shoppingService.IsOrderEligibleForDiscount(orderId);
            Assert.Equal(expected, result);
        }

        //Karar Tablosu Yöntemine Ait Unit Test

        [Theory]
        [InlineData(false, 50, true)] //standart kullanıcı düşük toplam
        [InlineData(false, 150, false)] //standart kullanıcı yüksek toplam
        [InlineData(true, 110, true)] //premium kullanıcı yüksek toplam
        [InlineData(true, 90, true)] //premium kullanıcı düşük toplam

        public void IsOrderEligibleForDiscount_ShouldMatchDecisionTable(bool isPremiumUser, decimal totalAmount, bool expected)
        {
            User? user = shoppingService._context.Users.FirstOrDefault(u => u.IsPremium == isPremiumUser);

            if(user == null) { throw new NullReferenceException("User not found");}

            shoppingService.AddOrder(user.Id, totalAmount);
            int orderId = shoppingService._context.Orders.Last().Id;
            bool result = shoppingService.IsOrderEligibleForDiscount(orderId);

            Assert.Equal(expected, result);
        }


        // Durum Geçiş Testi Yöntemine Ait Unit Test
        [Fact]
        public void TransitionOrderStatus_ShouldHandleValidAndInvalidTransitions()
        {
            User user = shoppingService._context.Users.First();
            shoppingService.AddOrder(user.Id, 100);

            var orderId = shoppingService._context.Orders.Last().Id;

            bool r1 = shoppingService.TransitionOrderStatus(orderId, OrderStatus.Created);
            bool r2 = shoppingService.TransitionOrderStatus(orderId, OrderStatus.Completed);
            Assert.False(r1);
            Assert.False(r2);

            bool r3 = shoppingService.TransitionOrderStatus(orderId, OrderStatus.Processing);
            bool r4 = shoppingService.TransitionOrderStatus(orderId, OrderStatus.Completed);
            Assert.True(r3);
            Assert.True(r4);


            bool r5 = shoppingService.TransitionOrderStatus(orderId, OrderStatus.Created);
            bool r6 = shoppingService.TransitionOrderStatus(orderId, OrderStatus.Processing);
            Assert.False(r5);
            Assert.False(r6);
        }

    }
}
