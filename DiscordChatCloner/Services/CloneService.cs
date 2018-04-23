using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using DiscordChatCloner.Models;

namespace DiscordChatCloner.Services
{
    public partial class CloneService : ICloneService
    {
        private readonly IDataService _dataService;
        private Timer aTimer;

        public CloneService(IDataService dataService) {
            _dataService = dataService;
        }

        public async Task<Timer> CloneAsync(string token, Cloner cloner) {
            aTimer = new Timer(cloner.PollingFrequency);
            aTimer.AutoReset = true;

            var testMsgs = await _dataService.GetChannelMessagesAsync(token, cloner.FromChannel.Id, null);
            var lastMessageId = testMsgs[0].Id;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += async (s, e) => {
                Console.WriteLine("{0:HH:mm:ss.fff} [{1}] polling for messages", e.SignalTime, cloner.Name);
                var messages = await _dataService.GetChannelMessagesAsync(token, cloner.FromChannel.Id, lastMessageId);
                foreach (var msg in messages) {
                    var newMessage = await _dataService.PublishStringAsync(token, cloner.ToChannel.Id, msg.Content);
                    lastMessageId = msg.Id;
                }
            };

            aTimer.Enabled = true;
            return aTimer;
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