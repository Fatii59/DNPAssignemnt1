using Entities;
using Microsoft.EntityFrameworkCore;
using RepostitoryContracts;

namespace EfcRepositoriess;

public class EfcPostRepository : IPostRepository
{
    private readonly AppContext _context;

    public EfcPostRepository(AppContext context)
    {
        _context = context;
    }

    public IQueryable<Post> GetMany()
    {
        return _context.Posts
            .Include(p => p.User) // Include related user
            .Include(p => p.Comments) // Include comments
            .AsQueryable();
    }

    public async Task<Post?> GetSingleAsync(int id)
    {
        return await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Post> AddAsync(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var post = await GetSingleAsync(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}