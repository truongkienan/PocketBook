namespace ArticleService.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}