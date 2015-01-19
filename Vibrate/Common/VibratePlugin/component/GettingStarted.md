# Getting Started with Vibrate Plugin

**ANDROID Specific**
Please ensure you have the VIBRATE persmission enabled:

```
<uses-permission android:name="android.permission.VIBRATE" />
```

#### API Usage

To gain access to the Vibrate class simply use this method:

```
var v = CrossVibrate.Current;
```

#### Methods

```
 CrossVibrate.Current.Vibrate(int milliseconds = 500);
```

Vibrate device for specified amount of time. 500 is the default and will vibrate for 500 milliseconds.

**iOS Specific**
There is no API to vibrate for a specific amount of time, so it will vibrate for the default no matter what.
