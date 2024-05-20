namespace APIMSimulator.Abstract;

public interface IMessageBody
{
    T As<T>(bool preserveContent = false);
}
