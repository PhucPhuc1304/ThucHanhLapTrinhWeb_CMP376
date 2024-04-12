using System.Threading.Tasks;
using Lab3.Models;
namespace Lab3.Repositories
{
    

   
        public interface IOrderDetailsRepository
        {
            Task<OrderDetail> GetByIdAsync(int id);
            Task AddAsync(OrderDetail orderDetail);
            Task UpdateAsync(OrderDetail orderDetail);
            Task DeleteAsync(int id);
        }
    

}
