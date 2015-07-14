using System;

namespace NetatmoBot.Model
{
    public class AuthenticationToken
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; }

        /// <summary>
        /// Returns true if the token is within 10 minutes of expiring.
        /// </summary>
        /// <returns></returns>
        public bool IsCloseToExpiry()
        {
            return ExpiresAt.Subtract(DateTime.UtcNow).TotalMinutes < 10;
        }
    }
}