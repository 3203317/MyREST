using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Foreworld.Utils
{
    public class MD5
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <returns></returns>
        public static string Encrypt(string pToEncrypt)
        {
            MD5CryptoServiceProvider __md5 = new MD5CryptoServiceProvider();
            string __result = BitConverter.ToString(__md5.ComputeHash(UTF8Encoding.Default.GetBytes(pToEncrypt)));
            __result = __result.Replace("-", "").ToLower();
            return __result;
        }
    }
}
