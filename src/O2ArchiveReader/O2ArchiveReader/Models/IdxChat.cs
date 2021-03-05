using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace O2ArchiveReader.Models
{
    [DebuggerDisplay("{Name}. Id: {Id}, ({Time})")]
    public class IdxChat
    {
        public string Name { get; set; }
        public string Network { get; set; }
        public DateTime Time { get; set; }
        public int Flags { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
        public int Id { get; set; }

        public List<DatChat> Chats { get; } = new List<DatChat>();
    }
}
