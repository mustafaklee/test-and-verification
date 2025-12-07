using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest_3.OrderModule
{
    public interface IOrderService
    {
        bool ValidateOrder(Order order);

        Task<bool> SendOrderAsync(Order order);

        void NotifyUser(Order order, bool success);
    }
}
