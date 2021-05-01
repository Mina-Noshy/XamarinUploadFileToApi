using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        Uri _uri = new Uri("http://eastaria.com/api/");

        async void btnGet_Clicked(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = _uri;

            var content = await httpClient.GetStringAsync("UploadFile");
            var _employee = JsonConvert.DeserializeObject<Employee>(content);

            if (_employee == null)
                await DisplayAlert("error", "\n\t Getting Faild!", "ok");

            txtId.Text = _employee.Id.ToString();
            txtName.Text = _employee.Name;
            txtPhone.Text = _employee.Phone;
            txtImage.Text = _employee.Image;

            await DisplayAlert("success", "\n\t Getting Successfully", "ok");
        }

        async void btnSelect_Clicked(object sender, EventArgs e)
        {
            var _file = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "select file to upload"
            });

            if (_file == null)
                return;

            txtImage.Text = _file.FullPath;

            if (!File.Exists(txtImage.Text))
                return;


            
            try
            {
                img.Source = ImageSource.FromFile(txtImage.Text);
            }
            catch
            {
                return;
            }
            
        }

        async void btnPost_Clicked(object sender, EventArgs e)
        {
            if (txtImage.Text == "" || txtId.Text == "" || txtName.Text == "" || txtPhone.Text == "" )
                return;

            if (!System.IO.File.Exists(txtImage.Text))
                return;

            byte[] _byte = File.ReadAllBytes(txtImage.Text);

            string extension = Path.GetExtension(txtImage.Text);

            var content = await Upload(_byte, extension);

            if (content != string.Empty)
                await DisplayAlert("success", "\n\t Uploading Successfully", "ok");
            else
                await DisplayAlert("error", "\n\t Uploading Faild!", "ok");

            Employee _emp = JsonConvert.DeserializeObject<Employee>(content);

            txtId.Text = txtImage.Text = txtName.Text = txtPhone.Text = string.Empty;

            await Navigation.PushAsync(new EmployeeDetails(_emp));
        }


        public async Task<string> Upload(byte[] image, string extension)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = _uri;

                    using (var content = new MultipartFormDataContent())
                    {
                        HttpContent _name = new StringContent(txtName.Text);
                        HttpContent _phone = new StringContent(txtPhone.Text);

                        content.Add(new StreamContent(new MemoryStream(image)), "Image", $"{Guid.NewGuid()}.{extension}");
                        content.Add(_name, "Name");
                        content.Add(_phone, "Phone");

                        // send parameters in header request
                        content.Headers.Add("Id", txtId.Text);


                        using (var response = await httpClient.PostAsync("UploadFile", content))
                        {
                            var postContent = await response.Content.ReadAsStringAsync();

                            return postContent;
                        }
                        
                        return string.Empty;
                    }
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
    }

}
