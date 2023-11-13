using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ArticleService.Data;
using ArticleService.Dtos;
using ArticleService.Models;
using Microsoft.AspNetCore.Authorization;
using Utils;
using RedisCaching.Attributes;

namespace ArticleService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IArticleRepo _repository;
        private readonly IMapper _mapper;

        public CategoryController(IArticleRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("random")]
        public IActionResult Random()
        {
            string r = $"--> test value: {Helper.test}";
            return Ok(r);
        }

        [HttpGet]
        // [Authorize(Roles = "User")]
        [Cache(1000)]
        public IActionResult Get()
        {
            Console.WriteLine("--> Getting Categories from Topic service");

            var categories = _repository.GetAllCategories();

            return Ok(_mapper.Map<IEnumerable<CategoryReadDto>>(categories));
        }
    }


}