using System;
using NUnit.Framework;
using Plugin.Settings.Tests.Portable.Helpers;


namespace Plugin.Settings.NUnitTest
{
  [TestFixture]
  public class TestsSample
  {

    [SetUp]
    public void Setup() { }


    [TearDown]
    public void Tear() { }

    [Test]
    public void Int64()
    {
      Int64 test = 10;

      TestSettings.Int64Setting = test;
      Assert.True(TestSettings.Int64Setting == test, "Int64 not saved");
    }


    [Test]
    public void Int32()
    {
      Int32 test = 10;

      TestSettings.Int32Setting = test;
      Assert.True(TestSettings.Int32Setting == test, "Int32 not saved");
    }


    [Test]
    public void Int()
    {
      int test = 10;

      TestSettings.IntSetting = test;
      Assert.True(TestSettings.IntSetting == test, "Int not saved");
    }

    [Test]
    public void Bool()
    {
      var test = true;

      TestSettings.BoolSetting = test;
      Assert.True(TestSettings.BoolSetting == test, "Bool not saved");
    }

    [Test]
    public void Double()
    {
      double test = 10.001;

      TestSettings.DoubleSetting = test;
      Assert.True(TestSettings.DoubleSetting == test, "Double not saved");
    }

    [Test]
    public void Decimal()
    {
      decimal test = 0.099M;

      TestSettings.DecimalSetting = test;
      Assert.True(TestSettings.DecimalSetting == test, "Decimal not saved");
    }

    [Test]
    public void DateTime()
    {
      
      DateTime test = new DateTime(1986, 6, 25, 4, 0, 0);

      TestSettings.DateTimeSetting = test;
      Assert.True(TestSettings.DateTimeSetting.Value.Ticks == test.Ticks, "DateTime not saved");
    }

    [Test]
    public void Guid()
    {
      Guid test = new Guid("EFFB4B96-92F3-4551-9732-36B11DC8B051");

      TestSettings.GuidSetting = test;
      Assert.True(TestSettings.GuidSetting.ToString() == test.ToString(), "Guid not saved");
    }


    [Test]
    public void AddRemove()
    {
      TestSettings.StringSetting = "Hello World";

      TestSettings.DateTimeSetting = null;

      Assert.IsTrue(TestSettings.DateTimeSetting.HasValue, "Date wasn't set to null, it is: " + TestSettings.StringSetting);



      TestSettings.Remove("date_setting");

      Assert.IsFalse(TestSettings.DateTimeSetting.HasValue, "String should be back to default of string.empty, it is: " + TestSettings.StringSetting);
    }

    [Test]
    public void Upgrade140To150Test()
    {
      //old value was stored as a long to test
      TestSettings.AppSettings.AddOrUpdateValue("test1", (long)100);

      Assert.IsTrue(TestSettings.AppSettings.GetValueOrDefault<decimal>("test1", (decimal)101.01M) == (decimal)100M, "Decimal did not upgrade correctly");
      Assert.IsTrue(TestSettings.AppSettings.GetValueOrDefault<decimal>("test1", (decimal)101.01M) == (decimal)100M, "Decimal did not upgrade correctly");


      //new value is stored as a string via decimal
      TestSettings.AppSettings.AddOrUpdateValue("test1", (decimal)100.01M);

      Assert.IsTrue(TestSettings.AppSettings.GetValueOrDefault<decimal>("test1", (decimal)100.02M) == (decimal)100.01M, "Decimal did not upgrade correctly");


      //old value was stored as a long to test
      TestSettings.AppSettings.AddOrUpdateValue("test2", (long)100);

      Assert.IsTrue(TestSettings.AppSettings.GetValueOrDefault<double>("test2", (double)101) == (double)100, "Double did not upgrade correctly");
      Assert.IsTrue(TestSettings.AppSettings.GetValueOrDefault<double>("test2", (double)101) == (double)100, "Double did not upgrade correctly");
    
      //new value is stored as a string via decimal
      TestSettings.AppSettings.AddOrUpdateValue("test2", (double)100.01);

      Assert.IsTrue(TestSettings.AppSettings.GetValueOrDefault<double>("test2", (double)100.02) == (double)100.01, "Double did not upgrade correctly");
    }

    [Test]
    public void Upgrade140To150TestAddAndUpdate()
    {
      //old value was stored as a long to test
      TestSettings.AppSettings.AddOrUpdateValue("test1", (long)100);
      //new value is stored as a string via decimal
      TestSettings.AppSettings.AddOrUpdateValue("test1", (decimal)100.01M);

      Assert.IsTrue(TestSettings.AppSettings.GetValueOrDefault<decimal>("test1", (decimal)100.02M) == (decimal)100.01M, "Decimal did not upgrade correctly");


      //old value was stored as a long to test
      TestSettings.AppSettings.AddOrUpdateValue("test2", (long)100);
      TestSettings.AppSettings.AddOrUpdateValue("test2", (double)100.01);

      Assert.IsTrue(TestSettings.AppSettings.GetValueOrDefault<double>("test2", (double)100.02) == (double)100.01, "Double did not upgrade correctly");
    }

  }
}