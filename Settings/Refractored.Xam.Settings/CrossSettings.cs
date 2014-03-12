/*
 * MvxSettings:
 * Copyright (C) 2014 Refractored: 
 * 
 * Contributors:
 * http://github.com/JamesMontemagno
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * Original concept for Internal IoC came from: http://pclstorage.codeplex.com under Microsoft Public License (Ms-PL)
 * 
 */

using System;
using Refractored.Xam.Settings.Abstractions;

namespace Refractored.Xam.Settings
{
    public static class CrossSettings
    {
      static Lazy<ISettings> settings = new Lazy<ISettings>(() => CreateSettings(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

      /// <summary>
      /// The implementation of <see cref="IFileSystem"/> for the current platform
      /// </summary>
      public static ISettings Current
      {
        get
        {
          ISettings ret = settings.Value;
          if (ret == null)
          {
            throw NotImplementedInReferenceAssembly();
          }
          return ret;
        }
      }

      static ISettings CreateSettings()
      {
#if PORTABLE
        return null;
#else
        return new Settings();
#endif
      }

        internal static Exception NotImplementedInReferenceAssembly()
        {
          return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the PCLStorage NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
