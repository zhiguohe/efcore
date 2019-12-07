// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions
{
    public class SqlFunctionExpression : SqlExpression
    {
        public static SqlFunctionExpression CreateNiladic(
            string name,
            Type type,
            RelationalTypeMapping typeMapping)
            => new SqlFunctionExpression(
                instance: null,
                schema: null,
                name,
                niladic: true,
                arguments: null,
                builtIn: true,
                canBeNull: true,
                argumentsNullabilityPropagation: null,
                type,
                typeMapping);

        public static SqlFunctionExpression CreateNiladic(
            string schema,
            string name,
            Type type,
            RelationalTypeMapping typeMapping)
            => new SqlFunctionExpression(
                instance: null,
                schema,
                name,
                niladic: true,
                arguments: null,
                builtIn: true,
                canBeNull: true,
                argumentsNullabilityPropagation: null,
                type,
                typeMapping);

        public static SqlFunctionExpression CreateNiladic(
            SqlExpression instance,
            string name,
            Type type,
            RelationalTypeMapping typeMapping)
            => new SqlFunctionExpression(
                instance,
                schema: null,
                name,
                niladic: true,
                arguments: null,
                builtIn: true,
                canBeNull: true,
                argumentsNullabilityPropagation: null,
                type,
                typeMapping);

        public static SqlFunctionExpression Create(
            SqlExpression instance,
            string name,
            IEnumerable<SqlExpression> arguments,
            Type type,
            RelationalTypeMapping typeMapping)
            => new SqlFunctionExpression(
                instance,
                schema: null,
                name,
                niladic: false,
                arguments,
                builtIn: true,
                canBeNull: true,
                argumentsNullabilityPropagation: null,
                type,
                typeMapping);

        public static SqlFunctionExpression Create(
            string name,
            IEnumerable<SqlExpression> arguments,
            Type type,
            RelationalTypeMapping typeMapping)
            => new SqlFunctionExpression(
                instance: null,
                schema: null,
                name,
                niladic: false,
                arguments,
                builtIn: true,
                canBeNull: true,
                argumentsNullabilityPropagation: null,
                type,
                typeMapping);

        public static SqlFunctionExpression Create(
            string schema,
            string name,
            IEnumerable<SqlExpression> arguments,
            Type type,
            RelationalTypeMapping typeMapping)
            => new SqlFunctionExpression(
                instance: null,
                schema,
                name,
                niladic: false,
                arguments,
                builtIn: false,
                canBeNull: true,
                argumentsNullabilityPropagation: null,
                type,
                typeMapping);

        public SqlFunctionExpression(
            SqlExpression instance,
            string schema,
            string name,
            bool niladic,
            IEnumerable<SqlExpression> arguments,
            bool builtIn,
            Type type,
            RelationalTypeMapping typeMapping)
            : base(type, typeMapping)
        {
            Instance = instance;
            Name = name;
            Schema = schema;
            IsNiladic = niladic;
            IsBuiltIn = builtIn;
            Arguments = (arguments ?? Array.Empty<SqlExpression>()).ToList();
        }

        public static SqlFunctionExpression CreateNiladic(
            string name,
            bool canBeNull,
            Type type,
            RelationalTypeMapping typeMapping)
            => new SqlFunctionExpression(
                instance: null,
                schema: null,
                name,
                niladic: true,
                arguments: null,
                builtIn: true,
                canBeNull,
                argumentsNullabilityPropagation: null,
                type,
                typeMapping);

        public static SqlFunctionExpression CreateNiladic(
            string schema,
            string name,
            bool canBeNull,
            Type type,
            RelationalTypeMapping typeMapping)
            => new SqlFunctionExpression(
                instance: null,
                schema,
                name,
                niladic: true,
                arguments: null,
                builtIn: true,
                canBeNull,
                argumentsNullabilityPropagation: null,
                type,
                typeMapping);

        public static SqlFunctionExpression CreateNiladic(
            SqlExpression instance,
            string name,
            bool canBeNull,
            Type type,
            RelationalTypeMapping typeMapping)
            => new SqlFunctionExpression(
                instance,
                schema: null,
                name,
                niladic: true,
                arguments: null,
                builtIn: true,
                canBeNull,
                argumentsNullabilityPropagation: null,
                type,
                typeMapping);

        public static SqlFunctionExpression Create(
            SqlExpression instance,
            string name,
            IEnumerable<SqlExpression> arguments,
            bool canBeBnull,
            IEnumerable<bool> argumentsNullabilityPropagation,
            Type type,
            RelationalTypeMapping typeMapping)
            => new SqlFunctionExpression(
                instance,
                schema: null,
                name,
                niladic: false,
                arguments,
                builtIn: true,
                canBeBnull,
                argumentsNullabilityPropagation,
                type,
                typeMapping);

        public static SqlFunctionExpression Create(
            string name,
            IEnumerable<SqlExpression> arguments,
            bool canBeBnull,
            IEnumerable<bool> argumentsNullabilityPropagation,
            Type type,
            RelationalTypeMapping typeMapping)
            => new SqlFunctionExpression(
                instance: null,
                schema: null,
                name,
                niladic: false,
                arguments,
                builtIn: true,
                canBeBnull,
                argumentsNullabilityPropagation,
                type,
                typeMapping);

        public static SqlFunctionExpression Create(
            string schema,
            string name,
            IEnumerable<SqlExpression> arguments,
            bool canBeBnull,
            IEnumerable<bool> argumentsNullabilityPropagation,
            Type type,
            RelationalTypeMapping typeMapping)
            => new SqlFunctionExpression(
                instance: null,
                schema,
                name,
                niladic: false,
                arguments,
                builtIn: false,
                canBeBnull,
                argumentsNullabilityPropagation,
                type,
                typeMapping);

        public SqlFunctionExpression(
            SqlExpression instance,
            string schema,
            string name,
            bool niladic,
            IEnumerable<SqlExpression> arguments,
            bool builtIn,
            bool canBeNull,
            IEnumerable<bool> argumentsNullabilityPropagation,
            Type type,
            RelationalTypeMapping typeMapping)
            : base(type, typeMapping)
        {
            Instance = instance;
            Name = name;
            Schema = schema;
            IsNiladic = niladic;
            IsBuiltIn = builtIn;
            Arguments = (arguments ?? Array.Empty<SqlExpression>()).ToList();
            CanBeNull = canBeNull;
            ArgumentsNullabilityPropagation = (argumentsNullabilityPropagation ?? new bool[Arguments.Count]).ToList();
        }

        public virtual string Name { get; }
        public virtual string Schema { get; }
        public virtual bool IsNiladic { get; }
        public virtual bool IsBuiltIn { get; }
        public virtual IReadOnlyList<SqlExpression> Arguments { get; }
        public virtual SqlExpression Instance { get; }

        public virtual bool CanBeNull { get; }
        public IReadOnlyList<bool> ArgumentsNullabilityPropagation { get; }

        protected override Expression VisitChildren(ExpressionVisitor visitor)
        {
            var changed = false;
            var instance = (SqlExpression)visitor.Visit(Instance);
            changed |= instance != Instance;
            var arguments = new SqlExpression[Arguments.Count];
            for (var i = 0; i < arguments.Length; i++)
            {
                arguments[i] = (SqlExpression)visitor.Visit(Arguments[i]);
                changed |= arguments[i] != Arguments[i];
            }

            return changed
                ? new SqlFunctionExpression(
                    instance,
                    Schema,
                    Name,
                    IsNiladic,
                    arguments,
                    IsBuiltIn,
                    CanBeNull,
                    ArgumentsNullabilityPropagation,
                    Type,
                    TypeMapping)
                : this;
        }

        public virtual SqlFunctionExpression ApplyTypeMapping(RelationalTypeMapping typeMapping)
            => new SqlFunctionExpression(
                Instance,
                Schema,
                Name,
                IsNiladic,
                Arguments,
                IsBuiltIn,
                CanBeNull,
                ArgumentsNullabilityPropagation,
                Type,
                typeMapping ?? TypeMapping);

        public virtual SqlFunctionExpression Update(SqlExpression instance, IReadOnlyList<SqlExpression> arguments)
            => instance != Instance || !arguments.SequenceEqual(Arguments)
                ? new SqlFunctionExpression(
                    instance,
                    Schema,
                    Name,
                    IsNiladic,
                    arguments,
                    IsBuiltIn,
                    CanBeNull,
                    ArgumentsNullabilityPropagation,
                    Type,
                    TypeMapping)
                : this;

        public override void Print(ExpressionPrinter expressionPrinter)
        {
            if (!string.IsNullOrEmpty(Schema))
            {
                expressionPrinter.Append(Schema).Append(".").Append(Name);
            }
            else
            {
                if (Instance != null)
                {
                    expressionPrinter.Visit(Instance);
                    expressionPrinter.Append(".");
                }

                expressionPrinter.Append(Name);
            }

            if (!IsNiladic)
            {
                expressionPrinter.Append("(");
                expressionPrinter.VisitList(Arguments);
                expressionPrinter.Append(")");
            }
        }

        public override bool Equals(object obj)
            => obj != null
                && (ReferenceEquals(this, obj)
                    || obj is SqlFunctionExpression sqlFunctionExpression
                    && Equals(sqlFunctionExpression));

        private bool Equals(SqlFunctionExpression sqlFunctionExpression)
            => base.Equals(sqlFunctionExpression)
                && string.Equals(Name, sqlFunctionExpression.Name)
                && string.Equals(Schema, sqlFunctionExpression.Schema)
                && ((Instance == null && sqlFunctionExpression.Instance == null)
                    || (Instance != null && Instance.Equals(sqlFunctionExpression.Instance)))
                && Arguments.SequenceEqual(sqlFunctionExpression.Arguments);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(Name);
            hash.Add(IsNiladic);
            hash.Add(Schema);
            hash.Add(Instance);
            for (var i = 0; i < Arguments.Count; i++)
            {
                hash.Add(Arguments[i]);
            }

            return hash.ToHashCode();
        }
    }
}
