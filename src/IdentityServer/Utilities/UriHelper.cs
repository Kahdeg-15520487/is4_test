using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Utilities
{
    public static class UriHelper
    {
        public static string GetAbsoluteUri(string signoutUri, string authority)
        {
            Uri signOutUri = new Uri(signoutUri, UriKind.RelativeOrAbsolute);
            Uri authorityUri = new Uri(authority, UriKind.Absolute);

            Uri uri = signOutUri.IsAbsoluteUri ? signOutUri : new Uri(authorityUri, signOutUri);
            return uri.AbsoluteUri;
        }
    }
}
