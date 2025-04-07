using Karshare.API.Data;
using Karshare.API.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Karshare.API.Tests;

[TestClass]
public class GenericRepositoryTests
{
    private UnitOfWork _unitOfWork = null!;

    private SqliteConnection _connection = null!;

    private AppDbContext _dbContext = null!;

    private readonly List<User> _data =
    [
        new()
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "Test",
            Address = "Test",
            Age = 25,
            City = "Test",
            Country = "Test",
            HasLicense = true,
            IsVerified = true,
            Username = "Test",
            PhoneNumber = "0123456789",
            CreatedAt = DateTime.UtcNow,
            YearsOfLicense = 2,
            PasswordHash = "Test",
            Role = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Admin"
            }
        }
    ];

    [TestInitialize]
    public void Initialize()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        _dbContext = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlite(_connection).Options);
        _dbContext.Database.EnsureCreated();

        _dbContext.Users.AddRange(_data);
        _dbContext.SaveChanges();

        _unitOfWork = new UnitOfWork(_dbContext);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _connection.Close();
        _connection.Dispose();
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [TestMethod]
    public async Task FindAsync_WithValidId_ShouldReturnUser()
    {
        var expected = _data.First();

        var actual = await _unitOfWork.UserRepository.FindAsync(expected.Id);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task FindAsync_WithEmptyId_ShouldReturnNull()
    {
        var actual = await _unitOfWork.UserRepository.FindAsync(Guid.Empty);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public async Task FindAsync_WithValidExpression_ShouldReturnUser()
    {
        var expected = _data.First();

        var actual = await _unitOfWork.UserRepository.FindAsync(x => x.Email == expected.Email);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task FindAsync_WithInvalidExpression_ShouldReturnNull()
    {
        var actual = await _unitOfWork.UserRepository.FindAsync(x => x.Email == string.Empty);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        var expected = _data;

        var actual = await _unitOfWork.UserRepository.GetAllAsync();

        CollectionAssert.AreEqual(expected, actual.ToList());
    }

    [TestMethod]
    public async Task GetAsync_WithValidExpression_ShouldReturnUser()
    {
        var expected = _dbContext.Users.Where(x => x.Email == "test@test.com").ToList();

        var actual = await _unitOfWork.UserRepository.GetAsync(x => x.Email == "test@test.com");

        CollectionAssert.AreEqual(expected, actual.ToList());
    }

    [TestMethod]
    public async Task GetAsync_WithInvalidExpression_ShouldReturnEmptyList()
    {
        var actual = await _unitOfWork.UserRepository.GetAsync(x => x.Email == string.Empty);

        Assert.AreEqual(0, actual.Count());
    }

    [TestMethod]
    public async Task AddAsync_WithValidEntity_ShouldReturnUserAndAddEntityInDatabase()
    {
        var expected = new User()
        {
            Id = Guid.NewGuid(),
            Email = "test2@test2.com",
            FirstName = "Test2",
            LastName = "Test2",
            Address = "Test2",
            Age = 34,
            City = "Test2",
            Country = "Test2",
            HasLicense = false,
            IsVerified = true,
            Username = "Test2",
            PhoneNumber = "0987654321",
            CreatedAt = DateTime.UtcNow,
            YearsOfLicense = 4,
            PasswordHash = "Test2",
            Role = new Role
            {
                Id = Guid.NewGuid(),
                Name = "User"
            }
        };

        var actual = await _unitOfWork.UserRepository.AddAsync(expected);

        Assert.AreEqual(expected, actual);

        var userInDb = await _dbContext.Users.FindAsync(expected.Id);

        Assert.IsNotNull(userInDb);
    }

    [TestMethod]
    public async Task UpdateAsync_WithValidEntity_ShouldReturnUserAndUpdateEntityInDatabase()
    {
        var dbList = _dbContext.Users.Where(x => x.Email == "test@test.com").ToList();

        var expected = dbList.First();

        expected.FirstName = "UpdatedFirstName";
        expected.LastName = "UpdatedLastName";

        var actual = await _unitOfWork.UserRepository.UpdateAsync(expected);

        Assert.AreEqual(expected, actual);

        var userInDb = await _dbContext.Users.FindAsync(expected.Id);
        Assert.AreEqual("UpdatedFirstName", userInDb.FirstName);
        Assert.AreEqual("UpdatedLastName", userInDb.LastName);
    }

    [TestMethod]
    public async Task UpdateAsync_WithValidEntityNotModified_ShouldThrowNullReferenceException()
    {
        var dbList = _dbContext.Users.Where(x => x.Email == "test@test.com").ToList();

        var expected = dbList.First();

        await Assert.ThrowsExceptionAsync<NullReferenceException>(async () =>
            await _unitOfWork.UserRepository.UpdateAsync(expected));
    }

    [TestMethod]
    public async Task DeleteAsync_WithValidEntity_ShouldReturnTrueAndEntityIsRemovedFromDatabase()
    {
        var userInDb = await _dbContext.Users.FindAsync(_data.First().Id);
        
        var actual = await _unitOfWork.UserRepository.DeleteAsync(userInDb.Id);
        
        Assert.IsTrue(actual);
        
        userInDb = await _dbContext.Users.FindAsync(userInDb.Id);
        Assert.IsNull(userInDb);
    }
    
    [TestMethod]
    public async Task DeleteAsync_WithInvalidEntity_ShouldReturnFalse()
    {
        var actual = await _unitOfWork.UserRepository.DeleteAsync(Guid.Empty);

        Assert.IsFalse(actual);
    }
}