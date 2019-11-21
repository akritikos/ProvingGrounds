namespace Kritikos.JwtConfigTests
{
	using Kritikos.JwtConfig;
	using Xunit;

	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			var jwt = new JwtConfiguration { Audience = "everyone", Issuer = "me", Key = "very secret key", };

			Assert.NotNull(jwt);
			Assert.True(true);
		}
	}
}
