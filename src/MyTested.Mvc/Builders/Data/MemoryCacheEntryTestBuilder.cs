﻿namespace MyTested.Mvc.Builders.Data
{
    using System;
    using System.Collections.Generic;
    using Contracts.Data;
    using Exceptions;
    using Internal.Contracts;
    using Internal.TestContexts;
    using Microsoft.Extensions.Caching.Memory;
    using Utilities;
    using Utilities.Extensions;
    using Utilities.Validators;

    /// <summary>
    /// Used for building mocked <see cref="IMemoryCache"/> entry.
    /// </summary>
    public class MemoryCacheEntryTestBuilder : MemoryCacheEntryBuilder, IAndMemoryCacheEntryTestBuilder
    {
        private readonly ControllerTestContext testContext;
        private readonly ICollection<Action<IMockedMemoryCacheEntry, IMockedMemoryCacheEntry>> validations;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheEntryTestBuilder"/> class.
        /// </summary>
        /// <param name="testContext">Controller test context containing data about the currently executed assertion chain.</param>
        public MemoryCacheEntryTestBuilder(ControllerTestContext testContext)
        {
            CommonValidator.CheckForNullReference(testContext, nameof(testContext));

            this.testContext = testContext;
            this.validations = new List<Action<IMockedMemoryCacheEntry, IMockedMemoryCacheEntry>>();
        }

        /// <inheritdoc />
        public override IAndMemoryCacheEntryTestBuilder WithValue(object value)
        {
            this.validations.Add((expected, actual) =>
            {
                if (Reflection.AreNotDeeplyEqual(expected.Value, actual.Value))
                {
                    this.ThrowNewDataProviderAssertionException(
                        "to have entry with the given value",
                        "in fact it was different");
                }
            });

            return base.WithValue(value);
        }

        /// <inheritdoc />
        public override IAndMemoryCacheEntryTestBuilder WithAbsoluteExpiration(DateTimeOffset? absoluteExpiration)
        {
            this.validations.Add((expected, actual) =>
            {
                var expectedExpiration = expected.Options.AbsoluteExpiration;
                var actualExpiration = actual.Options.AbsoluteExpiration;

                if (expectedExpiration != actualExpiration)
                {
                    this.ThrowNewDataProviderAssertionException(
                        $"to have entry with {expectedExpiration.GetErrorMessageName()} absolute expiration",
                        $"in fact found {actualExpiration.GetErrorMessageName()}");
                }
            });

            return base.WithAbsoluteExpiration(absoluteExpiration);
        }

        /// <inheritdoc />
        public override IAndMemoryCacheEntryTestBuilder WithAbsoluteExpirationRelativeToNow(TimeSpan? absoluteExpirationRelativeToNow)
        {
            this.validations.Add((expected, actual) =>
            {
                var expectedRelativeExpiration = expected.Options.AbsoluteExpirationRelativeToNow;
                var actualRelativeExpiration = actual.Options.AbsoluteExpirationRelativeToNow;

                if (expectedRelativeExpiration != actualRelativeExpiration)
                {
                    this.ThrowNewDataProviderAssertionException(
                        $"to have entry with {expectedRelativeExpiration.GetErrorMessageName()} absolute expiration relative to now",
                        $"in fact found {actualRelativeExpiration.GetErrorMessageName()}");
                }
            });

            return base.WithAbsoluteExpirationRelativeToNow(absoluteExpirationRelativeToNow);
        }

        /// <inheritdoc />
        public override IAndMemoryCacheEntryTestBuilder WithPriority(CacheItemPriority priority)
        {
            this.validations.Add((expected, actual) =>
            {
                var expectedPriority = expected.Options.Priority;
                var actualPriority = actual.Options.Priority;

                if (expected.Options.Priority != actual.Options.Priority)
                {
                    this.ThrowNewDataProviderAssertionException(
                        $"to have entry with {expectedPriority.GetErrorMessageName(includeQuotes: false)} priority",
                        $"in fact found {actualPriority.GetErrorMessageName(includeQuotes: false)}");
                }
            });

            return base.WithPriority(priority);
        }

        /// <inheritdoc />
        public override IAndMemoryCacheEntryTestBuilder WithSlidingExpiration(TimeSpan? slidingExpiration)
        {
            this.validations.Add((expected, actual) =>
            {
                var expectedSlidingExpiration = expected.Options.SlidingExpiration;
                var actualSlidingExpiration = actual.Options.SlidingExpiration;

                if (expectedSlidingExpiration != actualSlidingExpiration)
                {
                    this.ThrowNewDataProviderAssertionException(
                        $"to have entry with {expectedSlidingExpiration.GetErrorMessageName()} sliding expiration",
                        $"in fact found {actualSlidingExpiration.GetErrorMessageName()}");
                }
            });

            return base.WithSlidingExpiration(slidingExpiration);
        }

        internal ICollection<Action<IMockedMemoryCacheEntry, IMockedMemoryCacheEntry>> GetMockedMemoryCacheEntryValidations()
            => this.validations;

        private void ThrowNewDataProviderAssertionException(string expectedValue, string actualValue)
        {
            throw new DataProviderAssertionException(string.Format(
                "When calling {0} action in {1} expected memory cache {2}, but {3}.",
                this.testContext.ActionName,
                this.testContext.Controller.GetName(),
                expectedValue,
                actualValue));
        }
    }
}
