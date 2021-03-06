namespace Kritikos.ProvingGrounds.Hasher
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using System.Security.Cryptography;

	using Microsoft.AspNetCore.Cryptography.KeyDerivation;

	public class PasswordHasher
	{
		private readonly int iterCount;
		private readonly RandomNumberGenerator rng;

		public PasswordHasher(RandomNumberGenerator? rng = null, int iterCount = 10000)
		{
			this.rng = rng ?? RandomNumberGenerator.Create();
			this.iterCount = iterCount;
		}

		/// <summary>
		/// Returns a hashed representation of the supplied <paramref name="password"/>.
		/// </summary>
		/// <param name="password">The password to hash.</param>
		/// <returns>A hashed representation of the supplied <paramref name="password"/>.</returns>
		public virtual string HashPassword(string password)
		{
			if (password == null)
			{
				throw new ArgumentNullException(nameof(password));
			}

			return Convert.ToBase64String(HashPasswordV3(password, rng));
		}

		/// <summary>
		/// Returns the result of a password hash comparison.
		/// </summary>
		/// <param name="hashedPassword">The hash value for a user's stored password.</param>
		/// <param name="providedPassword">The password supplied for comparison.</param>
		/// <returns>The result of a password hash comparison.</returns>
		/// <remarks>Implementations of this method should be time consistent.</remarks>
		public virtual bool VerifyHashedPassword(string hashedPassword, string providedPassword)
		{
			if (hashedPassword == null)
			{
				throw new ArgumentNullException(nameof(hashedPassword));
			}

			if (providedPassword == null)
			{
				throw new ArgumentNullException(nameof(providedPassword));
			}

			var decodedHashedPassword = Convert.FromBase64String(hashedPassword);

			// read the format marker from the hashed password
			return decodedHashedPassword.Length != 0 && VerifyHashedPasswordV3(decodedHashedPassword, providedPassword, out _);
		}

		private static byte[] HashPasswordV3(
			string password,
			RandomNumberGenerator rng,
			KeyDerivationPrf prf,
			int iterCount,
			int saltSize,
			int numBytesRequested)
		{
			// Produce a version 3 (see comment above) text hash.
			var salt = new byte[saltSize];
			rng.GetBytes(salt);
			var subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

			var outputBytes = new byte[13 + salt.Length + subkey.Length];
			outputBytes[0] = 0x01; // format marker
			WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
			WriteNetworkByteOrder(outputBytes, 5, (uint)iterCount);
			WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
			Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
			Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);
			return outputBytes;
		}

		private static bool VerifyHashedPasswordV3(byte[] hashedPassword, string password, out int iterCount)
		{
			iterCount = default;

			try
			{
				// Read header information
				var prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
				iterCount = (int)ReadNetworkByteOrder(hashedPassword, 5);
				var saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);

				// Read the salt: must be >= 128 bits
				if (saltLength < 128 / 8)
				{
					return false;
				}

				var salt = new byte[saltLength];
				Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);

				// Read the subkey (the rest of the payload): must be >= 128 bits
				var subkeyLength = hashedPassword.Length - 13 - salt.Length;
				if (subkeyLength < 128 / 8)
				{
					return false;
				}

				var expectedSubkey = new byte[subkeyLength];
				Buffer.BlockCopy(hashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

				// Hash the incoming password and verify it
				var actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subkeyLength);
				return ByteArraysEqual(actualSubkey, expectedSubkey);
			}
			catch
			{
				// This should never occur except in the case of a malformed payload, where
				// we might go off the end of the array. Regardless, a malformed payload
				// implies verification failed.
				return false;
				throw;
			}
		}

		// Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		private static bool ByteArraysEqual(IReadOnlyList<byte> a, IReadOnlyList<byte> b)
		{
			if (a == null && b == null)
			{
				return true;
			}

			if (a == null || b == null || a.Count != b.Count)
			{
				return false;
			}

			var areSame = true;
			for (var i = 0; i < a.Count; i++)
			{
				areSame &= a[i] == b[i];
			}

			return areSame;
		}

		private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
		{
			buffer[offset + 0] = (byte)(value >> 24);
			buffer[offset + 1] = (byte)(value >> 16);
			buffer[offset + 2] = (byte)(value >> 8);
			buffer[offset + 3] = (byte)(value >> 0);
		}

		private static uint ReadNetworkByteOrder(byte[] buffer, int offset) =>
			((uint)buffer[offset + 0] << 24)
			| ((uint)buffer[offset + 1] << 16)
			| ((uint)buffer[offset + 2] << 8)
			| buffer[offset + 3];

		private byte[] HashPasswordV3(string password, RandomNumberGenerator rng)
			=> HashPasswordV3(
				password,
				rng,
				prf: KeyDerivationPrf.HMACSHA256,
				iterCount: iterCount,
				saltSize: 128 / 8,
				numBytesRequested: 256 / 8);
	}
}
