// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.Infrastructure.Clock;
using IdentityServer4.Models;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;
using Token = IdentityServer4.Models.Token;

namespace IdentityServer4.Services
{
    /// <summary>
    /// Default token creation service
    /// </summary>
    public class DefaultTokenCreationService : ITokenCreationService
    {
        /// <summary>
        /// The key service
        /// </summary>
        protected readonly IKeyMaterialService Keys;

        /// <summary>
        /// The logger
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        ///  The clock
        /// </summary>
        protected readonly IClock Clock;

        /// <summary>
        /// The options
        /// </summary>
        protected readonly IdentityServerOptions Options;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTokenCreationService"/> class.
        /// </summary>
        /// <param name="clock">The options.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        public DefaultTokenCreationService(
            IClock clock,
            IKeyMaterialService keys,
            IdentityServerOptions options,
            ILogger<DefaultTokenCreationService> logger)
        {
            Clock = clock;
            Keys = keys;
            Options = options;
            Logger = logger;
        }

        /// <summary>
        /// Creates the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        /// A protected and serialized security token
        /// </returns>
        public virtual async Task<string> CreateTokenAsync(Token token)
        {
            var additionalHeaderElements = await CreateHeaderAsync(token);
            var payload = await CreatePayloadAsync(token);

            return await CreateJwtAsync(token, payload, additionalHeaderElements);
        }

        /// <summary>
        /// Creates the JWT header
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The JWT header</returns>
        protected virtual Task<Dictionary<string, object>> CreateHeaderAsync(Token token)
        {
            var additionalHeaderElements = new Dictionary<string, object>();

            if (token.Type == TokenTypes.AccessToken)
            {
                if (Options.AccessTokenJwtType.IsPresent())
                {
                    additionalHeaderElements["typ"] = Options.AccessTokenJwtType;
                }
            }

            return Task.FromResult(additionalHeaderElements);
        }

        /// <summary>
        /// Creates the JWT payload
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The JWT payload</returns>
        protected virtual Task<JwtPayload> CreatePayloadAsync(Token token)
        {
            var payload = token.CreateJwtPayload(Clock, Options, Logger);
            return Task.FromResult(payload);
        }

        /// <summary>
        /// Creates and signs the JWT
        /// </summary>
        /// <param name="token"></param>
        /// <param name="payload"></param>
        /// <param name="additionalHeaderElements"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected async virtual Task<string> CreateJwtAsync(Token token, JwtPayload payload, Dictionary<string, object> additionalHeaderElements)
        {
            var credential = await Keys.GetSigningCredentialsAsync(token.AllowedSigningAlgorithms);

            if (credential == null)
            {
                throw new InvalidOperationException("No signing credential is configured. Can't create JWT token");
            }

            var payloadJson = JsonConvert.SerializeObject(payload);

            var handler = new JsonWebTokenHandler { SetDefaultTimesOnTokenCreation = false };
            return handler.CreateToken(payloadJson, credential, additionalHeaderElements);
        }
    }
}