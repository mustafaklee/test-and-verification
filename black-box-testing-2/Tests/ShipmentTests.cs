using black_box_testing_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace black_box_testing_2.Tests
{
    public class ShipmentTests
    {
        //Durum Geçiş Testleri

        [Fact]
        public void ValidStateTransitions_ShouldWorkCorrectly()
        {
            //arrange
            var shipment = new Shipment();


            //act
            shipment.PickUp(); // created-> pickedup
            Assert.Equal(shipment.State, "PickedUp");



            shipment.Transit(); //pickedUp -> InTransit
            Assert.Equal(shipment.State, "InTransit");

            shipment.Deliver(); //Intransit -> Delivered
            Assert.Equal(shipment.State, "Delivered");

        }


        [Fact]
        public void InvalidStateTransitions_ShouldThrowExceptions()
        {
            //arrange 
            var shipment = new Shipment();

            //act 
            Assert.Throws<InvalidOperationException>(() => shipment.Transit()); // Created → InTransit (Geçersiz)

            //act 
            Assert.Throws<InvalidOperationException>(() => shipment.Deliver()); // Created → Delivered (Geçersiz)
        }

    }
}
