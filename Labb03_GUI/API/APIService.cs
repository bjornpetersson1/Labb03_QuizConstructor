using Labb03_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Labb03_GUI.API
{
    internal class APIService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public async Task<List<Question>> GetQuestionsAsync()
        {
            var url = "https://opentdb.com/api.php?amount=10";
            var questions = await _httpClient.GetFromJsonAsync<List<Question>>(url);
            return questions ?? new List<Question>();
        }
        public async Task<List<Category>> GetCategoriesAsync()
        {
            var url = "https://opentdb.com/api_category.php";
            var response = await _httpClient.GetFromJsonAsync<CategoryRespons>(url);
            return response?.Trivia_categories ?? new List<Category>();
        }
    }
}
