using Entities;

namespace EfcRepositoriess;

using Microsoft.EntityFrameworkCore;
using RepostitoryContracts;

public class EfcUserRepository : IUserRepository
{
    private readonly AppContext _context;

    public EfcUserRepository(AppContext context)
    {
        _context = context;
    }

    public IQueryable<User> GetMany()
    {
        return _context.Users.AsQueryable();
    }

    public async Task<User?> GetSingleAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await GetSingleAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
