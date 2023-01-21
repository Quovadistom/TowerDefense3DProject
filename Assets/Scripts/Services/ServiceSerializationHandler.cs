using System;
using Zenject;

public abstract class ServiceSerializationHandler<T> : SerializationHandler<T>, IInitializable where T : new()
{
    protected ServiceSerializationHandler(SerializationService serializationService) : base(serializationService)
    {
    }

    public virtual void Initialize()
    {
        Dto = new T();

        // Read();
    }
}
