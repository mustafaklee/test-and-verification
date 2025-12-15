using black_box_testing.Context;
using black_box_testing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace black_box_testing.Services
{
    public class ShoppingService
    {
        public readonly AppDbContext _context;
        public ShoppingService(AppDbContext context)
        {
            _context = context;
        }

        public void AddUser(string name, bool isPremium)
        {
            User u1 = new User { IsPremium = isPremium, Name = name };

            _context.Users.Add(u1);
            _context.SaveChanges();
        }


        public void AddOrder(int UserId, decimal total)
        {
            User u1 = _context.Users.Find(UserId);
            if(u1 == null)
            {
                throw new ArgumentException("User not found");
            }
            Order order = new Order()
            {
                Status = OrderStatus.Created,
                Total = total,
                UserId = UserId
            };

            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        //Bir siparişe indirim uygulanabilmesi için ya kullanıcı premium olmalı veya sipariş tutarı 100'ün üzerinde olmalı
        public bool IsOrderEligibleForDiscount(int orderId)
        {
            Order order = _context.Orders.Find(orderId);
            if(order == null)
            {
                throw new Exception("Order not found");
            }

            User u1 = _context.Users.Find(order.UserId);

            return u1.IsPremium || order.Total >= 100;
        }

        public  bool TransitionOrderStatus(int orderId, OrderStatus newStatus)
        {
            var order = _context.Orders.Find(orderId);
            if(order == null)
            {
                throw new InvalidOperationException("Order not found");
            }

            var validTransitions = new Dictionary<OrderStatus, List<OrderStatus>>
                {
                    {OrderStatus.Created, new List<OrderStatus>{ OrderStatus.Processing} },
                    {OrderStatus.Processing, new List<OrderStatus>{ OrderStatus.Completed} },
                    {OrderStatus.Completed, new List<OrderStatus>() },
                };


            if (!validTransitions[order.Status].Contains(newStatus))
                return false;

            order.Status = newStatus;
            _context.SaveChanges();
            return true;

        }



    }
}
