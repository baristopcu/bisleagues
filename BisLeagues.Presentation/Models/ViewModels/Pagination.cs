using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Models.ViewModels
{
    public class Pagination
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalLineCount { get; set; }
        public int TotalPageCount { get; set; }
        public Pagination()
        {
            this.PageNumber = 1;
            this.PageSize = 5;
        }
        public Pagination(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 20 ? 20 : pageSize;
        }

        public int GetSkipCount()
        {
            if (this.PageNumber > 1)
            {
                return (this.PageNumber - 1) * this.PageSize;

            }
            else
            {
                return 0;
            }
        }

        public int GetPageSize()
        {
            return this.PageSize;
        }
    }
}
