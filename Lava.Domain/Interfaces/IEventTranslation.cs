namespace Lava.Domain.Interfaces;

public interface IEventTranslation
{
    IEvent Convert(string type, string json);
}