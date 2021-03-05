using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using O2ArchiveReader.Models;
using O2ArchiveReader.Services;

namespace O2ArchiveReader.ViewModels
{
    public class ShellViewModel : ObservableObject
    {
        private readonly ChatReaderService _charReaderService = new(); //TODO: use ioc
        private bool _isBusy;
        private List<IdxChat> _chats;
        private IdxChat _selectedChat;

        public ShellViewModel()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            Task.Run(async () => 
            {
                IsBusy = true;
                var settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText("appSettings.json"));
                Chats = await _charReaderService.ReadChatsAsync(settings.ArchiveRootFolder);
                IsBusy = false;
            });
        }
  
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public List<IdxChat> Chats
        {
            get => _chats;
            set => SetProperty(ref _chats, value);
        }

        public IdxChat SelectedChat
        {
            get => _selectedChat;
            set => SetProperty(ref _selectedChat, value);
        }
    }   
}
