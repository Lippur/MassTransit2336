using System.Threading.Tasks;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace MassTransit2336.Test
{
    [TestFixture]
    public class PingTest
    {
        protected readonly WebApplicationFactory<Startup> Factory;
        
        public PingTest ()
        {
            Factory = new WebApplicationFactory<Startup>();
        }
        
        [Test]
        public async Task WithBus_FilterIsUsed ()
        {
            var bus = (IBus) Factory.Services.GetRequiredService(typeof(IBus));

            var response = await bus.Request<Ping, Pong>(new());
            
            Assert.AreEqual(response.Message.Message, "PONG!");
        }
        
        [Test]
        public async Task WithMediator_FilterIsNotUsed ()
        {
            var mediator = (IMediator) Factory.Services.GetRequiredService(typeof(IMediator));

            var response = await mediator.CreateRequestClient<Ping>().GetResponse<Pong>(new());
            
            Assert.AreEqual(response.Message.Message, "Pong!");
        }
    }
}