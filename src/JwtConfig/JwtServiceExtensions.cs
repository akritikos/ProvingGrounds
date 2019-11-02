using System;
using System.Collections.Generic;
using System.Text;
using JwtConfig;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Kritikos.JwtConfig
{
	public static class JwtServiceExtensions
	{
		public static void WithJwt(this IServiceCollection services, Jwt config)
		{
			services.AddAuthentication(options =>
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
}
