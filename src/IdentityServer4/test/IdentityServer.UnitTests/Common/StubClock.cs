// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using IdentityServer4.Infrastructure.Clock;

namespace IdentityServer.UnitTests.Common
{
    internal class StubClock : IClock
    {
        public Func<DateTime> UtcNowFunc = () => DateTime.UtcNow;
        public DateTimeOffset UtcNow => new DateTimeOffset(UtcNowFunc());
    }
}
