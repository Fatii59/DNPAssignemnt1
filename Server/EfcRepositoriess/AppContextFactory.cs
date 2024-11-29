using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EfcRepositoriess;

public class AppContextFactory : IDesignTimeDbContextFactory<AppContext>
{
    public AppContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppContext>();
        optionsBuilder.UseSqlite("Data Source=C:/Users/Bruger/RiderProjects/DNPAssignemnt1.work/Server/EfcRepositoriess/app.db");

        return new AppContext(optionsBuilder.Options);
    }
}