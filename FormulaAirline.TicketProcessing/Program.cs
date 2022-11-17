// See https://aka.ms/new-console-template for more information
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Welcome to the ticketing service");

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

var consumer = new EventingBasicConsumer(channel);

consumer.Received+=(model, eventArgs) =>
{
    // getting bytes array
    var body = eventArgs.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    System.Console.WriteLine($"New ticket processing is initiating {message}");
};

channel.BasicConsume("bookings", true, consumer);

Console.ReadKey();
