namespace Bookify.web.Core.ViewModel
{
    public class RentalReportViewModel
    {
       
        public string Duration { get; set; } =null!;
        public PaginatedList<RentalCopy> Rentals { get; set; }
    }
}
