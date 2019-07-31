namespace Kritikos.ProvingGrounds.Hasher
{
	using System;
	using System.Text;

	public class RandomPasswordGenerator
	{
		public RandomPasswordGenerator(PasswordOptions options = null, Random rnd = null)
		{
			Options = options ?? new PasswordOptions();
			Random = rnd ?? new Random();
		}

		private PasswordOptions Options { get; }

		private Random Random { get; }

		public string GeneratePassword()
		{
			var nonAlphanumericMissing = Options.RequireNonAlphanumeric;
			var digitMissing = Options.RequireDigit;
			var lowercaseMissing = Options.RequireLowercase;
			var uppercaseMissing = Options.RequireUppercase;

			var password = new StringBuilder(Options.RequiredLength);
			while (password.Length < Options.RequiredLength)
			{
				var c = (char)Random.Next(32, 126);
				password.Append(c);

				digitMissing &= !char.IsDigit(c);
				lowercaseMissing &= !char.IsLower(c);
				uppercaseMissing &= !char.IsUpper(c);
				nonAlphanumericMissing &= !char.IsLetterOrDigit(c);
			}

			if (nonAlphanumericMissing)
			{
				password.Append((char)Random.Next(33, 48));
			}

			if (digitMissing)
			{
				password.Append((char)Random.Next(48, 58));
			}

			if (lowercaseMissing)
			{
				password.Append((char)Random.Next(97, 123));
			}

			if (uppercaseMissing)
			{
				password.Append((char)Random.Next(65, 91));
			}

			return password.ToString();
		}
	}
}
