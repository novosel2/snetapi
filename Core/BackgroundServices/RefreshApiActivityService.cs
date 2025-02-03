using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BackgroundServices
{
    public class RefreshApiActivityService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(10);

        public RefreshApiActivityService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var _httpClient = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();

                await _httpClient.GetAsync("https://snetapi-evgqgtdcc0b6a2e9.germanywestcentral-01.azurewebsites.net/swagger/index.html", stoppingToken);

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
