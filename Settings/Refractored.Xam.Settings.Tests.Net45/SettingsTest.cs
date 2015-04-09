using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refractored.Xam.Settings.Tests.Portable.Helpers;

namespace Refractored.Xam.Settings.Tests.Net45
{
    [TestClass]
    public class SettingsTest
    {
        [TestMethod]
        public void Int64()
        {
            Int64 test = 10;

            TestSettings.Int64Setting = test;
            Assert.IsTrue(TestSettings.Int64Setting == test, "Int64 not saved");
        }


        [TestMethod]
        public void Int32()
        {
            Int32 test = 10;

            TestSettings.Int32Setting = test;
            Assert.IsTrue(TestSettings.Int32Setting == test, "Int32 not saved");
        }


        [TestMethod]
        public void Int()
        {
            int test = 10;

            TestSettings.IntSetting = test;
            Assert.IsTrue(TestSettings.IntSetting == test, "Int not saved");
        }

        [TestMethod]
        public void Bool()
        {
            var test = true;

            TestSettings.BoolSetting = test;
            Assert.IsTrue(TestSettings.BoolSetting == test, "Bool not saved");
        }

        [TestMethod]
        public void Double()
        {
            double test = 10.001;

            TestSettings.DoubleSetting = test;
            Assert.IsTrue(TestSettings.DoubleSetting == test, "Double not saved");
        }

        [TestMethod]
        public void Decimal()
        {
            decimal test = 0.099M;

            TestSettings.DecimalSetting = test;
            Assert.IsTrue(TestSettings.DecimalSetting == test, "Decimal not saved");
        }

        [TestMethod]
        public void DateTime()
        {

            DateTime test = new DateTime(1986, 6, 25, 4, 0, 0);

            TestSettings.DateTimeSetting = test;
            Assert.IsTrue(TestSettings.DateTimeSetting.Value.Ticks == test.Ticks, "DateTime not saved");
        }

        [TestMethod]
        public void Guid()
        {
            Guid test = new Guid("EFFB4B96-92F3-4551-9732-36B11DC8B051");

            TestSettings.GuidSetting = test;
            Assert.IsTrue(TestSettings.GuidSetting.ToString() == test.ToString(), "Guid not saved");
        }


        [TestMethod]
        public void AddRemove()
        {
            TestSettings.DateTimeSetting = null;

            Assert.IsFalse(TestSettings.DateTimeSetting.HasValue, "Date wasn't set to null, it is: " + TestSettings.DateTimeSetting);



            TestSettings.Remove("date_setting");

            Assert.IsFalse(TestSettings.DateTimeSetting.HasValue, "String should be back to default of string.empty, it is: " + TestSettings.StringSetting);
        }

        [TestMethod]
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

        [TestMethod]
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
