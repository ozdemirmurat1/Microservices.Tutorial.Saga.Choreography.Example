using MassTransit;
using Order.API.Models.Context;
using Shared.Events;

namespace Order.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymenFailedEvent>
    {
        private readonly OrderAPIDbContext _context;

        public PaymentFailedEventConsumer(OrderAPIDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<PaymenFailedEvent> context)
        {
            var order=await _context.Orders.FindAsync(context.Message.OrderId);
            if (order == null)
                throw new NullReferenceException();    
            order.OrderStatus=Enums.OrderStatus.Fail;
            await _context.SaveChangesAsync();
        }
    }
}
