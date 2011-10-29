using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace MiniGame.DataModel
{
    public class ImageHelper
    {
        public static byte[] ToByteArrayA(WriteableBitmap bmp)
        {
            int[] p = bmp.Pixels;
            int len = p.Length * 4;
            byte[] result = new byte[len]; // ARGB
            Buffer.BlockCopy(p, 0, result, 0, len);
            return result;
        }

        public static byte[] ToByteArrayB(WriteableBitmap bmp)
        {
            int[] p = bmp.Pixels;
            int len = p.Length << 2;
            byte[] result = new byte[len];
            Buffer.BlockCopy(p, 0, result, 0, len);
            return result;
        }

        public static WriteableBitmap FromByteArray( byte[] buffer)
        {
            WriteableBitmap bmp = new WriteableBitmap(50,50);
            Buffer.BlockCopy(buffer, 0, bmp.Pixels, 0, buffer.Length);
            return bmp;
        }

    }
}
