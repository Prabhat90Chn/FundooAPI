namespace FundooAPI.RabitMQ.Interface
{
    public interface IPublishSubscribeMQProducer
    {
        public void Publish<T>(T message);
    }
}
