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
        public async Task SaveToJsonAsync(List<QuestionPack> packs)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            using FileStream strem = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(strem, packs, options);
        }

        public async Task<List<QuestionPack>> LoadFromJsonAsync(string filePath)
        {
            if(!File.Exists(filePath))
                return new List<QuestionPack>();

            using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var packs = await JsonSerializer.DeserializeAsync<List<QuestionPack>>(stream);
            return packs ?? new List<QuestionPack>();
        }
        
    }
}
