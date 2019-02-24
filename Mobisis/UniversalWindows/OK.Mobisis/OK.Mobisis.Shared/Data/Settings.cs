using System;
using System.Collections.Generic;
using System.Text;

namespace OK.Mobisis.Data
{
    public class Settings
    {
        public string ApiUsername { get; set; }
        public string ApiPassword { get; set; }
        public double MidtermFactor { get; set; }
        public double FinalFactor { get; set; }
        public double FF_Limit { get; set; }
        public double FD_Limit { get; set; }
        public double DD_Limit { get; set; }
        public double DC_Limit { get; set; }
        public double CC_Limit { get; set; }
        public double CB_Limit { get; set; }
        public double BB_Limit { get; set; }
        public double BA_Limit { get; set; }
        public double AA_Limit { get; set; }
        public bool NotificationState { get; set; }


        public Settings()
        {
#if WINDOWS_PHONE_APP
            ApiUsername = "OK.Mobisis.WindowsPhone";
            ApiPassword = "3f6B302c";
#else
            ApiUsername = "OK.Mobisis.Windows";
            ApiPassword = "3f6B302c";
#endif
            MidtermFactor = 40;
            FinalFactor = 60;
            FF_Limit = 0;
            FD_Limit = 30;
            DD_Limit = 40;
            DC_Limit = 45;
            CC_Limit = 50;
            CB_Limit = 57;
            BB_Limit = 65;
            BA_Limit = 75;
            AA_Limit = 85;
            NotificationState = false;
        }
    }
}
