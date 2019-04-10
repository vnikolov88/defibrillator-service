using System.Collections.Generic;

namespace DefibrillatorService.Models
{
    public class PagedSearch<TItemsModel>
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public List<TItemsModel> Items { get; set; }
    }
}
