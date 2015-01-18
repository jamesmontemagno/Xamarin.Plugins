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
using System.Threading.Tasks;

namespace Geolocator.Plugin
{
  internal class Timeout
  {
    

    public Timeout(int timeout, Action timesup)
    {
      if (timeout < 0)
        throw new ArgumentOutOfRangeException("timeout");
      if (timesup == null)
        throw new ArgumentNullException("timesup");

      this.timeout = TimeSpan.FromMilliseconds(timeout);
      this.timesup = timesup;

      Task.Factory.StartNew(Runner, TaskCreationOptions.LongRunning);
    }

    public void Cancel()
    {
      this.canceled = true;
    }

    private readonly TimeSpan timeout;
    private readonly Action timesup;
    private volatile bool canceled;

    private void Runner()
    {
      DateTime start = DateTime.Now;
      while (!this.canceled)
      {
        if (DateTime.Now - start < this.timeout)
        {
          Task.Delay(1).Wait();
          continue;
        }

        this.timesup();
        return;
      }
    }


    public const int Infite = -1;
  }
}
