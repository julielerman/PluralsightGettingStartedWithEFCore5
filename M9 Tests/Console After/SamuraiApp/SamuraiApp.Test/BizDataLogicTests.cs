using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiApp.Test
{
    [TestClass]
    public class BizDataLogicTests
    {
        [TestMethod]
        public void CanAddSamuraisByName()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("CanAddSamuraisByName");

            using var context = new SamuraiContext(builder.Options);
            var bizLogic = new BusinessDataLogic(context);

            var namelist = new string[] { "Kikuchiyo", "Kyuzo", "Rikchi" };

            var result = bizLogic.AddSamuraisByName(namelist);
            Assert.AreEqual(namelist.Length, result);
        }

        [TestMethod]
        public void CanInsertSingleSamurai()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("CanInsertSingleSamurai");

            using (var context = new SamuraiContext(builder.Options))
            {
                var bizLogic = new BusinessDataLogic(context);
                bizLogic.InsertNewSamurai(new Samurai());
            }
            using (var context2=new SamuraiContext(builder.Options))
            {
                Assert.AreEqual(1, context2.Samurais.Count());
            }
        }


        [TestMethod]
        public void CanInsertSamuraiwithQuotes()
        {

            var samuraiGraph = new Samurai
            {
                Name = "Kyūzō",
                Quotes = new List<Quote> {
          new Quote { Text = "Watch out for my sharp sword!" },
          new Quote { Text = "I told you to watch out for the sharp sword! Oh well!" }
        }
            };
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("SamuraiWithQuotes");
            using (var context = new SamuraiContext(builder.Options))
            {
                var bizlogic = new BusinessDataLogic(context);
                var result = bizlogic.InsertNewSamurai(samuraiGraph);
            };
            using (var context2 = new SamuraiContext(builder.Options))
            {
                var samuraiWithQuotes = context2.Samurais.Include(s => s.Quotes).FirstOrDefaultAsync().Result;
                Assert.AreEqual(2, samuraiWithQuotes.Quotes.Count);
            }

        }
        [TestMethod, TestCategory("SamuraiWithQuotes")]
        public void CanGetSamuraiwithQuotes()
        {
            int samuraiId;
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("SamuraiWithQuotes");
            using (var seedcontext = new SamuraiContext(builder.Options))
            {
                var samuraiGraph = new Samurai
                {
                    Name = "Kyūzō",
                    Quotes = new List<Quote> {
            new Quote { Text = "Watch out for my sharp sword!" },
            new Quote { Text = "I told you to watch out for the sharp sword! Oh well!" }
          }
                };
                seedcontext.Samurais.Add(samuraiGraph);
                seedcontext.SaveChanges();
                samuraiId = samuraiGraph.Id;
            }
            using (var context = new SamuraiContext(builder.Options))
            {
                var bizlogic = new BusinessDataLogic(context);
                var result = bizlogic.GetSamuraiWithQuotes(samuraiId);
                Assert.AreEqual(2, result.Quotes.Count);
            };
        }
    }
}
