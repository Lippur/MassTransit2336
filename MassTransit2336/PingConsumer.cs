using System.Threading.Tasks;
using MassTransit;

namespace MassTransit2336
{
    public class PingConsumer : IConsumer<Ping>
    {
        public async Task Consume (ConsumeContext<Ping> context)
        {
            await context.RespondAsync(new Pong {Message = "Pong!"});
        }
    }
}