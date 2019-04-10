using Flurl.Http.Xml;
using System.IO;
using System.Xml.Serialization;

namespace DefibrillatorService.Extensions
{
    public static class XmlExtensions
    {
        public static string SerializeObjectToXml<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new Utf8StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
    }
}
