using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HaveAVoice.Models;
using HaveAVoice.Models.View;

namespace HaveAVoice.Tests.Models {
    public static class DatamodelFactory {
        public static string FIRST_NAME = "Henryk";
        public static string LAST_NAME = "Henryk";
        public static string FULL_NAME = "Henryk Sarat";
        public static string EMAIL = "henryksarat@yahoo.com";
        public static string PASSWORD = "aPassword";
        public static DateTime BIRTHDAY = new DateTime(1987, 05, 03);
        public static string STATE = "IL";
        public static string CITY = "Chicago";
        public static string USERNAME = "DaBomb";
        public static bool AGREEMENT = true;
        public static bool CAPTCHA_VALID = true;
        public static string IP_ADDRESS = "192.0.0.1";
        public static string FORGOT_PASSWORD_HASH = "KHBK*WY^DDBSUADGBUAAIDB";

        public static CreateUserModelBuilder createUserModelBuilder() {
            return new CreateUserModelBuilder()
                .Email(EMAIL)
                .Username(USERNAME)
                .Password(PASSWORD)
                .City(CITY)
                .FullName(FULL_NAME)
                .DateOfBirth(BIRTHDAY)
                .State(STATE)
                .Agreement(AGREEMENT);
        }
    }
}
