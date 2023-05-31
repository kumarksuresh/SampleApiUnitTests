using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleApi.Controllers;
using SampleApi.Data;

namespace SampleApiTests;

public class UnitTest1
{
    private DbContextOptions<InsuredInfoContext> GetInMemoryDbContextOptions(string dbName)
    {
        return new DbContextOptionsBuilder<InsuredInfoContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
    }

    [Fact]
    public void GetInsuredList_ReturnsListOfInsureds()
    {
        var dbName = Guid.NewGuid().ToString();
            var dbContextOptions = GetInMemoryDbContextOptions(dbName);

            using (var dbContext = new InsuredInfoContext(dbContextOptions))
            {   
                dbContext.Insureds.AddRange(
                    new Insured { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1985,05,15) },
                    new Insured { Id = 2, FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateTime(1990,09,28) },
                    new Insured { Id = 3, FirstName = "Michael", LastName = "Johnson", DateOfBirth = new DateTime(1978,12,10) }
                );
                dbContext.SaveChanges();
            }

            using (var dbContext = new InsuredInfoContext(dbContextOptions))
            {
                var controller = new InsuredController(dbContext);
                
                var response = controller.GetInsuredList();
                
                var okResult = Assert.IsType<OkObjectResult>(response.Result);
                var insured = Assert.IsAssignableFrom<IEnumerable<Insured>>(okResult.Value);
                Assert.Equal(3, insured.Count());
            }
    }

}