namespace store_api.Dtos;

public interface IBaseDto<T>
{
    public virtual T ToEntity()
    {
        throw new NotImplementedException();
    }
}