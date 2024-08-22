using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrepPal.Models
{
    public class FridgeItem
    {
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsExpired { get; set; }
        public bool IsUsed { get; set; }
    }
}
