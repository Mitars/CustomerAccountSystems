namespace Cas.Account.Core.Dtos
{
    public record AccountToCreateDto
    {
        public long CustomerId { get; init; }
        public string Name { get; init; }
    }
}
