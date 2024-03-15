using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public class MockCategoryRepository : ICategoryRepository
    {
        private List<Category> _categoryList;
        public MockCategoryRepository()
        {
            _categoryList = new List<Category>
            {
                new Category { Id = 1, Name = "Laptop" },
                new Category { Id = 2, Name = "Desktop" },
            };
        }
        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryList;
        }
        public Category GetById(int id)
        {
            return _categoryList.FirstOrDefault(p => p.Id == id);
        }
    }

}
