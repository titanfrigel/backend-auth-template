using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendAuthTemplate.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{v:apiVersion}/docs/websockets")]
    [AllowAnonymous]
    public class WebSocketDocsController : BaseController
    {
        private static readonly object CategoriesHubDoc = new
        {
            hubName = "CategoriesHub",
            negotiateEndpoint = "/hubs/v1/categories/negotiate",
            websocketUrl = "/hubs/v1/categories",
            requires = new[]
            {
                "Query or Header: access_token (JWT for User role)"
            },
            method = "WebSocket via SignalR",
            protocol = "json",
            handshake = "{\"protocol\":\"json\",\"version\":1}\x1E",
            eventsReceived = new[]
            {
                new
                {
                    name = "CategoryCreated",
                    payload = new { categoryId = "Guid" }
                }
            },
            notes = "Admin users receive all new categories in real-time. Must provide a valid JWT token with Admin role."
        };

        private static readonly object SubcategoriesHubDoc = new
        {
            hubName = "SubcategoriesHub",
            negotiateEndpoint = "/hubs/v1/subcategories/negotiate",
            websocketUrl = "/hubs/v1/subcategories",
            requires = new[]
            {
                "Query or Header: access_token (JWT for User role)"
            },
            method = "WebSocket via SignalR",
            protocol = "json",
            handshake = "{\"protocol\":\"json\",\"version\":1}\x1E",
            eventsReceived = new[]
            {
                new
                {
                    name = "SubcategoryCreated",
                    payload = new { subcategoryId = "Guid" }
                }
            },
            notes = "Admin users receive all new categories in real-time. Must provide a valid JWT token with Admin role."
        };

        /// <summary>
        /// Documentation for the CategoriesHub (used by admins to receive real-time notifications about all categories).
        /// </summary>
        [HttpGet("categories")]
        public ActionResult GetCategoriesHubDocs()
        {
            return Ok(CategoriesHubDoc);
        }

        /// <summary>
        /// Documentation for the SubcategoriesHub (used by admins to receive real-time notifications about all orders).
        /// </summary>
        [HttpGet("subcategories")]
        public ActionResult GetSubcategoriesHubDocs()
        {
            return Ok(SubcategoriesHubDoc);
        }
    }
}
