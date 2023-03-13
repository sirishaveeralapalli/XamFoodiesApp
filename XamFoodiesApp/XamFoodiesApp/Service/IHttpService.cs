
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;


namespace XamFoodiesApp
{
    public interface IHttpService
    {
        Task<T> ReadContentAsync<T>(HttpResponseMessage response);
        //Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PutAsync<T>(string url, T content);
        //Task<string> SendAsync<T>(string url, T content, HttpMethod type);
        Task<HttpResponseMessage>  SendAsync<T>(string url, T content, HttpMethod type);

        Task<HttpResponseMessage> SendPkasAsync<T>(string url, T content, HttpMethod type);

        
        Task<HttpResponseMessage> GetAsync<T>(string url);

        Task<HttpResponseMessage> GetAsyncpdf(string url);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
        Dictionary<string, string> DefaultHeader { get; set; }
        Task<byte[]> DownloadRequest(string url);
        Task<HttpResponseMessage> DeleteAsync<T>(string url);
    }
}
