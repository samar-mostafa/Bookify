namespace Bookify.web.Core.ViewModel
{
	public class RentalViewModel
	{
		public int Id { get; set; }
		public SubscriberViewModel? Subscriber { get; set; }

		public DateTime StartAt { get; set; }=DateTime.Today;
        public DateTime CreatedOn { get; set; }
        public bool PenaltyPaid { get; set; }

		public IEnumerable<RentalCopyViewModel> RentalCopies { get; set; } = new List<RentalCopyViewModel>();

		public int TotalDelayInDays { get {
				return RentalCopies.Sum(c => c.DelayInDays);
			
			} }

		public int TotalNumberOfCopy { get
			{
				return RentalCopies.Count();
			} }
	}
}
