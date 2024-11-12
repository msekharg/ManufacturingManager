using System.Xml.Linq;

namespace ManufacturingManager.Web.Services
{
    public static class ExtensionMethods
    {
        public static XElement GetElementWithAttribute(this IEnumerable<XElement> elements, String attributeName, String attributeValue)
        {
            var element = elements.First(x => x.Attribute(attributeName).Value == attributeValue);

            return element;
        }

        /// <summary>
        /// Attempts to map the file extension to a mime type. Returns application/octet-stream if not supported.
        /// </summary>
        public static String FileExtensionToMimeType(this String fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".doc":
                    return "application/msword";

                case ".docx":
                    return " application/vnd.openxmlformats-officedocument.wordprocessingml.document";

                case ".xls":
                    return "application/vnd.ms-excel";

                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                case ".ppt":
                    return "application/vnd.ms-powerpoint";

                case ".pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.slideshow";

                case ".pdf":
                    return "application/pdf";

                case ".bmp":
                    return "image/bmp";

                case ".gif":
                    return "image/gif";

                case ".tif":
                    return "image/tiff";

                case ".tiff":
                    return "image/tiff";

                case ".jpg":
                    return "image/jpeg";

                case ".jpeg":
                    return "image/jpeg";

                case ".png":
                    return "image/png";

                case ".txt":
                    return "text/plain";

                default:
                    return "application/octet-stream";
            }
        }

        public static Guid ObjectIdToGuid(this String objectId)
        {
            if (String.IsNullOrWhiteSpace(objectId))
            {
                throw new ArgumentNullException("objectId", "objectId cannot be null.");
            }

            if (objectId.Length != 40)
            {
                throw new ArgumentException("objectId is not properly formatted.");
            }

            // ObjectId has 'idd_' prefix on a properly-formatted guid string
            var guidString = objectId.Substring(4, 36);
            Guid result;

            if (Guid.TryParse(guidString, out result))
            {
                return result;
            }

            throw new Exception(String.Format("Could not parse objectId as Guid: {0}", objectId));
        }


    }
}