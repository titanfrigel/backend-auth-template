using BackendAuthTemplate.Application.Features.Categories.Commands.CreateCategoryCommand;
using BackendAuthTemplate.Application.Features.Categories.Commands.DeleteCategoryCommand;
using BackendAuthTemplate.Application.Features.Categories.Commands.UpdateCategoryCommand;

namespace BackendAuthTemplate.Tests.Common.Categories
{
    public static class CategoriesCommandsTestHelper
    {
        public static CreateCategoryCommand CreateCategoryCommand(
            string name = "Default Category",
            string description = "Default Category Description"
        )
        {
            return new()
            {
                Name = name,
                Description = description
            };
        }

        public static UpdateCategoryCommand UpdateCategoryCommand(
            Guid? id = null,
            string name = "Updated Category",
            string description = "Updated Category Description"
        )
        {
            return new()
            {
                CategoryId = id ?? Guid.NewGuid(),
                Name = name,
                Description = description
            };
        }

        public static DeleteCategoryCommand DeleteCategoryCommand(
            Guid? id = null
        )
        {
            return new()
            {
                CategoryId = id ?? Guid.NewGuid()
            };
        }
    }
}
