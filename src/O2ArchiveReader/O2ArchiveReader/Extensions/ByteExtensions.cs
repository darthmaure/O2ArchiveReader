using System;
using System.Linq;
using System.Text;

namespace O2ArchiveReader.Extensions
{
    public static class ByteExtensions
    {
        public static int GetInt(this byte[] source, int offset) => BitConverter.ToInt32(source.Skip(offset).Take(sizeof(int)).ToArray());
        public static DateTime GetDate(this byte[] source, int offset) => DateTime.FromOADate(BitConverter.ToDouble(source.Skip(offset).Take(sizeof(double)).ToArray()));
        public static string GetString(this byte[] source, int offset, int size) => Encoding.UTF8.GetString(source.Skip(offset).Take(size).ToArray()).Replace("\0", "");
        public static string GetString(this byte[] source, string encoding) => Encoding.GetEncoding(encoding).GetString(source).Replace("\0", "");
    }
}
