using System.IO;
using System.Web.UI;

namespace Social.Generic.Helpers {
    public class CaptchaHelper {
         public static string GenerateCaptcha() {       
             var captchaControl = new Recaptcha.RecaptchaControl  
                     {  
                         ID = "recaptcha",  
                         Theme = "blackglass",  
                         PublicKey = "6LeoSLoSAAAAAF177SrtbX0HIpkpG0RDFDdR_zT6",  
                         PrivateKey = "6LeoSLoSAAAAAOdfYpGR-mRktL4IGSjcj4EdhBb_"  
                      };  
           
             var htmlWriter = new HtmlTextWriter( new StringWriter() );  
             captchaControl.RenderControl(htmlWriter);  
             return htmlWriter.InnerWriter.ToString();  
         }  
    }
}
