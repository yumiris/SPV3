using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Serialization;

namespace SPV3.Bbkpify.Core.Common
{
  /// <summary>
  ///   Type offering common generic code to repository types.
  /// </summary>
  public abstract class Repository
  {
    /// <summary>
    ///   Serialises the inbound object to a deflated binary.
    /// </summary>
    /// <param name="instance">
    ///   Object to persistently serialise.
    /// </param>
    /// <param name="path">
    ///   Path of the binary on the filesystem which should persistently store the serialised data.
    /// </param>
    protected static void Save<T>(T instance, string path)
    {
      using (var deflatedStream = new MemoryStream())
      using (var inflatedStream = new MemoryStream(Encoding.UTF8.GetBytes(ToXml(instance))))
      using (var compressStream = new DeflateStream(deflatedStream, CompressionMode.Compress))
      {
        inflatedStream.CopyTo(compressStream);
        compressStream.Close();
        File.WriteAllBytes(path, deflatedStream.ToArray());
      }
    }

    /// <summary>
    ///   Inflates and deserialises the inbound object to an object.
    /// </summary>
    /// <param name="path">
    ///   Path to the file containing the serialised & deflated object.
    /// </param>
    /// <typeparam name="T">
    ///   Type of object to deserialise the data in the path to.
    /// </typeparam>
    /// <returns>
    ///   Object representation of the file specified in the inbound path.
    /// </returns>
    protected static T Load<T>(string path)
    {
      using (var inflatedStream = new MemoryStream())
      using (var deflatedStream = new MemoryStream(File.ReadAllBytes(path)))
      using (var compressStream = new DeflateStream(deflatedStream, CompressionMode.Decompress))
      {
        compressStream.CopyTo(inflatedStream);
        compressStream.Close();
        return FromXml<T>(Encoding.UTF8.GetString(inflatedStream.ToArray()));
      }
    }

    /// <summary>
    ///   Serialises generic inbound object to its XML equivalent.
    /// </summary>
    /// <param name="instance">
    ///   Object to serialise.
    /// </param>
    /// <typeparam name="T">
    ///   Inbound object type.
    /// </typeparam>
    /// <returns>
    ///   XML representation of the inbound project's public properties.
    /// </returns>
    private static string ToXml<T>(T instance)
    {
      using (var writer = new StringWriter())
      {
        var serialiser = new XmlSerializer(typeof(T));
        serialiser.Serialize(writer, instance);
        return writer.ToString();
      }
    }

    /// <summary>
    ///   Deserialises inbound XML to its object equivalent.
    /// </summary>
    /// <param name="xml">
    ///   XML to deserialise to an object.
    /// </param>
    /// <typeparam name="T">
    ///   Type of object that the inbound XML represents.
    /// </typeparam>
    /// <returns>
    ///   Object representation of the inbound XML.
    /// </returns>
    private static T FromXml<T>(string xml)
    {
      using (var reader = new StringReader(xml))
      {
        var serialiser = new XmlSerializer(typeof(T));
        return (T) serialiser.Deserialize(reader);
      }
    }
  }
}