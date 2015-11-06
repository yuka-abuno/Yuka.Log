using System;
using System.IO;

namespace Yuka.Logger.LogWriter
{
    /// <summary>
    ///  Strem Log Writer
    /// </summary>
    public class StreamLogWriter : ILogWriter, IDisposable
    {
        private readonly StreamWriter _writer;

        private bool _disposedValue;

        /// <summary>
        ///     Constractor
        /// </summary>
        /// <param name="stream"></param>
        public StreamLogWriter(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanWrite) throw new ArgumentException("CanWrite is false", nameof(stream));
            _writer = new StreamWriter(stream);
        }


        /// <summary>
        /// Write Loh
        /// </summary>
        /// <param name="message"></param>
        public void WriteLog(string message)
        {
            _writer.WriteLine(message);
        }

        #region IDisposable Support

        /// <summary>
        ///     dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _writer.Dispose();
                }

                _disposedValue = true;
            }
        }
        /// <summary>
        ///     ディスポーズ
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}