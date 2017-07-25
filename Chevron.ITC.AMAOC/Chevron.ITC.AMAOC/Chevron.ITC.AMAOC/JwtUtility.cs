using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC
{
    public class JwtUtility
    {
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static IDictionary<string, object> GetClaims(string rawToken)
        {
            string[] tokenParts = rawToken.Split('.');

            if (tokenParts.Length != 3)
            {
                throw new InvalidTokenException("Invalid user token");
            }

            string mobileAppToken = GetDecodedPayload(tokenParts[1]);

            return JsonConvert.DeserializeObject<Dictionary<string, object>>(mobileAppToken);
        }

        public static DateTime? GetTokenExpiration(string token)
        {
            IDictionary<string, object> claims = GetClaims(token);

            if (claims.ContainsKey(JwtClaimNames.Expiration))
            {
                int secondsFromEpoch = int.Parse(claims[JwtClaimNames.Expiration].ToString());

                return (UnixEpoch + TimeSpan.FromSeconds(secondsFromEpoch)).ToUniversalTime();
            }

            return null;
        }

        public static string GetDecodedPayload(string tokenPayload)
        {
            if (tokenPayload.Length % 4 == 1)
            {
                throw new InvalidTokenException("Invalid user token");
            }

            //Replace 62nd and 63rd char of encoding
            tokenPayload = tokenPayload.Replace("-", "+");
            tokenPayload = tokenPayload.Replace("_", "/");

            // Add padding:
            int padding = 4 - (tokenPayload.Length % 4);
            tokenPayload = tokenPayload.PadRight(tokenPayload.Length + (padding % 4), '=');

            byte[] tokenPayloadBytes = Convert.FromBase64String(tokenPayload);

            return Encoding.UTF8.GetString(tokenPayloadBytes, 0, tokenPayloadBytes.Length);
        }
    }
}
