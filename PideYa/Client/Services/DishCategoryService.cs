namespace PideYa.Client.Services;

using System;
using PideYa.Shared.Entities;

public interface IDishCategoryService
{
    Task<IEnumerable<DishCategory>> GetAll();
    Task<DishCategory> Create(DishCategory dishCategory);
    Task Delete(int id);
}

public class DishCategoryService : IDishCategoryService
{
    private readonly IHttpService httpService;

    public DishCategoryService(IHttpService httpService)
    {
        this.httpService = httpService;
    }

    public async Task<DishCategory> Create(DishCategory dishCategory)
    {
        return await httpService.Post<DishCategory>("api/DishCategory", dishCategory);
    }

    public async Task Delete(int id)
    {
        await httpService.Delete($"api/DishCategory/{id}");
    }

    public async Task<IEnumerable<DishCategory>> GetAll()
    {
        return await httpService.Get<IEnumerable<DishCategory>>("/api/DishCategory");
    }
}

