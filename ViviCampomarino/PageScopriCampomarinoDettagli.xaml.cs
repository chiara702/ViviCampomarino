using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageScopriCampomarinoDettagli : ContentPage
	{
		public PageScopriCampomarinoDettagli (DataRow row)
		{
			InitializeComponent ();

			Device.BeginInvokeOnMainThread(() => {
				Video1.Source = row["LinkVideo"].ToString();
				Video1.Start();
				TxtTitolo.Text = row["Titolo"].ToString();
			});

		}
	}
}