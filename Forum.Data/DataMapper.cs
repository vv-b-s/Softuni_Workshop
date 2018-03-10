using Forum.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/*
 * This will be a singleton class, as there cannot be more than one DataMapper
 */
namespace Forum.Data
{
    public class DataMapper
    {
        private const string DATA_PATH = "../data/";
        private const string CONFIG_PATH = "config.ini";

        private static DataMapper instance;

        //Contains the names of the improvised "database files". Using singular to match the name of the classes for easy reflection
        private const string DefaultConfig = "user=user.csv\r\ncategory=category.csv\r\npost=post.csv\r\nreply=reply.csv";

        private static Dictionary<string, string> config;

        private DataMapper()
        {
            Directory.CreateDirectory(DATA_PATH);
            config = LoadConfig(DATA_PATH + CONFIG_PATH);
        }

        private static void EnsureConfigFile(string configPath)
        {
            if (!File.Exists(configPath))
                File.WriteAllText(configPath, DefaultConfig);
        }

        private static void EnsureFile(string path)
        {
            if (!File.Exists(path))
                File.Create(path).Close();
        }

        /// <summary>
        /// Parses the information from the config.ini file into a Dictionary containing the names of the database files
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns>Dictionary with the file name as a key and it's full name as a value</returns>
        private static Dictionary<string, string> LoadConfig(string configPath)
        {
            EnsureConfigFile(configPath);

            var contents = ReadLines(configPath);

            var config = contents
                .Select(l => l.Split('='))
                .ToDictionary(t => t[0], t => DATA_PATH + t[1]);

            return config;      
        }

        private static string[] ReadLines(string path)
        {
            EnsureFile(path);
            var lines = File.ReadAllLines(path);
            return lines;
        }

        /// <summary>
        /// Saves the given lines to the file where the path is pointing to
        /// </summary>
        /// <param name="path">Usually the value from the coresponding config pair, which has its own .csv extention</param>
        /// <param name="lines">the content which the .csv file will be feeded wit - a string array with where every attribute is separated with semmicolin ';'</param>
        private static void WriteLines(string path, string[] lines) => 
            File.WriteAllLines(path, lines);

        public static List<TModel> LoadModels<TModel>() where TModel : Model
        {
            var configKey = typeof(TModel).Name.ToLower();
            var models = new List<TModel>();
            var dataLines = ReadLines(config[configKey]);

            foreach (var line in dataLines)
            {
                object[] args = line.Split(";", StringSplitOptions.RemoveEmptyEntries);
                var model = Activator.CreateInstance(typeof(TModel), args) as TModel;
                models.Add(model);
            }

            return models;
        }

        public static void SaveChanges<TModel>(List<TModel> models) where TModel : Model
        {
            var configKey = typeof(TModel).Name.ToLower();
            var lines = new List<string>();

            foreach (var model in models)
            {
                var line = model.ToString();
                lines.Add(line);
            }

            WriteLines(config[configKey], lines.ToArray());
        }

        public static DataMapper GetDataMapper()
        {
            if (instance is null)
                instance = new DataMapper();

            return instance;
        }
    }
}
