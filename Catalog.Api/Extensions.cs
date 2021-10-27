using Catalog.Api.Dtos;
using Catalog.Api.Entities;

namespace Catalog.Api
{
  public static class Extensions
  {
    public static ItemDto AsDto(this Item item)
    {
      return new() { Id = item.Id, Name = item.Name, Price = item.Price, CreatedDate = item.CreatedDate, };
    }
  }
}