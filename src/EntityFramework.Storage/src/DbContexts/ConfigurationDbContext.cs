// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer4.EntityFramework.DbContexts
{
    /// <summary>
    /// DbContext for the IdentityServer configuration data.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// <seealso cref="IdentityServer4.EntityFramework.Interfaces.IConfigurationDbContext" />
    public class ConfigurationDbContext : ConfigurationDbContext<ConfigurationDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="storeOptions">The store options.</param>
        /// <exception cref="ArgumentNullException">storeOptions</exception>
        public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options, ConfigurationStoreOptions storeOptions)
            : base(options, storeOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="storeOptions">The store options.</param>
        protected ConfigurationDbContext(DbContextOptions options, ConfigurationStoreOptions storeOptions)
            : base(options, storeOptions)
        {
        }
    }

    /// <summary>
    /// DbContext for the IdentityServer configuration data.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// <seealso cref="IdentityServer4.EntityFramework.Interfaces.IConfigurationDbContext" />
    public class ConfigurationDbContext<TContext> : DbContext, IConfigurationDbContext
        where TContext : DbContext, IConfigurationDbContext
    {
        private readonly ConfigurationStoreOptions storeOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="storeOptions">The store options.</param>
        /// <exception cref="ArgumentNullException">storeOptions</exception>
        public ConfigurationDbContext(DbContextOptions<TContext> options, ConfigurationStoreOptions storeOptions)
            : base(options)
        {
            CheckStoreOptions(storeOptions);
            this.storeOptions = storeOptions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="storeOptions">The store options.</param>
        /// <exception cref="ArgumentNullException">storeOptions</exception>
        protected ConfigurationDbContext(DbContextOptions options, ConfigurationStoreOptions storeOptions)
            : base(options)
        {
            CheckStoreOptions(storeOptions);
            this.storeOptions = storeOptions;
        }

        private void CheckStoreOptions(ConfigurationStoreOptions storeOptions)
        {
            if (storeOptions == null)
            {
                throw new ArgumentNullException(nameof(storeOptions));
            }
        }

        /// <summary>
        /// Gets or sets the clients.
        /// </summary>
        /// <value>
        /// The clients.
        /// </value>
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Gets or sets the clients' CORS origins.
        /// </summary>
        /// <value>
        /// The clients CORS origins.
        /// </value>
        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }

        /// <summary>
        /// Gets or sets the identity resources.
        /// </summary>
        /// <value>
        /// The identity resources.
        /// </value>
        public DbSet<IdentityResource> IdentityResources { get; set; }

        /// <summary>
        /// Gets or sets the API resources.
        /// </summary>
        /// <value>
        /// The API resources.
        /// </value>
        public DbSet<ApiResource> ApiResources { get; set; }

        /// <summary>
        /// Gets or sets the API scopes.
        /// </summary>
        /// <value>
        /// The API resources.
        /// </value>
        public DbSet<ApiScope> ApiScopes { get; set; }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureClientContext(storeOptions);
            modelBuilder.ConfigureResourcesContext(storeOptions);

            base.OnModelCreating(modelBuilder);
        }
    }
}