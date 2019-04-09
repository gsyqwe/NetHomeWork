using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
//实现序列化，借鉴了网上代码实现序列化操作
//https://blog.csdn.net/Joetao/article/details/2835676
namespace thirdhomework
{
    [Serializable]
    class xuliehua : ISerializable
    {
        public xuliehua() { }
        internal xuliehua(SerializationInfo info, StreamingContext context)
        {
            xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(info.GetString("xml"));
        }
        [NonSerialized]
        private XmlDocument xmlDocument;

        public XmlDocument XmlDocument { get => xmlDocument; set => xmlDocument = value; }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("xml", xmlDocument.InnerXml.ToString());
            info.AddValue("typeObj", this.GetType());
        }
    }
}
