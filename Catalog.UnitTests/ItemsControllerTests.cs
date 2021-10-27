using System;
using System.Threading.Tasks;
using Catalog.Api.Controllers;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.UnitTests
{
  public class ItemsControllerTests
  {
    private readonly Mock<IItemsRepository> repositoryStub = new();
    private readonly Mock<ILogger<ItemsController>> loggerStub = new();
    private readonly Random rand = new();

    [Fact]
    public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
    {
      // Arrange
      repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
        .ReturnsAsync((Item)null);

      var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);
      // Act
      var result = await controller.GetItemAsync(Guid.NewGuid());
      // Assert
      result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
    {
      // Arrange
      var expectedItem = CreateRandomItem();
      repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
        .ReturnsAsync(expectedItem);
      var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);
      // Act
      var result = await controller.GetItemAsync(Guid.NewGuid());
      // Assert
      result.Value.Should().BeEquivalentTo(
          expectedItem,
          options => options.ComparingByMembers<Item>());
    }

    private Item CreateRandomItem()
    {
      return new()
      {
        Id = Guid.NewGuid(),
        Name = Guid.NewGuid().ToString(),
        Price = rand.Next(1000),
        CreatedDate = DateTimeOffset.UtcNow
      };
    }
  }
}
