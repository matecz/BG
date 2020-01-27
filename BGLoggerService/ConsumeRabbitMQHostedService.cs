using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BGInfrastructure.RabbitMQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BGLoggerService
{
    public class ConsumeRabbitMQHostedService : BackgroundService
    {
        private readonly RabbitOptions _rabbitOptions;
        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;

        public ConsumeRabbitMQHostedService(ILoggerFactory loggerFactory, RabbitOptions rabbitOptions)
        {
            _rabbitOptions = rabbitOptions;
            _logger = loggerFactory.CreateLogger<ConsumeRabbitMQHostedService>();
            
            while(!InitRabbitMq()){};
        }

        private bool InitRabbitMq()
        {
            Thread.Sleep(4000);
            var factory = new ConnectionFactory()  
            {  
                HostName = _rabbitOptions.HostName,  
                UserName = _rabbitOptions.UserName,  
                Password = _rabbitOptions.Password,  
                Port = _rabbitOptions.Port,  
                VirtualHost = _rabbitOptions.VHost,  
            }; 
            
            // try create connection
            try
            {
                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                _logger.LogInformation("Waiting for MQ...");
                return false;
            }
            
            // create channel  
            _channel = _connection.CreateModel();

            // declaring the queue
            _channel.QueueDeclare("bg.queue.log", false, false, false, null);
            _channel.BasicQos(0, 1, false);
            
            _logger.LogInformation("Rabbit MQ connection is up. Listening to Queue: bg.queue.log");

            return true;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                // received message  
                var content = System.Text.Encoding.UTF8.GetString(ea.Body);

                // handle the received message  
                HandleMessage(content);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("bg.queue.log", false, consumer);
            return Task.CompletedTask;
        }

        private void HandleMessage(string content)
        {
            // we just print this message   
            _logger.LogInformation($"{content}");
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}