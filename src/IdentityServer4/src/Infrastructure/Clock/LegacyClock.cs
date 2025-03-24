using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer4.Infrastructure.Clock
{
    internal class LegacyClock : IClock
    {
#pragma warning disable CS0618 // Type or member is obsolete
        private readonly ISystemClock _clock;
#pragma warning restore CS0618 // Type or member is obsolete

#pragma warning disable CS0618 // Type or member is obsolete
        public LegacyClock(ISystemClock clock)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            _clock = clock;
        }

        public DateTimeOffset UtcNow
        {
            get => _clock.UtcNow;
        }
    }
}
