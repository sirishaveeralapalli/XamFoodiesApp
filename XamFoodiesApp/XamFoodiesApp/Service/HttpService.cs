
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;
using XamFoodiesApp;

namespace XamFoodiesApp
{
    public class HttpService : IHttpService
    {
        public Dictionary<string, string> DefaultHeader { get; set; } = new Dictionary<string, string>();

        public async Task<T> ReadContentAsync<T>(HttpResponseMessage response)
        {
            var message = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<T>(message);
            return content;
        }
        public async Task<HttpResponseMessage> PutAsync<T>(string url, T content)
        {
            //   HttpResponseMessage httpresponse = new HttpResponseMessage();

            //     HttpClient client = (handler == null) ? new System.Net.Http.HttpClient() : new HttpClient(new NativeMessageHandler());
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                var json = JsonConvert.SerializeObject(content);
                var address = $"{Configuration.BaseUrl}{url}";
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                //var response =  client.PutAsync(new Uri(address), httpContent);

                //var stream = await response.Result.Content.ReadAsStringAsync();

                var response = await client.PutAsync(new Uri(address), httpContent);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
            }
            return null;
        }
        public async Task<HttpResponseMessage> SendAsync<T>(string url, T content, HttpMethod type)
        {
            //    var client = new HttpClient();

            //   HttpClient client = (handler == null) ? new System.Net.Http.HttpClient() : new HttpClient(new NativeMessageHandler());
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                try
                {
                    var address = $"{Configuration.BaseUrl}{url}";
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(address),
                        Method = type,
                    };
                    //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                    foreach (var hdr in DefaultHeader)
                    {
                        request.Headers.Add(hdr.Key, hdr.Value);
                    }

                    var json = JsonConvert.SerializeObject(content);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Content = httpContent;
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }
                }
                catch(Exception ex)
                {
                    return null;
                }
            }
            return null;
        }
        public async Task<HttpResponseMessage> GetAsync<T>(string url)
        {
            try
            {
                //  HttpClient client = (handler == null) ? new System.Net.Http.HttpClient() : new HttpClient(new NativeMessageHandler());
                using (var client = new HttpClient(new NativeMessageHandler()))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    var request = new HttpRequestMessage();
                    //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                    foreach (var hdr in DefaultHeader)
                    {
                        //  request.Headers.Add(hdr.Key, hdr.Value);
                        client.DefaultRequestHeaders.Add(hdr.Key, hdr.Value);
                    }

                    var address = $"{Configuration.BaseUrl}{url}";
                    var response = await client.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }
                }

                return null;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> SendPkasAsync<T>(string url, T content, HttpMethod type)
        {
            //    var client = new HttpClient();

            //   HttpClient client = (handler == null) ? new System.Net.Http.HttpClient() : new HttpClient(new NativeMessageHandler());
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                try
                {
                    var address = $"{Configuration.BaseUrl}{url}";
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(address),
                        Method = type,
                    };
                  
                    foreach (var hdr in DefaultHeader)
                    {
                        request.Headers.Add(hdr.Key, hdr.Value);
                    }
                    var json = JsonConvert.SerializeObject(content);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Content = httpContent;
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }


        public async Task<HttpResponseMessage> GetAsyncpdf(string url)
        {

            //  HttpClient client = (handler == null) ? new System.Net.Http.HttpClient() : new HttpClient(new NativeMessageHandler());
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var request = new HttpRequestMessage();
                //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
                foreach (var hdr in DefaultHeader)
                {
                    //  request.Headers.Add(hdr.Key, hdr.Value);
                    //client.DefaultRequestHeaders.Add(hdr.Key, hdr.Value);
                    if (hdr.Key == "UserId")
                    {
                        url = url + "&" + "UID" + "=" + hdr.Value;
                    }
                    else if (hdr.Key == "Token")
                    {
                        url = url + "&" + "TK" + "=" + hdr.Value;
                    }
                    else if (hdr.Key == "UserName")
                    {
                        url = url + "&" + "UName" + "=" + hdr.Value;
                    }

                }
               
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
            }

            return null;
        }
        public async Task<HttpResponseMessage> DeleteAsync<T>(string url)
        {

            //  HttpClient client = (handler == null) ? new System.Net.Http.HttpClient() : new HttpClient(new NativeMessageHandler());
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var request = new HttpRequestMessage();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                foreach (var hdr in DefaultHeader)
                {
                    //  request.Headers.Add(hdr.Key, hdr.Value);
                    client.DefaultRequestHeaders.Add(hdr.Key, hdr.Value);
                }
                var address = $"{Configuration.BaseUrl}{url}";
                var response = await client.DeleteAsync(address);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
            }

            return null;
        }
        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {

            // HttpClient client = (handler == null) ? new System.Net.Http.HttpClient() : new HttpClient(new NativeMessageHandler());
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var request = new HttpRequestMessage();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                foreach (var hdr in DefaultHeader)
                {

                    client.DefaultRequestHeaders.Add(hdr.Key, hdr.Value);
                }

                var address = $"{Configuration.BaseUrl}{url}";
                var response = await client.PostAsync(address, content);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
            }
            return null;
        }
        public async Task<byte[]> DownloadRequest(string url)
        {

            //  HttpClient client = (handler == null) ? new System.Net.Http.HttpClient() : new HttpClient(new NativeMessageHandler());
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var imagecontent = await client.GetByteArrayAsync(url);
                return imagecontent;
            }
        }
    }

}