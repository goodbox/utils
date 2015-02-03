using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodboxDesign.Utils.Entities
{
    public class PagedDataResult<T>
    {
        public IEnumerable<T> Items { get; set; }

        public PagedDataResult()
        {
            this.Items = new List<T>();
        }

        public PagedDataResult(IEnumerable<T> items)
        {
            this.Items = items;
        }

        public int TotalRecords { get; set; }

        public int RecordsPerPage { get; set; }

        private int? totalPages;

        public int TotalPages
        {
            get
            {
                if(!totalPages.HasValue)
                {
                    if(RecordsPerPage > 0)
                    {
                        totalPages = (((TotalRecords % RecordsPerPage == 0) ? (TotalRecords / RecordsPerPage) : (TotalRecords / RecordsPerPage) + 1));
                    }
                    else
                    {
                        totalPages = 0;
                    }
                }
                return totalPages.Value;
            }
        }

        public int CurrentPage { get; set; }

        public string SortExpression { get; set; }

        public string SortDirection { get; set; }
    }
}
