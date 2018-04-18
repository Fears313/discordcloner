using System;
using System.Collections.Generic;
using DiscordChatExporter.Models;
using GalaSoft.MvvmLight.CommandWpf;

namespace DiscordChatExporter.ViewModels
{
    public interface ICloneSetupViewModel
    {
        Guild Guild { get; }
        Channel Channel { get; }
        string FilePath { get; set; }
        IReadOnlyList<ExportFormat> AvailableFormats { get; }
        ExportFormat SelectedFormat { get; set; }
        DateTime? From { get; set; }
        DateTime? To { get; set; }

        RelayCommand CloneCommand { get; }
    }
}