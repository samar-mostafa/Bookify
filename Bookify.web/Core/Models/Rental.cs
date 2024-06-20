namespace Bookify.web.Core.Models
{
	public class Rental:BaseModel
	{
		public int Id { get; set; }
		public int SubscriperId { get; set; }
		public Subscriper? Subscriper { get; set; }
		public DateTime StartAt { get; set; } = DateTime.Today;
		public bool PenaltyPaid { get; set; }

		public ICollection<RentalCopy> RentalCopies { get; set; } = new List<RentalCopy>();
	}
}
