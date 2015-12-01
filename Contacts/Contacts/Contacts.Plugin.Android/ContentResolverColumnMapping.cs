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

namespace Plugin.Contacts
{
  internal class ContentResolverColumnMapping
  {
    public ContentResolverColumnMapping(string column, Type returnType)
    {
      if (returnType == null)
        throw new ArgumentNullException("returnType");

      if (column != null)
        Columns = new[] { column };

      ReturnType = returnType;
    }

    public ContentResolverColumnMapping(string column, Type returnType, Func<object, object> toQueryable, Func<object, object> fromQueryable)
      : this(column, returnType)
    {
      if (toQueryable == null)
        throw new ArgumentNullException("toQueryable");
      if (fromQueryable == null)
        throw new ArgumentNullException("fromQueryable");

      ValueToQueryable = toQueryable;
      QueryableToValue = fromQueryable;
    }

    public ContentResolverColumnMapping(string[] columns, Type returnType)
    {
      if (returnType == null)
        throw new ArgumentNullException("returnType");

      Columns = columns;
      ReturnType = returnType;
    }

    public ContentResolverColumnMapping(string[] columns, Type returnType, Func<object, object> toQueryable, Func<object, object> fromQueryable)
      : this(columns, returnType)
    {
      if (toQueryable == null)
        throw new ArgumentNullException("toQueryable");
      if (fromQueryable == null)
        throw new ArgumentNullException("fromQueryable");

      ValueToQueryable = toQueryable;
      QueryableToValue = fromQueryable;
    }

    public string[] Columns
    {
      get;
      private set;
    }

    public Func<object, object> ValueToQueryable
    {
      get;
      private set;
    }

    public Func<object, object> QueryableToValue
    {
      get;
      private set;
    }

    public Type ReturnType
    {
      get;
      private set;
    }
  }
}