using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;
using System.IO;

namespace CSGO.Domain.Repository
{
    public class LoginRepository
    {

        public SteamClient _steamClient;
        public SteamUser _steamUser;
        public string _user;
        public string _pass;
        public byte[] _sentryHash;
        public string _authCode;
        public string _twoFactorAuth;
        public CallbackManager _manager;
        public bool _isRunning;
        public LoginRepository()
        {
            _steamClient = new SteamClient();
           _manager = new CallbackManager(_steamClient);
        }

        public void Login(string user, string pass)
        {
            //Create Login logic via SteamKit2   
            _steamUser = _steamClient.GetHandler<SteamUser>();
            _user = user;
            _pass = pass;

            //Register a few callbacks that are pertinent to the steam client
            new Callback<SteamClient.ConnectedCallback>(OnConnected, _manager);
            new Callback<SteamClient.DisconnectedCallback>(OnDisconnected, _manager);
            new Callback<SteamUser.LoggedOnCallback>(OnLoggedOn, _manager);
            new Callback<SteamUser.LoggedOffCallback>(OnLoggedOff, _manager);
            new Callback<SteamUser.UpdateMachineAuthCallback>(OnMachineAuth, _manager);

            _isRunning = true;
            _steamClient.Connect();

            while(_isRunning)
            {
                //wait for the callbacks to get handled
                _manager.RunWaitCallbacks(TimeSpan.FromMilliseconds(.500));
            }

        }

        void OnConnected(SteamClient.ConnectedCallback callback)
        {
            if (callback.Result != EResult.OK)
            {
                Console.WriteLine("Unable to connect to Steam: {0}", callback.Result);

                _isRunning = false;
            }

            Console.WriteLine("Connected to Steam! Logging in account {0}", _user);

            //TODO: Call database and see if they have a sentry file on hand. If so, use that so that they don't have to keep
            //running into Steamguard issues

            _steamUser.LogOn(new SteamUser.LogOnDetails
                {
                    Username = _user,
                    Password = _pass,
                    AuthCode = _authCode,
                    TwoFactorCode = _twoFactorAuth,
                    SentryFileHash = _sentryHash
                });
        }

        void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            Console.WriteLine("Disconnected from Steam");

             //_isRunning = false;
        }

        void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {
            bool isSteamGuard = callback.Result == EResult.AccountLogonDenied;
            bool is2FA = callback.Result == EResult.AccountLogonDeniedNeedTwoFactorCode;

            if(isSteamGuard || is2FA)
            {
                Console.WriteLine("This account is SteamGuard protected!");
                if(is2FA)
                {
                    Console.Write("Please enter your 2 factor auth code from your authenticator app: ");
                    //TODO: Redirect to 2 factor auth code page
                    //_twoFactorAuth = Console.ReadLine();
                }
                else
                {
                    Console.Write("Please enter the auth code sent to the email at {0}: ", callback.EmailDomain);
                    //TODO: Redirect to auth code page
                    //_authCode = "R5CK3";
                }
                return;
            }

            if (callback.Result != EResult.OK)
            {
                Console.WriteLine("Unable to logon to Steam. Result: {0} | ExtendedResult: {1}", callback.Result, callback.ExtendedResult);
                _isRunning = false;
                return;
            }

            Console.WriteLine("Login Successful!");
        }

        void OnLoggedOff(SteamUser.LoggedOffCallback callback)
        {
            Console.WriteLine("Logged off of Steam: {0}", callback.Result);
        }

        void OnMachineAuth(SteamUser.UpdateMachineAuthCallback callback)
        {
            Console.WriteLine("Updating sentryfile...");
            byte[] sentryHash = CryptoHelper.SHAHash(callback.Data);
            _sentryHash = sentryHash;
            _steamUser.SendMachineAuthResponse(new SteamUser.MachineAuthDetails
            {
                JobID = callback.JobID,
                FileName = callback.FileName,
                BytesWritten = callback.BytesToWrite,
                Offset = callback.Offset,
                Result = EResult.OK,
                LastError = 0,
                OneTimePassword = callback.OneTimePassword,
                SentryFileHash = sentryHash
            });

            //TODO: Save sentryFileHash to database

            Console.WriteLine("Done!");
        }
    }
}
