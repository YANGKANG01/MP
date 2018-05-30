using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MP.Tools
{
    /// <summary>
    /// 加密类
    /// </summary>
    public class EncryptHelper
    {
        /// <summary>  
        /// MD5加密
        /// </summary>  
        /// <param name="inputText"></param>  
        /// <returns></returns>  
        public static string MD5DesEncrypt(string inputText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(inputText);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }

            return byte2String;
        }
    }
}
