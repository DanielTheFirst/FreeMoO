using System;
using System.Collections.Generic;
using System.IO;

namespace FreeMoO
{

    public struct ChannelInfo
    {
        public string FileName;
        public TextWriter Writer;
    }

    public static class Log
    {
        // liberally borrowed from OpenRA
        static readonly Dictionary<string, ChannelInfo> Channels = new Dictionary<string, ChannelInfo>();

        public static ChannelInfo Channel(string channelName)
        {
            ChannelInfo info;
            if (!Channels.TryGetValue(channelName, out info))
            {
                throw new Exception("You tried to write to a channel that was never initialized " + channelName);
            }
            return info;
        }

        private static IEnumerable<string> GetSafeFilename(string chanName, string fileName)
        {
            var path = Path.Combine(Config.DataFolder, "Log");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            for (int i = 0;; i++)
            {
                yield return Path.Combine(path, i > 0 ? "{0}.{1}".Fmt(fileName, i) : fileName);
            }
        }

        public static void AddChannel(string channelName, string baseFilename)
        {
            if (Channels.ContainsKey(channelName)) return;

            if (string.IsNullOrEmpty(baseFilename))
            {
                Channels.Add(channelName, new ChannelInfo());
            }

            foreach(var filename in GetSafeFilename(channelName, baseFilename))
            {
                if (File.Exists(filename)) continue;

                var writer = File.CreateText(filename);
                writer.AutoFlush = true;

                Channels.Add(channelName, new ChannelInfo() { FileName = filename, Writer = TextWriter.Synchronized(writer) });
            }
        }

        public static void Write(string channelName, string val)
        {
            var writer = Channel(channelName).Writer;
            if (writer == null) return;

            writer.WriteLine(val);
        }

        public static void Write(string channelName, string val, params object[] args)
        {
            var writer = Channel(channelName).Writer;
            if (writer == null) return;

            writer.WriteLine(val, args);
        }
    }
}
