﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.TestModels.FunkyDataModel;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Xunit.Abstractions;
using Xunit.Sdk;
using Assert = Xunit.Assert;

namespace Microsoft.EntityFrameworkCore.Query
{
    public class FunkyDataQuerySqlServerTest : FunkyDataQueryTestBase<FunkyDataQuerySqlServerTest.FunkyDataQuerySqlServerFixture>
    {
        public FunkyDataQuerySqlServerTest(FunkyDataQuerySqlServerFixture fixture, ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            Fixture.TestSqlLoggerFactory.Clear();
            Fixture.TestSqlLoggerFactory.SetTestOutputHelper(testOutputHelper);
        }

        public override async Task String_contains_on_argument_with_wildcard_constant(bool async)
        {
            await base.String_contains_on_argument_with_wildcard_constant(async);

            AssertSql(
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CHARINDEX(N'%B', [f].[FirstName]) > 0",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CHARINDEX(N'a_', [f].[FirstName]) > 0",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CHARINDEX(NULL, [f].[FirstName]) > 0",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CHARINDEX(N'_Ba_', [f].[FirstName]) > 0",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE NOT (CHARINDEX(N'%B%a%r', [f].[FirstName]) > 0)",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE NOT (CHARINDEX(NULL, [f].[FirstName]) > 0)");
        }

        public override async Task String_contains_on_argument_with_wildcard_parameter(bool async)
        {
            await base.String_contains_on_argument_with_wildcard_parameter(async);

            AssertSql(
                @"@__prm1_0='%B' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE (@__prm1_0 LIKE N'') OR (CHARINDEX(@__prm1_0, [f].[FirstName]) > 0)",
                //
                @"@__prm2_0='a_' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE (@__prm2_0 LIKE N'') OR (CHARINDEX(@__prm2_0, [f].[FirstName]) > 0)",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE CHARINDEX(NULL, [f].[FirstName]) > 0",
                //
                @"@__prm4_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE (@__prm4_0 LIKE N'') OR (CHARINDEX(@__prm4_0, [f].[FirstName]) > 0)",
                //
                @"@__prm5_0='_Ba_' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE (@__prm5_0 LIKE N'') OR (CHARINDEX(@__prm5_0, [f].[FirstName]) > 0)",
                //
                @"@__prm6_0='%B%a%r' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE NOT ((@__prm6_0 LIKE N'') OR (CHARINDEX(@__prm6_0, [f].[FirstName]) > 0))",
                //
                @"@__prm7_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE NOT ((@__prm7_0 LIKE N'') OR (CHARINDEX(@__prm7_0, [f].[FirstName]) > 0))",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE NOT (CHARINDEX(NULL, [f].[FirstName]) > 0)");
        }

        public override async Task String_contains_on_argument_with_wildcard_column(bool async)
        {
            await base.String_contains_on_argument_with_wildcard_column(async);

            AssertSql(
                @"SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE ([f0].[LastName] LIKE N'') OR (CHARINDEX([f0].[LastName], [f].[FirstName]) > 0)");
        }

        public override async Task String_contains_on_argument_with_wildcard_column_negated(bool async)
        {
            await base.String_contains_on_argument_with_wildcard_column_negated(async);

            AssertSql(
                @"SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE NOT (([f0].[LastName] LIKE N'') OR (CHARINDEX([f0].[LastName], [f].[FirstName]) > 0))");
        }

        public override async Task String_starts_with_on_argument_with_wildcard_constant(bool async)
        {
            await base.String_starts_with_on_argument_with_wildcard_constant(async);

            AssertSql(
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'\%B%' ESCAPE N'\')",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'a\_%' ESCAPE N'\')",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'\_Ba\_%' ESCAPE N'\')",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND NOT ([f].[FirstName] LIKE N'\%B\%a\%r%' ESCAPE N'\')",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1");
        }

        public override async Task String_starts_with_on_argument_with_wildcard_parameter(bool async)
        {
            await base.String_starts_with_on_argument_with_wildcard_parameter(async);

            AssertSql(
                @"@__prm1_0='%B' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE (@__prm1_0 LIKE N'') OR ([f].[FirstName] IS NOT NULL AND (LEFT([f].[FirstName], LEN(@__prm1_0)) = @__prm1_0))",
                //
                @"@__prm2_0='a_' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE (@__prm2_0 LIKE N'') OR ([f].[FirstName] IS NOT NULL AND (LEFT([f].[FirstName], LEN(@__prm2_0)) = @__prm2_0))",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1",
                //
                @"@__prm4_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE (@__prm4_0 LIKE N'') OR ([f].[FirstName] IS NOT NULL AND (LEFT([f].[FirstName], LEN(@__prm4_0)) = @__prm4_0))",
                //
                @"@__prm5_0='_Ba_' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE (@__prm5_0 LIKE N'') OR ([f].[FirstName] IS NOT NULL AND (LEFT([f].[FirstName], LEN(@__prm5_0)) = @__prm5_0))",
                //
                @"@__prm6_0='%B%a%r' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE NOT (@__prm6_0 LIKE N'') AND ([f].[FirstName] IS NOT NULL AND (LEFT([f].[FirstName], LEN(@__prm6_0)) <> @__prm6_0))",
                //
                @"@__prm7_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE NOT (@__prm7_0 LIKE N'') AND ([f].[FirstName] IS NOT NULL AND (LEFT([f].[FirstName], LEN(@__prm7_0)) <> @__prm7_0))",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1");
        }

        public override async Task String_starts_with_on_argument_with_bracket(bool async)
        {
            await base.String_starts_with_on_argument_with_bracket(async);

            AssertSql(
                @"SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'\[%' ESCAPE N'\')",
                //
                @"SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'B\[%' ESCAPE N'\')",
                //
                @"SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'B\[\[a^%' ESCAPE N'\')",
                //
                @"@__prm1_0='[' (Size = 4000)

SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE (@__prm1_0 LIKE N'') OR ([f].[FirstName] IS NOT NULL AND (LEFT([f].[FirstName], LEN(@__prm1_0)) = @__prm1_0))",
                //
                @"@__prm2_0='B[' (Size = 4000)

SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE (@__prm2_0 LIKE N'') OR ([f].[FirstName] IS NOT NULL AND (LEFT([f].[FirstName], LEN(@__prm2_0)) = @__prm2_0))",
                //
                @"@__prm3_0='B[[a^' (Size = 4000)

SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE (@__prm3_0 LIKE N'') OR ([f].[FirstName] IS NOT NULL AND (LEFT([f].[FirstName], LEN(@__prm3_0)) = @__prm3_0))",
                //
                @"SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE ([f].[LastName] LIKE N'') OR ([f].[FirstName] IS NOT NULL AND ([f].[LastName] IS NOT NULL AND (LEFT([f].[FirstName], LEN([f].[LastName])) = [f].[LastName])))");
        }

        public override async Task String_starts_with_on_argument_with_wildcard_column(bool async)
        {
            await base.String_starts_with_on_argument_with_wildcard_column(async);

            AssertSql(
                @"SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE ([f0].[LastName] LIKE N'') OR ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (LEFT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName])))");
        }

        public override async Task String_starts_with_on_argument_with_wildcard_column_negated(bool async)
        {
            await base.String_starts_with_on_argument_with_wildcard_column_negated(async);

            AssertSql(
                @"SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE NOT ([f0].[LastName] LIKE N'') AND ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (LEFT([f].[FirstName], LEN([f0].[LastName])) <> [f0].[LastName])))");
        }

        public override async Task String_ends_with_on_argument_with_wildcard_constant(bool async)
        {
            await base.String_ends_with_on_argument_with_wildcard_constant(async);

            AssertSql(
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'%\%B' ESCAPE N'\')",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'%a\_' ESCAPE N'\')",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND ([f].[FirstName] LIKE N'%\_Ba\_' ESCAPE N'\')",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND NOT ([f].[FirstName] LIKE N'%\%B\%a\%r' ESCAPE N'\')",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1");
        }

        public override async Task String_ends_with_on_argument_with_wildcard_parameter(bool async)
        {
            await base.String_ends_with_on_argument_with_wildcard_parameter(async);

            AssertSql(
                @"@__prm1_0='%B' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE (@__prm1_0 LIKE N'') OR ([f].[FirstName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN(@__prm1_0)) = @__prm1_0))",
                //
                @"@__prm2_0='a_' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE (@__prm2_0 LIKE N'') OR ([f].[FirstName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN(@__prm2_0)) = @__prm2_0))",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1",
                //
                @"@__prm4_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE (@__prm4_0 LIKE N'') OR ([f].[FirstName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN(@__prm4_0)) = @__prm4_0))",
                //
                @"@__prm5_0='_Ba_' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE (@__prm5_0 LIKE N'') OR ([f].[FirstName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN(@__prm5_0)) = @__prm5_0))",
                //
                @"@__prm6_0='%B%a%r' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE NOT (@__prm6_0 LIKE N'') AND ([f].[FirstName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN(@__prm6_0)) <> @__prm6_0))",
                //
                @"@__prm7_0='' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE NOT (@__prm7_0 LIKE N'') AND ([f].[FirstName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN(@__prm7_0)) <> @__prm7_0))",
                //
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1");
        }

        public override async Task String_ends_with_on_argument_with_wildcard_column(bool async)
        {
            await base.String_ends_with_on_argument_with_wildcard_column(async);

            AssertSql(
                @"SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE ([f0].[LastName] LIKE N'') OR ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName])))");
        }

        public override async Task String_ends_with_on_argument_with_wildcard_column_negated(bool async)
        {
            await base.String_ends_with_on_argument_with_wildcard_column_negated(async);

            AssertSql(
                @"SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE NOT ([f0].[LastName] LIKE N'') AND ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN([f0].[LastName])) <> [f0].[LastName])))");
        }

        public override async Task String_ends_with_inside_conditional(bool async)
        {
            await base.String_ends_with_inside_conditional(async);

            AssertSql(
                @"SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE CASE
    WHEN ([f0].[LastName] LIKE N'') OR ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName]))) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END = CAST(1 AS bit)");
        }

        public override async Task String_ends_with_inside_conditional_negated(bool async)
        {
            await base.String_ends_with_inside_conditional_negated(async);

            AssertSql(
                @"SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE CASE
    WHEN NOT ([f0].[LastName] LIKE N'') AND ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN([f0].[LastName])) <> [f0].[LastName]))) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END = CAST(1 AS bit)");
        }

        public override async Task String_ends_with_equals_nullable_column(bool async)
        {
            await base.String_ends_with_equals_nullable_column(async);

            AssertSql(
                @"SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool], [f0].[Id], [f0].[FirstName], [f0].[LastName], [f0].[NullableBool]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE (CASE
    WHEN ([f0].[LastName] LIKE N'') OR ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName]))) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END = [f].[NullableBool]) OR (CASE
    WHEN ([f0].[LastName] LIKE N'') OR ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName]))) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END IS NULL AND [f].[NullableBool] IS NULL)");
        }

        public override async Task String_ends_with_not_equals_nullable_column(bool async)
        {
            await base.String_ends_with_not_equals_nullable_column(async);

            AssertSql(
                @"SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool], [f0].[Id], [f0].[FirstName], [f0].[LastName], [f0].[NullableBool]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE ((CASE
    WHEN ([f0].[LastName] LIKE N'') OR ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName]))) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END <> [f].[NullableBool]) OR (CASE
    WHEN ([f0].[LastName] LIKE N'') OR ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName]))) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END IS NULL OR [f].[NullableBool] IS NULL)) AND (CASE
    WHEN ([f0].[LastName] LIKE N'') OR ([f].[FirstName] IS NOT NULL AND ([f0].[LastName] IS NOT NULL AND (RIGHT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName]))) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END IS NOT NULL OR [f].[NullableBool] IS NOT NULL)");
        }

        public override async Task String_equals_with_trailing_whitespace_constant(bool async)
        {
            await base.String_equals_with_trailing_whitespace_constant(async);

            AssertSql(
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[LastName] LIKE N'WithoutTrailing '");
        }

        // TODO: We can rewrite equality to LIKE even when the constant pattern has no trailing whitespace
        // (just as long as it has no wildcard characters). This would change string equality in many places.
        public override Task String_equals_with_trailing_whitespace_column(bool async)
            => Assert.ThrowsAnyAsync<XunitException>(() =>
                base.String_equals_with_trailing_whitespace_and_like_wildcard(async));

        public override async Task String_equals_with_like_wildcard(bool async)
        {
            await base.String_equals_with_like_wildcard(async);

            AssertSql(
                @"SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] = N'With%Wildcard'");
        }

        // In SQL Server we work around trailing whitespace issues by using LIKE instead of the equality operator,
        // but we can't do that if there are LIKE wildcard chars present.
        public override Task String_equals_with_trailing_whitespace_and_like_wildcard(bool async)
            => Assert.ThrowsAnyAsync<XunitException>(() =>
                base.String_equals_with_trailing_whitespace_and_like_wildcard(async));

        protected override void ClearLog()
            => Fixture.TestSqlLoggerFactory.Clear();

        private void AssertSql(params string[] expected)
            => Fixture.TestSqlLoggerFactory.AssertBaseline(expected);

        public class FunkyDataQuerySqlServerFixture : FunkyDataQueryFixtureBase
        {
            public TestSqlLoggerFactory TestSqlLoggerFactory => (TestSqlLoggerFactory)ListLoggerFactory;

            protected override ITestStoreFactory TestStoreFactory => SqlServerTestStoreFactory.Instance;

            protected override bool CanExecuteQueryString => true;

            protected override QueryAsserter<FunkyDataContext> CreateQueryAsserter(
                Dictionary<Type, object> entitySorters,
                Dictionary<Type, object> entityAsserters)
                => new RelationalQueryAsserter<FunkyDataContext>(
                    CreateContext,
                    new FunkyDataData(),
                    entitySorters,
                    entityAsserters,
                    CanExecuteQueryString);
        }
    }
}
