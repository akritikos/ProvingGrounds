namespace Kritikos.ProvingGrounds.Runner
{
	using System;

	public static class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			Console.WriteLine($"Args used: {args}");
			var hasher = new PasswordHasher();
			hasher.VerifyHashedPassword(
				"AQAAAAEAACcQAAAAEJc77umxwOoUmdXwhe5sgCx0hoqxi50fZ/f+XclcqnhArSEUMMsNVpcYgHlWfvYYQQ==",
				"123456");
			var generator = new RandomPasswordGenerator();
			var t = generator.GeneratePassword();
		}
	}
}
