using Zenject;

public abstract class ServiceSerializationHandler<T> : SerializationHandler<T>, IInitializable where T : new()
{
    protected ServiceSerializationHandler(SerializationService serializationService, DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
    }

    public virtual void Initialize()
    {
        Dto = new T();

        Read();
    }
}
