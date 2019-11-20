using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalWebApplication.Model
{
    public enum BikeCategory { Standard, Mountain, Trecking, Racing }

    public class Bike
    {
        [Key]
        public int BikeId { get; set; }

        [Required]
        [MaxLength(25)]
        public string Brand { get; set; }

        [Required]
        public DateTime PurchesDate { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        public DateTime LastService { get; set; }
        [Required, RegularExpression("\\d*\\.\\d{1,2}$")]
        public decimal PriceFirstHour { get; set; }

        [Required, RegularExpression("\\d*\\.\\d{1,2}$")]
        public decimal PriceAdditionalHours { get; set; }

        [Required]
        public BikeCategory BikeCategory { get; set; }
    }
}
