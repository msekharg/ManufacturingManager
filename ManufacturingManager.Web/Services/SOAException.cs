using System.Net;

namespace ManufacturingManager.Web.Services
{
    [Serializable]
    public class SOAException : Exception
    {
        public SOAException(HttpStatusCode statusCode, String responseText)
        {
            StatusCode = statusCode;
            ResponseText = responseText;
        }

        public HttpStatusCode StatusCode { get; private set; }

        public String ResponseText { get; private set; }
    }
}
