using BackendAuthTemplate.Application.Features.Subcategories.Commands.CreateSubcategoryCommand;
using BackendAuthTemplate.Application.Features.Subcategories.Commands.DeleteSubcategoryCommand;
using BackendAuthTemplate.Application.Features.Subcategories.Commands.UpdateSubcategoryCommand;

namespace BackendAuthTemplate.Tests.Common.Subcategories
{
    public static class SubcategoriesCommandsTestHelper
    {
        public static CreateSubcategoryCommand CreateSubcategoryCommand(
            Guid? categoryId = null,
            string name = "Default Subcategory",
            string description = "Default Subcategory Description"
        )
        {
            return new()
            {
                Name = name,
                Description = description,
                CategoryId = categoryId ?? Guid.NewGuid()
            };
        }

        public static UpdateSubcategoryCommand UpdateSubcategoryCommand(
            Guid? id = null,
            Guid? categoryId = null,
            string name = "Updated Subcategory",
            string description = "Updated Subcategory Description"
        )
        {
            return new()
            {
                SubcategoryId = id ?? Guid.NewGuid(),
                Name = name,
                Description = description,
                CategoryId = categoryId ?? Guid.NewGuid()
            };
        }

        public static DeleteSubcategoryCommand DeleteSubcategoryCommand(
            Guid? id = null
        )
        {
            return new()
            {
                SubcategoryId = id ?? Guid.NewGuid()
            };
        }
    }
}
