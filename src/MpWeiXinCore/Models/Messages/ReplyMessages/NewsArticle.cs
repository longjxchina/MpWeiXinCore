using System;
using System.Xml;
using System.Xml.Serialization;

namespace MpWeiXinCore.Models.Messages.ReplyMessages
{
    [Serializable]
    public class NewsArticle
    {
        [XmlIgnore]
        public string Title { get; set; }
        [XmlElement("Title")]
        public XmlCDataSection TitleCDATA
        {
            get
            {
                return new XmlDocument().CreateCDataSection(this.Title);
            }
            set
            {
                this.Title = value.Value;
            }
        }
        [XmlIgnore]
        public string Description { get; set; }
        [XmlElement("Description")]
        public XmlCDataSection DescriptionCDATA
        {
            get
            {
                return new XmlDocument().CreateCDataSection(this.Description);
            }
            set
            {
                this.Description = value.Value;
            }
        }
        [XmlIgnore]
        public string PicUrl { get; set; }
        [XmlElement("PicUrl")]
        public XmlCDataSection PicUrlCDATA
        {
            get
            {
                return new XmlDocument().CreateCDataSection(this.PicUrl);
            }
            set
            {
                this.PicUrl = value.Value;
            }
        }
        [XmlIgnore]
        public string Url { get; set; }
        [XmlElement("Url")]
        public XmlCDataSection UrlCDATA
        {
            get
            {
                return new XmlDocument().CreateCDataSection(this.Url);
            }
            set
            {
                this.Url = value.Value;
            }
        }
    }
}
