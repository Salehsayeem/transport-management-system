using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TMS.Models
{
    public class Route
    {
        [Key]
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public int RouteDistance { get; set; }
    }
}
