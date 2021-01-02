using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TMS.Models
{
    public class VehicleInformation
    {
        [Key]
        public int VehicleId { get; set; }
        public string VehicleNo { get; set; }
        public int SeatCapacity { get; set; }

        public int VehicleTypeId { get; set; }
        public int RouteId { get; set; }
        public int UserId { get; set; }
        public VehicleType VehicleType { get; set; }
        public Route Route { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        


    }
}
