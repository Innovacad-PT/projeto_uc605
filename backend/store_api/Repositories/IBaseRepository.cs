using store_api.Utils;

namespace store_api.Controllers;

public interface IBaseRepository<T> where T: class
{
    public Result<T> Add(T entity);
    public Result<T> Update(T entity);
    public Result<T> Delete(T entity);
    public Result<T> GetById(int id);
    public Result<T> GetById(Guid id);
    public Result<IEnumerable<T>> GetAll();
}