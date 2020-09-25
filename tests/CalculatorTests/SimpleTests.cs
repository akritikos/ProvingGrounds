using System;

using Xunit;

using Kritikos.Calculator;

namespace Kritikos.CalculatorTests
{
	public class SimpleTests
	{
		private static readonly Random Random = new Random();

		[Fact]
		public void Addition()
		{
			for (var count = 0; count < 100; count++)
			{
				var i = Random.Next();
				var j = Random.Next();

				var result = Calc.Add(i, j);
				Assert.Equal(i + j, result);
			}
		}

		[Fact]
		public void Subtraction()
		{
			for (var count = 0; count < 100; count++)
			{
				var i = Random.Next();
				var j = Random.Next();

				var result = Calc.Subtract(i, j);
				Assert.Equal(i - j, result);
				Assert.Equal(result, Calc.Add(i, j * -1));
			}
		}
	}
}
