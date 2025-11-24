using BackendAuthTemplate.API.Requests.FeatureName;

namespace BackendAuthTemplate.Tests.Common.FeatureName
{
    public class FeatureNameRequestsTestHelper
    {
        public static CreateEntityNameRequest CreateEntityNameRequest(
            string name = "Default EntityName"
        )
        {
            return new()
            {
                Name = name
            };
        }

        public static UpdateEntityNameRequest UpdateEntityNameRequest(
            string name = "Updated EntityName"
        )
        {
            return new()
            {
                Name = name
            };
        }
    }
}
