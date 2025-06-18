namespace BackendAuthTemplate.Application.Common.Include
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class IncludeParameterAttribute(Type entityType) : Attribute
    {
        public Type EntityType { get; } = entityType;
    }
}
