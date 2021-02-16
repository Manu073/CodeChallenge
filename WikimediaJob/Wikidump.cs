using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace WikimediaJob
{
    public class Wikidump
    {
        public bool GetData()
        {
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour - 1, 0, 0);
            string year = date.Year.ToString();
            string month = SetDigits(date.Month);
            string day = SetDigits(date.Day);
            string hour = SetDigits(date.Hour);
            string fileName = $"pageviews-{year}{month}{day}-{hour}0000.gz";
            string fileAddress = $"https://dumps.wikimedia.org/other/pageviews/{year}/{year}-{month}/{fileName}";
            var webRequest = WebRequest.Create(fileAddress);


            List<Entity> list = new List<Entity>();
            Entity entity = null;

            using (WebResponse response = webRequest.GetResponse())
            {
                using (Stream content = response.GetResponseStream())
                {
                    using (GZipStream zipFile = new GZipStream(content, CompressionMode.Decompress, true))
                    {
                        using (StreamReader unzipFile = new StreamReader(zipFile))
                        {
                            while (!unzipFile.EndOfStream)
                            {
                                entity = ReadLine(date, unzipFile.ReadLine());
                                if (entity != null)
                                    list.Add(entity);
                            }
                        }
                    }
                }
            }

            BusinessController.BusinessController bc = new BusinessController.BusinessController();
            return bc.InsertWikidump(list);
        }

        public string SetDigits(int datePart)
        {
            return (datePart < 10) ? "0" + datePart.ToString() : datePart.ToString();
        }

        public Entity ReadLine(DateTime date, string line)
        {
            Entity entity = new Entity();
            string[] data = line.Split(' ');
            string[] lanDom = SeparateLanguageDomain(Convert.ToString(data[0]));

            if (lanDom[1] == "mw")
                return null;

            entity.Period = date;
            entity.Language = lanDom[0];
            entity.Domain = lanDom[1];
            entity.PageTitle = Convert.ToString(data[1]);
            entity.ViewCount = Convert.ToInt32(data[2]);
            entity.ResponseSize = Convert.ToInt32(data[3]);

            return entity;
        }

        public string[] SeparateLanguageDomain(string data)
        {
            string[] splittedData = data.Split('.');
            string[] lanDom = new string[2] { splittedData[0], (splittedData.Length == 1) ? "Wikipedia" : GetDomain(splittedData[1]) };

            return lanDom;
        }

        public string GetDomain(string domainCode)
        {
            switch (domainCode)
            {
                case ".b":
                    return "Wikibooks";
                case ".d":
                    return "Wiktionary";
                case ".f":
                    return "Wikimedia foundation";
                case ".mw":
                    return "mw";
                case ".n":
                    return "Wikinews";
                case ".q":
                    return "Wikiquote";
                case ".s":
                    return "Wikisource";
                case ".v":
                    return "Wikiversity";
                case ".voy":
                    return "Wikivoyage";
                case ".w":
                    return "Mediawiki";
                case ".wd":
                    return "Wikidata";
                default:
                    return "Wikipedia";
            }
        }
    }
}