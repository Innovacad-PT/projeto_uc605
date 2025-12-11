using store_api.Dtos;
using store_api.Utils;

namespace store_api.Controllers;

public interface IBaseRepository<T> where T: class
{
    public virtual T? Add(T dto)
    {
        throw new NotImplementedException();
    }

    public virtual T? Update(Guid id, IBaseDto<T> dto)
    {
        throw new NotImplementedException();
    }

    public virtual T? Update(int id, IBaseDto<T> dto)
    {
        throw new NotImplementedException();
    }

    public virtual T? Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public virtual T? Delete(int id)
    {
        throw new NotImplementedException();
    }

    public virtual T? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public virtual T? GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public virtual IEnumerable<T>? GetAll()
    {
        throw new NotImplementedException();
    }
}