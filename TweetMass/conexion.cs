using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;
using Hammock;
using System.Diagnostics;

namespace TweetMass
{
    class conexion
    {
        /*TwitterService servicio = new TwitterService("wvDWmU9u9NxitbA3R1mhGQ", "6PvMIZeLYos5DBZgKYjzLnQIHn67PJmzRgn15RGd6Ek");

        public conexion()
        {
            OAuthRequestToken requestToken = servicio.GetRequestToken();
            string authurl = servicio.GetAuthenticationUrl(requestToken).ToString();
            Process.Start(authurl);

            OAuthAccessToken accessToken = servicio.GetAccessToken(requestToken, pin);

            servicio.AuthenticateWith(accessToken.Token, accessToken.TokenSecret); //Si todo va bien, hasta aquí se logró conectar con el servicio

            Console.WriteLine("Bienvenido {0}", accessToken.ScreenName);
        }
        public TwitterService getservicio()
        {
            return servicio;
        }*/
    }
}
