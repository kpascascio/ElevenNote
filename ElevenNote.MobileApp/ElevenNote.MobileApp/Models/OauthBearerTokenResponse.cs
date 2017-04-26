using System;
using System.Collections.Generic;
using System.Text;

namespace ElevenNote.MobileApp.Models
{
    internal class OauthBearerTokenResponse
    {
        // when we deserialize this from JSON, that important because this is what is in the string that is an exact replica.
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
    }
}
