using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo_Encrypt_LoadFile.Feature.API
{
    class GoogleDriveFileRespository
    {
        private string[] scopes = { Google.Apis.Drive.v3.DriveService.Scope.Drive };
        private Google.Apis.Drive.v3.DriveService driveService;

        public GoogleDriveFileRespository()
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secret_123769918993-sge5nmf8mpl3v6btc6crr75cnrs9fujk.apps.googleusercontent.com.json", FileMode.Open, FileAccess.Read))
            {
                string creadPath = "token.json";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(creadPath, true)).Result;
                    
            }

            driveService = new Google.Apis.Drive.v3.DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Star Academy"

            });
        }
    }
}
