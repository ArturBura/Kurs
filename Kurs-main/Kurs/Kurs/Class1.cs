using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules
{
    public class Car
    {
        public int Id { get; set; }
        public string Firm { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string LicensePlate { get; set; }
        public bool IsOperational { get; set; } = true;
    }
public class Driver
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PassportNumber { get; set; }
        public int Experience { get; set; }
        public Car AssignedCar { get; set; }
        public Dictionary<DateTime, decimal> EarningsByDate { get; set; } = new Dictionary<DateTime, decimal>();


        public decimal TotalEarnings() => EarningsByDate.Values.Sum();
    }   

public class TaxiPark
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }


}
