using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace com.hujun64.util
{
    public class CommonFilter : Stream
    {
        private readonly Stream _responseStream;
        private readonly FileStream _cacheStream;

        public override bool CanRead
        {
            get
            {
                return false;
            }
        }
        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }
        public override bool CanWrite
        {
            get
            {
                return _responseStream.CanWrite;
            }
        }
        public override long Length
        {
            get
            {
                throw new NotSupportedException();
            }
        }
        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public CommonFilter(Stream responseStream, FileStream stream)
        {
            _responseStream = responseStream;
            _cacheStream = stream;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }
        public override void SetLength(long length)
        {
            throw new NotSupportedException();
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
        public override void Flush()
        {
            _responseStream.Flush();
            _cacheStream.Flush();
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            _cacheStream.Write(buffer, offset, count);
            _responseStream.Write(buffer, offset, count);
        }
        public override void Close()
        {
            _responseStream.Close();
            _cacheStream.Close();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _responseStream.Dispose();
                _cacheStream.Dispose();
            }
        }
    }

}
