using CLI.UI;
using FileRepositories;
using RepostitoryContracts;
using Services;


Console.WriteLine("Starting CLI application...");

IUserRepository userRepository= new UserFileRepository();
ICommentRepository commentRepository= new CommentFileRepository();
IPostRepository postRepository= new PostFileRepository();
UserService userService = new UserService(userRepository);
//CommentService commentService = new CommentService(commentRepository);
//PostService postService = new PostService(postRepository);

//CliApp cliApp = new CliApp(userService, commentService, postService);
//await cliApp.StartAsync();
