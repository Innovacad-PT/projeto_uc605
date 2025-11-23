using store_api.Controllers;
using store_api.Dtos;
using store_api.Entities;
using store_api.Utils;

namespace store_api.Repositories;

public class OrdersRepository : IBaseRepository<OrderEntity>
{
    public Result<OrderEntity> Add(IBaseDto<OrderEntity> entity)
    {
        throw new NotImplementedException();
    }
    
    public Result<OrderEntity> Update(int id, IBaseDto<OrderEntity> entity)
    {
        throw new NotImplementedException();
    }
    
    public Result<OrderEntity> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Result<OrderEntity> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Result<OrderEntity> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Result<IEnumerable<OrderEntity>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Result<IEnumerable<OrderEntity>> GetOrdersByUser(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Result<IEnumerable<OrderEntity>> AddOrderItems(Guid userId, List<OrderItemEntity> orderItems)
    {
        throw new NotImplementedException();
    }
}