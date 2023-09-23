using System;
using Zenject;

public abstract class ServiceSerializationHandler<T> : SerializationHandler<T>, IInitializable where T : new()
{
    protected ServiceSerializationHandler(SerializationService serializationService, DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
    }

    public event Action ServiceRead;

    public virtual void Initialize()
    {
        Dto = new T();

        Read();

        ServiceRead?.Invoke();
    }
}
