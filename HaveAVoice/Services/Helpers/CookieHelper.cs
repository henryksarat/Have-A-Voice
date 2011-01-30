using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Services.Helpers {
    public class CookieHelper {
        public const string REMEMBER_ME_COOKIE = "HaveAVoiceRememberMeCookie";
        public const string COOKIE_USER_ID = REMEMBER_ME_COOKIE + "UserId";
        public const string COOKIE_HASH = REMEMBER_ME_COOKIE + "CookieHash";
        public const string COOKIE_EXPIRATION = REMEMBER_ME_COOKIE + "Expiration";
        public const int REMEMBER_ME_COOKIE_HOURS = 4;

        public static void ClearCookies() {
            HttpContext.Current.Request.Cookies.Remove(REMEMBER_ME_COOKIE);
            HttpContext.Current.Response.Cookies.Remove(REMEMBER_ME_COOKIE);
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
            /*
            int myUserId = 0;
            if (HttpContext.Current.Request.Cookies[COOKIE_USER_ID].Value != null) {
                myUserId = Int32.Parse(HttpContext.Current.Request.Cookies[CookieHelper.COOKIE_USER_ID].Value);
            }
            return myUserId;*/
        }

        public static string GetCookieHashFromCookie() {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[REMEMBER_ME_COOKIE];
            string myCookieHash = string.Empty;
            if (myCookie != null) {
                myCookieHash = myCookie[COOKIE_HASH];
            }

            return myCookieHash;
            
            /*string myCookieHash = string.Empty;
            if (HttpContext.Current.Request.Cookies[CookieHelper.COOKIE_HASH].Value != null) {
                myCookieHash = HttpContext.Current.Request.Cookies[COOKIE_HASH].Value;
            }
            return myCookieHash;*/
        }
    }
}