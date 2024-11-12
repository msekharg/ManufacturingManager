// using System.Diagnostics;
// using System.Xml.Serialization;
// using System.Xml;
// using PCSCaseManagement.Web.FileNetIntegration;
// using PCSCaseManagement.Web.FileNetIntegration.XML;
//
// namespace PCSCaseManagement.Web.Services;
//
// public interface ISoapXmlSerialization
// {
//     String Serialize<T>(Envelope<T> input, String payloadType);
// }
//
// public class SoapXmlSerialization : ISoapXmlSerialization
// {
//     public String Serialize<T>(Envelope<T> input, String payloadType)
//     {
//         Stopwatch.StartNew();
//
//         var ns = new XmlSerializerNamespaces();
//         ns.Add(XmlNamespaces.EnvelopePrefix, XmlNamespaces.EnvelopeNamespace);
//         ns.Add(XmlNamespaces.WSSecurityPrefix, XmlNamespaces.WSSecurityNamespace);
//         ns.Add(XmlNamespaces.LeaseSchemaPrefix, XmlNamespaces.LeaseSchemaNamespace);
//         ns.Add(XmlNamespaces.LDAPSchemaPrefix, XmlNamespaces.LDAPSchemaNamespace);
//         ns.Add(XmlNamespaces.UsrStnSchemaPrefix, XmlNamespaces.UsrStnSchemaNamespace);
//         ns.Add(XmlNamespaces.SOACommonPrefix, XmlNamespaces.SOACommonNamespace);
//         ns.Add(XmlNamespaces.CodeSchemaPrefix, XmlNamespaces.CodeSchemaNamespace);
//         ns.Add(XmlNamespaces.CMISMessagingSchemaPrefix, XmlNamespaces.CMISMessagingSchemaNamespace);
//         ns.Add(XmlNamespaces.CMISCoreSchemaPrefix, XmlNamespaces.CMISCoreSchemaNamespace);
//         ns.Add(XmlNamespaces.CreateLeaseSchemaPrefix, XmlNamespaces.CreateLeaseSchemaNamespace);
//         ns.Add(XmlNamespaces.ReportSchemaPrefix, XmlNamespaces.ReportSchemaNamespace);
//
//         using (var stringWriter = new StringWriter())
//         using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true }))
//         {
//             new XmlSerializer(input.GetType()).Serialize(xmlWriter, input, ns);
//             var result = stringWriter.ToString();
//
//             // There has to be a better way than this...
//             result = result.Replace("soapenv:BodyContent", payloadType);
//
// #if DEBUG
//             //Console.WriteLine(String.Format("Serializing of {0} finished in {1} ms.", payloadType, stopwatch.ElapsedMilliseconds));
// #endif
//
//             return result;
//         }
//     }
// }
//  