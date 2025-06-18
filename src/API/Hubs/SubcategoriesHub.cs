using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BackendAuthTemplate.API.Hubs
{
    [ApiVersion("1.0")]
    [Route("hubs/v{v:apiVersion}/subcategories")]
    [Authorize(Roles = "Admin")]
    public class SubcategoriesHub : Hub
    {
    }
}
