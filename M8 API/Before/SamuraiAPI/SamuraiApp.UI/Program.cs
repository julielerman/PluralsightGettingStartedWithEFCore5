using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using System;
using System.Linq;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContextNoTracking _contextNT = new SamuraiContextNoTracking();
        private static SamuraiContext _context = new SamuraiContext();

        private static void Main(string[] args)
        {
            //QuerySamuraiBattleStats();
            //QueryUsingRawSql();
            QueryRelatedUsingRawSql();
            //QueryUsingRawSqlWithInterpolation();
            //DANGERQueryUsingRawSqlWithInterpolation();
            //QueryUsingFromSqlRawStoredProc();
            //QueryUsingFromSqlIntStoredProc();
           // ExecuteSomeRawSql();
        }

        private static void QuerySamuraiBattleStats()
        {
            // var stats = _context.SamuraiBattleStats.ToList();
            //var firststat = _context.SamuraiBattleStats.FirstOrDefault();
            var sampsonState = _context.SamuraiBattleStats
               .FirstOrDefault(b => b.Name == "SampsonSan");
            // var findone = _context.SamuraiBattleStats.Find(2);

        }
        private static void QueryUsingRawSql()
        {
             var samurais = _context.Samurais.FromSqlRaw("Select * from samurais").ToList();
             //var samurais = _context.Samurais.FromSqlRaw(
             //    "Select Id, Name, Quotes, Battles, Horse from Samurais").ToList();

        }
        private static void QueryRelatedUsingRawSql()
        {
            var samurais = _contextNT.Samurais.FromSqlRaw(
                "Select Id, Name from Samurais").Include(s=>s.Quotes).ToList();

        }
        private static void QueryUsingRawSqlWithInterpolation()
        {
            string name = "Kikuchyo";
            var samurais = _context.Samurais
                .FromSqlInterpolated($"Select * from Samurais Where Name= {name}")
                .ToList();
        }
        private static void DANGERQueryUsingRawSqlWithInterpolation()
        {
            string name = "Kikuchyo";
            var samurais = _context.Samurais
                .FromSqlRaw($"Select * from Samurais Where Name= '{name}'")
                .ToList();
        }
        private static void QueryUsingFromSqlRawStoredProc()
        {
            var text = "Happy";
            var samurais = _context.Samurais.FromSqlRaw(
             "EXEC dbo.SamuraisWhoSaidAWord {0}", text).ToList();
        }
        private static void QueryUsingFromSqlIntStoredProc()
        {
            var text = "Happy";
            var samurais = _context.Samurais.FromSqlInterpolated(
             $"EXEC dbo.SamuraisWhoSaidAWord {text}").ToList();
        }
        
        private static void ExecuteSomeRawSql()
        {
            //var samuraiId = 2;
            //var affected= _context.Database
            //    .ExecuteSqlRaw("EXEC DeleteQuotesForSamurai {0}", samuraiId) ;
            var samuraiId = 2;
            var affected = _context.Database
                .ExecuteSqlInterpolated($"EXEC DeleteQuotesForSamurai {samuraiId}");
        }

    }
} 

