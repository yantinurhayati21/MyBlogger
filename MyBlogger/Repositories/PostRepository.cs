using MyBlogger.Models;

namespace MyBlogger.Repositories
{
	public class PostRepository:IPostRepository
	{
		private readonly AppDbContext _context;
        public PostRepository(AppDbContext c)
        {
            _context = c;
        }

        public List<Post> GetAllPost()
        {
            return _context.Post.ToList();
        }

        public Post GetOnePost(int id) 
        {
            return _context.Post.FirstOrDefault(x => x.Id == id);
        }

        public Post CreatePost(PostRequest data)
        {
			Post post = new Post()
			{
				Title = data.Title,
				Content = data.Content,
				Likes = 0,
				CreatedDate = DateTime.Now,
			};
			_context.Post.Add(post);
			_context.SaveChanges();

            return post;
		}
    }

}
