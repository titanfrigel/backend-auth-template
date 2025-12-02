using Asp.Versioning;
using AutoMapper;
using BackendAuthTemplate.API.Common.Attributes;
using BackendAuthTemplate.API.Common.Extensions;
using BackendAuthTemplate.API.Requests.FeatureName;
using BackendAuthTemplate.Application.Common.Sorting;
using BackendAuthTemplate.Application.Common.PaginatedList;
using BackendAuthTemplate.Application.Common.Result;
using BackendAuthTemplate.Application.Features.FeatureName.Commands.CreateEntityNameCommand;
using BackendAuthTemplate.Application.Features.FeatureName.Commands.DeleteEntityNameCommand;
using BackendAuthTemplate.Application.Features.FeatureName.Commands.UpdateEntityNameCommand;
using BackendAuthTemplate.Application.Features.FeatureName.Dtos;
using BackendAuthTemplate.Application.Features.FeatureName.Queries.GetFeatureNameWithPaginationQuery;
using BackendAuthTemplate.Application.Features.FeatureName.Queries.GetEntityNameByIdQuery;
using BackendAuthTemplate.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendAuthTemplate.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{v:apiVersion}/featureName")]
    [Authorize(Roles = "Admin")]
    public class FeatureNameController(IMediator mediator, IMapper mapper) : BaseController
    {
        [HttpPost]
        public async Task<ActionResult> CreateEntityName([FromBody] CreateEntityNameRequest request)
        {
            CreateEntityNameCommand command = mapper.Map<CreateEntityNameCommand>(request);

            Result<Guid> result = await mediator.Send(command);

            return await result.MatchAsync(
                onSuccess: async entityNameId =>
                {
                    Result<ReadEntityNameDto> entityNameResult = await mediator.Send(new GetEntityNameByIdQuery { EntityNameId = entityNameId });

                    return entityNameResult.Match(
                        onSuccess: entityName => CreatedAtAction(nameof(GetEntityNameById), new { entityNameId }, entityName),
                        onFailure: Problem
                    );
                },
                onFailure: Problem
            );
        }

        [HttpGet("{entityNameId:guid}")]
        [AllowAnonymous]
        [ApiEntity(typeof(EntityName))]
        public async Task<ActionResult<ReadEntityNameDto>> GetEntityNameById([FromRoute] Guid entityNameId, [FromQuery] IList<string>? includes = null)
        {
            GetEntityNameByIdQuery query = new()
            {
                EntityNameId = entityNameId,
                Includes = includes
            };

            Result<ReadEntityNameDto> result = await mediator.Send(query);

            return result.Match(
                onSuccess: Ok,
                onFailure: Problem
            );
        }

        [HttpGet]
        [AllowAnonymous]
        [ApiEntity(typeof(EntityName))]
        public async Task<ActionResult<PaginatedList<ReadEntityNameDto>>> GetFeatureNamePaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] IList<string>? includes = null, [FromQuery] IList<Sort>? sorts = null)
        {
            GetFeatureNameWithPaginationQuery query = new()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Includes = includes,
                Sorts = sorts
            };

            Result<PaginatedList<ReadEntityNameDto>> result = await mediator.Send(query);

            return result.Match(
                onSuccess: Ok,
                onFailure: Problem
            );
        }

        [HttpPut("{entityNameId:guid}")]
        public async Task<ActionResult> UpdateEntityName([FromRoute] Guid entityNameId, [FromBody] UpdateEntityNameRequest request)
        {
            UpdateEntityNameCommand command = mapper.Map<UpdateEntityNameCommand>(request, opt => opt.Items["EntityNameId"] = entityNameId);

            Result result = await mediator.Send(command);

            return result.Match(
                onSuccess: NoContent,
                onFailure: Problem
            );
        }

        [HttpDelete("{entityNameId:guid}")]
        public async Task<ActionResult> DeleteEntityName([FromRoute] Guid entityNameId)
        {
            DeleteEntityNameCommand command = new()
            {
                EntityNameId = entityNameId
            };

            Result result = await mediator.Send(command);

            return result.Match(
                onSuccess: NoContent,
                onFailure: Problem
            );
        }
    }
}
