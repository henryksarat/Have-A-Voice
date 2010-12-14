using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using HaveAVoice.Models;

namespace HaveAVoice.Helpers {
    public class CloneHelper {
        public static User CloneUserWithReflection(User user) {
            // Get all the fields of the myType, also the privates.
            FieldInfo[] fields = user.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            // Create a new object
            User newUser = new User();
            // Loop through all the fields and copy the information from the parameter class
            // to the newUser object.
            foreach (FieldInfo field in fields) {
                field.SetValue(newUser, field.GetValue(user));
            }
            // Return the cloned object.
            return newUser;
        }

        public static Issue CloneIssueWithReflection(Issue anIssue) {
            FieldInfo[] fields = anIssue.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            Issue newIssue = new Issue();
            foreach (FieldInfo field in fields) {
                field.SetValue(newIssue, field.GetValue(anIssue));
            }

            return newIssue;
        }
    }
}
