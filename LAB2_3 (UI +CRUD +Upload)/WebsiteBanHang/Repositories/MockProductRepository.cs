﻿using System.Collections.Generic;
using System.Linq;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;
using WebsiteBanHang.Models; // Thay thế bằng namespace thực tế của bạn
public class MockProductRepository : IProductRepository
{
    private readonly List<Product> _products;
    public MockProductRepository()
    {
        _products = new List<Product>
{
    new Product
    {
        Id = 1,
        Name = "Laptop",
        Price = 1000,
        Description= "A high-end laptop",
        ImageUrl = "/images/10055428-laptop-asus-vivobook-15-i5-1335u-x1504va-nj070w-1.jpg",
        ImageUrls = new List<string>
        {
            "/images/10055428-laptop-asus-vivobook-15-i5-1335u-x1504va-nj070w-2.jpg",
           
        }
    },
};
    }
    public IEnumerable<Product> GetAll()
    {
        return _products;
    }
    public Product GetById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }
    public void Add(Product product)
    {
        product.Id = _products.Max(p => p.Id) + 1;
        _products.Add(product);
    }
    public void Update(Product product)
    {
        var index = _products.FindIndex(p => p.Id == product.Id);
        if (index != -1)
        {
            _products[index] = product;
        }
    }
    public void Delete(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            _products.Remove(product);
        }
    }
}