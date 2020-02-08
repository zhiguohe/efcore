// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore.TestUtilities
{
    public class ExpectedQueryRewritingVisitor : ExpressionVisitor
    {
        private static readonly MethodInfo _maybeMethod
            = typeof(ExpectedQueryRewritingVisitor).GetMethod(nameof(ExpectedQueryRewritingVisitor.Maybe));

        private static readonly MethodInfo _maybeScalarMethod
            = typeof(ExpectedQueryRewritingVisitor).GetMethod(nameof(ExpectedQueryRewritingVisitor.MaybeScalar));

        private static readonly MethodInfo _maybeScalar2Method
            = typeof(ExpectedQueryRewritingVisitor).GetMethod(nameof(ExpectedQueryRewritingVisitor.MaybeScalar2));


        public ExpectedQueryRewritingVisitor()
        {
        }

        public static TResult Maybe<TCaller, TResult>(TCaller caller, Func<TCaller, TResult> expression)
            where TResult : class
        {
            var result = caller == null ? null : expression(caller);

            return result;
        }

        public static TResult? MaybeScalar<TCaller, TResult>(TCaller caller, Func<TCaller, TResult?> expression)
            where TResult : struct
        {
            var result = caller == null ? null : expression(caller);

            return result;
        }

        public static TResult? MaybeScalar2<TCaller, TResult>(TCaller caller, Func<TCaller, TResult> expression)
            where TResult : struct
        {
            var result = caller == null ? null : (TResult?)expression(caller);

            return result;
        }

        protected override Expression VisitMember(MemberExpression memberExpression)
        {
            if ((memberExpression.Type.IsNullableType()
                || memberExpression.Type == typeof(bool))
                && memberExpression.Expression != null)
            {
                var expression = Visit(memberExpression.Expression);

                var lambdaParameter = Expression.Parameter(expression.Type, "x");
                var lambda = Expression.Lambda(memberExpression.Update(lambdaParameter), lambdaParameter);

                var method = memberExpression.Type.IsNullableValueType()
                    ? _maybeScalarMethod.MakeGenericMethod(expression.Type, memberExpression.Type.UnwrapNullableType())
                    : memberExpression.Type == typeof(bool)
                        ? _maybeScalar2Method.MakeGenericMethod(expression.Type, memberExpression.Type)
                        : _maybeMethod.MakeGenericMethod(expression.Type, memberExpression.Type);

                var result = Expression.Call(method, expression, lambda);

                return memberExpression.Type != typeof(bool)
                    ? (Expression)result
                    : Expression.Equal(
                        result,
                        Expression.Constant(true, typeof(bool?)));
            }

            return base.VisitMember(memberExpression);
        }

        //protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        //{
        //    if (methodCallExpression.Method.DeclaringType == typeof(Enumerable)
        //        && methodCallExpression.Method.IsStatic
        //        && methodCallExpression.Arguments.Count > 0
        //        && (methodCallExpression.Type.IsNullableType() || methodCallExpression.Type == typeof(bool)))
        //    {
        //        var arguments = new List<Expression>();
        //        foreach (var argument in methodCallExpression.Arguments)
        //        {
        //            var newArgument = Visit(argument);


        //            arguments.Add(newArgument);
        //        }

        //        var lambdaParameter = Expression.Parameter(arguments[0].Type, "x");
        //        var updatedMethod = methodCallExpression.Update(
        //            methodCallExpression.Object,
        //            new[] { lambdaParameter }.Concat(arguments.Skip(1)));

        //        var lambda = Expression.Lambda(updatedMethod, lambdaParameter);

        //        var method = methodCallExpression.Type.IsNullableValueType()
        //            ? _maybeScalarMethod.MakeGenericMethod(arguments[0].Type, methodCallExpression.Type.UnwrapNullableType())
        //            : methodCallExpression.Type == typeof(bool)
        //                ? _maybeScalar2Method.MakeGenericMethod(arguments[0].Type, methodCallExpression.Type)
        //                : _maybeMethod.MakeGenericMethod(arguments[0].Type, methodCallExpression.Type);

        //        var result = Expression.Call(method, arguments[0], lambda);

        //        return methodCallExpression.Type != typeof(bool)
        //            ? (Expression)result
        //            : Expression.Equal(
        //                result,
        //                Expression.Constant(true, typeof(bool?)));
        //    }

        //    if (methodCallExpression.Object != null)
        //    {
        //        if (methodCallExpression.Type.IsNullableType()
        //            || methodCallExpression.Type == typeof(bool))
        //        {
        //            var callerExpression = Visit(methodCallExpression.Object);
        //            var arguments = new List<Expression>();
        //            foreach (var argument in methodCallExpression.Arguments)
        //            {
        //                arguments.Add(Visit(argument));
        //            }

        //            var lambdaParameter = Expression.Parameter(callerExpression.Type, "x");
        //            var lambda = Expression.Lambda(methodCallExpression.Update(lambdaParameter, arguments), lambdaParameter);

        //            var method = methodCallExpression.Type.IsNullableValueType()
        //                ? _maybeScalarMethod.MakeGenericMethod(callerExpression.Type, methodCallExpression.Type.UnwrapNullableType())
        //                : methodCallExpression.Type == typeof(bool)
        //                    ? _maybeScalar2Method.MakeGenericMethod(callerExpression.Type, methodCallExpression.Type)
        //                    : _maybeMethod.MakeGenericMethod(callerExpression.Type, methodCallExpression.Type);

        //            var result = Expression.Call(method, callerExpression, lambda);

        //            return methodCallExpression.Type != typeof(bool)
        //                ? (Expression)result
        //                : Expression.Equal(
        //                    result,
        //                    Expression.Constant(true, typeof(bool?)));
        //        }
        //    }

        //    return base.VisitMethodCall(methodCallExpression);
        //}
    }
}
