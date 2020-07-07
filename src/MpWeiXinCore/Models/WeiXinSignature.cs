using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography;
using System.Text;

namespace MpWeiXinCore.Models
{
    public class WeiXinSignature
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly MpWeiXinOptions config;

        public WeiXinSignature(
            IHttpContextAccessor httpContextAccessor,
            IOptions<MpWeiXinOptions> wxOption)
        {
            this.httpContextAccessor = httpContextAccessor;
            config = wxOption.Value;
        }

        #region 变量区

        /// <summary>
        /// 查询字符参参数 signature
        /// </summary>
        public const string QS_SIGNATURE = "signature";

        /// <summary>
        /// 查询字符参参数 timestamp
        /// </summary>
        public const string QS_TIMESTAMP = "timestamp";

        /// <summary>
        /// 查询字符参参数 nonce
        /// </summary>
        public const string QS_NONCE = "nonce";

        /// <summary>
        /// 查询字符参参数 echostr
        /// </summary>
        public const string QS_ECHOSTR = "echostr";

        /// <summary>
        /// 获取 signature.
        /// </summary>
        /// <value>
        /// The signature.
        /// </value>
        public string Signature
        {
            get
            {
                return httpContextAccessor.HttpContext.Request.Query[QS_SIGNATURE].ToString();
            }
        }

        /// <summary>
        /// 获取 Nonce.
        /// </summary>
        /// <value>
        /// The Nonce.
        /// </value>
        public string Nonce
        {
            get
            {
                return httpContextAccessor.HttpContext.Request.Query[QS_NONCE].ToString();
            }
        }

        /// <summary>
        /// 获取 Timestamp.
        /// </summary>
        /// <value>
        /// The Timestamp.
        /// </value>
        public string Timestamp
        {
            get
            {
                return httpContextAccessor.HttpContext.Request.Query[QS_TIMESTAMP].ToString();
            }
        }

        /// <summary>
        /// 获取 Echostr.
        /// </summary>
        /// <value>
        /// The Echostr.
        /// </value>
        public string Echostr
        {
            get
            {
                return httpContextAccessor.HttpContext.Request.Query[QS_ECHOSTR].ToString();
            }
        }

        #endregion

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <returns></returns>
        public bool ValidateSignature()
        {
            var arrParams = new string[] { config.Token, Timestamp, Nonce };

            Array.Sort(arrParams);

            var content = string.Join(string.Empty, arrParams);
            var encrypted = Encrypt(content);

            return encrypted.Equals(Signature);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        private string Encrypt(string source)
        {
            var bytes = Encoding.UTF8.GetBytes(source);
            var sha = SHA1.Create();

            bytes = sha.ComputeHash(bytes);

            var res = new StringBuilder();

            foreach (var b in bytes)
            {
                res.AppendFormat("{0:x2}", b);
            }

            return res.ToString();
        }
    }
}