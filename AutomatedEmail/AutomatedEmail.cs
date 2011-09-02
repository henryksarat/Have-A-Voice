/*******************************************************************************
* Copyright 2009-2011 Amazon.com, Inc. or its affiliates. All Rights Reserved.
* 
* Licensed under the Apache License, Version 2.0 (the "License"). You may
* not use this file except in compliance with the License. A copy of the
* License is located at
* 
* http://aws.amazon.com/apache2.0/
* 
* or in the "license" file accompanying this file. This file is
* distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied. See the License for the specific
* language governing permissions and limitations under the License.
*******************************************************************************/

using System;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;

using Amazon.S3;
using Amazon.S3.Model;
using System.Threading;
using Amazon.SimpleEmail;
using AutomatedEmail.Services;

namespace GettingStartedGuide {
    class S3Sample {
        static AmazonSimpleEmailServiceClient theClient;

        public static void Main(string[] args) {
            if (checkRequiredFields()) {
                NameValueCollection appConfig = ConfigurationManager.AppSettings;

                string accessKeyID = appConfig["AWSAccessKey"];
                string secretAccessKeyID = appConfig["AWSSecretKey"];
                theClient = new AmazonSimpleEmailServiceClient(accessKeyID, secretAccessKeyID);
                int myTimeToSleep = 30000;

                if (args.Count<string>() == 1) {
                    myTimeToSleep = Int16.Parse(args[0]);
                }

                ConsoleMessageWithDate("Started Automated Email with a delay of " + myTimeToSleep);

                IEmailService myEmailService = new EmailService();

                while (true) {
                    myEmailService.SendEmails(theClient);
                    Thread.Sleep(myTimeToSleep);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static bool checkRequiredFields() {
            NameValueCollection appConfig = ConfigurationManager.AppSettings;

            if (string.IsNullOrEmpty(appConfig["AWSAccessKey"])) {
                Console.WriteLine("AWSAccessKey was not set in the App.config file.");
                return false;
            }
            if (string.IsNullOrEmpty(appConfig["AWSSecretKey"])) {
                Console.WriteLine("AWSSecretKey was not set in the App.config file.");
                return false;
            }

            return true;
        }

        private static void ConsoleMessageWithDate(string aMessage) {
            Console.WriteLine(DateTime.UtcNow.ToString() + ": " + aMessage);
        }
    }
}