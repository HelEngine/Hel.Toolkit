using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Hel.Toolkit.Encryption;
using Newtonsoft.Json;

namespace Hel.Toolkit.Serializer
{
    /// <summary>
    /// Helper class for serializing objects
    /// </summary>
    public static class ByteSerializer
    {
        /// <summary>
        /// Deserialize a loaded array of bytes back into an object.
        /// </summary>
        /// <param name="arrBytes">File data</param>
        /// <returns></returns>
        public static T ByteArrayToObject<T>(byte[] arrBytes, byte[] key)
        {
            // Decrypt the bytes to a string.
            var json = HelAes.DecryptStringFromBytes(arrBytes, key);

            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });
        }

        /// <summary>
        /// Serialize an object into an encrypted json string
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns></returns>
        public static byte[] ObjectToByteArray(object obj, byte[] key)
        {
            var serializeObject = JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            return HelAes.EncryptStringToBytes(serializeObject, key);
        }
    }
}