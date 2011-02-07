using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers;

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
        public static bool AGREEMENT = true;
        public static bool CAPTCHA_VALID = true;
        public static string IP_ADDRESS = "192.0.0.1";
        public static string FORGOT_PASSWORD_HASH = "KHBK*WY^DDBSUADGBUAAIDB";
        public static DateTime LAST_LOGIN = new DateTime(2010, 10, 08);
        public static DateTime REGISTRATION_DATE = new DateTime(2010, 10, 07);
        public static string UTC_OFFSET = "-6:00";

        public static CreateUserModelBuilder createUserModelBuilder() {
            return new CreateUserModelBuilder() {
                Email = EMAIL,
                Password = PASSWORD,
                City = CITY,
                FirstName = FIRST_NAME,
                LastName = LAST_NAME,
                DateOfBirth = BIRTHDAY,
                State = STATE,
                Agreement = AGREEMENT
            };
        }

        public static User createUser(int anId) {
            return User.CreateUser(anId, EMAIL, PASSWORD, FIRST_NAME, LAST_NAME, CITY, STATE, BIRTHDAY, LAST_LOGIN, REGISTRATION_DATE, IP_ADDRESS, UTC_OFFSET, "M");
        }
    }
}
