using System;

namespace Play.Catalog.Service.Dtos
{
	public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);

	// Id and CreatedDate are automatically populated.
	// This is a contract and whoever want to do a post to create an item, they need to obey this contract
	public record CreateItemDto(string Name, string Description, decimal Price);

    public record UpdateItemDto(string Name, string Description, decimal Price);
}
