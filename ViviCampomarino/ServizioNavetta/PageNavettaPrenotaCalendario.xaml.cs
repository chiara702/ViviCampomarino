using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.ServizioNavetta {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageNavettaPrenotaCalendario : ContentPage {
        public PageNavettaPrenotaCalendario() {
            InitializeComponent();
            //InizializzaCalendario(7);
            StkContainer.Children.Add( CreateCalendar(2023, 6));
        }
        private View CreaLabelGiorno(String Giorno) {
            var tmp = new AbsoluteLayout();
            var lbl = new Label();
            lbl.Text=Giorno;
            tmp.Children.Add(tmp);
            return tmp;
        }

        private void InizializzaCalendario(int mese) {
            var tmp = CreaLabelGiorno("L");
            //GridCalendario.Children.Add(tmp, 0, 0);
        }
        private async void BtnIndietro_Clicked(object sender, EventArgs e) {
            await Navigation.PopAsync();
        }


        private Grid CreateCalendar(int year, int month) {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = 30 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 30 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 30 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 30 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 30 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 30 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 30 });

            // Header
            var headerLabel = new Label();
            headerLabel.Text = $"{month}/{year}";
            //headerLabel.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            headerLabel.FontAttributes=FontAttributes.Bold;
            headerLabel.HorizontalOptions = LayoutOptions.Center;
            grid.Children.Add(headerLabel, 0, 0);
            Grid.SetColumnSpan(headerLabel, 7);

            // Days of the week
            var daysOfWeek = new string[] { "DOM", "LUN", "MAR", "MER", "GIO", "VEN", "SAB" };
            for (int i = 0; i < daysOfWeek.Length; i++) {
                var dayLabel = new Label();
                dayLabel.Text = daysOfWeek[i];
                dayLabel.HorizontalOptions = LayoutOptions.Center;
                grid.Children.Add(dayLabel, i, 1);
            }

            // Calendar days
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var firstDayOfMonth = new DateTime(year, month, 1).DayOfWeek;

            var currentDay = 1;
            var row = 2;
            var col = (int)firstDayOfMonth;
            while (currentDay <= daysInMonth) {
                var dayLabel = new Label();
                dayLabel.Text = currentDay.ToString();
                dayLabel.HorizontalOptions = LayoutOptions.Center;
                grid.Children.Add(dayLabel, col, row);
                currentDay++;
                col++;

                if (col == 7) {
                    col = 0;
                    row++;
                }
            }
            return grid;
        }

    }
}