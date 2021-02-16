using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Entities;

namespace Wikimedia.Algorithm
{
    public class WikiDump
    {
        BusinessController.BusinessController bc = new BusinessController.BusinessController();

        public List<Entity> GetLanguageDomainTop(DateTime date)
        {
            DateTime dateFrom, dateTo;
            dateTo = date.AddDays(-1);
            dateFrom = date.AddDays(-6);
            List<Entity> list = bc.GetLanguageDomainTop(dateFrom, dateTo);
            
            return list;
        }

        public List<Entity> GetPageTop(DateTime date)
        {
            DateTime dateFrom, dateTo;
            dateTo = date.AddDays(-1);
            dateFrom = date.AddDays(-6);
            List<Entity> list = bc.GetPageTop(dateFrom, dateTo);

            return list;
        }
    }
}