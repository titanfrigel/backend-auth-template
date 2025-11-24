using BackendAuthTemplate.Application.Features.FeatureName.Commands.CreateEntityNameCommand;
using BackendAuthTemplate.Application.Features.FeatureName.Commands.DeleteEntityNameCommand;
using BackendAuthTemplate.Application.Features.FeatureName.Commands.UpdateEntityNameCommand;

namespace BackendAuthTemplate.Tests.Common.FeatureName
{
    public static class FeatureNameCommandsTestHelper
    {
        public static CreateEntityNameCommand CreateEntityNameCommand(
            string name = "Default EntityName"
        )
        {
            return new()
            {
                Name = name
            };
        }

        public static UpdateEntityNameCommand UpdateEntityNameCommand(
            Guid? id = null,
            string name = "Updated EntityName"
        )
        {
            return new()
            {
                EntityNameId = id ?? Guid.NewGuid(),
                Name = name
            };
        }

        public static DeleteEntityNameCommand DeleteEntityNameCommand(
            Guid? id = null
        )
        {
            return new()
            {
                EntityNameId = id ?? Guid.NewGuid()
            };
        }
    }
}
