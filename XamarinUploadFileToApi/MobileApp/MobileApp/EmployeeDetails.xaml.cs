using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EmployeeDetails : ContentPage
	{
		public EmployeeDetails (Employee employee)
		{
			InitializeComponent ();

			txtId.Text = employee.Id.ToString()??"";
			txtName.Text = employee.Name??"";
			txtPhone.Text = employee.Phone??"";
			txtImage.Text = employee.Image??"";

			img.Source = ImageSource.FromUri(new Uri(employee.Image));
		}

        async void btnGet_Clicked(object sender, EventArgs e)
        {
			await Navigation.PopAsync();
        }
    }
}