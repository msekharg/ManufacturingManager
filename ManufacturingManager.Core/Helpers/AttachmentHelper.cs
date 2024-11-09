using File = System.IO.File;

namespace ManufacturingManager.Core.Helpers
{
    public class AttachmentHelper
    {
        public static byte[] ConvertFileToByteArray(string filePath)
        {
            if (File.Exists(filePath))
            {
                byte[] fileByteArray = File.ReadAllBytes(filePath);
                return fileByteArray;
            }

            throw new FileNotFoundException(filePath + " does not exists");
        }

        public static void SaveByteArrayToFile(byte[] data, string filePath)
        {
            try
            {
                File.WriteAllBytes(filePath, data);
                return;
            }
            catch (Exception exception)
            {
                throw new Exception("Unable to create " + filePath, exception);
            }
        }
        
    }
}
