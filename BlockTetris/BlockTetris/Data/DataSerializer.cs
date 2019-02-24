using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BlockTetris.Data
{
    class DataSerializer<T>
    {
        public static string Serialize(T data)
        {
            StringWriter xml = new StringWriter();
            try
            {
                XmlSerializer xmlIzer = new XmlSerializer(typeof(T));
                xmlIzer.Serialize(xml, data);
                return xml.ToString();
            }

            catch (Exception)
            {
                return xml.ToString();
            }
        }

        public static T Deserialize(string xml)
        {
            T data = default(T);
            try
            {
                XmlSerializer xmlIzer = new XmlSerializer(typeof(T));
                data = (T)xmlIzer.Deserialize(new StringReader(xml));
                return data;
            }
            catch (Exception)
            {
                return data;
            }
        }
    }
}
