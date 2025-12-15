using store_api.Dtos;
using store_api.Dtos.Orders;
using store_api.Entities;
using store_api.Repositories;
using store_api.Utils;

namespace store_api.Services;

public class OrdersService(IConfiguration configuration)
{
    private readonly OrdersRepository _ordersRepository = new(configuration);
    private readonly ProductsService _productsService = new(configuration);

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
                Result<ProductEntity?> productResult = await _productsService.GetProductById(kvp.Key);
                
                if (productResult is Failure<ProductEntity?> peFail)
                {
                     throw new Exception($"Product ID {kvp.Key} disappeared during checkout.");
                }
                
                ProductEntity product = ((Success<ProductEntity?>)productResult).Value!;
                
                orderItemEntities.Add(new OrderItemEntity(
                    Guid.NewGuid(),
                    product.Id,
                    kvp.Value,
                    product.Price
                ));
            }
            
            foreach (var kvp in orderDto.Products)
            {
                Result<ProductEntity?> decreaseResult = await _productsService.DecreaseStock(kvp.Key, kvp.Value);
                
                if (decreaseResult is Failure<ProductEntity?>)
                {
                    throw new Exception($"Stock decrease failed for product {kvp.Key}. This might be a race condition.");
                }
            }
            
            var newOrderEntity = new OrderEntity(
                Guid.NewGuid(),
                orderDto.UserId,
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                orderDto.Total,
                orderDto.Status,
                orderItemEntities
            );

            var createdOrder = await _ordersRepository.Add(newOrderEntity);

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
                await _productsService.IncreaseStock(kvp.Key, kvp.Value);
            }

            return new Failure<OrderEntity?>(ResultCode.ORDER_NOT_CREATED, $"NOT CREATED. Transaction failed: {ex.Message}");
        }
    }

    public async Task<Result<OrderEntity?>> Update(Guid id, OrderUpdateDto<OrderEntity> orderDto)
    {
        OrderEntity? orderEntity = await _ordersRepository.Update(id, orderDto);

        if (orderEntity == null)
            return new Failure<OrderEntity?>(ResultCode.ORDER_NOT_CREATED, "NOT CREATED");

        return new Success<OrderEntity?>(ResultCode.ORDER_CREATED, "CREATED", orderEntity);
    }

    public async Task<Result<OrderEntity?>> Delete(Guid id)
    {
        OrderEntity? orderEntity = await _ordersRepository.Delete(id);

        if (orderEntity == null)
            return new Failure<OrderEntity?>(ResultCode.ORDER_NOT_DELETED, "NOT DELETED");

        return new Success<OrderEntity?>(ResultCode.ORDER_DELETED, "DELETED", orderEntity);
    }

    public async Task<Result<IEnumerable<OrderEntity>>?> GetAllOrder()
    {
        var orders = await _ordersRepository.GetAll();

        if (orders == null || !orders.ToList().Any())
            return new Failure<IEnumerable<OrderEntity>>(ResultCode.ORDER_NOT_FOUND, "NOT FOUND");

        return new Success<IEnumerable<OrderEntity>>(ResultCode.ORDER_FOUND, "FOUND", orders);
    }

    public async Task<Result<OrderEntity?>> GetOrderById(Guid id)
    {
        var order = await _ordersRepository.GetById(id);

        if (order == null)
            return new Failure<OrderEntity?>(ResultCode.ORDER_NOT_FOUND, "NOT FOUND");

        return new Success<OrderEntity?>(ResultCode.ORDER_FOUND, "FOUND", order);
    }

    public async Task<Result<IEnumerable<OrderEntity>?>> GetOrdersByUser(Guid userId)
    {
        var orders = await _ordersRepository.GetOrdersByUser(userId);

        if (!orders.ToList().Any())
            return new Failure<IEnumerable<OrderEntity>>(ResultCode.ORDER_NOT_FOUND, "NOT FOUND")!;

        return new Success<IEnumerable<OrderEntity>>(ResultCode.ORDER_FOUND, "FOUND", orders);
    }

    public async Task<Result<decimal>> CalculateOrderTotal(Guid orderId)
    {
        decimal total = await _ordersRepository.CalculateOrderTotal(orderId);

        if (total < 0)
            return new Failure<decimal>(ResultCode.ORDER_NOT_FOUND, "NOT FOUND");

        return new Success<decimal>(ResultCode.ORDER_FOUND, "FOUND", total);
    }
}