using McHizok.Entities.DataTransferObjects;
using McHizok.Entities.Exceptions;
using McHizok.Web.Data;
using McHizok.Web.Services;
using McHizok.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace McHizok.Tests;

public class CouponInventoryServiceTests
{
    private McHizokDbContext _mcHizokDbContext;
    private CouponInventoryService _couponInventoryService;
    private User _adminUser;
    private User _regularUser;

	public CouponInventoryServiceTests()
	{
        var options = new DbContextOptionsBuilder<McHizokDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        _mcHizokDbContext = new McHizokDbContext(options);
        _mcHizokDbContext.Database.EnsureCreated();

        _adminUser = new User { UserName = "Administrator", AccountFor = "Myself" };
        _regularUser = new User { UserName = "RegularUser", AccountFor = "RegularUser" };

        _mcHizokDbContext.Users.Add(_adminUser);
        _mcHizokDbContext.Users.Add(_regularUser);
        _mcHizokDbContext.SaveChanges();

        _couponInventoryService = new CouponInventoryService(_mcHizokDbContext);
    }

    [Fact]
    public async Task GetCouponsForUserAsync_GettingCouponsForUser_ReturnsAmountOfStoredCoupons()
    {
        var coupons = new List<CouponInventory>
        { 
            new CouponInventory
            {
                CouponBase64 = Guid.NewGuid().ToString(),
                CouponCode = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.Today.AddDays(5),
                FileName = Guid.NewGuid().ToString(),
                User = _regularUser
            },
            new CouponInventory
            {
                CouponBase64 = Guid.NewGuid().ToString(),
                CouponCode = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.Today.AddDays(5),
                FileName = Guid.NewGuid().ToString(),
                User = _regularUser
            },
            new CouponInventory
            {
                CouponBase64 = Guid.NewGuid().ToString(),
                CouponCode = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.Today.AddDays(5),
                FileName = Guid.NewGuid().ToString(),
                User = _adminUser
            },
        };

        await _mcHizokDbContext.CouponInventories.AddRangeAsync(coupons);
        await _mcHizokDbContext.SaveChangesAsync();

        var couponsFromDb = await _couponInventoryService.GetCouponsForUserAsync(_regularUser.Id);

        Assert.Equal(2, couponsFromDb.Count());
    }

    [Fact]
    public async Task GetCouponsForUserAsync_GettingCouponsForUser_ReturnedCouponsAreOrderedByExpiry()
    {
        var coupons = new List<CouponInventory>
        {
            new CouponInventory
            {
                CouponBase64 = Guid.NewGuid().ToString(),
                CouponCode = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.Today.AddDays(7),
                FileName = "freshCoupon",
                User = _regularUser
            },
            new CouponInventory
            {
                CouponBase64 = Guid.NewGuid().ToString(),
                CouponCode = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.Today.AddDays(2),
                FileName = "olderCoupon",
                User = _regularUser
            }
        };

        await _mcHizokDbContext.CouponInventories.AddRangeAsync(coupons);
        await _mcHizokDbContext.SaveChangesAsync();

        var couponsFromDb = await _couponInventoryService.GetCouponsForUserAsync(_regularUser.Id);

        var firstCoupon = couponsFromDb.First();

        Assert.Equal("olderCoupon", firstCoupon.FileName);
    }

    [Fact]
    public async Task SaveCouponAsync_UserIdDoestNotExist_ThrowsUserNotFoundException()
    {
        var couponDto = new CouponDto(Guid.Empty, "base64Content", "fileName", DateTime.Now.AddDays(7), "couponCode", "notExistingUserId");

        var thrownException = await Assert.ThrowsAsync<UserNotFoundException>(() => _couponInventoryService.SaveCouponAsync(couponDto));

        Assert.Equal("The user with id: notExistingUserId doesn't exist in the database", thrownException.Message);
    }

    [Fact]
    public async Task SaveCouponAsync_HappyPath_CouponSavedToTheDatabase()
    {
        var couponDto = new CouponDto(Guid.Empty, "base64Content", "fileName", DateTime.Now.AddDays(7), "newCouponCode", _regularUser.Id);

        await _couponInventoryService.SaveCouponAsync(couponDto);

        var couponFromDb = await _mcHizokDbContext.CouponInventories.FirstOrDefaultAsync(c => c.CouponCode == "newCouponCode" && c.UserId == _regularUser.Id);

        Assert.NotNull(couponFromDb);
    }

    [Fact]
    public async Task DeleteCouponAsync_CouponDoesNotExist_ThrowsCouponNotFoundException()
    {
        var nonExistingCouponId = Guid.NewGuid();

        var thrownException = await Assert.ThrowsAsync<CouponNotFoundException>(() => _couponInventoryService.DeleteCouponAsync(nonExistingCouponId));

        Assert.Equal($"The coupon with the Id: {nonExistingCouponId} doesn't exist", thrownException.Message);
    }

    [Fact]
    public async Task DeleteCouponAsync_CouponExists_CouponIsDeleted()
    {
        var coupon = new CouponInventory
        {
            CouponBase64 = Guid.NewGuid().ToString(),
            CouponCode = Guid.NewGuid().ToString(),
            ExpiresAt = DateTime.Today.AddDays(5),
            FileName = Guid.NewGuid().ToString(),
            User = _regularUser
        };

        await _mcHizokDbContext.CouponInventories.AddAsync(coupon);
        await _mcHizokDbContext.SaveChangesAsync();

        await _couponInventoryService.DeleteCouponAsync(coupon.Id);

        Assert.DoesNotContain(coupon, _mcHizokDbContext.CouponInventories);
    }
}
