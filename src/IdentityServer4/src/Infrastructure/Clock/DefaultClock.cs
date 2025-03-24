using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer4.Infrastructure.Clock
{
    internal class DefaultClock : IClock
    {
        private readonly TimeProvider _timeProvider;

        public DefaultClock()
        {
            _timeProvider = TimeProvider.System;
        }

        public DefaultClock(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public DateTimeOffset UtcNow { get => _timeProvider.GetUtcNow(); }
    }
}
