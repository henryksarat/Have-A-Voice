using System;
using System.Web;

namespace Social.Authentication.Helpers {
    public class CookieHelper {
        public const string REMEMBER_ME_COOKIE = "HaveAVoiceRememberMeCookie";
        public const string COOKIE_USER_ID = REMEMBER_ME_COOKIE + "UserId";
        public const string COOKIE_HASH = REMEMBER_ME_COOKIE + "CookieHash";
        public const string COOKIE_EXPIRATION = REMEMBER_ME_COOKIE + "Expiration";
        public const int REMEMBER_ME_COOKIE_HOURS = 1;

        public static void ClearCookies() {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[REMEMBER_ME_COOKIE];
            if (myCookie != null) {
                myCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }

        public static void CreateCookie(int aUserId, string aCookieHash) {
            HttpCookie myUserCookie = new HttpCookie(REMEMBER_ME_COOKIE);
            myUserCookie[COOKIE_USER_ID] = aUserId.ToString();
            myUserCookie[COOKIE_HASH] = aCookieHash;
            myUserCookie.Expires = DateTime.Now.AddHours(REMEMBER_ME_COOKIE_HOURS);

            HttpContext.Current.Response.Cookies.Add(myUserCookie);

        }

        public static int GetUserIdFromCookie() {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[REMEMBER_ME_COOKIE];
            int myUserId = 0;
            if (myCookie != null) {
                myUserId = Int32.Parse(myCookie[COOKIE_USER_ID]);
            }

            return myUserId;
        }

        public static string GetCookieHashFromCookie() {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[REMEMBER_ME_COOKIE];
            string myCookieHash = string.Empty;
            if (myCookie != null) {
                myCookieHash = myCookie[COOKIE_HASH];
            }

            return myCookieHash;
        }
    }
}