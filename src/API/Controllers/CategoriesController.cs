using Asp.Versioning;
using AutoMapper;
using BackendAuthTemplate.API.Common.Extensions;
using BackendAuthTemplate.API.Requests.Categories;
using BackendAuthTemplate.Application.Common.Include;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.Categories.Commands.CreateCategoryCommand;
using BackendAuthTemplate.Application.Features.Categories.Commands.DeleteCategoryCommand;
using BackendAuthTemplate.Application.Features.Categories.Commands.UpdateCategoryCommand;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using BackendAuthTemplate.Application.Features.Categories.Queries.GetAllCategoriesWithPaginationQuery;
using BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoryByIdQuery;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendAuthTemplate.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{v:apiVersion}/categories")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController(IMediator mediator, IMapper mapper) : BaseController
    {
        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            CreateCategoryCommand command = mapper.Map<CreateCategoryCommand>(request);

            Result<Guid> result = await mediator.Send(command);

            return await result.MatchAsync(
                onSuccess: async categoryId =>
                {
                    Result<ReadCategoryDto> categoryResult = await mediator.Send(new GetCategoryByIdQuery { CategoryId = categoryId });

                    return categoryResult.Match(
                        onSuccess: category => CreatedAtAction(nameof(GetCategoryById), new { categoryId }, category),
                        onFailure: Problem
                    );
                },
                onFailure: Problem
            );
        }

        [HttpGet("{categoryId:guid}")]
        [AllowAnonymous]
        [IncludeParameter(typeof(Category))]
        public async Task<ActionResult<ReadCategoryDto>> GetCategoryById([FromRoute] Guid categoryId, [FromQuery] List<string> include)
        {
            GetCategoryByIdQuery query = new()
            {
                CategoryId = categoryId,
                Include = include
            };

            Result<ReadCategoryDto> result = await mediator.Send(query);

            return result.Match(
                onSuccess: Ok,
                onFailure: Problem
            );
        }

        [HttpGet]
        [AllowAnonymous]
        [IncludeParameter(typeof(Category))]
        public async Task<ActionResult<PaginatedList<ReadCategoryDto>>> GetAllCategories([FromQuery] List<string> include, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            GetAllCategoriesWithPaginationQuery query = new()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Include = include
            };

            Result<PaginatedList<ReadCategoryDto>> result = await mediator.Send(query);

            return result.Match(
                onSuccess: Ok,
                onFailure: Problem
            );
        }

        [HttpPut("{categoryId:guid}")]
        public async Task<ActionResult> UpdateCategory([FromRoute] Guid categoryId, [FromBody] UpdateCategoryRequest request)
        {
            UpdateCategoryCommand command = mapper.Map<UpdateCategoryCommand>(request, opt => opt.Items["CategoryId"] = categoryId);

            Result result = await mediator.Send(command);

            return result.Match(
                onSuccess: NoContent,
                onFailure: Problem
            );
        }

        [HttpDelete("{categoryId:guid}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] Guid categoryId)
        {
            DeleteCategoryCommand command = new()
            {
                CategoryId = categoryId
            };

            Result result = await mediator.Send(command);

            return result.Match(
                onSuccess: NoContent,
                onFailure: Problem
            );
        }
    }
}
