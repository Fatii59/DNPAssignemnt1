using CLI.UI;
using FileRepositories;
using RepostitoryContracts;
using Services;


Console.WriteLine("Starting CLI application...");

IUserRepository userRepository= new UserFileRepository();
ICommentRepository commentRepository= new CommentFileRepository();
IPostRepository postRepository= new PostFileRepository();
UserService userService = new UserService(userRepository);

CliApp cliApp = new CliApp(userService, commentRepository, postRepository);
await cliApp.StartAsync();