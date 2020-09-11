namespace Kritikos.JwtConfig
{
	using System;
	using System.Text;

	using Microsoft.AspNetCore.Authentication.JwtBearer;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.IdentityModel.Tokens;

	public static class JwtExtensions
	{
		public static void AddJwt(this IServiceCollection services, JwtConfiguration config)
			=> services.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(o =>
				{
					o.SaveToken = true;
					o.Audience = config.Audience;
					o.ClaimsIssuer = config.Issuer;
					var signingKey = Encoding.UTF8.GetBytes(config.Key);
					var key = new SymmetricSecurityKey(signingKey);
					o.TokenValidationParameters = new TokenValidationParameters
					{
						ValidIssuer = config.Issuer,
						ValidateIssuer = true,
						ValidAudience = config.Audience,
						ValidateAudience = true,
						IssuerSigningKey = key,
						ValidateIssuerSigningKey = true,
						ClockSkew = TimeSpan.Zero,
					};
				});
	}
}
