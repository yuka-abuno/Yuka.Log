using System;
using System.Collections.Generic;

namespace Yuka.Logger
{
    /// <summary>
    ///     Logger Interface
    /// </summary>
    internal interface ILogger
    {
        /// <summary>LogWriters</summary>
        IList<ILogWriter> LogWriters { get; }

        /// <summary>MinLogLevel</summary>
        LogLevel MinLogLevel { get; set; }

        /// <summary>MaxLogLevel</summary>
        LogLevel MaxLogLevel { get; set; }

        /// <summary>
        /// Create Trace Log
        /// </summary>
        /// <param name="messageFunc">Message Func</param>
        /// <returns></returns>
        void Trace(Func<string> messageFunc);

        /// <summary>
        /// Create Debug Log
        /// </summary>
        /// <param name="messageFunc">Message Func</param>
        /// <returns></returns>
        void Debug(Func<string> messageFunc);

        /// <summary>
        /// Creaate Info Log
        /// </summary>
        /// <param name="messageFunc"></param>
        /// <returns></returns>
        void Info(Func<string> messageFunc);

        /// <summary>
        /// Create Warn Log
        /// </summary>
        /// <param name="messageFunc"></param>
        /// <returns></returns>
        void Warning(Func<string> messageFunc);

        /// <summary>
        /// Create Error Log
        /// </summary>
        /// <param name="messageFunc"></param>
        /// <returns></returns>
        void Error(Func<string> messageFunc);

        /// <summary>
        /// Create Error Log
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        void Error(Exception ex);

    }
}