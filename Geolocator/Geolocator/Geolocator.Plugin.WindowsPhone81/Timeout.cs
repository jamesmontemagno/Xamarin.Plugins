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
using System.Threading;
using System.Threading.Tasks;

namespace Geolocator.Plugin
{
  internal class Timeout
  {
    

    public Timeout(int timeout, Action timesup)
    {
      if (timeout == Infite)
        return; // nothing to do
      if (timeout < 0)
          throw new ArgumentOutOfRangeException("timeoutMilliseconds");
      if (timesup == null)
        throw new ArgumentNullException("timesup");
      
      Task.Delay(TimeSpan.FromMilliseconds(timeout), this.canceller.Token)
          .ContinueWith(t =>
          {
              if (!t.IsCanceled)
                  timesup();
          });
    }

    public void Cancel()
    {
      this.canceller.Cancel();
    }

    private readonly CancellationTokenSource canceller = new CancellationTokenSource();

    public const int Infite = -1;
  }
}
