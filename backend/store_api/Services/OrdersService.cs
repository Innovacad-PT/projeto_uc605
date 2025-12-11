using store_api.Dtos;
using store_api.Dtos.Orders;
using store_api.Entities;
using store_api.Repositories;
using store_api.Utils;

namespace store_api.Services;

public class OrdersService
{
    
    private OrdersRepository _repository;
    private ProductsService _productsService;

    public OrdersService(IConfiguration configuration)
    {
        _repository = new ();
        _productsService = new(configuration);
    }

    public async Task<Result<OrderEntity?>> CreateOrder(OrderAddDto orderDto)
    {
        foreach (var kvp in orderDto.Products)
        {
            bool canCreate = await _productsService.CanCreateOrder(kvp.Key, kvp.Value);

            if (!canCreate)
            {
                return new Failure<OrderEntity?>(ResultCode.ORDER_NOT_CREATED,
                    $"Order cannot be created. Product ID {kvp.Key} has insufficient stock for quantity {kvp.Value}.");
            }
        }
        
        List<OrderItemEntity> orderItemEntities = new();
        
        try
        {
            foreach (var kvp in orderDto.Products)
            {
                Result<ProductEntity?> productResult = _productsService.GetProductById(kvp.Key);
                
                if (productResult is Failure<ProductEntity?> peFail)
                {
                     throw new Exception($"Product ID {kvp.Key} disappeared during checkout.");
                }
                
                ProductEntity product = ((Success<ProductEntity?>)productResult).Value!;
                
                orderItemEntities.Add(new OrderItemEntity(
                    0,
                    product.Id,
                    kvp.Value,
                    product.Price
                ));
            }
            
            foreach (var kvp in orderDto.Products)
            {
                Result<ProductEntity?> decreaseResult = _productsService.DecreaseStock(kvp.Key, kvp.Value);
                
                if (decreaseResult is Failure<ProductEntity?>)
                {
                    throw new Exception($"Stock decrease failed for product {kvp.Key}. This might be a race condition.");
                }
            }
            
            var newOrderEntity = new OrderEntity(
                (int)new Random().NextInt64(),
                orderDto.UserId,
                DateTime.UtcNow,
                orderDto.Total,
                orderDto.Status,
                orderItemEntities
            );

            var createdOrder = _repository.Add(newOrderEntity);

            if (createdOrder == null)
            {
                throw new Exception("Repository failed to persist the order record.");
            }
            
            return new Success<OrderEntity?>(ResultCode.ORDER_CREATED, "CREATED", createdOrder);
        }
        catch (Exception ex)
        {
            foreach (var kvp in orderDto.Products)
            {
                _productsService.IncreaseStock(kvp.Key, kvp.Value);
            }

            return new Failure<OrderEntity?>(ResultCode.ORDER_NOT_CREATED, $"NOT CREATED. Transaction failed: {ex.Message}");
        }
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