

using System.Data;
using System.Security;
using ManufacturingManager.ADO;

namespace ManufacturingManager.Core.Helpers
{
    public class Functions
    {
        public static string GetDatabaseVersion()
        {
            Database db = DatabaseFactory.CreateDatabase("PCSCase");
            using var connection = db.CreateConnection();
            connection.Open();
            var dbCommand = connection.CreateCommand();
            dbCommand.CommandText = "GetDatabaseVersion";
            dbCommand.CommandType = CommandType.StoredProcedure;
            var version = (string)dbCommand.ExecuteScalar();
            return version;
        }

        public static string ReplaceNewLinesToHtmlBreak(string value)
        {
            return value.Replace("\n", "<BR>");
        }
        public static Guid FileNetObjectIdToGuid(String objectId)
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
        //File clean name
        public static string ValidateSecureFilePath(string text)
        {
            string cleanPath;
            try
            {
                if (text.Contains(".."))
                    return "";
                cleanPath = CleanString(text, CleanCharFilePath);
                if (cleanPath == string.Empty)
                    return "";
            }
            catch (Exception ex)
            {
                throw new SecurityException("Invalid path", ex);
            }

            //if we haven't thrown an exception at this point it must be a good path
            return cleanPath;
        }

        private const char DefaultBadCharacter = '^';
        private static String CleanString(String aString, Func<char, char> cleanChar)
        {
            if (aString == null) return null;
            String cleanString = "";

            Char[] charArray = aString.ToCharArray();
            foreach (char t in charArray)
            {
                char aChar = cleanChar(t);
                if (aChar == DefaultBadCharacter)
                {
                    //invalid character found
                    throw new SecurityException("Invalid character found");
                }
                cleanString += aChar;
            }
            return cleanString;
        }

        private static char CleanCharFilePath(char aChar)
        {
            // 0 - 9
            for (int i = 48; i < 58; ++i)
            {
                if (aChar == i) return (char)i;
            }

            // 'A' - 'Z'
            for (int i = 65; i < 91; ++i)
            {
                if (aChar == i) return (char)i;
            }

            // 'a' - 'z'
            for (int i = 97; i < 123; ++i)
            {
                if (aChar == i) return (char)i;
            }

            //whitelist = @"[^a-zA-Z0-9\\_()+-:$\.,%&#@~ ]"
            switch (aChar)
            {
                case '\\':
                    return '\\';
                case '_':
                    return '_';
                case '(':
                    return '(';
                case ')':
                    return ')';
                case '+':
                    return '+';
                case '-':
                    return '-';
                case ':':
                    return ':';
                case '$':
                    return '$';
                case '.':
                    return '.';
                case ' ':
                    return ' ';
                case ',':
                    return ',';
                case ';':
                    return ';';
                case '?':
                    return '?';
                case '~':
                    return '~';
                case '`':
                    return '`';
                case '@':
                    return '@';
                case '%':
                    return '%';
                case '^':
                    return '^';
                case '&':
                    return '&';
                case '[':
                    return '[';
                case ']':
                    return ']';
                case '{':
                    return '{';
                case '}':
                    return '}';
                case '"':
                    return '"';
                case '!':
                    return '!';
                case '#':
                    return '#';
                case '=':
                    return '=';
            }
            return DefaultBadCharacter;
        }

        public static string[] ListOfStringsSplit(string value, char keyChar)
        {

            return value.Split(keyChar);
        }
        public static void DeleteFileInFolder(string pathArg, string filename)
        {
            var path = ValidateSecureFilePath(pathArg);
            if (path != pathArg)
            {
                //Logging.WriteToLog($"Exception in Functions.DeleteFileInfolder(). Invalid filename : {path} clean filename {pathArg}");
            }
            else if (path.Contains("vafsc"))
            {
                //only delete it if path contains vafsc and file exists in that folder.
                var di = new DirectoryInfo(path);
                FileInfo[] files = di.GetFiles(filename);
                if (files.Length > 0)
                    files[0].Delete();
            }
        }

    }
}
