using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TMS.Models
{
    public class VehicleType
    {
        [Key]
        public int VehicleTypeId { get; set; }
        public string VehicleTypeName { get; set; }

    }
}
