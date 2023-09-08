using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApplicationRabbitWorker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceiverController : ControllerBase
    {
        [HttpGet]
        public async Task<string> GetReceivedOrders()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "orderQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            string resultado = null;
            var tcs = new TaskCompletionSource<string>();

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                resultado = JsonSerializer.Deserialize<string>(message);
                tcs.SetResult(resultado);
            };
            channel.BasicConsume(queue: "orderQueue",
                                 autoAck: true,
                                 consumer: consumer);

            await tcs.Task;
            return resultado;
        }
    }
}
