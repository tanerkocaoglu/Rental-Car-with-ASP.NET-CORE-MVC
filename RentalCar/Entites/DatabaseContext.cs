namespace RentalCar.Entites
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<Contact> Contacts { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Reservation> Reservations { get; set; }
		public DbSet<Car> Cars { get; set; }

	}
}
