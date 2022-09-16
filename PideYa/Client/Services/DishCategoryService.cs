namespace PideYa.Client.Services;

using System;
using PideYa.Shared.Entities;

public interface IDishCategoryService
{
    Task<IEnumerable<DishCategory>> GetAll();
}

public class DishCategoryService : IDishCategoryService
{
    private readonly IHttpService httpService;

    public DishCategoryService(IHttpService httpService)
    {
        this.httpService = httpService;
    }

    public async Task<IEnumerable<DishCategory>> GetAll()
    {
        return await httpService.Get<IEnumerable<DishCategory>>("/api/DishCategory");
    }
}

