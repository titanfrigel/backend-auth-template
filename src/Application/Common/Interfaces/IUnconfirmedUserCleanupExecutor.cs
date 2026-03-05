using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendAuthTemplate.Application.Common.Interfaces
{
    public interface IUnconfirmedUserCleanupExecutor
    {
        Task CleanupUnconfirmedAccounts(CancellationToken cancellationToken = default);
    }
}
