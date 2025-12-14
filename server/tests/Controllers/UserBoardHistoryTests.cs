using api.Entity;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using api.Controllers;
using api.services;
using efscaffold.Models;
using tests.Containers;


namespace tests.Controllers;


public class UserBoardHistoryTests : TestBase
{
    public UserBoardHistoryTests(PostgresFixture fixture) : base(fixture) {}


    [Fact]
    public async Task GetByUserId_ShouldReturnOrderedHistory()
    {
        var userId = "u1";

        _fixture.DbContext.UserBoardHistory.AddRange(
            new UserBoardHistory { UserId = userId, Date = DateTime.UtcNow.AddDays(-1) },
            new UserBoardHistory { UserId = userId, Date = DateTime.UtcNow }
        );
        await _fixture.DbContext.SaveChangesAsync();

        var controller = GetController<UserBoardHistoryController>();
        var result = await controller.GetByUserId(userId);

        var ok = Assert.IsType<OkObjectResult>(result);
        var list = Assert.IsAssignableFrom<List<UserBoardHistory>>(ok.Value);

        Assert.True(list[0].Date > list[1].Date);
    }

}