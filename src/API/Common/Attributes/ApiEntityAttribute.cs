namespace BackendAuthTemplate.API.Common.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ApiEntityAttribute(Type entityType) : Attribute
{
    public Type EntityType { get; } = entityType;
}
