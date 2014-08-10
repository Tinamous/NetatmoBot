using System;

namespace NetatmoBot.Model
{
    public class AuthenticationToken
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; }
    }
}