using Labb03_GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Labb03_GUI.API
{
    internal class APIService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public async Task<(List<APIQuestion> questions, int responseCode)> GetQuestionsAsync(int numberOfQuestion, ImportQuestion importQuestion)
        {
            var difficulty = importQuestion.Difficulty?.ToLower() ?? "medium";
            var url = $"https://opentdb.com/api.php?amount={numberOfQuestion}&category={importQuestion.Category.Id}&difficulty={difficulty}&type=multiple";
            var response = await _httpClient.GetStringAsync(url);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var data = JsonSerializer.Deserialize<APIQuestionResponse>(response, options);



            return (data.Results, data.Response_Code);
        }
        public async Task<List<Category>> GetCategoriesAsync()
        {
            var url = "https://opentdb.com/api_category.php";
            var response = await _httpClient.GetFromJsonAsync<CategoryRespons>(url);
            return response?.Trivia_categories ?? new List<Category>();
        }
    }
}
