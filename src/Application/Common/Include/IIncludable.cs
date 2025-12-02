namespace BackendAuthTemplate.Application.Common.Include
{
    public interface IIncludable
    {
        IList<string>? Includes { get; init; }
    }
}
