using RabbitMQ.Client;
using System.Text.Json;

const string exchangeName = "customer.exchange";
const string queueName = "customer.queue";
const string routingKey = "customer.create";

var factory = new RabbitMQ.Client.ConnectionFactory()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest",
    VirtualHost = "/",
    AutomaticRecoveryEnabled = true,
    NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
    exchange: exchangeName,
    type: RabbitMQ.Client.ExchangeType.Direct,
    durable: true,
    autoDelete: false,
    arguments: null);

await channel.QueueDeclareAsync(
    queue: queueName,
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null);

await channel.QueueBindAsync(
    queue: queueName,
    exchange: exchangeName,
    routingKey: routingKey,
    arguments: null);

Console.WriteLine("Quantos clientes quer gerar?");

if (!int.TryParse(Console.ReadLine(), out var customerCount) || customerCount <= 0)
{
    customerCount = 10;
}

var customers = Customer.GenerateFakeList(10, new Random(10));

foreach (var customer in customers)
{
    var body = JsonSerializer.SerializeToUtf8Bytes(customer);
    var properties = new BasicProperties
    {
        Persistent = true,
        ContentType = "application/json",
        ContentEncoding = "utf-8"
    };

    await channel.BasicPublishAsync(
        exchange: exchangeName,
        routingKey: routingKey,
        mandatory: false,
        basicProperties: properties,
        body: body);
    Console.WriteLine($"[x] Sent customer {customer.Id}");
}