using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSGO.Models
{
    public class LoginWithSteamGuardViewModel : LoginViewModel
    {
        public string AuthCode { get; set; }
        public string TwoFactorAuthCode { get; set; }
    }
}