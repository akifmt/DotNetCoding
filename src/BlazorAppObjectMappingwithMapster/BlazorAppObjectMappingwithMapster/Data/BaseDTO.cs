using Mapster;

namespace BlazorAppObjectMappingwithMapster.Data;

public abstract class BaseDTO<TDTO, TEntity> : IRegister
                                               where TDTO : class, new()
                                               where TEntity : class, new()
{

    public TEntity ToEntity()
    {
        return this.Adapt<TEntity>();
    }

    public TEntity ToEntity(TEntity entity)
    {
        return (this as TDTO).Adapt(entity);
    }

    public static TDTO FromEntity(TEntity entity)
    {
        return entity.Adapt<TDTO>();
    }


    private TypeAdapterConfig Config { get; set; }

    public virtual void AddCustomMappings() { }


    protected TypeAdapterSetter<TDTO, TEntity> SetCustomMappings()
        => Config.ForType<TDTO, TEntity>();

    protected TypeAdapterSetter<TEntity, TDTO> SetCustomMappingsInverse()
        => Config.ForType<TEntity, TDTO>();

    public void Register(TypeAdapterConfig config)
    {
        Config = config;
        AddCustomMappings();
    }
}
