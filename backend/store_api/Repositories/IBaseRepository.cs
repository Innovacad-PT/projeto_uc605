using store_api.Dtos;
using store_api.Utils;

namespace store_api.Controllers;

public interface IBaseRepository<T> where T: class
{
    public Result<T> Add(IBaseDto<T> dto);

    public virtual Result<T> Update(Guid id, IBaseDto<T> dto)
    {
        throw new NotImplementedException();
    }

    public virtual Result<T> Update(int id, IBaseDto<T> dto)
    {
        throw new NotImplementedException();
    }

    public virtual Result<T> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public virtual Result<T> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public virtual Result<T> GetById(int id)
    {
        throw new NotImplementedException();
    }
    public Result<T> GetById(Guid id);
    public Result<IEnumerable<T>> GetAll();
}