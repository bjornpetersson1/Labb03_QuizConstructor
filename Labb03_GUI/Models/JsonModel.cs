using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Labb03_GUI.Models
{
    internal class JsonModel
    {
        private readonly string _filePath;
        public JsonModel()
        {
            _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "packs.json");
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);                
        }
        public async Task SaveToJsonAsync(List<QuestionPack> packs)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            using FileStream strem = new FileStream(_filePath, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(strem, packs, options);
        }

        public async Task<List<QuestionPack>> LoadFromJsonAsync()
        {
            if(!File.Exists(_filePath))
                return new List<QuestionPack>();

            using FileStream stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            var packs = await JsonSerializer.DeserializeAsync<List<QuestionPack>>(stream);
            return packs ?? new List<QuestionPack>();
        }
        
    }
}
