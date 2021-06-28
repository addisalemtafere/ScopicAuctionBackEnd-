﻿namespace Api.Extensions
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtensions
    {
        public static IConfigurationSection GetJwtSecretSection(this IConfiguration configuration)
        {
            return configuration.GetSection("JwtSettings");
        }

        public static IConfigurationSection GetRedisSection(this IConfiguration configuration)
        {
            return configuration.GetSection("RedisCacheSettings");
        }

        public static string GetSendGridApiKey(this IConfiguration configuration)
        {
            return configuration.GetSection("SendGrid:ApiKey").Value;
        }

        public static string GetCloudinaryCloudName(this IConfiguration configuration)
        {
            return configuration.GetSection("Cloudinary:CloudName").Value;
        }

        public static string GetCloudinaryApiKey(this IConfiguration configuration)
        {
            return configuration.GetSection("Cloudinary:ApiKey").Value;
        }

        public static string GetCloudinaryApiSecret(this IConfiguration configuration)
        {
            return configuration.GetSection("Cloudinary:ApiSecret").Value;
        }
    }
}