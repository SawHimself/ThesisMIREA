using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.UseCase
{
    public class UserUseCase
    {
        private readonly AppDbContext _context;
        public UserUseCase(AppDbContext context)
        {
            _context = context;
        }

        public SecretData AddData(int UserId, SecretData data)
        {
            var user = _context.users.Single(
                         x => x.Id == data.UserId);
            if (user == null)
            {
                throw new ArgumentException("Client not found");
            }
            else
            {
                if (user.SecretDatas == null)
                {
                    user.SecretDatas = new List<SecretData>();
                    _context.SaveChanges();
                }
                user.SecretDatas.Add(data);
                _context.SaveChanges();
                return data;

            }
        }

        public async Task<List<SecretData>?> GetSecrets(string UserId, SecretData data)
        {
            var user = _context.users.Single(
                x => x.Id == data.UserId);
            if(user == null) 
            {
                throw new ArgumentException("Client not found");
            }
            else
            {
                return await _context.secrets
                    .AsNoTracking()
                    .Where(x => x.UserId == UserId).ToListAsync();
            }
        }
    }
}
