//
//  Copyright 2012, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.IO;
#if ! __UNIFIED__
using MonoTouch.UIKit;
using MonoTouch.Foundation;
#else
using UIKit;
using Foundation;
#endif
using System.Runtime.InteropServices;

namespace Plugin.Media
{
    class NSDataStream : Stream
    {
        NSData theData;
        uint pos;

        public NSDataStream(NSData data)
        {
            this.theData = data;
        }

        protected override void Dispose(bool disposing)
        {
            if (theData != null)
            {
                theData.Dispose();
                theData = null;
            }
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (pos >= theData.Length)
            {
                return 0;
            }
            else
            {
#if ! __UNIFIED__
                var len = (int)Math.Min(count, theData.Length - pos);
#else
				var len = (int)Math.Min (count, (double)(theData.Length - pos));
#endif

                Marshal.Copy(new IntPtr(theData.Bytes.ToInt64() + pos), buffer, offset, len);
                pos += (uint)len;
                return len;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead
        {
            get
            {
                return true;
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
                return false;
            }
        }

        public override long Length
        {
            get
            {
                // override does not allow nint
#if ! __UNIFIED__
                return theData.Length;
#else
				return (long) theData.Length;
#endif
            }
        }

        public override long Position
        {
            get
            {
                return pos;
            }
            set
            {
            }
        }
    }
}
