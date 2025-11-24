using store_api.Controllers;
using store_api.Dtos;
using store_api.Dtos.Orders;
using store_api.Entities;
using store_api.Exceptions;
using store_api.Utils;

namespace store_api.Repositories;

public class OrdersRepository : IBaseRepository<OrderEntity>
{
    
    private readonly static List<OrderEntity> _orders = new ();
    
    public OrderEntity? Add(OrderEntity entity)
    {
        if (_orders.Any(o => o.Id == entity.Id))
        {
            throw new SameIdException("Order with this id already exists");
        }
        
        _orders.Add(entity);
        return entity;
    }
    
    public OrderEntity? Update(int id, IBaseDto<OrderEntity> entity)
    {
        OrderUpdateDto updateDto = entity as OrderUpdateDto;

        if (updateDto == null)
        {
            throw new InvalidDataException("Order update dto is null");
        }

        if (!_orders.Any(o => o.Id == id))
        {
            return null;
        }
        
        OrderEntity orderEntity = _orders.First(o => o.Id == id);
        
        orderEntity.Status = updateDto.Status ?? orderEntity.Status;
        orderEntity.Total = updateDto.Total ?? orderEntity.Total;
        orderEntity.OrderItems = updateDto.OrderItems ?? orderEntity.OrderItems;
        
        return orderEntity;
    }
    
    public OrderEntity? Delete(int id)
    {
        if (!_orders.Any(o => o.Id == id))
        {
            return null;
        }
        
        OrderEntity orderEntity = _orders.First(o => o.Id == id);
        _orders.Remove(orderEntity);
        return orderEntity;
    }

    public OrderEntity? GetById(int id)
    {
        OrderEntity? orderEntity = _orders.FirstOrDefault(o => o.Id == id);
        return orderEntity;
    }

    public IEnumerable<OrderEntity>? GetAll()
    {
        return _orders;
    }

    public IEnumerable<OrderEntity>? GetOrdersByUser(Guid userId)
    {
        IEnumerable<OrderEntity>? orders = _orders.Where(o => o.UserId == userId);
        
        return orders;
    }
}