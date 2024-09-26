using CLI.UI;
using FileRepositories;
using RepostitoryContracts;


Console.WriteLine("Starting CLI application...");
IUserRepository userRepository= new UserFileRepository();
ICommentRepository commentRepository= new CommentFileRepository();
IPostRepository postRepository= new PostFileRepository();

CliApp cliApp = new CliApp(userRepository, commentRepository, postRepository);
await cliApp.StartAsync();