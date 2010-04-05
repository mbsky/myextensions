using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace System.Web.Mvc
{
    public class ResponseWrapper : Stream
    {
        #region Fields
        private Stream _innerStream;
        private MemoryStream _memStream;
        private string _eTag = string.Empty;
        #endregion

        #region Constructors
        public ResponseWrapper(Stream stream, string eTag)
        {
            _innerStream = stream;
            _memStream = new MemoryStream();
            _eTag = eTag;
        }
        #endregion

        #region Properties
        public byte[] Data
        {
            get
            {
                _memStream.Position = 0;
                byte[] data = new byte[_memStream.Length];
                _memStream.Read(data, 0, (int)_memStream.Length);
                return data;
            }
        }
        #endregion

        #region overrides of Stream Class
        public override bool CanRead
        {
            get { return _innerStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _innerStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _innerStream.CanWrite; }
        }

        public override void Flush()//可能会有这样一种情况：如果数据比较大则可能在未真正传输结束前就要Flush
        {
            var httpContext = HttpContext.Current;
            string currentETag = generateETagValue(Data);
            if(_eTag != null)
            {
                if(currentETag.Equals(_eTag))
                {
                    httpContext.Response.StatusCode = 304;
                    httpContext.Response.StatusDescription = "Not Modified";
                    return;
                }
            }
            httpContext.Response.Cache.SetCacheability(HttpCacheability.Public);
            httpContext.Response.Cache.SetETag(currentETag);
            httpContext.Response.Cache.SetLastModified(DateTime.Now);
            httpContext.Response.Cache.SetSlidingExpiration(true);
            copyStreamToStream(_memStream, _innerStream);
            _innerStream.Flush();
        }

        public override long Length
        {
            get { return _innerStream.Length; }
        }

        public override long Position
        {
            get
            {
                return _innerStream.Position;
            }
            set
            {
                _innerStream.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _innerStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _innerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _innerStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            //_innerStream.Write(buffer, offset, count);
            _memStream.Write(buffer, offset, count);
        }

        public override void Close()
        {
            _innerStream.Close();
        }
        #endregion

        #region private Helper Methods
        private void copyStreamToStream(Stream src, Stream target)
        {
            src.Position = 0;
            int nRead = 0;
            byte[] buf = new byte[128];
            while((nRead = src.Read(buf, 0, 128)) != 0)
            {
                target.Write(buf, 0, nRead);
            }
        }
        private string generateETagValue(byte[] data)
        {
            var encryptor = new System.Security.Cryptography.SHA1Managed();
            byte[] encryptedData = encryptor.ComputeHash(data);
            return Convert.ToBase64String(encryptedData);
        }
        #endregion
    }
}
