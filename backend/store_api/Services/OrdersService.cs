using store_api.Dtos;
using store_api.Dtos.Orders;
using store_api.Entities;
using store_api.Repositories;
using store_api.Utils;

namespace store_api.Services;

public class OrdersService
{
    
    private OrdersRepository _repository = new OrdersRepository();
    private ProductsService _productsService = new();

    public async Task<Result<OrderEntity?>> CreateOrder(OrderAddDto orderDto)
    {
        var createdOrder = _repository.Add(orderDto.ToEntity());

        bool canCreateOrder = false;

       foreach (var kvp in orderDto.Products)
       {
           canCreateOrder = await _productsService.CanCreateOrder(kvp.Key, kvp.Value);

           if (canCreateOrder == false) break;
       }

        if (createdOrder == null || !canCreateOrder)
            return new Failure<OrderEntity?>(ResultCode.ORDER_NOT_CREATED, "NOT CREATED");

        foreach (var kvp in orderDto.Products)
        {
            _productsService.DecreaseStock(kvp.Key,  kvp.Value);
        }

        return new Success<OrderEntity?>(ResultCode.ORDER_CREATED, "CREATED", createdOrder);
    }

    public Result<OrderEntity?> Update(int id, OrderUpdateDto orderDto)
    {
        OrderEntity? orderEntity = _repository.Update(id, orderDto);

        if (orderEntity == null)
            return new Failure<OrderEntity?>(ResultCode.ORDER_NOT_CREATED, "NOT CREATED");

        return new Success<OrderEntity?>(ResultCode.ORDER_CREATED, "CREATED", orderEntity);
    }

    public Result<OrderEntity?> Delete(int id)
    {
        OrderEntity? orderEntity = _repository.Delete(id);

        if (orderEntity == null)
            return new Failure<OrderEntity?>(ResultCode.ORDER_NOT_DELETED, "NOT DELETED");

        return new Success<OrderEntity?>(ResultCode.ORDER_DELETED, "DELETED", orderEntity);
    }

    public Result<IEnumerable<OrderEntity>> GetAllOrder()
    {
        var orders = _repository.GetAll();

        if (!orders.ToList().Any())
            return new Failure<IEnumerable<OrderEntity>>(ResultCode.ORDER_NOT_FOUND, "NOT FOUND");

        return new Success<IEnumerable<OrderEntity>>(ResultCode.ORDER_FOUND, "FOUND", orders);
    }

    public Result<OrderEntity?> GetOrderById(int id)
    {
        var order = _repository.GetById(id);

        if (order == null)
            return new Failure<OrderEntity?>(ResultCode.ORDER_NOT_FOUND, "NOT FOUND");

        return new Success<OrderEntity?>(ResultCode.ORDER_FOUND, "FOUND", order);
    }

    public Result<IEnumerable<OrderEntity>> GetOrdersByUser(Guid userId)
    {
        var orders = _repository.GetOrdersByUser(userId);

        if (!orders.ToList().Any())
            return new Failure<IEnumerable<OrderEntity>>(ResultCode.ORDER_NOT_FOUND, "NOT FOUND");

        return new Success<IEnumerable<OrderEntity>>(ResultCode.ORDER_FOUND, "FOUND", orders);
    }

    public Result<decimal> CalculateOrderTotal(int orderId)
    {
        decimal total = _repository.CalculateOrderTotal(orderId);

        if (total < 0)
            return new Failure<decimal>(ResultCode.ORDER_NOT_FOUND, "NOT FOUND");

        return new Success<decimal>(ResultCode.ORDER_FOUND, "FOUND", total);
    }
}