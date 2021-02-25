using System.Threading.Tasks;
using GreenPipes;
using MassTransit;

namespace MassTransit2336
{
    public class PongFilter<T> : IFilter<SendContext<T>> where T : class
    {
        public void Probe (ProbeContext context) { }

        public async Task Send (SendContext<T> context, IPipe<SendContext<T>> next)
        {
            if (context.Message is Pong and not null)
            {
                (context.Message as Pong)!.Message = (context.Message as Pong)!.Message.ToUpper();
            }

            await next.Send(context);
        }
    }
}