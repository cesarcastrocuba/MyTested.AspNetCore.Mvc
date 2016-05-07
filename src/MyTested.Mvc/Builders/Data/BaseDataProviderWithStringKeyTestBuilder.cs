﻿namespace MyTested.Mvc.Builders.Data
{
    using System.Collections.Generic;
    using Internal.TestContexts;
    using Microsoft.AspNetCore.Routing;

    /// <summary>
    /// Base class for all test builders with data provider containing string as key.
    /// </summary>
    /// <typeparam name="TDataProviderTestBuilder">Type of data provider.</typeparam>
    public abstract class BaseDataProviderWithStringKeyTestBuilder<TDataProviderTestBuilder> : BaseDataProviderTestBuilder
        where TDataProviderTestBuilder : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataProviderTestBuilder"/> class.
        /// </summary>
        /// <param name="testContext">Controller test context containing data about the currently executed assertion chain.</param>
        /// <param name="dataProviderName">Name of the data provider.</param>
        protected BaseDataProviderWithStringKeyTestBuilder(ControllerTestContext testContext, string dataProviderName)
            : base(testContext, dataProviderName)
        {
        }

        /// <summary>
        /// Gets the data provider test builder.
        /// </summary>
        /// <value>Test builder of <see cref="TDataProviderTestBuilder"/>.</value>
        protected abstract TDataProviderTestBuilder DataProviderTestBuilder { get; }
        
        /// <inheritdoc />
        public TDataProviderTestBuilder ContainingEntryWithKey(string key)
        {
            this.ValidateContainingEntryWithKey(key);
            return this.DataProviderTestBuilder;
        }

        /// <inheritdoc />
        public TDataProviderTestBuilder ContainingEntryWithValue<TEntry>(TEntry value)
        {
            this.ValidateContainingEntryWithValue(value);
            return this.DataProviderTestBuilder;
        }

        /// <inheritdoc />
        public TDataProviderTestBuilder ContainingEntryOfType<TEntry>()
        {
            this.ValidateContainingEntryOfType<TEntry>();
            return this.DataProviderTestBuilder;
        }

        /// <inheritdoc />
        public TDataProviderTestBuilder ContainingEntryOfType<TEntry>(string key)
        {
            this.ValidateContainingEntryOfType<TEntry>(key);
            return this.DataProviderTestBuilder;
        }

        /// <inheritdoc />
        public TDataProviderTestBuilder ContainingEntry(string key, object value)
        {
            this.ValidateContainingEntry(key, value);
            return this.DataProviderTestBuilder;
        }

        /// <inheritdoc />
        public TDataProviderTestBuilder ContainingEntries(object entries)
            => this.ContainingEntries(new RouteValueDictionary(entries));

        /// <inheritdoc />
        public TDataProviderTestBuilder ContainingEntries(IDictionary<string, object> entries)
        {
            this.ValidateContainingEntries(entries);
            return this.DataProviderTestBuilder;
        }
    }
}
