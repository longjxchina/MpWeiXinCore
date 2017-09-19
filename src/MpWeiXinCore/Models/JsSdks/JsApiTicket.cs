//-----------------------------------------------------------------------
// <copyright file="JsApiTicket.cs" company="long">
//     Copyright (c) long. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MpWeiXinCore.Models.JsSdks
{
    /// <summary>
    /// js api ticket
    /// </summary>
    public class JsApiTicket : WxError
    {
        /// <summary>
        /// Gets or sets the errcode.
        /// </summary>
        /// <value>
        /// The errcode.
        /// </value>
        public string errcode { get; set; }

        /// <summary>
        /// Gets or sets the errmsg.
        /// </summary>
        /// <value>
        /// The errmsg.
        /// </value>
        public string errmsg { get; set; }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public string ticket { get; set; }

        /// <summary>
        /// Gets or sets the expires in.
        /// </summary>
        /// <value>
        /// The expires in.
        /// </value>
        public int expires_in { get; set; }
    }
}
