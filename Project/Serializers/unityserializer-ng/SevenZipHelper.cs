using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.IO;

/// <summary>
/// Class CompressionHelper.
/// </summary>
public static class CompressionHelper
{
    /// <summary>
    /// The technique
    /// </summary>
    public static string technique = "ZipStream";

    /// <summary>
    /// Compresses the specified data.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>System.String.</returns>
    public static string Compress(byte[] data)
    {
        using (var m = new MemoryStream())
        {
            switch (technique)
            {
                case "ZipStream":
                    using (var br = new BinaryWriter(m))
                    using (var z = new DeflaterOutputStream(m))
                    {
                        br.Write(data.Length);
                        z.Write(data, 0, data.Length);
                        z.Flush();
                    }
                    break;
            }
            return technique + ":" + Convert.ToBase64String(m.GetBuffer());
        }
    }

    /// <summary>
    /// Decompresses the specified data.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns>System.Byte[].</returns>
    public static byte[] Decompress(string data)
    {
        byte[] output = null;
        if (data.StartsWith("ZipStream:"))
        {
            using (var m = new MemoryStream(Convert.FromBase64String(data.Substring(10))))
            using (var z = new InflaterInputStream(m))
            using (var br = new BinaryReader(m))
            {
                var length = br.ReadInt32();
                output = new byte[length];
                z.Read(output, 0, length);
            }
        }
        return output;
    }
}