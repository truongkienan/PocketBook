using System;
using System.Collections.Generic;
using AutoMapper;
using ArticleService.Data;
using ArticleService.Dtos;
using ArticleService.Models;
using Microsoft.AspNetCore.Mvc;
using RedisCaching.Attributes;

namespace ArticleService.Controllers
{
    [Route("api/c/category/{categoryId}/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepo _repository;
        private readonly IMapper _mapper;

        public ArticleController(IArticleRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [Cache(100)]
        public ActionResult<IEnumerable<ArticleReadDto>> GetArticlesForPlatform(int categoryId)
        {
            Console.WriteLine($"--> Hit GetArticlesForCategory: {categoryId}");

            if (!_repository.CategoryExits(categoryId))
            {
                return NotFound();
            }

            var articles = _repository.GetArticlesForCategory(categoryId);

            return Ok(_mapper.Map<IEnumerable<ArticleReadDto>>(articles));
        }

        [HttpGet("{topicId}", Name = "GetArticleForCategory")]
        public ActionResult<ArticleReadDto> GetArticleForCategory(int categoryId, int topicId)
        {
            Console.WriteLine($"--> Hit GetTopicForCategory: {categoryId} / {topicId}");

            if (!_repository.CategoryExits(categoryId))
            {
                return NotFound();
            }

            var topic = _repository.GetArticle(categoryId, topicId);

            if(topic == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ArticleReadDto>(topic));
        }

        [HttpPost]
        public ActionResult<ArticleReadDto> CreateArticleForCategory(int categoryId, ArticleCreateDto topicDto)
        {
             Console.WriteLine($"--> Hit CreateTopicForCategory: {categoryId}");

            if (!_repository.CategoryExits(categoryId))
            {
                return NotFound();
            }

            var topic = _mapper.Map<Article>(topicDto);

            _repository.CreateArticle(categoryId, topic);
            _repository.SaveChanges();

            var topicReadDto = _mapper.Map<ArticleReadDto>(topic);

            return CreatedAtRoute(nameof(GetArticleForCategory),
                new {categoryId = categoryId, topicId = topicReadDto.Id}, topicReadDto);
        }

    }
}