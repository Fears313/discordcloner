using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DiscordChatExporter.Models;
using Tyrrrz.Extensions;

namespace DiscordChatExporter.Services
{
    public partial class CloneService : ICloneService
    {
        private readonly ISettingsService _settingsService;
        private readonly IDataService _dataService;

        public CloneService(ISettingsService settingsService, IDataService dataService)
        {
            _settingsService = settingsService;
            _dataService = dataService;
        }

        private async Task CloneAsTextAsync(string token, Channel toChannel, ChannelChatLog log)
        {
            // Chat log
            foreach (var group in log.MessageGroups)
            {
                var timeStampFormatted = group.TimeStamp.ToString(_settingsService.DateFormat);

                // Messages
                foreach (var message in group.Messages)
                {
                    // Content
                    if (message.Content.IsNotBlank())
                    {
                        var contentFormatted = FormatMessageContentText(message);

                        var doodle = await _dataService.PublishMessage(token, toChannel.Id, contentFormatted);

                        Console.WriteLine("Have a message to clone " + message.Content);
                    }

                    // Attachments
                    foreach (var attachment in message.Attachments)
                    {
                       Console.WriteLine("Have an attachment " + attachment.Url);
                    }
                }

            }
        }

        public Task CloneAsync(string token, Channel toChannel, ChannelChatLog channelChatLog)
        {
            return CloneAsTextAsync(token, toChannel, channelChatLog);
        }
    }

    public partial class CloneService
    {
        private static string HtmlEncode(string str)
        {
            return WebUtility.HtmlEncode(str);
        }

        private static string HtmlEncode(object obj)
        {
            return WebUtility.HtmlEncode(obj.ToString());
        }

        private static string FormatFileSize(long fileSize)
        {
            string[] units = {"B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};
            double size = fileSize;
            var unit = 0;

            while (size >= 1024)
            {
                size /= 1024;
                ++unit;
            }

            return $"{size:0.#} {units[unit]}";
        }

        public static string FormatMessageContentText(Message message)
        {
            var content = message.Content;

            // New lines
            content = content.Replace("\n", Environment.NewLine);

            // User mentions (<@id>)
            foreach (var mentionedUser in message.MentionedUsers)
                content = Regex.Replace(content, $"<@!?{mentionedUser.Id}>", $"@{mentionedUser}");

            // Role mentions (<@&id>)
            foreach (var mentionedRole in message.MentionedRoles)
                content = content.Replace($"<@&{mentionedRole.Id}>", $"@{mentionedRole.Name}");

            // Channel mentions (<#id>)
            foreach (var mentionedChannel in message.MentionedChannels)
                content = content.Replace($"<#{mentionedChannel.Id}>", $"#{mentionedChannel.Name}");

            // Custom emojis (<:name:id>)
            content = Regex.Replace(content, "<(:.*?:)\\d*>", "$1");

            return content;
        }

        private static string FormatMessageContentHtml(Message message)
        {
            var content = message.Content;

            // Encode HTML
            content = HtmlEncode(content);

            // Pre multiline (```text```)
            content = Regex.Replace(content, "```+(?:[^`]*?\\n)?([^`]+)\\n?```+", "<div class=\"pre\">$1</div>");

            // Pre inline (`text`)
            content = Regex.Replace(content, "`([^`]+)`", "<span class=\"pre\">$1</span>");

            // Bold (**text**)
            content = Regex.Replace(content, "\\*\\*([^\\*]*?)\\*\\*", "<b>$1</b>");

            // Italic (*text*)
            content = Regex.Replace(content, "\\*([^\\*]*?)\\*", "<i>$1</i>");

            // Underline (__text__)
            content = Regex.Replace(content, "__([^_]*?)__", "<u>$1</u>");

            // Italic (_text_)
            content = Regex.Replace(content, "_([^_]*?)_", "<i>$1</i>");

            // Strike through (~~text~~)
            content = Regex.Replace(content, "~~([^~]*?)~~", "<s>$1</s>");

            // New lines
            content = content.Replace("\n", "<br />");

            // URL links
            content = Regex.Replace(content, "((https?|ftp)://[^\\s/$.?#].[^\\s<>]*)", "<a href=\"$1\">$1</a>");

            // Meta mentions (@everyone)
            content = content.Replace("@everyone", "<span class=\"mention\">@everyone</span>");

            // Meta mentions (@here)
            content = content.Replace("@here", "<span class=\"mention\">@here</span>");

            // User mentions (<@id>)
            foreach (var mentionedUser in message.MentionedUsers)
            {
                content = Regex.Replace(content, $"&lt;@!?{mentionedUser.Id}&gt;",
                    $"<span class=\"mention\" title=\"{HtmlEncode(mentionedUser)}\">" +
                    $"@{HtmlEncode(mentionedUser.Name)}" +
                    "</span>");
            }

            // Role mentions (<@&id>)
            foreach (var mentionedRole in message.MentionedRoles)
            {
                content = content.Replace($"&lt;@&amp;{mentionedRole.Id}&gt;",
                    "<span class=\"mention\">" +
                    $"@{HtmlEncode(mentionedRole.Name)}" +
                    "</span>");
            }

            // Channel mentions (<#id>)
            foreach (var mentionedChannel in message.MentionedChannels)
            {
                content = content.Replace($"&lt;#{mentionedChannel.Id}&gt;",
                    "<span class=\"mention\">" +
                    $"#{HtmlEncode(mentionedChannel.Name)}" +
                    "</span>");
            }

            // Custom emojis (<:name:id>)
            content = Regex.Replace(content, "&lt;(:.*?:)(\\d*)&gt;",
                "<img class=\"emoji\" title=\"$1\" src=\"https://cdn.discordapp.com/emojis/$2.png\" />");

            return content;
        }
    }
}