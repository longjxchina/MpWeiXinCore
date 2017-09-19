//-----------------------------------------------------------------------
// <copyright file="JsApiSign.cs" company="long">
//     Copyright (c) long. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MpWeiXinCore.Models.JsSdks
{
    /// <summary>
    /// jsapi签名类
    /// </summary>
    public class JsApiSign
    {
        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public string Signature { get; set; }

        /// <summary>
        /// Gets or sets the nonce string.
        /// </summary>
        /// <value>
        /// The nonce string.
        /// </value>
        public string NonceStr { get; set; }

        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
        public int TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>
        public string AppId { get; set; }
    }
}
