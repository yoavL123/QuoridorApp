﻿using System;
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
        private const string DEV_ANDROID_EMULATOR_URL = "http://10.0.2.2:21604/contactsAPI"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_URL = "http://192.168.1.14:21604/contactsAPI"; //API url when using physucal device on android
        private const string DEV_WINDOWS_URL = "https://localhost:44331/contactsAPI"; //API url when using windoes on development
        private const string DEV_ANDROID_EMULATOR_PHOTOS_URL = "http://10.0.2.2:21604/Images/"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_PHOTOS_URL = "http://192.168.1.14:21604/Images/"; //API url when using physucal device on android
        private const string DEV_WINDOWS_PHOTOS_URL = "https://localhost:44331/Images/"; //API url when using windoes on development


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

        /*
        Sign Up
        */

        public async Task<Player> SignUpAsync(string email, string userName, string firstName, string lastName, string playerPass)
        {
            /*
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/signUp?Email={email}" +
                    $"&userName={userName}&firstName={firstName}&lsatName={lastName}&playerPass={playerPass}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions 
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    User u = JsonSerializer.Deserialize<User>(content, options);
                    return u;
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
            */
        }
        

    }
}
