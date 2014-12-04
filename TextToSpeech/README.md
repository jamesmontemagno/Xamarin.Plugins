## Text To Speech

Simple but elegant way of performing Text To Speech across iOS, Android, and Windows Phone Project.

Preview: http://screencast.com/t/voW1P48Ka

### Features
* Speak back text
* Pitch
* Volume
* Speak Rate
* Locale/Language of Speech
* Decide to queue speech block


### Setup
* Available on NuGet:
* Xamarin.Forms or Traditional: Soon
* Install into your PCL project and Client projects.

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
/// <param name="locale">Locale of voice</param>
/// <param name="pitch">Pitch of voice (All 0.0 - 2.0f)</param>
/// <param name="speakRate">Speak Rate of voice (All) (0.0 - 2.0f)</param>
/// <param name="volume">Volume of voice (iOS/WP) (0.0-1.0)</param>
public async void Speak(string text, bool queue = false, string locale = null, float? pitch = null, float? speakRate = null, float? volume = null)
```  

#### Implementation

* iOS: AVSpeechSynthesizer
* Android: Android.Speech.Tts.TextToSpeech
* Windows Phone: SpeechSynthesizer + Ssml support for advanced playback

#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Licensed under main repo license
