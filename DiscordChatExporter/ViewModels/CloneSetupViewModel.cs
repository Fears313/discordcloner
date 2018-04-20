using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DiscordChatExporter.Messages;
using DiscordChatExporter.Models;
using DiscordChatExporter.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Tyrrrz.Extensions;

namespace DiscordChatExporter.ViewModels
{
    public class CloneSetupViewModel : ViewModelBase, ICloneSetupViewModel
    {
        private readonly ISettingsService _settingsService;

        private ExportFormat _format;
        private DateTime? _from;
        private DateTime? _to;

        public Channel _fromChannel;
        public Channel _toChannel;
        public IReadOnlyList<Channel> _availableChannels;

        public Guild Guild { get; private set; }

        public Channel Channel { get; private set; }

        public IReadOnlyList<Channel> AvailableChannels {
            get => _availableChannels;
            set => Set(ref _availableChannels, value);
        }

        public Channel SelectedChannel
        {
            get => _toChannel;
            set
            {
                Set(ref _toChannel, value);
            }
        }

        public IReadOnlyList<ExportFormat> AvailableFormats { get; }

        public ExportFormat SelectedFormat
        {
            get => _format;
            set
            {
                Set(ref _format, value);
            }
        }

        public DateTime? From
        {
            get => _from;
            set => Set(ref _from, value);
        }

        public DateTime? To
        {
            get => _to;
            set => Set(ref _to, value);
        }


        public Channel FromChannel
        {
            get => _fromChannel;
            set => Set(ref _fromChannel, value);
        }

        public Channel ToChannel
        {
            get => _toChannel;
            set => Set(ref _toChannel, value);
        }

        // Commands
        public RelayCommand CloneCommand { get; }

        public CloneSetupViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            // Defaults
            AvailableFormats = Enum.GetValues(typeof(ExportFormat)).Cast<ExportFormat>().ToArray();

            // Commands
//            CloneCommand = new RelayCommand(Clone, () => FilePath.IsNotBlank());
            CloneCommand = new RelayCommand(Clone);

            // Messages
            MessengerInstance.Register<ShowCloneSetupMessage>(this, m =>
            {
                Guild = m.Guild;
                Channel = m.Channel;

                FromChannel = m.Channel;
// TODO
                ToChannel = m.Channel;

                SelectedFormat = _settingsService.LastExportFormat;
                From = null;
                To = null;
                AvailableChannels = m.AvailableChannels;
            });
        }

        private void Clone()
        {
            // Save format
            _settingsService.LastExportFormat = SelectedFormat;

            // Start export
            MessengerInstance.Send(new StartCloneMessage(Channel, SelectedFormat, From, To, FromChannel, ToChannel));
        }
    }
}