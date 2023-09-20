using App2.Models;
using App2.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace App2.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _LoginValue;
        public string LoginValue { get { return _LoginValue; } set { _LoginValue = value; OnPropertyChanged(nameof(LoginValue)); } }
        private string _PasswordValue;
        public string PasswordValue { get { return _PasswordValue; } set { _PasswordValue = value; OnPropertyChanged(nameof(PasswordValue)); } }

        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginValue = "tsc_user_1";
            PasswordValue = "123456";
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            using (var httpClient = new HttpClient())
            {
                // Задайте базовый адрес сервера
                httpClient.BaseAddress = new Uri("http://192.168.2.200:8080"); // Замените на реальный адрес

                // Задайте параметры запроса, включая данные пользователя
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("client_id", "ScooterAuth"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", LoginValue),
                new KeyValuePair<string, string>("password", PasswordValue)
            });

                // Отправьте POST-запрос на сервер
                var response = await httpClient.PostAsync("/realms/TaogarSmartCloud/protocol/openid-connect/token", content); // Путь к эндпоинту для получения токена

                // Проверьте статус ответа
                if (response.IsSuccessStatusCode)
                {
                    // Прочитайте ответ сервера
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ответ сервера: {responseContent}");
                    TokenResponse jwt = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

                    // Получение значения access_token
                    string accessToken = jwt.AccessToken;
                    JWTKey.SetKey(accessToken);
                    // Здесь вы можете обработать полученный JWT токен
                    await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
                }
                else
                {
                    Console.WriteLine($"Ошибка HTTP: {response.StatusCode}");
                    // Обработайте ошибку, если это необходимо
                }
            }
        }
    }
}
