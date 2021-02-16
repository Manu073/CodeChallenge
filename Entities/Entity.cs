using System;

namespace Entities
{
    public class Entity
    {
        //Period from where the data were requested
        public DateTime Period { get; set; }
        //Language of the requested page
        public string Language { get; set; }
        //Domain of the requested page
        public string Domain { get; set; }
        //Title of the requested page
        public string PageTitle { get; set; }
        //The number of times this page has been viewed
        public int ViewCount { get; set; }
        //The total response size caused by the requests for this page
        public int ResponseSize { get; set; }
    }
}