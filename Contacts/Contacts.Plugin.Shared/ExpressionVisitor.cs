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
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Contacts.Plugin
{
  internal abstract class ExpressionVisitor
  {
    public virtual Expression Visit(Expression expression)
    {
      if (expression == null)
        throw new ArgumentNullException("expression");

      switch (expression.NodeType)
      {
        case ExpressionType.Negate:
        case ExpressionType.NegateChecked:
        case ExpressionType.Not:
        case ExpressionType.Convert:
        case ExpressionType.ConvertChecked:
        case ExpressionType.ArrayLength:
        case ExpressionType.Quote:
        case ExpressionType.TypeAs:
        case ExpressionType.UnaryPlus:
          return VisitUnary((UnaryExpression)expression);
        case ExpressionType.Add:
        case ExpressionType.AddChecked:
        case ExpressionType.Subtract:
        case ExpressionType.SubtractChecked:
        case ExpressionType.Multiply:
        case ExpressionType.MultiplyChecked:
        case ExpressionType.Divide:
        case ExpressionType.Modulo:
        case ExpressionType.Power:
        case ExpressionType.And:
        case ExpressionType.AndAlso:
        case ExpressionType.Or:
        case ExpressionType.OrElse:
        case ExpressionType.LessThan:
        case ExpressionType.LessThanOrEqual:
        case ExpressionType.GreaterThan:
        case ExpressionType.GreaterThanOrEqual:
        case ExpressionType.Equal:
        case ExpressionType.NotEqual:
        case ExpressionType.Coalesce:
        case ExpressionType.ArrayIndex:
        case ExpressionType.RightShift:
        case ExpressionType.LeftShift:
        case ExpressionType.ExclusiveOr:
          return VisitBinary((BinaryExpression)expression);
        case ExpressionType.TypeIs:
          return VisitTypeIs((TypeBinaryExpression)expression);
        case ExpressionType.Conditional:
          return VisitConditional((ConditionalExpression)expression);
        case ExpressionType.Constant:
          return VisitConstant((ConstantExpression)expression);
        case ExpressionType.Parameter:
          return VisitParameter((ParameterExpression)expression);
        case ExpressionType.MemberAccess:
          return VisitMemberAccess((MemberExpression)expression);
        case ExpressionType.Call:
          return VisitMethodCall((MethodCallExpression)expression);
        case ExpressionType.Lambda:
          return VisitLambda((LambdaExpression)expression);
        case ExpressionType.New:
          return VisitNew((NewExpression)expression);
        case ExpressionType.NewArrayInit:
        case ExpressionType.NewArrayBounds:
          return VisitNewArray((NewArrayExpression)expression);
        case ExpressionType.Invoke:
          return VisitInvocation((InvocationExpression)expression);
        default:
          throw new ArgumentException(string.Format("Unhandled expression type: '{0}'", expression.NodeType));
      }
    }

    protected virtual MemberBinding VisitBinding(MemberBinding binding)
    {
      switch (binding.BindingType)
      {
        case MemberBindingType.Assignment:
          return VisitMemberAssignment((MemberAssignment)binding);
        default:
          throw new ArgumentException(string.Format("Unhandled binding type '{0}'", binding.BindingType));
      }
    }

    protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
    {
      Expression[] args;
      if (VisitExpressionList(initializer.Arguments, out args))
        return Expression.ElementInit(initializer.AddMethod, args);

      return initializer;
    }

    protected virtual Expression VisitUnary(UnaryExpression unary)
    {
      Expression e = Visit(unary.Operand);
      if (e != unary.Operand)
        return Expression.MakeUnary(unary.NodeType, e, unary.Type, unary.Method);

      return unary;
    }

    protected virtual Expression VisitBinary(BinaryExpression binary)
    {
      Expression left = Visit(binary.Left);
      Expression right = Visit(binary.Right);

      LambdaExpression conv = null;
      if (binary.Conversion != null)
        conv = (LambdaExpression)Visit(binary.Conversion);

      if (left != binary.Left || right != binary.Right || conv != binary.Conversion)
        return Expression.MakeBinary(binary.NodeType, left, right, binary.IsLiftedToNull, binary.Method, conv);

      return binary;
    }

    protected virtual Expression VisitTypeIs(TypeBinaryExpression type)
    {
      Expression e = Visit(type.Expression);
      if (e != type.Expression)
        return Expression.TypeIs(e, type.TypeOperand);

      return type;
    }

    protected virtual Expression VisitConstant(ConstantExpression constant)
    {
      return constant;
    }

    protected virtual Expression VisitConditional(ConditionalExpression conditional)
    {
      Expression test = Visit(conditional.Test);
      Expression ifTrue = Visit(conditional.IfTrue);
      Expression ifFalse = Visit(conditional.IfFalse);

      if (test != conditional.Test || ifTrue != conditional.IfTrue || ifFalse != conditional.IfFalse)
        return Expression.Condition(test, ifTrue, ifFalse);

      return conditional;
    }

    protected virtual Expression VisitParameter(ParameterExpression parameter)
    {
      return parameter;
    }

    protected virtual Expression VisitMemberAccess(MemberExpression member)
    {
      Expression e = Visit(member.Expression);
      if (e != member.Expression)
        return Expression.MakeMemberAccess(e, member.Member);

      return member;
    }

    protected bool VisitExpressionList(IEnumerable<Expression> expressions, out Expression[] newExpressions)
    {
      Expression[] args = expressions.ToArray();

      newExpressions = new Expression[args.Length];

      bool changed = false;

      for (int i = 0; i < args.Length; ++i)
      {
        Expression original = args[i];
        Expression current = Visit(original);

        newExpressions[i] = current;

        if (original != current)
          changed = true;
      }

      return changed;
    }

    protected virtual Expression VisitMethodCall(MethodCallExpression methodCall)
    {
      bool changed = false;

      Expression obj = null;
      if (methodCall.Object != null)
      {
        obj = Visit(methodCall.Object);
        changed = (obj != methodCall.Object);
      }

      Expression[] args;
      changed = VisitExpressionList(methodCall.Arguments, out args) || changed;

      if (changed)
        return Expression.Call(obj, methodCall.Method, args);

      return methodCall;
    }

    protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
    {
      Expression e = Visit(assignment.Expression);
      if (e != assignment.Expression)
        return Expression.Bind(assignment.Member, e);

      return assignment;
    }

    protected virtual Expression VisitLambda(LambdaExpression lambda)
    {
      Expression body = Visit(lambda.Body);
      bool changed = (body != lambda.Body);

      Expression[] parameters;
      changed = VisitExpressionList(lambda.Parameters.Cast<Expression>(), out parameters) || changed;

      if (changed)
        return Expression.Lambda(body, parameters.Cast<ParameterExpression>().ToArray());

      return lambda;
    }

    protected virtual Expression VisitNew(NewExpression nex)
    {
      Expression[] args;
      if (VisitExpressionList(nex.Arguments, out args))
        return Expression.New(nex.Constructor, args, nex.Members);

      return nex;
    }

    protected virtual Expression VisitNewArray(NewArrayExpression newArray)
    {
      Expression[] args;
      if (VisitExpressionList(newArray.Expressions, out args))
        return Expression.NewArrayInit(newArray.Type, args);

      return newArray;
    }

    protected virtual Expression VisitInvocation(InvocationExpression invocation)
    {
      Expression[] args;
      bool changed = VisitExpressionList(invocation.Arguments, out args);

      Expression e = Visit(invocation.Expression);
      changed = (e != invocation.Expression) || changed;

      if (changed)
        return Expression.Invoke(e, args);

      return invocation;
    }
  }
}