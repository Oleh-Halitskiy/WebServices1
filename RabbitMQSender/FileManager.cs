namespace RabbitMQSender
{
    /// <summary>
    /// File manager for selecting files
    /// </summary>
    public class FileManager
    {
        /// <summary>
        /// Selects the file
        /// </summary>
        /// <param name="path">Path to the file selected</param>
        /// <returns>Returns the contains of the file in string</returns>
        public static string SelectFile(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
