using System;
using System.IO;
using System.Text;

namespace YuMi.Bbkpify.GUI
{
    public class Configuration
    {
        /// <summary>
        /// Location of the configuration file used for persistent values.
        /// </summary>
        private static string ConfigFile =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YuMi.Bbkpify.cfg");
        
        public string Placeholder { get; set; }
        public string Directory { get; set; }
        public bool NrmlPattern { get; set; }
        public bool MultiPattern { get; set; }
        public bool DiffPattern { get; set; }
        public string SapienExecutable { get; set; }

        /// <summary>
        ///     Serialises the inbound Configuration instance to a persistent file.
        /// </summary>
        /// <param name="configuration">
        ///    Instance of the Configuration class.
        /// </param>
        public static void Save(Configuration configuration)
        {
            var config = new Func<string>(() =>
            {
                var existing = Load();
                
                var s = new StringBuilder();
                s.Append($"{configuration.Placeholder ?? existing.Placeholder}|");
                s.Append($"{configuration.Directory ?? existing.Directory}|");
                s.Append($"{configuration.NrmlPattern}|");
                s.Append($"{configuration.MultiPattern}|");
                s.Append($"{configuration.DiffPattern}|");
                s.Append($"{configuration.SapienExecutable ?? existing.SapienExecutable}");
                return s.ToString();
            })();
            
            File.WriteAllText(ConfigFile, config);
        }

        /// <summary>
        ///     Deserialises the persistent file to a new Configuration instance.
        /// </summary>
        public static Configuration Load()
        {
            if (!File.Exists(ConfigFile))
            {
                return new Configuration();
            }

            var config = File.ReadAllText(ConfigFile).Split('|');

            return new Configuration
            {
                Placeholder = config[0],
                Directory = config[1],
                NrmlPattern = config[2].Equals("True"),
                MultiPattern = config[3].Equals("True"),
                DiffPattern = config[4].Equals("True"),
                SapienExecutable = config[5]
            };
        }
    }
}