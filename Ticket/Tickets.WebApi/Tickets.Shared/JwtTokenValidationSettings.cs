using System;

namespace Tickets.Shared
{
    public class JwtTokenValidationSettings
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string SecretKey { get; set; }
        public int Duration { get; set; }
    }
}
