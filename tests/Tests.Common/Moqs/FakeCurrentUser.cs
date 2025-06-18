using BackendAuthTemplate.Application.Common.Interfaces;

namespace BackendAuthTemplate.Tests.Common.Moqs
{
    public class FakeCurrentUser : IUser
    {
        private static readonly AsyncLocal<Guid?> _override = new();

        public Guid UserId => _override.Value ?? Guid.NewGuid();
        public bool HasContext => true;
        public IEnumerable<string> Roles => ["Admin", "User"];

        public IDisposable BeginScope(Guid userId)
        {
            Guid? previous = _override.Value;

            _override.Value = userId;

            return new DisposeAction(() => _override.Value = previous);
        }

        private sealed class DisposeAction(Action a) : IDisposable
        {
            private readonly Action _a = a;

            public void Dispose()
            {
                _a();
            }
        }
    }
}
