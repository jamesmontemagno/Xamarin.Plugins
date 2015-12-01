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
using Android.Content;
using Android.Database;

namespace Plugin.Contacts
{
  internal class ProjectionReader<T>
    : IEnumerable<T>
  {
    internal ProjectionReader(ContentResolver content, ContentQueryTranslator translator, Func<ICursor, int, T> selector)
    {
      this.content = content;
      this.translator = translator;
      this.selector = selector;
    }

    public IEnumerator<T> GetEnumerator()
    {
      string[] projections = null;
      if (this.translator.Projections != null)
      {
        projections = this.translator.Projections
                .Where(p => p.Columns != null)
                .SelectMany(t => t.Columns)
                .ToArray();

        if (projections.Length == 0)
          projections = null;
      }

      ICursor cursor = null;
      try
      {

        cursor = content.Query(translator.Table, projections,
                                translator.QueryString, translator.ClauseParameters, translator.SortString);

        while (cursor.MoveToNext())
        {
          int colIndex = cursor.GetColumnIndex(projections[0]);
          yield return this.selector(cursor, colIndex);
        }
      }
      finally
      {
        if (cursor != null)
          cursor.Close();
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private readonly ContentResolver content;
    private readonly ContentQueryTranslator translator;
    private readonly Func<ICursor, int, T> selector;
  }
}