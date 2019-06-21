using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
namespace HiMusic
{
    public class GestureRecognition
    {
        //手势识别函数，参数为图片路径(如d:/1.jpg), 只会返回Thumb_up,Thumb_down,Rock三种手势，没有这三种手势就返回空
        //如果图片里有一只手就按上面的返回，如果图片里有两只手，就会分别识别两只手，如果两只手都不属于这三种，就返回空，
        //如果有一只手属于这三种，就返回这一种，如果两只手都属于这三种，就返回两个手势，中间用空格隔开
        public string recognize(byte[] img) 
        {
            string token = "24.89cbede2c1c69ebd190a51cdd61a3c2b.2592000.1561879078.282335-16401733";         //事先获取的access_token
            string host = "https://aip.baidubce.com/rest/2.0/image-classify/v1/gesture?access_token=" + token;     //请求地址
            Encoding encoding = Encoding.Default;                     //encoding可以转换字节数组和字符串         
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);             //创建请求
            request.Method = "post";
            request.KeepAlive = true;
            string base64 = byteToBase64(img);                        // 获得图片的base64编码 
            String str = "image=" + UrlEncode(base64,System.Text.Encoding.UTF8);
            byte[] buffer = encoding.GetBytes(str);                                     //设置请求
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();                   //发出请求，获得返回值
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            string res = getClassname(result);
            return res;
        }

        public String getFileBase64(String fileName)      //将路径名称转换成一种编码
        {
            FileStream filestream = new FileStream(fileName, FileMode.Open);
            byte[] arr = new byte[filestream.Length];
            filestream.Read(arr, 0, (int)filestream.Length);
            string baser64 = Convert.ToBase64String(arr);
            filestream.Close();
            return baser64;
        }
        
        public String getClassname(String result) //将手势名称从输出流中提取出来
        {         
            int pos = result.IndexOf("classname")+13,pos2=result.LastIndexOf("classname")+13;
            char c = result[pos],c2=result[pos2];
            int length=0,length2=0;
            for(int i=pos+1;c!='"';i++)
            {
                c = result[i];
                length++;
            }
            for(int i=pos2+1;c2!='"';i++)
            {
                c2 = result[i];
                length2++;
            }
            
            string classname = result.Substring(pos, length);
            string classname2 = result.Substring(pos2, length2);

            if (classname == "Thumb_down" || classname2 == "Thumb_down")
            {
                classname = "Thumb_down";
            }
            else if (classname == "Rock" || classname2 == "Rock")
            {
                classname = "Rock";
            }
            return classname;
        }

        public string byteToBase64(byte[] rawImage)
        {
            System.Drawing.Bitmap mbp = new System.Drawing.Bitmap(1920, 1080);
            int pixels = 1920 * 1080;
            for(int i = 0;i<pixels;++i)
            {
                int pos = i << 2;
                mbp.SetPixel(i % 1920, i / 1920, System.Drawing.Color.FromArgb(rawImage[pos], rawImage[pos + 1], rawImage[pos + 2]));
            }

            string base64;
            using (MemoryStream ms = new MemoryStream())
            {
                mbp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                base64 = Convert.ToBase64String(arr);
            }
            return base64;
        }








        private string UrlEncode(string str, Encoding e)
        {
            if (str == null)
                return null;

            if (str == String.Empty)
                return String.Empty;

            bool needEncode = false;
            int len = str.Length;
            for (int i = 0; i < len; i++)
            {
                char c = str[i];
                if ((c < '0') || (c < 'A' && c > '9') || (c > 'Z' && c < 'a') || (c > 'z'))
                {
                    if (NotEncoded(c))
                        continue;

                    needEncode = true;
                    break;
                }
            }

            if (!needEncode)
                return str;

            // avoided GetByteCount call
            byte[] bytes = new byte[e.GetMaxByteCount(str.Length)];
            int realLen = e.GetBytes(str, 0, str.Length, bytes, 0);
            return Encoding.ASCII.GetString(UrlEncodeToBytes(bytes, 0, realLen));
        }
        private bool NotEncoded(char c)
        {
            return (c == '!' || c == '(' || c == ')' || c == '*' || c == '-' || c == '.' || c == '_'
            );
        }

        private byte[] UrlEncodeToBytes(string str)
        {
            return UrlEncodeToBytes(str, Encoding.UTF8);
        }

        private byte[] UrlEncodeToBytes(string str, Encoding e)
        {
            if (str == null)
                return null;

            if (str.Length == 0)
                return new byte[0];

            byte[] bytes = e.GetBytes(str);
            return UrlEncodeToBytes(bytes, 0, bytes.Length);
        }

        private byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            int blen = bytes.Length;
            if (blen == 0)
                return new byte[0];

            if (offset < 0 || offset >= blen)
                throw new ArgumentOutOfRangeException("offset");

            if (count < 0 || count > blen - offset)
                throw new ArgumentOutOfRangeException("count");

            MemoryStream result = new MemoryStream(count);
            int end = offset + count;
            for (int i = offset; i < end; i++)
                UrlEncodeChar((char)bytes[i], result, false);

            return result.ToArray();
        }

        private void UrlEncodeChar(char c, Stream result, bool isUnicode)
        {
            if (c > 255)
            {
                //FIXME: what happens when there is an internal error?
                //if (!isUnicode)
                //	throw new ArgumentOutOfRangeException ("c", c, "c must be less than 256");
                int idx;
                int i = (int)c;

                result.WriteByte((byte)'%');
                result.WriteByte((byte)'u');
                idx = i >> 12;
                result.WriteByte((byte)hexChars[idx]);
                idx = (i >> 8) & 0x0F;
                result.WriteByte((byte)hexChars[idx]);
                idx = (i >> 4) & 0x0F;
                result.WriteByte((byte)hexChars[idx]);
                idx = i & 0x0F;
                result.WriteByte((byte)hexChars[idx]);
                return;
            }

            if (c > ' ' && NotEncoded(c))
            {
                result.WriteByte((byte)c);
                return;
            }
            if (c == ' ')
            {
                result.WriteByte((byte)'+');
                return;
            }
            if ((c < '0') ||
                (c < 'A' && c > '9') ||
                (c > 'Z' && c < 'a') ||
                (c > 'z'))
            {
                if (isUnicode && c > 127)
                {
                    result.WriteByte((byte)'%');
                    result.WriteByte((byte)'u');
                    result.WriteByte((byte)'0');
                    result.WriteByte((byte)'0');
                }
                else
                    result.WriteByte((byte)'%');

                int idx = ((int)c) >> 4;
                result.WriteByte((byte)hexChars[idx]);
                idx = ((int)c) & 0x0F;
                result.WriteByte((byte)hexChars[idx]);
            }
            else
                result.WriteByte((byte)c);
        }

        private char[] hexChars = "0123456789abcdef".ToCharArray();
    }

}


