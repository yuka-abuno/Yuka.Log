namespace Yuka.Logger
{
    /// <summary>
    /// Writer Interface
    /// </summary>
    public interface ILogWriter
    {
        /// <summary>
        /// Write Log
        /// </summary>
        /// <param name="message"></param>
        void WriteLog(string message);
    }
}
