using System;
using NUnit.Framework;
using Refractored.Xam.Settings.Tests.Portable.Helpers;


namespace Refractored.Xam.Settings.NUnitTest
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
      double test = 10;

      TestSettings.DoubleSetting = test;
      Assert.True(TestSettings.DoubleSetting == test, "Double not saved");
    }

    [Test]
    public void Decimal()
    {
      decimal test = 10;

      TestSettings.DecimalSetting = test;
      Assert.True(TestSettings.DecimalSetting == test, "Decimal not saved");
    }

    [Test]
    public void DateTime()
    {
      
      DateTime test = new DateTime(1986, 6, 25, 4, 0, 0);

      TestSettings.DateTimeSetting = test;
      Assert.True(TestSettings.DateTimeSetting.Ticks == test.Ticks, "DateTime not saved");
    }

    [Test]
    public void Guid()
    {
      Guid test = new Guid("EFFB4B96-92F3-4551-9732-36B11DC8B051");

      TestSettings.GuidSetting = test;
      Assert.True(TestSettings.GuidSetting.ToString() == test.ToString(), "Guid not saved");
    }

  }
}