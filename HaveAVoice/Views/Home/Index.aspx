<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<title>have a voice | connecting you to the political world instantaneously</title>
<html>
<body bgcolor="#01b1da">
    <span style="width:100%; margin-left:25%; margin-right:75%; background-color:#01b1da">
        <img src="/Content/images/splash.png" />
    </span>
    <span>
<table style="width:100%">
<tr>
<td style="width:25%"></td>
<td>
            <script src="http://widgets.twimg.com/j/2/widget.js"></script>
            <script>
                new TWTR.Widget({
                    version: 2,
                    type: 'profile',
                    rpp: 5,
                    interval: 6000,
                    width: 600,
                    height: 265,
                    theme: {
                        shell: {
                            background: '#FFFFFF',
                            color: '#01b2da'
                        },
                        tweets: {
                            background: '#FFFFF',
                            color: '#000000',
                            links: '#01b2da'
                        }
                    },
                    features: {
                        scrollbar: true,
                        loop: false,
                        live: true,
                        hashtags: true,
                        timestamp: true,
                        avatars: false,
                        behavior: 'all'
                    }
                }).render().setUser('haveavoice_').start();
            </script>
</td>
</tr>
</table>
    </span>
    <span style="margin-left:75%; margin-right:25%">
		<a href="http://www.facebook.com/haveavoice" target="_blank">
		    <img src="/Content/images/facebook.png" alt="Follow Have a Voice on Facebook!">
		</a>		    	
        <a href="http://twitter.com/haveavoice_" target="_blank">
		    <img src="/Content/images/twitter.png" alt="Follow Have a Voice and it's development on Twitter!">
		</a>
    </span>
</body>
</html>
