using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRCoder;
using System.IO;
using System.IO.Pipelines;

namespace FrameCommon.Hepler
{
    public class QRCoderHelper
    {
        #region 普通二维码
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">存储内容</param>
        /// <param name="pixel">像素大小</param>
        /// <returns></returns>
        public static byte[] GetPTQRCode(string url, int pixel)
        {
            QRCodeGenerator generator = new QRCodeGenerator();
            QRCodeData codeData = generator.CreateQrCode(url, QRCodeGenerator.ECCLevel.M, true);
            BitmapByteQRCode qrcode = new BitmapByteQRCode(codeData);
            return qrcode.GetGraphic(pixel);
        }
        #endregion

        #region 带logo的二维码
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">存储内容</param>
        /// <param name="pixel">像素大小</param>
        /// <returns></returns>
        public static Bitmap GetLogoQRCode(string url, string logoPath, int pixel)
        {
            QRCodeGenerator generator = new QRCodeGenerator();
            QRCodeData codeData = generator.CreateQrCode(url, QRCodeGenerator.ECCLevel.M, true);
            BitmapByteQRCode qrcode = new BitmapByteQRCode(codeData);
            Bitmap icon = new Bitmap(logoPath);
            using (var memoryStream = new MemoryStream(qrcode.GetGraphic(pixel)))
            {
                Bitmap qrImage = new Bitmap(memoryStream);
                return qrImage;
            }
            #region 参数介绍
            //GetGraphic方法参数介绍
            //pixelsPerModule //生成二维码图片的像素大小 ，我这里设置的是5
            //darkColor       //暗色   一般设置为Color.Black 黑色
            //lightColor      //亮色   一般设置为Color.White  白色
            //icon             //二维码 水印图标 例如：Bitmap icon = new Bitmap(context.Server.MapPath("~/images/zs.png")); 默认为NULL ，加上这个二维码中间会显示一个图标
            //iconSizePercent  //水印图标的大小比例 ，可根据自己的喜好设置
            //iconBorderWidth  // 水印图标的边框
            //drawQuietZones   //静止区，位于二维码某一边的空白边界,用来阻止读者获取与正在浏览的二维码无关的信息 即是否绘画二维码的空白边框区域 默认为true
            #endregion
        }
        #endregion
    
         /// <summary>
         /// 生成二维码图片
         /// </summary>
         /// <param name="str"></param>
         /// <returns></returns>
         public static Bitmap CreateQRimg(string str)
         {
             QRCodeGenerator qrGenerator = new QRCodeGenerator();
             QRCodeData qrCodeData = qrGenerator.CreateQrCode(str, QRCodeGenerator.ECCLevel.Q);
             BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
             using (var memoryStream = new MemoryStream(qrCode.GetGraphic(30)))
             {
                 Bitmap qrImage = new Bitmap(memoryStream);
                 return qrImage;
             }
        }

         /// <summary>
         /// 生成二维码图片流（二维码上含文字）
         /// </summary>
         /// <param name="str1">二维码中要传递的数据（地址）</param>
         /// <param name="str2">二维码上显示的文字说明</param>
         public static byte[] GenerateQRCode(string str1, string str2)
         {
             using (Image codeImage = CreateQRimg(str1), strImage = ConvertStringToImage(str2))
             {
                 Image img = CombineImage(600, 600, codeImage, 60, 50, strImage, 0, 530);
                 using (var stream = new MemoryStream())
                 {
                     img.Save(stream, ImageFormat.Jpeg);
                     //输出图片流
                     return stream.ToArray();
                 }
             }
         }
 
         /// <summary>
         /// 生成二维码图片流（不含文字）
         /// </summary>
         /// <param name="str">二维码中要传递的数据（地址）</param>
         /// <returns></returns>
         public static byte[] GenerateQRCode(string str)
         {
             using (Image codeImage = CreateQRimg(str))
             {
                 using (var stream = new MemoryStream())
                 {
                     codeImage.Save(stream, ImageFormat.Jpeg);
 
                     return stream.ToArray();
                 }
             }
         }
 
         /// <summary>
         /// 生成文字图片
         /// </summary>
         /// <param name="str"></param>
         /// <returns></returns>
         public static Image ConvertStringToImage(string str)
         {
             Bitmap image = new Bitmap(600, 40, PixelFormat.Format24bppRgb);
 
             Graphics g = Graphics.FromImage(image);
 
             try
             {
                 Font font = new Font("SimHei", 14, FontStyle.Regular);
 
                 g.Clear(Color.White);
 
                 StringFormat format = new StringFormat();
                 format.Alignment = StringAlignment.Center;
                 format.LineAlignment = StringAlignment.Center;
 
                 Rectangle rectangle = new Rectangle(0, 0, 600, 40);
 
                 g.DrawString(str, font, new SolidBrush(Color.Black), rectangle, format);
 
                 return image;
             }
             catch (Exception e)
             {
                 throw e;
             }
             finally
             {
                 GC.Collect();
             }
         }
 
         /// <summary>
         /// 在画板中合并二维码图片和文字图片
         /// </summary>
         /// <param name="width"></param>
         /// <param name="height"></param>
         /// <param name="imgLeft"></param>
         /// <param name="imgLeft_left"></param>
         /// <param name="imgLeft_top"></param>
         /// <param name="imgRight"></param>
         /// <param name="imgRight_left"></param>
         /// <param name="imgRight_top"></param>
         /// <returns></returns>
         public static Image CombineImage(int width, int height, Image imgLeft, int imgLeft_left, int imgLeft_top, Image imgRight, int imgRight_left, int imgRight_top)
         {
             Bitmap image = new Bitmap(width, height, PixelFormat.Format24bppRgb);
 
             Graphics g = Graphics.FromImage(image);
 
             try
             {
                 g.Clear(Color.White);
                 g.DrawImage(imgLeft, imgLeft_left, imgLeft_top, 500, 500);
                 g.DrawImage(imgRight, imgRight_left, imgRight_top, imgRight.Width, imgRight.Height);
 
                 return image;
             }
             catch (Exception e)
             {
                 throw e;
             }
             finally
             {
                 g.Dispose();
             }
         }

    }
}
