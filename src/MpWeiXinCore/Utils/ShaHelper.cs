//-----------------------------------------------------------------------
// <copyright file="ShaHelper.cs" company="long">
//     Copyright (c) long. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Security.Cryptography;
using System.Text;

namespace MpWeiXinCore.Utils
{
    /// <summary>
    /// sha加密
    /// </summary>
    internal class ShaHelper
    {
        /// <summary>
        /// Sha1s the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string Sha1(string source)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(source);

            return BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", "").ToLower();
        }
    }
}
