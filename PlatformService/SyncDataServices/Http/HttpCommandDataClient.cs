using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using PlatformService.SyncDataServices.Http;

namespace platformservice.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendPlatformToCommand(PlatformReadDto plat)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(plat),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(_configuration["CommandService"], httpContent);

            if(response.IsSuccessStatusCode)
            {
                #if DEBUG
                Console.WriteLine("--> Sync POST to CommandService was OK!");
                #endif
            }           
            else
            {
                #if DEBUG
                Console.WriteLine("--> Sync POST to CommandService was NOT OK!");
                #endif
            }
                
        }
    }
}