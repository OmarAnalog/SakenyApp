using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakenny.Core.Models
{
   public  class DeviceToken
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
