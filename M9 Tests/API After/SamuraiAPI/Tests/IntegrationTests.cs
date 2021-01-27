using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamuraiAPI;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System.Linq;

namespace Tests
{
  [TestClass]
  public class IntegrationTests
  {
    [TestMethod]
    public void BizDataGetSamuraiReturnsSamurai()
    {
      //Arrange (set up builder & seed data)
      var builder = new DbContextOptionsBuilder<SamuraiContext>();
      builder.UseInMemoryDatabase("GetSamurai");
      int samuraiId = SeedWithOneSamurai(builder.Options);
      //Act (call the method)
      using (var context = new SamuraiContext(builder.Options))
      {
        var bizlogic = new BusinessLogicData(context);
        var samuraiRetrieved = bizlogic.GetSamuraiById(samuraiId);
      //Assert (check the results)
        Assert.AreEqual(samuraiId, samuraiRetrieved.Result.Id);
      };
    }

    [TestMethod]
    public void CanDisableTracking()
    {
      //Arrange (set up builder & seed data)
      var builder = new DbContextOptionsBuilder<SamuraiContext>();
      builder.UseInMemoryDatabase("UnTrackedSamurai")
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); 
      SeedWithOneSamurai(builder.Options);
      //Act (call the method)
      using (var context = new SamuraiContext(builder.Options))
      {
        context.Samurais.ToList();
        //Assert (check the results)
        Assert.AreEqual(0, context.ChangeTracker.Entries().Count());
      };
    }

    private int SeedWithOneSamurai(DbContextOptions<SamuraiContext> options)
    {
      using (var seedcontext = new SamuraiContext(options))
      {
        var samurai = new Samurai();
        seedcontext.Samurais.Add(samurai);
        seedcontext.SaveChanges();
        return samurai.Id;
      }
    }
  }
}