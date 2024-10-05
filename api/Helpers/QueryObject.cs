using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class QueryObject
    {
        public string? Symbol { get; set; } = null;
        public string? CompanyName { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsDescending {get; set;} = false; 
        public int PageNumber { get; set; } = 1; //by default it will give us the first page
        public int PageSize { get; set; } = 20; //number of stocks to show in a page
    }
}