using Bookify.web.Core.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.web.Core.ViewModel
{
	public class BooksReportViewModel
	{
        public List<int>? SelectedAuthors { get; set; } = new List<int>();
        public IEnumerable<SelectListItem> Authors { get; set; }= new List<SelectListItem>();
		public List<int>? SelectedCategories { get; set; } = new List<int>();
		public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        public PaginatedList<Book> Books { get; set; }
    }
}
