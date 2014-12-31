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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Android.Content;
using Android.Content.Res;
using Android.Database;

namespace Contacts.Plugin
{
  internal abstract class ContentQueryProvider
    : IQueryProvider
  {
    internal ContentQueryProvider(ContentResolver content, Resources resources, ITableFinder tableFinder)
    {
      this.content = content;
      this.resources = resources;
      this.tableFinder = tableFinder;
    }

    public ITableFinder TableFinder
    {
      get { return this.tableFinder; }
    }

    protected readonly ContentResolver content;
    protected readonly Resources resources;
    private readonly ITableFinder tableFinder;

    IQueryable IQueryProvider.CreateQuery(Expression expression)
    {
      throw new NotImplementedException();
    }

    object IQueryProvider.Execute(Expression expression)
    {
      var translator = new ContentQueryTranslator(this, this.tableFinder);
      expression = translator.Translate(expression);

      if (translator.IsCount || translator.IsAny)
      {
        ICursor cursor = null;
        try
        {
          string[] projections = (translator.Projections != null)
                                  ? translator.Projections
                                      .Where(p => p.Columns != null)
                                      .SelectMany(t => t.Columns)
                                      .ToArray()
                                  : null;

          cursor = this.content.Query(translator.Table, projections, translator.QueryString,
                                          translator.ClauseParameters, translator.SortString);

          if (translator.IsCount)
            return cursor.Count;
          else
            return (cursor.Count > 0);
        }
        finally
        {
          if (cursor != null)
            cursor.Close();
        }
      }

      IQueryable q = GetObjectReader(translator).AsQueryable();
      //IQueryable q = GetObjectReader (null).AsQueryable();

      expression = ReplaceQueryable(expression, q);

      if (expression.Type.IsGenericType && expression.Type.GetGenericTypeDefinition() == typeof(IOrderedQueryable<>))
        return q.Provider.CreateQuery(expression);
      else
        return q.Provider.Execute(expression);
    }

    protected abstract IEnumerable GetObjectReader(ContentQueryTranslator translator);

    IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
    {
      return new Query<TElement>(this, expression);
    }

    TResult IQueryProvider.Execute<TResult>(Expression expression)
    {
      return (TResult)((IQueryProvider)this).Execute(expression);
    }

    private Expression ReplaceQueryable(Expression expression, object value)
    {
      MethodCallExpression mc = expression as MethodCallExpression;
      if (mc != null)
      {
        Expression[] args = mc.Arguments.ToArray();
        Expression narg = ReplaceQueryable(mc.Arguments[0], value);
        if (narg != args[0])
        {
          args[0] = narg;
          return Expression.Call(mc.Method, args);
        }
        else
          return mc;
      }

      ConstantExpression c = expression as ConstantExpression;
      if (c != null && c.Type.GetInterfaces().Contains(typeof(IQueryable)))
        return Expression.Constant(value);

      return expression;
    }
  }
}