using Newtonsoft.Json; // Add this at the top
using EfcRepositoriess; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RepostitoryContracts;
using Services;
using System.Text;
using Entities;
using Microsoft.EntityFrameworkCore;
using AppContext = EfcRepositoriess.AppContext;

var builder = WebApplication.CreateBuilder(args);

// JWT Authentication Configuration
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer))
{
    throw new InvalidOperationException("JWT configuration is missing in appsettings.");
}

Console.WriteLine($"JWT Config - Key: {jwtKey}, Issuer: {jwtIssuer}");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});


// Add Authorization Services
builder.Services.AddAuthorization();

// Add Controllers
builder.Services.AddControllers();

// Add Swagger Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5164") // Replace with your client URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Allow cookies/credentials
    });
});

// Add repositories
builder.Services.AddScoped<ICommentRepository, EfcCommentRepository>();
builder.Services.AddScoped<IPostRepository, EfcPostRepository>();
builder.Services.AddScoped<IUserRepository, EfcUserRepository>();
builder.Services.AddScoped(provider =>
{
    var factory = new AppContextFactory();
    return factory.CreateDbContext(args);
});


// Add service layer
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommentService, CommentService>();

var app = builder.Build();

SeedDatabase(app);

app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"];
    Console.WriteLine($"Authorization Header: {token}");
    await next();
});

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(); // Enable CORS

app.UseRouting();

// Add Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


static void SeedDatabase(WebApplication app)
{
    try
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppContext>();

        var connectionString = context.Database.GetDbConnection().ConnectionString;
        Console.WriteLine($"Database Connection String: {connectionString}");

        if (!context.Users.Any() && !context.Posts.Any() && !context.Comments.Any())
        {
            Console.WriteLine("Seeding database...");

            var basePath = Directory.GetCurrentDirectory();
            var usersJsonPath = Path.Combine(basePath, "users.json");
            var postsJsonPath = Path.Combine(basePath, "posts.json");
            var commentsJsonPath = Path.Combine(basePath, "comments.json");

            var usersJson = File.ReadAllText(usersJsonPath);
            var postsJson = File.ReadAllText(postsJsonPath);
            var commentsJson = File.ReadAllText(commentsJsonPath);

            var users = JsonConvert.DeserializeObject<List<User>>(usersJson);
            var posts = JsonConvert.DeserializeObject<List<Post>>(postsJson);
            var comments = JsonConvert.DeserializeObject<List<Comment>>(commentsJson);

            if (users != null) context.Users.AddRange(users);
            if (posts != null) context.Posts.AddRange(posts);
            if (comments != null) context.Comments.AddRange(comments);

            context.SaveChanges();
            Console.WriteLine("Database seeding completed.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during database seeding: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
    }
}



