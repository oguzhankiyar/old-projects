using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace BlockTetris.Strings
{
    public enum LocalizedString
    {
        AppName,
        StartPlaying,
        NewScore,
        GameOver,
        Paused,
        Point,
        Scoreboard,
        Options,
        SelectColor,
        WriteName,
        About,
        AboutText,
        Reset,
        SendFeedback,
        FeedbackTitle,
        FeedbackText,
        YourName,
        YourEmail,
        YourMessage,
        SendButton,
        CancelButton,
        PlayTimerText,
        NameError,
        MessageError,
        FeedbackSuccessful,
        FeedbackError,
        Words,
        StartButton,
        GoTutorial,
        GoStore,
        VersionText
    }

    public static class LocalizedStrings
    {
        private static ResourceLoader resourceLoader;

        static LocalizedStrings()
        {
            resourceLoader = new ResourceLoader();
        }

        public static string Get(LocalizedString resource)
        {
            return resourceLoader.GetString(resource.ToString());
        }
    }
}
