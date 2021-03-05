using System;
using System.Diagnostics;

namespace O2ArchiveReader.Models
{
    [DebuggerDisplay("{Id} {Time} {Message}")]
    public class DatChat
    {
        public DateTime Time { get; set; }
        public int Flags { get; set; }
        public int Size { get; set; }
        public int Id { get; set; }
        public int Unknown { get; set; }
        public string Message { get; set; }

        public byte[] Bytes { get; set; } //debug data
    }
}
