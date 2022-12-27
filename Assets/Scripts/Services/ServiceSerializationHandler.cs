using Zenject;

public abstract class ServiceSerializationHandler<T> : SerializationHandler<T>, IInitializable where T : new()
{
    public void Initialize()
    {
        Dto = new T();

        Read();
    }
}
