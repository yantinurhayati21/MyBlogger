using MyBlogger.Models;

namespace MyBlogger.Repositories
{
	public interface IPostRepository
	{
		List<Post> GetAllPost();
		Post GetOnePost(int id);
		Post CreatePost(PostRequest request);

	}
}
