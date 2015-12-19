CurrentActivity Readme

Find the most up to date information at: 
https://github.com/jamesmontemagno/Xamarin.Plugins


This plugin provides base functionality for Plugins for Xamarin to gain access to the applications main activity.

When this plugin is installed it installs a "MainApplication.cs" into the root of your application.

This file exposes an Android "Application" that registers for Activity changes.

If you already have an "Application" class please comment out this class and implement:
Application.IActivityLifecycleCallbacks on your Application.

Then set: 
CrossCurrentActivity.Current.Activity = activity;

on the: OnActivityCreated, OnActivityStarted, OnActivityResumed

Additionally:
public override void OnCreate()
{
    base.OnCreate();
    RegisterActivityLifecycleCallbacks(this);
}

public override void OnTerminate()
{
    base.OnTerminate();
    UnregisterActivityLifecycleCallbacks(this);
}
