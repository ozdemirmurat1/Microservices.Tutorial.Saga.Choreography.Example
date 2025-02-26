using Shared.Messages;

namespace Shared.Events
{
    public class PaymenFailedEvent
    {
        public Guid OrderId { get; set; }

        public string Message { get; set; }

        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
