using System;
using System.Collections.Generic;
using System.Text;
using TestApps.Portable;

namespace TestApps
{
  public static class TestStuff
  {

    public static void Test()
    {
      Int64 test1 = 10;

      TestSettings.Int64Setting = test1;
      
      if(TestSettings.Int64Setting != test1)
          throw new Exception("Int64 not saved");

      Int32 test2 = 10;

      TestSettings.Int32Setting = test2;
      if(TestSettings.Int32Setting != test2)
        throw new Exception("Int32 not saved");

      int test3 = 10;

      TestSettings.IntSetting = test3;
      if(TestSettings.IntSetting != test3)
        throw new Exception("Int not saved");

      var test4 = true;

      TestSettings.BoolSetting = test4;
      if(TestSettings.BoolSetting != test4)
        throw new Exception("Bool not saved");

      double test5 = 10;

      TestSettings.DoubleSetting = test5;
      if(TestSettings.DoubleSetting != test5)
        throw new Exception("Double not saved");

      decimal test6 = 10;

      TestSettings.DecimalSetting = test6;
      if(TestSettings.DecimalSetting != test6)
        throw new Exception("Decimal not saved");

      DateTime test7 = new DateTime(1986, 6, 25, 4, 0, 0);

      TestSettings.DateTimeSetting = test7;
      if(TestSettings.DateTimeSetting.Ticks != test7.Ticks)
        throw new Exception("DateTime not saved");
      
      Guid test8 = new Guid("EFFB4B96-92F3-4551-9732-36B11DC8B051");

      TestSettings.GuidSetting = test8;
      if(TestSettings.GuidSetting.ToString() != test8.ToString())
        throw new Exception("Guid not saved");
    
    }
  }
}
