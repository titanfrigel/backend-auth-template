namespace BackendAuthTemplate.Application.Common.Interfaces
{
    public interface IUser
    {
        public Guid UserId { get; }
        public IEnumerable<string> Roles { get; }
        public bool HasContext { get; }
        public IDisposable BeginScope(Guid userId);
    }
}
