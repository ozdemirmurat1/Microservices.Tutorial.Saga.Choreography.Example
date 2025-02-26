using MassTransit;
using Order.API.Models.Context;
using Shared.Events;

namespace Order.API.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly OrderAPIDbContext _context;

        public PaymentCompletedEventConsumer(OrderAPIDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var order=await _context.Orders.FindAsync(context.Message.OrderId);
            if(order == null)
                throw new NullReferenceException();

            order.OrderStatus=Enums.OrderStatus.Completed;
            await _context.SaveChangesAsync();
        }
    }
}
