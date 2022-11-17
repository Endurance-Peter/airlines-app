using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace FormulaAirline.API.Services;

public class MessageProducer : IMessageProducer
{
    public void SendingMessage<T>(T meassage)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "user",
            Password = "mypas",
            VirtualHost = "/"
        };

        var con = factory.CreateConnection();

        using var channel = con.CreateModel();

        channel.QueueDeclare("bookings", durable:true, exclusive:true);

        var jsonString = JsonSerializer.Serialize(meassage);
        var body = Encoding.UTF8.GetBytes(jsonString);

        channel.BasicPublish("", "bookings", body: body);
    }
}