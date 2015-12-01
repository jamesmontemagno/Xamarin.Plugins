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

namespace Plugin.Contacts
{
  internal class MemberExpressionFinder
    : ExpressionVisitor
  {
    internal MemberExpressionFinder(ITableFinder tableFinder)
    {
      if (tableFinder == null)
        throw new ArgumentNullException("tableFinder");

      this.tableFinder = tableFinder;
    }

    private readonly List<MemberExpression> expressions = new List<MemberExpression>();
    private readonly ITableFinder tableFinder;


    protected override Expression VisitMemberAccess(MemberExpression member)
    {
      if (this.tableFinder.IsSupportedType(member.Member.DeclaringType))
        this.expressions.Add(member);

      return base.VisitMemberAccess(member);
    }

    internal static List<MemberExpression> Find(Expression expression, ITableFinder tableFinder)
    {
      if (expression == null)
        throw new ArgumentNullException("expression");
      if (tableFinder == null)
        throw new ArgumentNullException("tableFinder");

      var finder = new MemberExpressionFinder(tableFinder);
      finder.Visit(expression);

      return finder.expressions;
    }
  }
}