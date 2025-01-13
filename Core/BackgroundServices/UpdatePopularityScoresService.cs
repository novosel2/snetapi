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

namespace Core.BackgroundServices
{
    public class UpdatePopularityScoresService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(20);

        public UpdatePopularityScoresService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var _postsRepository = scope.ServiceProvider.GetService<IPostsRepository>()!;

                    int batchStart = 0;
                    int batchSize = 1000;
                    bool processed = false;

                    while (!processed)
                    {
                        List<Post> postsToUpdate = await _postsRepository.GetPostsForScoreUpdateAsync(batchStart, batchSize);

                        if (postsToUpdate.Any())
                        {
                            foreach (var post in postsToUpdate)
                            {
                                post.PopularityScore = CalculatePopularityScore(post);
                            }

                            batchStart += batchSize;
                        }
                        else
                        {
                            processed = true;
                        }
                    }

                    if (!await _postsRepository.IsSavedAsync())
                    {
                        throw new DbSavingFailedException("Failed to save popularity score changes to database.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"!!! Error updating Popularity Scores: {ex.Message}");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }

        private double CalculatePopularityScore(Post post)
        {
            double score;
            var hoursSincePost = (DateTime.UtcNow - post.CreatedOn).TotalHours;

            score = post.CommentCount * 2 + post.Reactions.Count * 1.5 - hoursSincePost * 0.1;

            return score;
        }
    }
}
