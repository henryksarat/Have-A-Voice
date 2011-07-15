using System.IO;
using System.Web.UI;

namespace Social.Generic.Helpers {
    public class CaptchaHelper {
         public static string GenerateCaptcha() {       
             var captchaControl = new Recaptcha.RecaptchaControl  
                     {  
                         ID = "recaptcha",  
                         Theme = "blackglass",
                         PublicKey = "6LcPJMYSAAAAAOJpX1mzOgwRGYAL7MabbzGm4DZ9",
                         PrivateKey = "6LcPJMYSAAAAAN5fSA_9ZaCZu7Yjqj1j9_kzSIri "  
                      };  
           
             var htmlWriter = new HtmlTextWriter( new StringWriter() );  
             captchaControl.RenderControl(htmlWriter);  
             return htmlWriter.InnerWriter.ToString();  
         }  
    }
}
