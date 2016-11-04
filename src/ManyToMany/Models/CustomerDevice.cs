
namespace ManyToMany.Models
{
    public class CustomerDevice
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
}
