namespace Kritikos.JwtConfigTests
{
  using Kritikos.JwtConfig;

  using Xunit;

	public class SillyTests
	{
		[Fact]
		public void FooFact()
		{
			var jwt = new JwtConfiguration();

			Assert.NotNull(jwt.Audience);
		}
	}
}
