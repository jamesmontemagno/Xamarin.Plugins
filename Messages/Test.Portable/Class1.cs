using Refractored.Xam.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Portable
{
    public static class Class1
    {
      public static void ShowMessage(string text)
      {
        var message = new Messages();
        message.ShowMessage(text);
      }
    }
}
