using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using QuoridorApp.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using QuoridorApp.ViewModels;
using QuoridorApp.Views;



namespace QuoridorApp.Services
{
    class QuoridorAPIProxy
    {

        private const string CLOUD_URL = "TBD"; //API url when going on the cloud
        private const string CLOUD_PHOTOS_URL = "TBD";
        private const string DEV_ANDROID_EMULATOR_URL = "http://10.0.2.2:20034/QuoridorAPI"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_URL = "http://192.168.1.14:20034/QuoridorAPI"; //API url when using physucal device on android
        private const string DEV_WINDOWS_URL = "http://localhost:20034/QuoridorAPI"; //API url when using windoes on development
        private const string DEV_ANDROID_EMULATOR_PHOTOS_URL = "http://10.0.2.2:20034/Images/"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_PHOTOS_URL = "http://192.168.1.14:20034/Images/"; //API url when using physucal device on android
        private const string DEV_WINDOWS_PHOTOS_URL = "https://localhost:20034/Images/"; //API url when using windoes on development


        private HttpClient client;
        private string baseUri;
        private string basePhotosUri;
        private static QuoridorAPIProxy proxy = null;

        public static QuoridorAPIProxy CreateProxy()
        {
            string baseUri;
            string basePhotosUri;
            if (App.IsDevEnv)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    if (DeviceInfo.DeviceType == DeviceType.Virtual)
                    {
                        baseUri = DEV_ANDROID_EMULATOR_URL;
                        basePhotosUri = DEV_ANDROID_EMULATOR_PHOTOS_URL;
                    }
                    else
                    {
                        baseUri = DEV_ANDROID_PHYSICAL_URL;
                        basePhotosUri = DEV_ANDROID_PHYSICAL_PHOTOS_URL;
                    }
                }
                else
                {
                    baseUri = DEV_WINDOWS_URL;
                    basePhotosUri = DEV_WINDOWS_PHOTOS_URL;
                }
            }
            else
            {
                baseUri = CLOUD_URL;
                basePhotosUri = CLOUD_PHOTOS_URL;
            }

            if (proxy == null)
                proxy = new QuoridorAPIProxy(baseUri, basePhotosUri);
            return proxy;
        }

        private QuoridorAPIProxy(string baseUri, string basePhotosUri)
        {
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();

            //Create client with the handler!
            this.client = new HttpClient(handler, true);
            this.baseUri = baseUri;
            this.basePhotosUri = basePhotosUri;
        }



        public int PlayerId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PlayerPass { get; set; }

        

        public async Task<Player> SignUpPlayer(Player player)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    //ReferenceHandler = ReferenceHandler.Preserve,
                    //Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<Player>(player, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                //HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/SignUpPlayer", content);
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/SignUpPlayer", content);
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    Player ret = JsonSerializer.Deserialize<Player>(jsonContent, options);
                    return ret;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        
        public async Task<Player> SignInAsync(string userName, string pass)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/SignInPlayer?userName={userName}&pass={pass}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    Player p = JsonSerializer.Deserialize<Player>(content, options);
                    return p;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        
        public async Task<Player> GetPlayer(string userName)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetPlayer?userName={userName}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    Player p = JsonSerializer.Deserialize<Player>(content, options);
                    return p;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                return null;
            }
        }
        
        public async Task UpdateRatingChange(RatingChange ratingChange)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                    PropertyNameCaseInsensitive = true
                };

                string jsonObject = JsonSerializer.Serialize<RatingChange>(ratingChange, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/UpdateRatingChange", content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new SystemException("Cannot update rating change");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<RatingChange> GetLastRatingChange(Player p)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetLastRatingChange?playerId={p.PlayerId}").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                   
                    string content = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(content)) return null;
                    RatingChange r = JsonSerializer.Deserialize<RatingChange>(content, options);
                    return r;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                //await Application.Current.MainPage.DisplayAlert($"Error: {e.Message}", "ok", "ok2");
                return null;
            }
        }


        public async Task<List<RatingChange>> GetRatingChanges(Player p)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetRatingChanges?playerId={p.PlayerId}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    List<RatingChange> r = JsonSerializer.Deserialize<List<RatingChange>>(content, options);
                    return r;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await Application.Current.MainPage.DisplayAlert($"Error: {e.Message}", "ok", "ok2");
                return null;
            }
        }

    }
}
