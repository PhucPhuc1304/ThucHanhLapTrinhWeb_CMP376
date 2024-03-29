﻿using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
        Category GetById(int id);

    }
}
