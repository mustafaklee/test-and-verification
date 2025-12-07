using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest_3.OrderModule
{
    public class OrderManager
    {
        //Bitiş'e kadar olan kısmı sınav kapsamında bilmenize gerek yok. Derste bu kodların amacı anlatılmıştır
        private readonly IOrderService _orderService;

        public OrderManager(IOrderService orderService)
        {
            _orderService = orderService;
        }
        //Bitiş

        public async Task ProcessOrder(Order order)
        {

            //3 Aşama var
            //1-Siparişi Doğrula
            //2-Siparişi Kargola
            //3-Kullanıcı Bilgilendir
            if (_orderService.ValidateOrder(order))
            {  //sipariş geçerliyse yani düzgünse
                var success = await _orderService.SendOrderAsync(order); // sipariş kaydını oluştur/gönder
                _orderService.NotifyUser(order, success); // Kullanıcıyı bilgilendir. 
            }
        }

    }
}
