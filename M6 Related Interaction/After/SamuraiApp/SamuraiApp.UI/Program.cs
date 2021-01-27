using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        private static void Main(string[] args)
        {
            // InsertNewSamuraiWithAQuote();
            //InsertNewSamuraiWithManyQuotes();
            //AddQuoteToExistingSamuraiWhileTracked();
            //AddQuoteToExistingSamuraiNotTracked(2);
            //Simpler_AddQuoteToExistingSamuraiNotTracked(2);
            //EagerLoadSamuraiWithQuotes();
            //ProjectSomeProperties();
            //ProjectSamuraisWithQuotes();
            //ExplicitLoadQuotes();
            //LazyLoadQuotes();
            //FiteringWithRelatedData();
            //ModifyingRelatedDataWhenTracked();
            //ModifyingRelatedDataWhenNotTracked();
            //AddingNewSamuraiToAnExistingBattle();
            //ReturnBattleWithSamurais();
            //ReturnAllBattlesWithSamurais();
            //AddAllSamuraisToAllBattles();
            //RemoveSamuraiFromABattle();
            //RemoveSamuraiFromABattleExplicit();
            //AddNewSamuraiWithHorse();
            //AddNewHorseToSamuraiUsingId();
            //AddNewHorseToSamuraiObject();
            //AddNewHorseToDisconnectedSamuraiObject();
            ReplaceAHorse();
        }

        private static void InsertNewSamuraiWithAQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
        {
          new Quote { Text = "I've come to save you" }
        }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        private static void InsertNewSamuraiWithManyQuotes()
        {
            var samurai = new Samurai
            {
                Name = "Kyūzō",
                Quotes = new List<Quote> {
            new Quote {Text = "Watch out for my sharp sword!"},
            new Quote {Text="I told you to watch out for the sharp sword! Oh well!" }
        }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you're happy that I've saved you!"
            });
            _context.SaveChanges();
        }
        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "Now that I saved you, will you feed me dinner?"
            });
            using (var newContext = new SamuraiContext())
            {
                newContext.Samurais.Attach(samurai);
                newContext.SaveChanges();
            }
        }
        private static void Simpler_AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var quote = new Quote { Text = "Thanks for dinner!", SamuraiId = samuraiId };
            using var newContext = new SamuraiContext();
            newContext.Quotes.Add(quote);
            newContext.SaveChanges();
        }
        private static void EagerLoadSamuraiWithQuotes()
        {
            //var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
            //var splitQuery = _context.Samurais.AsSplitQuery().Include(s => s.Quotes).ToList();
            //var filteredInclude = _context.Samurais
            //    .Include(s => s.Quotes.Where(q=>q.Text.Contains("Thanks"))).ToList();
            var filterPrimaryEntityWithInclude =
                _context.Samurais.Where(s => s.Name.Contains("Sampson"))
                        .Include(s => s.Quotes).FirstOrDefault();

        }
        private static void ProjectSomeProperties()
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
            var idAndNames = _context.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToList();
        }
        public struct IdAndName
        {
            public IdAndName(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public int Id;
            public string Name;

        }
        private static void ProjectSamuraisWithQuotes()
        {
            //var somePropsWithQuotes = _context.Samurais
            //    .Select(s => new { s.Id, s.Name, NumberOfQuotes=s.Quotes.Count })
            //    .ToList();
            //var somePropsWithQuotes = _context.Samurais
            //.Select(s => new { s.Id, s.Name, 
            //                   HappyQuotes = s.Quotes.Where(q=>q.Text.Contains("happy")) })
            //.ToList();
            var samuraisAndQuotes = _context.Samurais
            .Select(s => new {Samurai=s,
                HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))})
            .ToList();
        }
        private static void ExplicitLoadQuotes()
        {
            //make sure there's a horse in the DB, then clear the context's change tracker
            _context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name = "Mr. Ed" });
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            //-------------------
            var samurai = _context.Samurais.Find(1);
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.Horse).Load();
        }
        private static void LazyLoadQuotes()
        {
            var samurai = _context.Samurais.Find(2);
            var quoteCount = samurai.Quotes.Count(); //won't work without LL setup
        }
        private static void FiteringWithRelatedData()
        {
            var samurais = _context.Samurais
                                 .Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))
                                 .ToList();
        }
        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                                  .FirstOrDefault(s => s.Id == 2);
            samurai.Quotes[0].Text = "Did you hear that?";
            _context.Quotes.Remove(samurai.Quotes[2]);
            _context.SaveChanges();
        }
        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                                  .FirstOrDefault(s => s.Id == 2);
            var quote=samurai.Quotes[0];
            quote.Text += "Did you hear that again?";

            using var newContext=new SamuraiContext();
            //newContext.Quotes.Update(quote);
            newContext.Entry(quote).State = EntityState.Modified;
            newContext.SaveChanges();
        }
        private static void AddingNewSamuraiToAnExistingBattle()
        {
             var battle = _context.Battles.FirstOrDefault();
            battle.Samurais.Add(new Samurai { Name = "Takeda Shingen" });
            _context.SaveChanges();
        }    
        private static void ReturnBattleWithSamurais()
        {
            var battle=_context.Battles.Include(b => b.Samurais).FirstOrDefault();
        }
        private static void ReturnAllBattlesWithSamurais()
        {
            var battles = _context.Battles.Include(b => b.Samurais).ToList();
        }
        private static void AddAllSamuraisToAllBattles()
        {
            var allbattles = _context.Battles.Include(b=>b.Samurais).ToList();
            var allSamurais = _context.Samurais.ToList();
            foreach (var battle in allbattles)
            {
                battle.Samurais.AddRange(allSamurais);
            }
            _context.SaveChanges();
        }
        private static void RemoveSamuraiFromABattle()
        {
            var battleWithSamurai = _context.Battles
                .Include(b => b.Samurais.Where(s => s.Id == 12))
                .Single(s=>s.BattleId==1);
            var samurai = battleWithSamurai.Samurais[0];
            battleWithSamurai.Samurais.Remove(samurai);
            _context.SaveChanges();
        }
        private static void WillNotRemoveSamuraiFromABattle()
        {
            var battle = _context.Battles.Find(1);
            var samurai = _context.Samurais.Find(12);
            battle.Samurais.Remove(samurai);
            _context.SaveChanges(); //the relationship is not being tracked
        }
        private static void RemoveSamuraiFromABattleExplicit()
        {
            var b_s = _context.Set<BattleSamurai>()
                .SingleOrDefault(bs => bs.BattleId == 1 && bs.SamuraiId == 10);
            if (b_s != null)
            {
                _context.Remove(b_s); //_context.Set<BattleSamurai>().Remove works, too
                _context.SaveChanges(); 
            }
        }

        private static void AddNewSamuraiWithHorse()
        {
            var samurai = new Samurai { Name = "Jina Ujichika" };
            samurai.Horse = new Horse { Name = "Silver" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        private static void AddNewHorseToSamuraiUsingId()
        {
            var horse = new Horse { Name = "Scout", SamuraiId = 2 };
            _context.Add(horse);
            _context.SaveChanges();
        }
        private static void AddNewHorseToSamuraiObject()
        {
            var samurai = _context.Samurais.Find(12);
            samurai.Horse = new Horse { Name = "Black Beauty" };
            _context.SaveChanges();
        }
        private static void AddNewHorseToDisconnectedSamuraiObject()
        {
            var samurai = _context.Samurais.AsNoTracking().FirstOrDefault(s=>s.Id==5);
            samurai.Horse = new Horse { Name = "Mr. Ed" };
            
            using var newContext = new SamuraiContext();
            newContext.Samurais.Attach(samurai);
            newContext.SaveChanges();
        }

        private static void ReplaceAHorse()
        {
            //var samurai = _context.Samurais.Include(s=>s.Horse)
            //                      .FirstOrDefault(s => s.Id == 5);
            //samurai.Horse = new Horse { Name = "Trigger" };
            var horse = _context.Set<Horse>().FirstOrDefault(h => h.Name == "Mr. Ed");
            horse.SamuraiId = 5; //owns Trigger! savechanges will fail
            _context.SaveChanges();

        }
    }
}

  