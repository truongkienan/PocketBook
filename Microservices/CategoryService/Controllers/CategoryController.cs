using AutoMapper;
using CategoryService.AsyncDataServices;
using CategoryService.Data;
using CategoryService.Dtos;
using CategoryService.Models;
using Microsoft.AspNetCore.Mvc;
using RedisCaching.Attributes;
using RedisCaching.Services;

using Utils;
namespace CategoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _repository;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        private readonly IResponseCacheService _responseCacheService;
        public CategoryController(ICategoryRepo repository,
        IMapper mapper,
        IResponseCacheService responseCacheService,
        IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _responseCacheService = responseCacheService;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        [Cache(1000)]
        public ActionResult Get()
        {
            Console.WriteLine("--> Getting Category...");
            var obj = _repository.GetAll();
            return Ok(_mapper.Map<IEnumerable<CategoryReadDto>>(obj));
        }

        [HttpGet("{id}")]
        [Cache(1000)]
        public ActionResult<Category> Get(int id)
        {
            var obj = _repository.GetById(id);
            if (obj != null)
            {
                return Ok(_mapper.Map<CategoryReadDto>(obj));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto obj)
        {
            var category = _mapper.Map<Category>(obj);
            _repository.Create(category);
            _repository.SaveChanges();

            await _responseCacheService.RemoveCacheAsync("/api/category");

            var categoryReadDto = _mapper.Map<CategoryReadDto>(category);

            try
            {
                var categoryPublishedDto = _mapper.Map<CategoryPublishedDto>(categoryReadDto);
                categoryPublishedDto.Event = "Category_Published";

                _messageBusClient.PublishNewCategory(categoryPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtAction("Get", new { categoryReadDto.Id }, categoryReadDto);
        }
    }
}
