namespace PideYa.Client.Services;

using System;
using PideYa.Shared.Entities;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAll();
    Task<Order?> GetActive();
    Task<Order?> AddDetail(int orderId, int dishId);
    Task<Order?> RemoveDetail(int orderId, int detailId);
    Task<Order?> Process(int orderId);
}

public class OrderService : IOrderService
{
    private readonly IHttpService httpService;
    private readonly IAuthenticationService authenticationService;

    public OrderService(IHttpService httpService, IAuthenticationService authenticationService)
    {
        this.httpService = httpService;
        this.authenticationService = authenticationService;
    }

    public async Task<Order?> AddDetail(int orderId, int dishId)
    {
        try
        {
            return await httpService.Get<Order>($"/api/OrderAddDetail?orderId={orderId}&dishId={dishId}&quantity=1");
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Order?> GetActive()
    {
        try
        {
            return await httpService.Get<Order>($"/api/OrderByEmployee/{authenticationService.User.Id}");
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<IEnumerable<Order>> GetAll()
    {
        return await httpService.Get<IEnumerable<Order>>("/api/Order");
    }

    public async Task<Order?> Process(int orderId)
    {
        try
        {
            return await httpService.Get<Order?>($"/api/OrderProcesing?orderId={orderId}");
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Order?> RemoveDetail(int orderId, int detailId)
    {
        try
        {
            return await httpService.Get<Order>($"/api/OrderDeleteDetail?orderId={orderId}&detailId={detailId}");
        }
        catch (Exception)
        {
            return null;
        }
    }
}

