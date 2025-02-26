namespace Shared.Events
{
    public class PaymenFailedEvent
    {
        public Guid OrderId { get; set; }

        public string Message { get; set; }
    }
}
