// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.EntityFramework.Entities;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4.EntityFramework.Mappers
{
    /// <summary>
    /// Extension methods to map to/from entity/model for API resources.
    /// </summary>
    public static class ApiResourceMappers
    {
        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Models.ApiResource ToModel(this ApiResource entity)
        {
            return entity == null ? null : new Models.ApiResource
            {
                Enabled = entity.Enabled,
                Name = entity.Name,
                DisplayName = entity.DisplayName,
                Description = entity.Description,
                ShowInDiscoveryDocument = entity.ShowInDiscoveryDocument,
                UserClaims = entity.UserClaims?.Select(c => c.Type).ToList() ?? new List<string>(),
                Properties = entity.Properties?.ToDictionary(p => p.Key, p => p.Value) ?? new Dictionary<string, string>(),

                ApiSecrets = entity.Secrets?.Select(s => new Models.Secret
                {
                    Type = s.Type,
                    Value = s.Value,
                    Description = s.Description,
                    Expiration = s.Expiration,
                }).ToList() ?? new List<Models.Secret>(),
                Scopes = entity.Scopes?.Select(s => s.Scope).ToList() ?? new List<string>(),
                AllowedAccessTokenSigningAlgorithms = AllowedSigningAlgorithmsConverter.Convert(entity.AllowedAccessTokenSigningAlgorithms),
            };
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ApiResource ToEntity(this Models.ApiResource model)
        {
            return model == null ? null : new Entities.ApiResource
            {
                Enabled = model.Enabled,
                Name = model.Name,
                DisplayName = model.DisplayName,
                Description = model.Description,
                ShowInDiscoveryDocument = model.ShowInDiscoveryDocument,
                UserClaims = model.UserClaims?.Select(c => new Entities.ApiResourceClaim
                {
                    Type = c,
                }).ToList() ?? new List<Entities.ApiResourceClaim>(),
                Properties = model.Properties?.Select(p => new Entities.ApiResourceProperty
                {
                    Key = p.Key,
                    Value = p.Value
                }).ToList() ?? new List<Entities.ApiResourceProperty>(),

                Secrets = model.ApiSecrets?.Select(s => new Entities.ApiResourceSecret
                {
                    Type = s.Type,
                    Value = s.Value,
                    Description = s.Description,
                    Expiration = s.Expiration,
                }).ToList() ?? new List<Entities.ApiResourceSecret>(),
                Scopes = model.Scopes?.Select(s => new Entities.ApiResourceScope
                {
                    Scope = s
                }).ToList() ?? new List<Entities.ApiResourceScope>(),
                AllowedAccessTokenSigningAlgorithms = AllowedSigningAlgorithmsConverter.Convert(model.AllowedAccessTokenSigningAlgorithms),
            };
        }
    }
}