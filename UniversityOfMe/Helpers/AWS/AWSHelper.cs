using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon.S3;

namespace UniversityOfMe.Helpers.AWS {
    public class AWSHelper {
        private static AmazonS3 theAmazonClient;

        public static AmazonS3 GetClient() {
            SetInstance();
            return theAmazonClient;
        }

        private static void SetInstance() {
            if(theAmazonClient == null) {
                theAmazonClient =
                    Amazon.AWSClientFactory.CreateAmazonS3Client(Configuration.SiteConfiguration.AWSAccessKey(), Configuration.SiteConfiguration.AWSSecretKey());
            }
        }
    }
}