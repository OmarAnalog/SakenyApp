using Sakenny.Core.Helpers;

namespace Sakenny.Core.Models
{
    public class Connections
    {
        public int Id { get; set; }
        public string Connection { get; set; }
        public ConnectionType ConnectionType { get; set; }
        public string UserId { get; set; }
    }
}
