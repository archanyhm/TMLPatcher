﻿using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TMLPatcher.Common
{
    public sealed class ConfigurationFile
    {
        [JsonIgnore]
        public const string UndefinedPath = "undefined";

        public static string FilePath { get; private set; }

        public string ModsPath { get; set; }

        internal ConfigurationFile() { }

        public static ConfigurationFile Load(string filePath)
        {
            FilePath = filePath;

            if (File.Exists(filePath))
                return JsonConvert.DeserializeObject<ConfigurationFile>(File.ReadAllText(filePath));

            Console.WriteLine(" Configuration file not found! Generating a new config.json file...");

            ConfigurationFile config = new()
            {
                ModsPath = UndefinedPath
            };
            JsonSerializer serializer = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            using (StreamWriter writer = new(filePath))
            using (JsonWriter jWriter = new JsonTextWriter(writer)) 
            { serializer.Serialize(jWriter, config); }

            Console.WriteLine($" Created a new configuration file in: {filePath}");

            return JsonConvert.DeserializeObject<ConfigurationFile>(File.ReadAllText(filePath));
        }

        public static void Save()
        {
            ConfigurationFile config = new()
            {
                ModsPath = Program.Configuration.ModsPath
            }; 
            JsonSerializer serializer = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            using (StreamWriter writer = new(FilePath))
            using (JsonWriter jWriter = new JsonTextWriter(writer))
            { serializer.Serialize(jWriter, config); }

            Program.Configuration = JsonConvert.DeserializeObject<ConfigurationFile>(File.ReadAllText(FilePath));
        }
    }
}
