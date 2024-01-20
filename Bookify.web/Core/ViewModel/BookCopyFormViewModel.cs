namespace Bookify.web.Core.ViewModel
{
    public class BookCopyFormViewModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }

        [Display(Name = "Is Available For Rental?")]
        public bool IsAvailableForRental { get; set; }

        [Display(Name ="Edition Number")]
        [Range(0,1000 ,ErrorMessage =Errors.InvalidRange)]
        public int EditionNumber { get; set; }
        public bool ShowRentalInput { get; set; }
    }
}
