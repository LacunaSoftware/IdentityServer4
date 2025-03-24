using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer4.Infrastructure.Clock
{
    /// <summary>
    /// Abstraction for the date/time.
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// The current UTC date/time.
        /// </summary>
        DateTimeOffset UtcNow { get; }
    }
}
