namespace Kritikos.ProvingGrounds.Hasher
{
	public class PasswordOptions
	{
		public int RequiredLength { get; set; } = 6;

		public bool RequireNonAlphanumeric { get; set; }

		public bool RequireLowercase { get; set; }

		public bool RequireUppercase { get; set; }

		public bool RequireDigit { get; set; }
	}
}
