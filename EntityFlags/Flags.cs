using BrokeProtocol.Entities;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace EntityFlags
{
    public static class Flags
    {
        public class Flag
        {
            public string name;
            public object param;
        }

        private static readonly string regex = @"flags\[(.*?)\]";

        public static Flag[] GetFlags(this ShEntity entity)
        {
            Flag[] flags = new Flag[0];

            string data = string.IsNullOrWhiteSpace(entity.data) ? string.Empty : entity.data.ToLowerInvariant();

            Match match = Regex.Match(data, regex);

            if (match.Success)
            {
                string[] flagsStrings = match.Groups[1].Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                flags = new Flag[flagsStrings.Length];

                for (int i = 0; i < flags.Length; i++)
                {
                    string[] parts = flagsStrings[i].Split(':');
                    flags[i] = new Flag() { name = parts[0] };

                    if (parts.Length > 1)
                    {
                        flags[i].param = Utils.ParseValue(parts[1]);
                    }
                }
            }

            return flags;
        }

        public static T GetFlagValue<T>(this ShEntity entity, string flagName) => (T)entity.GetFlags().FirstOrDefault(x => x.name == flagName).param;

        public static bool TryGetFlagValue<T>(this ShEntity entity, string flagName, out T value)
        {
            Flag flag = entity.GetFlags().FirstOrDefault(x => x.name == flagName);

            if (flag != null && flag.param != null)
            {
                value = (T)flag.param;
                return true;
            }

            value = default;
            return false;
        }

        public static bool HasFlag(this ShEntity entity, string flagName) => entity.GetFlags().Any(x => x.name == flagName.ToLowerInvariant());

        public static bool HasFlags(this ShEntity entity) => Regex.IsMatch(string.IsNullOrWhiteSpace(entity.data) ? string.Empty : entity.data.ToLowerInvariant(), regex);
    }
}
