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

        private void dayClick(Object label) {
            DisplayAlert("", label.ToString(), "ok");
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
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width=40 });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width=40 });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width=40 });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width=40 });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width=40 });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width=40 });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width=40 });


            // Days of the week
            var daysOfWeek = new string[] { "DOM", "LUN", "MAR", "MER", "GIO", "VEN", "SAB" };
            for (int i = 0; i < daysOfWeek.Length; i++) {
                var dayLabel = new Label();
                dayLabel.FontSize=12;
                dayLabel.FontAttributes=FontAttributes.Bold;
                dayLabel.Text = daysOfWeek[i];
                dayLabel.HorizontalOptions = LayoutOptions.Center;
                grid.Children.Add(dayLabel, i, 0);
            }

            // Calendar days
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var firstDayOfMonth = new DateTime(year, month, 1).DayOfWeek;

            var currentDay = 1;
            var row = 1;
            var col = (int)firstDayOfMonth;
            while (currentDay <= daysInMonth) {
                //Tondo
                var pallino = new Frame();
                pallino.HorizontalOptions=LayoutOptions.Center;
                pallino.VerticalOptions=LayoutOptions.Center;
                pallino.CornerRadius = 10;
                pallino.WidthRequest=8;
                pallino.HeightRequest=8;
                pallino.BorderColor = Color.Black;
                pallino.BackgroundColor=Color.FromHex("55b7a8");
                grid.Children.Add(pallino, col, row);
                //
                var dayLabel = new Label();
                dayLabel.TextColor = Color.Black;
                dayLabel.FontSize=12;
                dayLabel.Text = currentDay.ToString();
                dayLabel.HorizontalOptions = LayoutOptions.Center;
                dayLabel.VerticalOptions = LayoutOptions.Center;
                grid.Children.Add(dayLabel, col, row);
                //add event
                pallino.GestureRecognizers.Add(new TapGestureRecognizer() {
                    Command=new Command(dayClick),
                    CommandParameter = dayLabel.Text
                });
                
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