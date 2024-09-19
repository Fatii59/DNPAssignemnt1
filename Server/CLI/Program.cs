using CLI.UI;
using InMemoryRepositories;
using RepostitoryContracts;


Console.WriteLine("Starting CLI application...");
IUserRepository userRepository= new UserInMemoryRepository();
ICommentRepository commentRepository= new CommentInMemoryRepository();
IPostRepository postRepository= new PostInMemoryRepository();

CliApp cliApp = new CliApp(userRepository, commentRepository, postRepository);
await cliApp.StartAsync();