namespace Kritikos.ProvingGrounds.Runner
{
	using System;
	using System.IO;

	using Kritikos.ProvingGrounds.Hasher;

	public static class Program
	{
		public static void Main()
		{
			Console.WriteLine("Enter Password to hash:");
			var password = Console.ReadLine();
			var hasher = new PasswordHasher();
			var hash = hasher.HashPassword(password);
			Console.WriteLine($"Calculated hash:\n{hash}");
			Console.WriteLine("Enter hash to verify");
			var verify = Console.ReadLine();
			Console.WriteLine("Enter password of that hash:");
			var pass = Console.ReadLine();
			using var sw = new MemoryStream();
			Console.WriteLine(hasher.VerifyHashedPassword(verify, pass));
			Console.ReadLine();
		}
	}
}
