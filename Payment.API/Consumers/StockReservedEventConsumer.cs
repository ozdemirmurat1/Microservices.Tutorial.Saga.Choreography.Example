using MassTransit;
using Shared.Events;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly IPublishEndpoint _publishEndPoint;

        public StockReservedEventConsumer(IPublishEndpoint publishEndPoint)
        {
            _publishEndPoint = publishEndPoint;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            if (true)
            {
                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId=context.Message.OrderId
                };

                await _publishEndPoint.Publish(paymentCompletedEvent);
                Console.WriteLine("ÖDEME BAŞARILI");
            }
            else
            {
                PaymenFailedEvent paymenFailedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    Message = "Yetersiz bakiye..",
                    OrderItems = context.Message.OrderItems
                };
                await _publishEndPoint.Publish(paymenFailedEvent);
                Console.WriteLine("ÖDEME BAŞARISIZ");
            }
        }
    }
}
