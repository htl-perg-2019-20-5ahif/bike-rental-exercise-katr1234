using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalWebApplication.Model
{
    public class Rental
    {
        [Key]
        public int RentalId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Required]
        public int BikeId { get; set; }

        public Bike Bike { get; set; }

        [Required]
        public DateTime RentalBegin { get; set; }

        public DateTime RentalEnd { get; set; }

        [Required]
        [RegularExpression("\\d*\\.\\d{1,2}$"), Range(0, Double.PositiveInfinity)]
        public decimal TotalCost { get; set; }

        public bool Paid { get; set; }
    }
}
