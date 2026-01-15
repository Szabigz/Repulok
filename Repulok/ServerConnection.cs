using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Repulok
{
    public class ServerConnection
    {
        private HttpClient _client = new HttpClient();
        string baseUrl = "";
        private string Token { get; set; }
        public ServerConnection(string url)
        {
            if (!url.StartsWith("http://")) throw new ArgumentException("Hibas url (http://");
            baseUrl = url;
            _client.BaseAddress = new Uri(baseUrl);
        }
        
        public async Task<Message> registerUser(string username,string password)
        {
            Message msg = new Message();
            string url = baseUrl + "/artworks";
            try
            {
                var JsonData = new
                {
                    username = username,
                    password = password
                };
                string jsonString = JsonSerializer.Serialize(JsonData);
                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(url, content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return msg;
        }
        
        public async Task<Message> loginUser(string username, string password)
        {
            Message msg = new Message();
            string url = baseUrl + "/artworks";
            try
            {
                var JsonData = new
                {
                    username = username,
                    password = password
                };
                string jsonString = JsonSerializer.Serialize(JsonData);
                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                msg = JsonSerializer.Deserialize<Message>(responseBody);

                Token = msg.token;
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return msg;
        }

        public async Task<List<Artworks>> getArtworks()
        {
            List<Artworks> ListOfArtworks = new List<Artworks>();
            string url = baseUrl + "/artworks";
            try
            {
                HttpResponseMessage response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                ListOfArtworks = JsonSerializer.Deserialize<List<Artworks>>(await response.Content.ReadAsStringAsync());

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return ListOfArtworks;
        }   
    }
}
