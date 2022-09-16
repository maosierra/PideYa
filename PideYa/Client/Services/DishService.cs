namespace PideYa.Client.Services;
using System;
using PideYa.Shared.Entities;

public interface IDishService
{
    Task<IEnumerable<Dish>> GetAll();
    Task<Dish> Create(Dish dish);
}

public class DishService : IDishService
{
    private readonly IHttpService httpService;

    public DishService(IHttpService httpService)
    {
        this.httpService = httpService;
    }

    public async Task<Dish> Create(Dish dish)
    {
        return await httpService.Post<Dish>("/api/Dish", dish);
    }

    public async Task<IEnumerable<Dish>> GetAll()
    {
        return await httpService.Get<IEnumerable<Dish>>("/api/Dish");
    }
}

