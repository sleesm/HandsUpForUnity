using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FantomLib;

public class AndroidTTSManager : MonoBehaviour
{
    // Start is called before the first frame update
    //Maily 'TextToSpeechController.StartSpeech', '.Locale' is called.
    public TextToSpeechController textToSpeechControl;

    public Text displayText;
    /*    public Text statusText;
        //public Animator statusAnimator;

        public Text speedText;
        public Text pitchText;

        public Dropdown localeDropdown;*/
    public bool useAndroidLocale = true;

    //Running messages
    //Message when speech start.
    public LocalizeString startMessage = new LocalizeString(SystemLanguage.English,
        new List<LocalizeString.Data>()
        {
            new LocalizeString.Data(SystemLanguage.English, "Speaking"),    //default language
            new LocalizeString.Data(SystemLanguage.Korean, "말하기 시작"),
        });

    //Message when speech finished.
    public LocalizeString doneMessage = new LocalizeString(SystemLanguage.English,
        new List<LocalizeString.Data>()
        {
            new LocalizeString.Data(SystemLanguage.English, "Finished"),    //default language
            new LocalizeString.Data(SystemLanguage.Korean, "말하기 끝남"),
        });

    //Message when speech interrupted.
    public LocalizeString stopMessage = new LocalizeString(SystemLanguage.English,
        new List<LocalizeString.Data>()
        {
            new LocalizeString.Data(SystemLanguage.English, "Interrupted"),    //default language
            new LocalizeString.Data(SystemLanguage.Korean, "말하기 멈춤"),
        });


    //Initialization/error messages
    //Message when TTS available 
    public LocalizeString ttsAvailableMessage = new LocalizeString(SystemLanguage.English,
        new List<LocalizeString.Data>()
        {
            new LocalizeString.Data(SystemLanguage.English, "Text To Speech is available."),    //default language
            new LocalizeString.Data(SystemLanguage.Korean, "TTS가 가능합니다."),
        });

    //Message when TTS initialization error
    public LocalizeString ttsInitializationError = new LocalizeString(SystemLanguage.English,
        new List<LocalizeString.Data>()
        {
            new LocalizeString.Data(SystemLanguage.English, "Failed to initialize Text To Speech."),    //default language
            new LocalizeString.Data(SystemLanguage.Korean, "TTS를 초기화하는데 실패했습니다."),
        });

    //Message when TTS locale error
    public LocalizeString ttsLocaleError = new LocalizeString(SystemLanguage.English,
        new List<LocalizeString.Data>()
        {
            new LocalizeString.Data(SystemLanguage.English, "It is a language that can not be used."),    //default language
            new LocalizeString.Data(SystemLanguage.Korean, "사용할 수 없는 언어입니다."),
        });

    //TTS
    //public Button ttsSearchButton;  //Market Search
    public Button ttsButton;        //Google TTS
    //public Button ttsButtonN2;      //KDDI N2 TTS

    //public AppInstallCheckController OnGooglePlayCheck;
    public AppInstallCheckController OnGoogleTTSCheck;
    //public AppInstallCheckController OnKddiTTSCheck;


    //==========================================================
    //Check the installation of TTS
    //TTSのインストールをチェックする
    //AppInstallCheckController callback handlers (using Controller sample)

    int isInstalledGooglePlay = -1;     //checked flag


    public void OnGoogleTTSInstalled(string appName, int verCode, string verName)
    {
        //DisplayText(appName + " ver." + verName + " is installed.", true, true);
        if (ttsButton != null)
            ttsButton.interactable = false;
    }

    public void OnGoogleTTSNotInstalled()
    {
        //DisplayText("Google TTS is not installed.", true, true);
        if (ttsButton != null)
            ttsButton.interactable = true;
    }


    //==========================================================
    //Display and edit text string

    //Display text string (and for reading)
    public void DisplayText(object message, bool add = false, bool newline = false)
    {
        if (displayText != null)
        {
            if (add)
                displayText.text += message + (newline ? "\n" : "");
            else
                displayText.text = message + (newline ? "\n" : "");
        }
    }

    //==========================================================
    //Example Text To Speech (Callback handlers)

    //TextToSpeechController.StartSpeech call
    public void StartTTS(string message)
    {
        if (textToSpeechControl != null)
            textToSpeechControl.StartSpeech(message);
    }

    //Receive status message from callback
    public void OnStatus(string message)
    {
        if (textToSpeechControl != null)
        {
        }

        if (isInstalledGooglePlay == -1)    //at first time only
        {
            if (OnGoogleTTSCheck != null)
                OnGoogleTTSCheck.CheckInstall();
        }
    }
}
