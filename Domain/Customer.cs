public sealed class Customer
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }

    private Customer() { }

    public static Customer GenerateFake(Random? random = null)
    {
        random ??= Random.Shared;
        return CreateSingle(random);
    }

    public static List<Customer> GenerateFakeList(int count, Random? random = null)
    {
        random ??= Random.Shared;

        if (count <= 0)
        {
            return new List<Customer>();
        }

        var list = new List<Customer>(count);
        for (int i = 0; i < count; i++)
        {
            list.Add(CreateSingle(random));
        }

        return list;
    }

    private static Customer CreateSingle(Random random)
    {
        var firstNames = new[]
        {
            "Ana", "Bruno", "Carla", "Daniel", "Eduarda",
            "Felipe", "Gabriela", "Henrique", "Isabela", "João"
        };

        var lastNames = new[]
        {
            "Silva", "Souza", "Pereira", "Oliveira", "Costa",
            "Santos", "Rodrigues", "Almeida", "Nunes", "Ferreira"
        };

        var domains = new[]
        {
            "exemplo.com", "teste.com", "mail.com", "empresa.com", "dominio.com"
        };

        var streetNames = new[]
        {
            "Rua das Flores", "Avenida Central", "Travessa do Sol", "Alameda Bela Vista", "Rua do Comércio"
        };

        var cities = new[]
        {
            "São Paulo", "Rio de Janeiro", "Belo Horizonte", "Porto Alegre", "Curitiba"
        };

        var states = new[]
        {
            "SP", "RJ", "MG", "RS", "PR"
        };

        string first = firstNames[random.Next(firstNames.Length)];
        string last = lastNames[random.Next(lastNames.Length)];
        string domain = domains[random.Next(domains.Length)];

        string email = $"{Sanitize(first)}.{Sanitize(last)}{random.Next(1, 999)}@{domain}".ToLowerInvariant();

        string phone = $"({random.Next(11, 99)}) {random.Next(100, 1000):D3}-{random.Next(1000, 10000):D4}";

        string street = $"{random.Next(1, 9999)} {streetNames[random.Next(streetNames.Length)]}";
        string city = cities[random.Next(cities.Length)];
        string state = states[random.Next(states.Length)];
        string zip = random.Next(10000, 99999).ToString("D5");

        return new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = first,
            LastName = last,
            Email = email,
            Phone = phone,
            Street = street,
            City = city,
            State = state,
            ZipCode = zip,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    private static string Sanitize(string input)
    {
        var normalized = input.Trim().ToLowerInvariant();
        normalized = normalized
            .Replace("á", "a")
            .Replace("à", "a")
            .Replace("ã", "a")
            .Replace("â", "a")
            .Replace("é", "e")
            .Replace("ê", "e")
            .Replace("í", "i")
            .Replace("ó", "o")
            .Replace("ô", "o")
            .Replace("õ", "o")
            .Replace("ú", "u")
            .Replace("ü", "u")
            .Replace("ç", "c")
            .Replace(" ", "");
        return normalized;
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName} <{Email}> ({Phone}) - {Street}, {City}/{State} {ZipCode}";
    }
}