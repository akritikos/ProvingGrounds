namespace Kritikos.ProvingGrounds.Hasher.Tests
{
	using Xunit;

	public class PasswordHasherTests
	{
		private PasswordHasher _hasher = new PasswordHasher();

		[Theory]
		[Trait("Category","Precalculated")]
		[InlineData("6zwd#.", "AQAAAAEAACcQAAAAEDzf+2PNhFLOXJCDzKPuJ7A1sVBD5PJBaD1Qf5UCb46pLHCna31o4Rm2VSDTxC8Alw==")]
		[InlineData("&ua4kC", "AQAAAAEAACcQAAAAEHAWIwIpD99EX2PcL1pfCY/qZEt0MbEKkwNU0qeSRS5kodfxPevInhlLovBQjBXG5Q==")]
		[InlineData("}QBU,+p", "AQAAAAEAACcQAAAAEMi1hq4/ntFxwAAorjcswjewfyzl4YVtiToJOB/Ss47RNbEhkFQQKdgqf7aVbuesgQ==")]
		[InlineData("MYrvO:", "AQAAAAEAACcQAAAAEKVaBESRje+soPQXtbbDC7K/6t2rQFdkRZ1X9JA6upB6bCSZRZisK0U6gEpoJzMj/g==")]
		[InlineData("M\"]dTB", "AQAAAAEAACcQAAAAEB4QWehBNIk1wjp52nl57Sy8NPBVm/UYY42BPFpCk5bEXj0PqYNThuEYJQt5aFqOKQ==")]
		public void PrecalculatedHashes(string password, string hash)
		{
			var result = _hasher.VerifyHashedPassword(hash, password);
			Assert.True(result);
		}
	}
}
