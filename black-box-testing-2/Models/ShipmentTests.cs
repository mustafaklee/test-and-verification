using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace black_box_testing_2.Models
{
    public class Shipment
    {
        public string State { get; private set; }

        public Shipment()
        {
            State = "Created";
        }

        public void PickUp()
        {
            if (State != "Created")
                throw new InvalidOperationException("Can only pick up a shipment from the 'Created' state.");
            State = "PickedUp";
        }

        public void Transit()
        {
            if (State != "PickedUp")
                throw new InvalidOperationException("Can only transition a shipment from the 'PickedUp' state.");
            State = "InTransit";
        }

        public void Deliver()
        {
            if (State != "InTransit")
                throw new InvalidOperationException("Can only deliver a shipment from the 'InTransit' state.");
            State = "Delivered";
        }

        public void Cancel()
        {
            if (State == "Delivered" || State == "Cancelled")
                throw new InvalidOperationException("Cannot cancel a shipment in 'Delivered' or 'Cancelled' state.");
            State = "Cancelled";
        }
    }
}
