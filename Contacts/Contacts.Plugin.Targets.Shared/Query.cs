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

namespace Plugin.Contacts
{
  internal class Query<T>
    : IOrderedQueryable<T>
  {
    public Query(IQueryProvider provider)
    {
      this.provider = provider;
      this.expression = Expression.Constant(this);
    }

    public Query(IQueryProvider provider, Expression expression)
    {
      this.provider = provider;
      this.expression = expression;
    }

    public IEnumerator<T> GetEnumerator()
    {
      var enumerable = ((IEnumerable<T>)this.provider.Execute(this.expression));
      return (enumerable ?? Enumerable.Empty<T>()).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public Type ElementType
    {
      get { return typeof(T); }
    }

    public Expression Expression
    {
      get { return this.expression; }
    }

    public IQueryProvider Provider
    {
      get { return this.provider; }
    }

    private readonly Expression expression;
    private readonly IQueryProvider provider;
  }
}