# Getting Started with Text To Speech Plugin

### Usage

**Simple Text**
```
CrossTextToSpeech.Current.Speak("Text to speak");
```

**Advanced speech API**
```
/// <summary>
/// Speack back text
/// </summary>
/// <param name="text">Text to speak</param>
/// <param name="queue">If you want to chain together speak command or cancel current</param>
/// <param name="crossLocale">Locale of voice</param>
/// <param name="pitch">Pitch of voice (All 0.0 - 2.0f)</param>
/// <param name="speakRate">Speak Rate of voice (All) (0.0 - 2.0f)</param>
/// <param name="volume">Volume of voice (iOS/WP) (0.0-1.0)</param>
public async void Speak(string text, bool queue = false, CrossLocale crossLocale = null, float? pitch = null, float? speakRate = null, float? volume = null)
```  

**CrossLocale**
I developed the CrossLocale struct mostly to support Android, but is nice because I added a Display Name.

You can query a list of current support CrossLocales on the device:

```
/// <summary>
/// Get all installed and valide lanaguages
/// </summary>
/// <returns>List of CrossLocales</returns>
public IEnumerable<CrossLocale> GetInstalledLanguages()
```

Each local has the Language and Display Name. The Country code is only used in Android. If you pass in null to Speak it will use the default.

#### Implementation

* iOS: AVSpeechSynthesizer
* Android: Android.Speech.Tts.TextToSpeech
* Windows Phone: SpeechSynthesizer + Ssml support for advanced playback