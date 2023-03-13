using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using XamFoodiesApp.DependencyServices;
using XamFoodiesApp.Models;

namespace XamFoodiesApp.Service
{
    public class UserService
	{
        public IHttpService _httpService;
        public JsonSerializerSettings settings;
        public UserService(HttpService _httpServiceRef)
        {
            _httpService = _httpServiceRef;
            settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }
        public class Response
        {
            public string Content { get; set; }
            public int StatusCode { get; set; }
        }
        private async Task<string> ReadAsStringAsync(HttpResponseMessage message)
        {
            var content = await message.Content.ReadAsStringAsync();
            if (content == "'null'" || content == "null" || string.IsNullOrEmpty(content))
            {
                return null;
            }
            return content;
        }
        public class PostResponse
        {
            public int ResponseValue { get; set; }
            public bool IsError { get; set; }
            public string ErrorMessage { get; set; }
        }
        public async Task<PostResponse> AddRestaurant(Restaurant dto)
        {
            try
            {
                var response=new HttpResponseMessage();
                if (dto.Id!=0)
                    response = await _httpService.SendAsync("api/Restaurants", dto, HttpMethod.Put);
                else
                    response = await _httpService.SendAsync("api/Restaurants", dto, HttpMethod.Post);

                if (response == null)
                {
                    return null;
                }
                var obj = new Response();
                var message = await ReadAsStringAsync(response);

                var result = JsonConvert.DeserializeObject<PostResponse>(message);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<User> UserValidate(string email, string pwd)
        {
            try
            {
                // var url = Config.USER_VALIDATE + "?Email=" + email + "&Password=" + Configuration.GetEncoded(pwd);
                //var response = await _httpService.GetAsync<string>(url);

                UserCredential dto = new UserCredential();
                dto.username = email;
                dto.password = pwd;
               
                //var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                //var response = await _httpService.PostAsync("Login", content);
                var response = await _httpService.SendAsync("Login", dto, HttpMethod.Post);

                if (response == null)
                {
                    return null;
                }
                var message = await ReadAsStringAsync(response);
                var userStatus = JsonConvert.DeserializeObject<User>(message, settings);
                return userStatus;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        internal async Task<List<Restaurant>> GetRestaurants()
        {
            try
            {
                var url = "api/Restaurants";
                var response = await _httpService.GetAsync<string>(url);
                if (response == null)
                {
                    return null;
                }
                var message = await ReadAsStringAsync(response);
                var userStatus = JsonConvert.DeserializeObject<List<Restaurant>>(message, settings);
                return userStatus;
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal async Task<Response> DeleteRestaurant(int Id)
        {
            try
            {
                string url = "api/Restaurants/" + Id;
                var response = await _httpService.DeleteAsync<string>(url);

                if (response == null)
                {
                    return null;
                }
                var obj = new Response();
                var message = await ReadAsStringAsync(response);

                var result = JsonConvert.DeserializeObject<Response>(message);
                return result;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}

