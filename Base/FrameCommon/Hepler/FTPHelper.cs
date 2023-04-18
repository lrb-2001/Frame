using FrameModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FrameCommon.Hepler
{
    public class FTPHelper
    {
        /// <summary>
        /// Ftp上传文件
        /// </summary>
        /// <param name="fileData">文件字节</param>
        /// <param name="ftpRemoteDir">上传到-文件夹名称</param>
        /// <param name="suffix">文件后缀</param>
        public static string UploadFileToFtp(byte[] fileData, string ftpRemoteDir, string suffix)
        {
            var ftpCon = GlobalConfig.frameCoreAgileConfig.connectionConfig.FtpCon;
            var fileName = Guid.NewGuid().ToString().Replace("-","");
            var ftpUrl = $"{ftpCon}/{ftpRemoteDir}/{fileName}.{suffix}";
            FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(ftpUrl);
            ftpWebRequest.Credentials = new NetworkCredential("frameftp", "lrb1216");
            ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpWebRequest.KeepAlive = true;
            ftpWebRequest.UseBinary = true;
            using (Stream ftpStream = ftpWebRequest.GetRequestStream())
            {
                ftpStream.Write(fileData, 0, fileData.Length);
            }
            return ftpUrl;
        }
    }
}
