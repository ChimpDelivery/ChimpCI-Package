using System;
using System.Collections.Generic;

namespace TalusCI.Editor
{
    public static class CommandLineParser
    {
        private const char COMMAND_PREFIX = '-';
        
        public static Dictionary<string, string> GetCommandLineArguments()
        {
            var commandToValueDictionary = new Dictionary<string, string>();
            string[] args = Environment.GetCommandLineArgs();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith(COMMAND_PREFIX.ToString(), StringComparison.Ordinal))
                {
                    string command = args[i];
                    string value = string.Empty;

                    if (i < args.Length - 1 && !args[i + 1].StartsWith(COMMAND_PREFIX.ToString(), StringComparison.Ordinal))
                    {
                        value = args[i + 1];
                        i++;
                    }

                    if (!commandToValueDictionary.ContainsKey(command))
                    {
                        commandToValueDictionary.Add(command, value);
                    }
                }
            }

            return commandToValueDictionary;
        }
    }
}
