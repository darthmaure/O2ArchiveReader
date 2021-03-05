using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using O2ArchiveReader.Extensions;
using O2ArchiveReader.Models;

namespace O2ArchiveReader.Services
{
    public class ChatReaderService
    {
        const int _chatMessageCoreSize = 24;
        const int _chatIndexSize = 26 + 6 + sizeof(double) + 4 * sizeof(int);
        const string _encoding = "ISO-8859-2";

        public async Task<List<IdxChat>> ReadChatsAsync(string root)
        {
            var results = new List<IdxChat>();
            try
            {
                using BinaryReader reader = new BinaryReader(File.Open(root + "\\chats.idx", FileMode.Open));

                var fileSize = reader.BaseStream.Length;

                for (int i = 0; i < fileSize / _chatIndexSize; i++)
                {
                    reader.BaseStream.Seek(i * _chatIndexSize, SeekOrigin.Begin);

                    var itemBytes = new byte[_chatIndexSize];
                    await reader.BaseStream.ReadAsync(itemBytes.AsMemory(0, _chatIndexSize));

                    var item = new IdxChat
                    {
                        Name = itemBytes.GetString(0, 26),
                        Network = itemBytes.GetString(26,6),
                        Time = itemBytes.GetDate(32),
                        Flags = itemBytes.GetInt(40),
                        Offset = itemBytes.GetInt(44),
                        Count = itemBytes.GetInt(48),
                        Id = itemBytes.GetInt(52)
                    };
                    results.Add(item);
                }

                await ReadMessages(root, results);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return results;
        }

        private async Task ReadMessages(string root, IList<IdxChat> chats)
        {
            var chatFile = root + "\\chats.dat";
            try
            {
                using BinaryReader reader = new(File.Open(chatFile, FileMode.Open), Encoding.GetEncoding(_encoding));

                int messagesReadOffset = 0;

                while (messagesReadOffset < reader.BaseStream.Length)
                {
                    reader.BaseStream.Seek(messagesReadOffset, SeekOrigin.Begin);
                    var messageCoreData = new byte[_chatMessageCoreSize];

                    await reader.BaseStream.ReadAsync(messageCoreData.AsMemory(0, _chatMessageCoreSize));
                    DatChat message = ReadMessageCoreData(messageCoreData);

                    await ReadMessageText(reader, messageCoreData, message);

                    messagesReadOffset += _chatMessageCoreSize + message.Size;

                    var parentChat = chats.FirstOrDefault(d => d.Id == message.Id);
                    if (parentChat != null)
                    {
                        parentChat.Chats.Add(message);
                    }
                }
            }
            catch (Exception exc)
            {
                exc.ToString();
            }
        }

        private static async Task ReadMessageText(BinaryReader reader, byte[] messageCoreData, DatChat message)
        {
            reader.BaseStream.Seek(_chatMessageCoreSize, SeekOrigin.Current);
            var messageTextBytes = new byte[message.Size];
            await reader.BaseStream.ReadAsync(messageTextBytes.AsMemory(0, message.Size));
            message.Message = messageTextBytes.GetString(_encoding);

            message.Bytes = messageCoreData.Concat(messageTextBytes).ToArray();
        }

        private static DatChat ReadMessageCoreData(byte[] messageCoreData) => new DatChat
        {
            Time = messageCoreData.GetDate(0),
            Flags = messageCoreData.GetInt(sizeof(double)),
            Size = messageCoreData.GetInt(sizeof(double) + sizeof(int)),
            Id = messageCoreData.GetInt(sizeof(double) + sizeof(int) * 2),
            Unknown = messageCoreData.GetInt(sizeof(double) + sizeof(int) * 3),
        };
    }
}
