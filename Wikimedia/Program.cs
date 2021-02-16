using Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wikimedia.Algorithm;
using Wikimedia.Utilities;

namespace Wikimedia
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            DateTime date = DateTime.Now.Date;
            var task1 = Task.Run(() => GetLanDomain(date));
            var task2 = Task.Run(() => GetPage(date));

            Console.Write("Getting the information, please wait...");

            var l1 = await task1;
            var l2 = await task2;

            Console.Clear();

            Console.Write("Language & Domain count" + Environment.NewLine);
            var t1 = new TablePrinter("Period", "Language", "Domain", "View Count");
            foreach (Entity item in l1)
            {
                t1.AddRow(item.Period.ToShortDateString(), item.Language, item.Domain, item.ViewCount.ToString());
            }
            t1.Print();
            Console.WriteLine();
            Console.Write("Language page max view count" + Environment.NewLine);
            var t2 = new TablePrinter("Period", "Page", "View Count");
            foreach (Entity item in l2)
            {
                t2.AddRow(item.Period.ToShortDateString(), item.PageTitle, item.ViewCount.ToString());
            }
            t2.Print();

            Console.ReadKey();
        }

        private static async Task<List<Entity>> GetLanDomain(DateTime date)
        {
            WikiDump wiki = new WikiDump();
            List<Entity> list1 = wiki.GetLanguageDomainTop(date);

            return list1;
        }

        private static async Task<List<Entity>> GetPage(DateTime date)
        {
            WikiDump wiki = new WikiDump();
            List<Entity> list2 = wiki.GetPageTop(date);

            return list2;
        }
    }
}