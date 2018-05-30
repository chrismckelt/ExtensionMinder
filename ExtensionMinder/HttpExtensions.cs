using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace ExtensionMinder
{
  public static class HttpExtensions
  {
    public static string ToQueryString(this NameValueCollection coll, bool useQuestionMarkToStart = false)
    {
      var qs= string.Join("&",
        coll.Cast<string>().Select(a => string.Format("{0}={1}",
          HttpUtility.UrlEncode(a),
          HttpUtility.UrlEncode(coll[a]))));

      if (useQuestionMarkToStart)
      {
        return "?" + qs;
      }

      return qs;
    }
  }
}