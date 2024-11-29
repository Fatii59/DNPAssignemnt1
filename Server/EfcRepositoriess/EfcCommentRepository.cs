
namespace EfcRepositoriess;

using Microsoft.EntityFrameworkCore;
using Entities;
using RepostitoryContracts;


public class EfcCommentRepository : ICommentRepository
{
    private readonly AppContext _context;

    public EfcCommentRepository(AppContext context)
    {
        _context = context;
    }

    public async Task<IQueryable<Comment>> GetManyAsync()
    {
        return _context.Comments
            .Include(c => c.Post) // Include related post
            .Include(c => c.User) // Include related user
            .AsQueryable();
    }


    public async Task<Comment?> GetSingleAsync(int id)
    {
        return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var comment = await GetSingleAsync(id);
        if (comment != null)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
