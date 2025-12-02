using Asp.Versioning;
using AutoMapper;
using BackendAuthTemplate.API.Common.Attributes;
using BackendAuthTemplate.API.Common.Extensions;
using BackendAuthTemplate.API.Requests.Categories;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Common.Sorting;
using BackendAuthTemplate.Application.Features.Categories.Commands.CreateCategoryCommand;
using BackendAuthTemplate.Application.Features.Categories.Commands.DeleteCategoryCommand;
using BackendAuthTemplate.Application.Features.Categories.Commands.UpdateCategoryCommand;
using BackendAuthTemplate.Application.Features.Categories.Dtos;
using BackendAuthTemplate.Application.Features.Categories.Queries.GetCategoriesWithPaginationQuery;
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
        [ApiEntity(typeof(Category))]
        public async Task<ActionResult<ReadCategoryDto>> GetCategoryById([FromRoute] Guid categoryId, [FromQuery] IList<string>? includes)
        {
            GetCategoryByIdQuery query = new()
            {
                CategoryId = categoryId,
                Includes = includes
            };

            Result<ReadCategoryDto> result = await mediator.Send(query);

            return result.Match(
                onSuccess: Ok,
                onFailure: Problem
            );
        }

        [HttpGet]
        [AllowAnonymous]
        [ApiEntity(typeof(Category))]
        public async Task<ActionResult<PaginatedList<ReadCategoryDto>>> GetCategoriesPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] IList<string>? includes = null, [FromQuery] IList<Sort>? sorts = null)
        {
            GetCategoriesWithPaginationQuery query = new()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Includes = includes,
                Sorts = sorts
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
