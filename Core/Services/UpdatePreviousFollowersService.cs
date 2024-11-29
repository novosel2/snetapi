using Core.Data.Entities;
using Core.Exceptions;
using Core.IRepositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class UpdatePreviousFollowersService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromDays(3);

        public UpdatePreviousFollowersService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var _profileRepository = scope.ServiceProvider.GetService<IProfileRepository>()!;

                    List<Profile> profiles = await _profileRepository.GetProfilesAsync();
                    foreach (var profile in profiles)
                    {
                        profile.PreviousFollowers = profile.Followers.Count;
                    }

                    await _profileRepository.IsSavedAsync();

                    Console.WriteLine("Previous Followers updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating Previous Followers: {ex.Message}");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
