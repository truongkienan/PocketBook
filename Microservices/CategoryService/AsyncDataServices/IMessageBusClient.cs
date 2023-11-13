using CategoryService.Dtos;

namespace CategoryService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewCategory(CategoryPublishedDto obj);
    }
}