using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestdto request)
        {
            var blogPost = new BlogPost
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                UrlHandle = request.UrlHandle,
                Author = request.Author,
                PublishedDate = request.PublishedDate,
                IsVisible = request.IsVisible
            };

            var recieved_blogpost = await blogPostRepository.CreateAsync(blogPost);
            var response = new BlogPostDto
            {
                Id = recieved_blogpost.Id,
                Title = recieved_blogpost.Title,
                ShortDescription = recieved_blogpost.ShortDescription,
                Content = recieved_blogpost.Content,
                FeaturedImageUrl = recieved_blogpost.FeaturedImageUrl,
                UrlHandle = recieved_blogpost.UrlHandle,
                Author = recieved_blogpost.Author,
                PublishedDate = recieved_blogpost.PublishedDate,
                IsVisible = recieved_blogpost.IsVisible
            };

            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogPost()
        {
            var blogposts = await blogPostRepository.GetAllAsync();
            var response = new List<BlogPostDto>();
            foreach (var item in blogposts)
            {
                response.Add(new BlogPostDto
                {
                    Id = item.Id,
                    Title = item.Title,
                    ShortDescription = item.ShortDescription,
                    Content = item.Content,
                    FeaturedImageUrl = item.FeaturedImageUrl,
                    UrlHandle = item.UrlHandle,
                    Author = item.Author,
                    PublishedDate = item.PublishedDate,
                    IsVisible = item.IsVisible
                });
            }
            return Ok(response);
        }
    }
}
