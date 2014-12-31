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
using System.Reflection;
using System.Text;
using Uri = Android.Net.Uri;

namespace Contacts.Plugin
{
  internal interface ITableFinder
  {
    /// <summary>
    /// Gets the default table (content hierarchy root).
    /// </summary>
    Uri DefaultTable { get; }

    TableFindResult Find(Expression expression);

    /// <summary>
    /// Gets whether the <paramref name="type"/> is a supported type for this finder.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the <paramref name="type"/> is supported, <c>false</c> otherwise.</returns>
    bool IsSupportedType(Type type);

    /// <summary>
    /// Gets the Android column name for the model's member.
    /// </summary>
    /// <param name="memberInfo">The <see cref="MemberInfo"/> for the model's member.</param>
    /// <returns>Android column name for the model's member, <c>null</c> if unknown.</returns>
    ContentResolverColumnMapping GetColumn(MemberInfo memberInfo);
  }

  internal class TableFindResult
  {
    internal TableFindResult(Uri table, string mimeType)
    {
      Table = table;
      MimeType = mimeType;
    }

    public Uri Table
    {
      get;
      private set;
    }

    public string MimeType
    {
      get;
      private set;
    }
  }
}