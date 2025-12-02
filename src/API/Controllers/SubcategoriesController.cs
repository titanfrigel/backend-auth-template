using Asp.Versioning;
using AutoMapper;
using BackendAuthTemplate.API.Common.Attributes;
using BackendAuthTemplate.API.Common.Extensions;
using BackendAuthTemplate.API.Requests.Subcategories;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Common.Sorting;
using BackendAuthTemplate.Application.Features.Subcategories.Commands.CreateSubcategoryCommand;
using BackendAuthTemplate.Application.Features.Subcategories.Commands.DeleteSubcategoryCommand;
using BackendAuthTemplate.Application.Features.Subcategories.Commands.UpdateSubcategoryCommand;
using BackendAuthTemplate.Application.Features.Subcategories.Dtos;
using BackendAuthTemplate.Application.Features.Subcategories.Queries.GetSubcategoriesWithPaginationQuery;
using BackendAuthTemplate.Application.Features.Subcategories.Queries.GetSubcategoryByIdQuery;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendAuthTemplate.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{v:apiVersion}/subcategories")]
    [Authorize(Roles = "Admin")]
    public class SubcategoriesController(IMediator mediator, IMapper mapper) : BaseController
    {
        [HttpPost]
        public async Task<ActionResult> CreateSubcategory([FromBody] CreateSubcategoryRequest request)
        {
            CreateSubcategoryCommand command = mapper.Map<CreateSubcategoryCommand>(request);

            Result<Guid> result = await mediator.Send(command);

            return await result.MatchAsync(
                onSuccess: async subcategoryId =>
                {
                    Result<ReadSubcategoryDto> subcategoryResult = await mediator.Send(new GetSubcategoryByIdQuery { SubcategoryId = subcategoryId });

                    return subcategoryResult.Match(
                        onSuccess: subcategory => CreatedAtAction(nameof(GetSubcategoryById), new { subcategoryId }, subcategory),
                        onFailure: Problem
                    );
                },
                onFailure: Problem
            );
        }

        [HttpGet("{subcategoryId:guid}")]
        [AllowAnonymous]
        [ApiEntity(typeof(Subcategory))]
        public async Task<ActionResult<ReadSubcategoryDto>> GetSubcategoryById(Guid subcategoryId, [FromQuery] IList<string>? includes)
        {
            GetSubcategoryByIdQuery query = new()
            {
                SubcategoryId = subcategoryId,
                Includes = includes
            };

            Result<ReadSubcategoryDto> result = await mediator.Send(query);

            return result.Match(
                onSuccess: Ok,
                onFailure: Problem
            );
        }

        [HttpGet]
        [AllowAnonymous]
        [ApiEntity(typeof(Subcategory))]
        public async Task<ActionResult<PaginatedList<ReadSubcategoryDto>>> GetSubcategoriesPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] IList<string>? includes = null, [FromQuery] IList<Sort>? sorts = null)
        {
            GetSubcategoriesWithPaginationQuery query = new()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Includes = includes,
                Sorts = sorts
            };

            Result<PaginatedList<ReadSubcategoryDto>> result = await mediator.Send(query);

            return result.Match(
                onSuccess: Ok,
                onFailure: Problem
            );
        }

        [HttpPut("{subcategoryId:guid}")]
        public async Task<ActionResult> UpdateSubcategory(Guid subcategoryId, [FromBody] UpdateSubcategoryRequest request)
        {
            UpdateSubcategoryCommand command = mapper.Map<UpdateSubcategoryCommand>(request, opt => opt.Items["SubcategoryId"] = subcategoryId);

            Result result = await mediator.Send(command);

            return result.Match(
                onSuccess: NoContent,
                onFailure: Problem
            );
        }

        [HttpDelete("{subcategoryId:guid}")]
        public async Task<ActionResult> DeleteSubcategory(Guid subcategoryId)
        {
            DeleteSubcategoryCommand command = new()
            {
                SubcategoryId = subcategoryId
            };

            Result result = await mediator.Send(command);

            return result.Match(
                onSuccess: NoContent,
                onFailure: Problem
            );
        }
    }
}
