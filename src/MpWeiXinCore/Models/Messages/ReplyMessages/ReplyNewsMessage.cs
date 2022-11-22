using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MpWeiXinCore.Models.Messages.ReplyMessages
{
    [Serializable]
    public class ReplyNewsMessage : ReplyMessage
    {
        [XmlArray(nameof(Articles))]
        [XmlArrayItem("item")]
        public List<NewsArticle> Articles { get; set; } = new List<NewsArticle>();

        public int ArticleCount 
        { 
            get 
            { 
                return Articles.Count; 
            } 
        }
    }
}
