using App2.Models;
using App2.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private Item _selectedItem;

        public ObservableCollection<Item> Items { get; }
        public Command AddItemCommand { get; }
        public Command<Item> ItemTapped { get; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();

            ItemTapped = new Command<Item>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);

            using (var httpClient = new HttpClient())
            {
                string apiUrl = "http://192.168.2.200:5272/Scooter";

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {JWTKey.Key}");

                try
                {
                    HttpResponseMessage response = httpClient.GetAsync(apiUrl).Result;
                    // Проверяем успешность ответа
                    if (response.IsSuccessStatusCode)
                    {
                        // Получаем содержимое ответа в виде строки
                        string content = response.Content.ReadAsStringAsync().Result;
                        List<Item> re = JsonConvert.DeserializeObject<List<Item>>(content);
                        foreach (var a in re)
                        {
                            Items.Add(a);
                        }
                        // Выводим содержимое ответа
                        Console.WriteLine(content);
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }
    }
}