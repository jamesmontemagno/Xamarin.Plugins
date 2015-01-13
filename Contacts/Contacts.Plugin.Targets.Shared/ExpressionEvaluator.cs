//
//  Copyright 2011-2013, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Contacts.Plugin
{
  internal static class ExpressionEvaluator
  {
    public static Expression Evaluate(Expression expression, Func<Expression, bool> predicate)
    {
      HashSet<Expression> canidates = new EvaluationNominator(predicate).Nominate(expression);
      return new SubtreeEvaluator(canidates).Visit(expression);
    }

    public static Expression Evaluate(Expression expression)
    {
      return Evaluate(expression, e => e.NodeType != ExpressionType.Parameter);
    }

    private class SubtreeEvaluator
      : ExpressionVisitor
    {
      public SubtreeEvaluator(HashSet<Expression> candidate)
      {
        this.candidate = candidate;
      }

      public override Expression Visit(Expression expression)
      {
        if (expression == null)
          return null;

        if (this.candidate.Contains(expression))
          return EvaluateCandidate(expression);

        return base.Visit(expression);
      }

      private readonly HashSet<Expression> candidate;

      private Expression EvaluateCandidate(Expression expression)
      {
        if (expression.NodeType == ExpressionType.Constant)
          return expression;

        LambdaExpression lambda = Expression.Lambda(expression);
        Delegate fn = lambda.Compile();

        return Expression.Constant(fn.DynamicInvoke(null), expression.Type);
      }
    }
  }
}