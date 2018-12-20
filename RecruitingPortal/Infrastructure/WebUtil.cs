using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using RecruitingPortal.Models;

namespace RecruitingPortal.Infrastructure
{
    public enum EnumPageType
    {
        REGISTRATION = 1
    }

    public enum ReportType
    {
        JOBPOSTING,
        GUARDREQUEDST
    }

    public static class WebUtil
    {
        //public static readonly string msgLoginSuccess = "Login Success";
        //public static readonly string msgLoginFail = "Login Fail";
        //public static readonly string msgActivationRequired = "Activation Required";
        //public static readonly string msgRegisterSuccess = "Register Success";
        //public static readonly string msgRegisterFail = "Register Fail";
        //public static readonly string msgSuccess = "Success";
        //public static readonly string msgFail = "Fail";         
        public static readonly string templateName = "EmailTemplate.html";

        public static string QuoteStr(string Key, string Value)
        {
            return ("\"" + Key + "\":\"" + Value + "\"");
        }

        public static string QuoteStr(string Value)
        {
            return ("\"" + Value + "\"");
        }

        public static string RemoveLineEndings(string value)
        {
            /*
            LF: Line Feed, U+000A
            VT: Vertical Tab, U+000B
            FF: Form Feed, U+000C
            CR: Carriage Return, U+000D
            CR+LF: CR (U+000D) followed by LF (U+000A)
            NEL: Next Line, U+0085
            LS: Line Separator, U+2028
            PS: Paragraph Separator, U+2029
            */
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();
            string lineFeedSeparator = ((char)0x000A).ToString();
            string verticalTabSeparator = ((char)0x000B).ToString();
            string formFeedSeparator = ((char)0x000C).ToString();
            string carriageReturnSeparator = ((char)0x000D).ToString();
            string nextLineSeparator = ((char)0x0085).ToString();

            return value.Replace("\r\n", string.Empty).Replace("\n", string.Empty)
                                                      .Replace("\r", string.Empty)
                                                      .Replace(lineSeparator, string.Empty)
                                                      .Replace(paragraphSeparator, string.Empty)
                                                      .Replace(lineFeedSeparator, string.Empty)
                                                      .Replace(verticalTabSeparator, string.Empty)
                                                      .Replace(formFeedSeparator, string.Empty)
                                                      .Replace(carriageReturnSeparator, string.Empty)
                                                      .Replace(nextLineSeparator, string.Empty)
                                                      .Replace(carriageReturnSeparator + lineFeedSeparator, string.Empty);
        }

        public static void PreventMultipleClicks(ref System.Web.UI.WebControls.LinkButton button)
        {
            if (button.ID == "lbtnChange") // do not create __doPostBack of linkButton. Let jQuery to submit
            {
                button.Attributes.Add("onclick", "document.getElementById('" + button.ClientID + "').disabled=true;");
            }
            else // create __doPostBack of linkButton
            {
                button.Attributes.Add("onclick", "this.disabled=true;" + button.Page.ClientScript.GetPostBackEventReference(button, String.Empty).ToString());
            }
        }

        public static void ClearDomainCookie(HttpContext context)
        {
            // ref: http://stackoverflow.com/questions/412300/formsauthentication-signout-does-not-log-the-user-out
            FormsAuthentication.SignOut();
            context.Session.Clear();
            context.Session.Abandon();

            // Note: Overriding the existing FormsAuthentication cookie with a new empty cookie ensures that even if the client winds back their system clock, they will still not be able to retrieve any user data from the cookie.
            // HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Domain = ConfigurationManager.AppSettings["Level2DomainName"];
            cookie.Expires = DateTime.Now.AddDays(-1); // it is not working...probably it is not persistent base??
            cookie.Path = FormsAuthentication.FormsCookiePath;
            context.Response.Cookies.Add(cookie);

            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetNoStore();
        }

        //public static SiteConfig GetSiteConfig()
        //{
        //    return Service2FindService.GetSiteConfig();
        //}

        public static void ClearSession()
        {
            HttpContext.Current.Session.RemoveAll();
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        public static void ClearSession(string Name)
        {
            HttpContext.Current.Session.Remove(Name);
        }

        public static T GetSession<T>(string Name)
        {
            T local = (T)HttpContext.Current.Session[Name];
            if (local == null)
            {
                return default(T);
            }
            return local;
        }

        public static string GetSession(string Name)
        {
            object obj2 = HttpContext.Current.Session[Name];
            if ((obj2 != null) && !string.IsNullOrEmpty(obj2.ToString()))
            {
                return obj2.ToString();
            }
            return string.Empty;
        }

        public static void SetSession<T>(string Name, T obj)
        {
            HttpContext.Current.Session[Name] = obj;
        }

        public static void SetSession(string Name, string Value)
        {
            HttpContext.Current.Session[Name] = Value;
        }

        // replace the tokens in template body with corresponding values
        public static string PrepareMailBodyWith(params string[] pairs)
        {
            string body = GetMailBodyOfTemplate();
            
            for (var i = 0; i < pairs.Length; i += 2)
            {
                // note: no sapce <% and %>
                body = body.Replace("<%{0}%>".FormatWith(pairs[i]), pairs[i + 1]);
            }

            // common
            // ref: https://stackoverflow.com/questions/21437916/html-image-not-showing-in-gmail            
            body = body.Replace("<%{0}%>".FormatWith("LogoUrl"), string.Format("<img src='{0}images/ppl-logo.png' style='display:block' />", ConfigurationManager.AppSettings["hostWebName"]));
            body = body.Replace("<%{0}%>".FormatWith("HomeUrl"), string.Format("<a href='{0}' target='_blank'>Recruiting Portal</a>", ConfigurationManager.AppSettings["hostWebName"]));
            body = body.Replace("<%{0}%>".FormatWith("BodySubContent"), string.Format("Please do not reply to this email"));
            
            return body;
        }

        private static string GetMailBodyOfTemplate()
        {
            string cacheKey = string.Concat("mailTemplate:", templateName);
            string body;
            body = (string)System.Web.HttpContext.Current.Cache[cacheKey];
            //read template file text
            body = ReadFileFrom(templateName);

            return body;
        }

        private static string ReadFileFrom(string templateName)
        {
            string filePath = System.Web.HttpContext.Current.Server.MapPath("~/MailTemplates/" + templateName);

            string body = File.ReadAllText(filePath);

            return body;
        }

        public static string FormatWith(this string target, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, target, args);
        }

        // ref: http://stackoverflow.com/questions/444798/case-insensitive-containsstring
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }


        public static string CreateProviderFolder(string uploadPath)
        {
            if (!Directory.Exists(uploadPath))
            {
                DirectoryInfo dirInfo = Directory.CreateDirectory(uploadPath);
                return dirInfo.FullName;
            }
            return uploadPath;
        }        

        public static void Logout()
        {
            WebUtil.ClearSession();
            FormsAuthentication.SignOut();
        }

        public static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext); // henter info fra windows registry
            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type").ToString();
            }
            else if (ext == ".png") // a couple of extra info, due to missing information on the server
            {
                mimeType = "image/png";
            }
            else if (ext == ".flv")
            {
                mimeType = "video/x-flv";
            }
            return mimeType;
        }

        //Submit Button Helper
        public static MvcHtmlString SubmitButton(this HtmlHelper helper, string buttonText)
        {
            string str = "<input type=\"submit\" value=\"" + buttonText + "\" />";
            return new MvcHtmlString(str);
        }

        public static MvcHtmlString PageLinks(this HtmlHelper html,
                                                PagingInfo pagingInfo,
                                                Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            int totalPage = pagingInfo != null ? pagingInfo.TotalPages : 1;
            int currentPage = pagingInfo != null ? pagingInfo.CurrentPage : 1;
            for (int i = 1; i <= totalPage; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == currentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }

        ////Readonly Strongly-Typed TextBox Helper
        //public static MvcHtmlString TextBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>>
        //expression, bool isReadonly)
        //{
        //    MvcHtmlString html = default(MvcHtmlString);

        //    if (isReadonly)
        //    {
        //        html = System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper,
        //        expression, new
        //        {
        //            @class = "readOnly",
        //            @readonly = "read-only"
        //        });
        //    }
        //    else
        //    {
        //        html = System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper,
        //        expression);
        //    }
        //    return html;
        //} 



        // Note: good for one to one (origin, target), but not one to many (many target). Please use "GetPostalCodeByDistance" below
        public static float GetDistanceByPostalCode(string originPostalCode, string destinationPostalCode)
        {
            // ref: http://forums.asp.net/t/2024700.aspx?Calculate+distance+between+two+places+using+google+app
            // ref: http://stanhub.com/find-distance-between-two-postcodes-zipcodes-driving-time-in-current-traffic-using-google-maps-api/
            // ref: https://developers.google.com/maps/documentation/distance-matrix/intro

            // ref: http://salesforce.stackexchange.com/questions/27566/google-geocoding-api-error-message-you-have-exceeded-when-trying-to-get-la
            // ref: http://stackoverflow.com/questions/22806571/what-is-solution-of-this-error-requests-to-this-api-must-be-over-ssl
            string googleAPIServerKey = ConfigurationManager.AppSettings["googleAPIServerKey"];
            float distance = 0.0F;
            var url = string.Format("https://maps.googleapis.com/maps/api/distancematrix/json?origins={0}&destinations={1}&language=en-EN&key={2}", originPostalCode, destinationPostalCode, googleAPIServerKey);
            var request = WebRequest.Create(url);

            // Indicate you are looking for a JSON response
            request.ContentType = "application/json; charset=utf-8";
            var response = (HttpWebResponse)request.GetResponse();

            // Read through the response
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                // Define a serializer to read your response
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                // Get your results
                dynamic result = serializer.DeserializeObject(sr.ReadToEnd());

                // Read the distance property from the JSON request
                try
                {
                    if (result["rows"][0]["elements"][0]["status"] == "OK")
                    {
                        distance = (float)(result["rows"][0]["elements"][0]["distance"]["value"] / 1000.00);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return distance;

        }

        public static List<string> GetPostalCodeByDistance(string originPostalCode, List<string> destinationPostalCodes, string distance)
        {
            List<string> resultPostCodes = new List<string>();

            // ref: http://forums.asp.net/t/2024700.aspx?Calculate+distance+between+two+places+using+google+app
            // ref: http://stanhub.com/find-distance-between-two-postcodes-zipcodes-driving-time-in-current-traffic-using-google-maps-api/
            // ref: https://developers.google.com/maps/documentation/distance-matrix/intro

            // ref: http://salesforce.stackexchange.com/questions/27566/google-geocoding-api-error-message-you-have-exceeded-when-trying-to-get-la
            // ref: http://stackoverflow.com/questions/22806571/what-is-solution-of-this-error-requests-to-this-api-must-be-over-ssl
            string googleAPIServerKey = ConfigurationManager.AppSettings["googleAPIServerKey"];

            var multiplePostCodes = string.Join<string>("|", destinationPostalCodes);

            var url = string.Format("https://maps.googleapis.com/maps/api/distancematrix/json?origins={0}&destinations={1}&language=en-EN&key={2}", originPostalCode, multiplePostCodes, googleAPIServerKey);
            var request = WebRequest.Create(url);

            // Indicate you are looking for a JSON response
            request.ContentType = "application/json; charset=utf-8";
            var response = (HttpWebResponse)request.GetResponse();

            // Read through the response
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                // Define a serializer to read your response
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                // Get your results
                dynamic result = serializer.DeserializeObject(sr.ReadToEnd());

                // Read the distance property from the JSON request
                try
                {
                    if (result["rows"] != null)
                    {
                        for (int i = 0; i < result["rows"][0]["elements"].Length; i++)
                        {
                            if (result["rows"][0]["elements"][i]["status"] == "OK") // OK or NOT FOUND
                            {
                                float distanceBetween = (float)(result["rows"][0]["elements"][i]["distance"]["value"] / 1000.00);

                                if (distanceBetween > 0.0 && distanceBetween <= float.Parse(distance))
                                {
                                    resultPostCodes.Add(result["destination_addresses"][i]); // e.g. "Thornhill, ON L4J 5X2, Canada"
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            return resultPostCodes;
        }

        // ref: https://stackoverflow.com/questions/343899/how-to-cache-data-in-a-mvc-application
        public static T GetOrSetCache<T>(string cacheKey, Func<T> getItemCallback) where T : class
        {
            T item = MemoryCache.Default.Get(cacheKey) as T;
            if (item == null)
            {
                item = getItemCallback();
                int cacheBySecond = ConfigurationManager.AppSettings["cacheBySecond"] != null ? int.Parse(ConfigurationManager.AppSettings["cacheBySecond"]) : 10;
                MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddSeconds(cacheBySecond));
            }
            return item;
        }
    }
}