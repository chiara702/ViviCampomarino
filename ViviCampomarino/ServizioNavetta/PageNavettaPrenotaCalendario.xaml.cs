﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViviCampomarino.GuestPass;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.ServizioNavetta {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageNavettaPrenotaCalendario : ContentPage {
        private DataTable TableGiorniAbilitati;
        private List<DataRow> RowGiorniMeseAbilitati = new List<DataRow>();
        public PageNavettaPrenotaCalendario() {
            InitializeComponent();
            
            BtnMesePrec.Text=DateTime.Now.AddMonths(-1).ToString("MMMM");
            BtnMeseAttuale.Text=DateTime.Now.AddMonths(0).ToString("MMMM");
            BtnMeseSucc.Text=DateTime.Now.AddMonths(1).ToString("MMMM");
            BtnMeseAttuale_Clicked(null, null);
        }

        private async void RiempiGiorniAbilitatiFromDB(int Mese) {
            var Db = new MySqlvc();
            TableGiorniAbilitati=Db.EseguiQuery("Select * From NavettaGiorniAbilitati");
            foreach (DataRow x in TableGiorniAbilitati.Rows) {
                if (Convert.ToDateTime(x["GiornoAbilitato"].ToString()).Month==Mese) RowGiorniMeseAbilitati.Add(x);
            }
        }
        
        

        
        private async void BtnIndietro_Clicked(object sender, EventArgs e) {
            await Navigation.PopAsync();
        }

        private void dayClick(Object label) {
            DisplayAlert("", label.ToString(), "ok");
        }
        private void CreateCalendar(int year, int month) {
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
            StkContainer.Children.Clear();
            StkContainer.Children.Add(grid);
        }

        static Color ColoreBottoniStandard=Color.LightGray;
        static Color ColoreBottoniSelezionato = Color.LightSalmon;
        private void BtnMesePrec_Clicked(object sender, EventArgs e) {
            CreateCalendar(DateTime.Now.Year, DateTime.Now.Month-1);
            BtnMesePrec.BackgroundColor=ColoreBottoniStandard; BtnMeseAttuale.BackgroundColor=ColoreBottoniStandard; BtnMeseSucc.BackgroundColor=ColoreBottoniStandard;
            BtnMesePrec.BackgroundColor=ColoreBottoniSelezionato;
        }

        private void BtnMeseAttuale_Clicked(object sender, EventArgs e) {
            CreateCalendar(DateTime.Now.Year, DateTime.Now.Month);
            BtnMesePrec.BackgroundColor=ColoreBottoniStandard; BtnMeseAttuale.BackgroundColor=ColoreBottoniStandard; BtnMeseSucc.BackgroundColor=ColoreBottoniStandard;
            BtnMeseAttuale.BackgroundColor=ColoreBottoniSelezionato;
        }

        private void BtnMeseSucc_Clicked(object sender, EventArgs e) {
            CreateCalendar(DateTime.Now.Year, DateTime.Now.Month+1);
            BtnMesePrec.BackgroundColor=ColoreBottoniStandard; BtnMeseAttuale.BackgroundColor=ColoreBottoniStandard; BtnMeseSucc.BackgroundColor=ColoreBottoniStandard;
            BtnMeseSucc.BackgroundColor=ColoreBottoniSelezionato;
        }
    }
}