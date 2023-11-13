using System;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using ArticleService.Data;
using ArticleService.Dtos;
using ArticleService.Models;

namespace ArticleService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, AutoMapper.IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.CategoryPublished:
                    AddCategory(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notifcationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);

            switch(eventType.Event)
            {
                case "Category_Published":
                    Console.WriteLine("--> Category Published Event Detected");
                    return EventType.CategoryPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private void AddCategory(string categoryPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IArticleRepo>();
                
                var categoryPublishedDto = JsonSerializer.Deserialize<CategoryPublishedDto>(categoryPublishedMessage);

                try
                {
                    var plat = _mapper.Map<Category>(categoryPublishedDto);
                    if(!repo.ExternalCategoryExists(plat.ExternalID))
                    {
                        repo.CreateCategory(plat);
                        repo.SaveChanges();
                        Console.WriteLine("--> Category added!");
                    }
                    else
                    {
                        Console.WriteLine("--> Category already exisits...");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Category to DB {ex.Message}");
                }
            }
        }
    }
    enum EventType
    {
        CategoryPublished,
        Undetermined
    }
}