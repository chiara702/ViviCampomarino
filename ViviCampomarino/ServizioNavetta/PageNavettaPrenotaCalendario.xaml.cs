using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
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
        private int MeseVisualizzato = 0;
        private int AnnoVisualizzato = 0;
        public PageNavettaPrenotaCalendario() {
            InitializeComponent();
            
            BtnMesePrec.Text=DateTime.Now.AddMonths(-1).ToString("MMMM");
            BtnMeseAttuale.Text=DateTime.Now.AddMonths(0).ToString("MMMM");
            BtnMeseSucc.Text=DateTime.Now.AddMonths(1).ToString("MMMM");
            BtnMeseAttuale_Clicked(null, null);
        }

        private void RiempiGiorniAbilitatiFromDB(int Mese) {
            RowGiorniMeseAbilitati.Clear();
            var Db = new MySqlvc();
            TableGiorniAbilitati=Db.EseguiQuery("Select * From NavettaGiorniAbilitati Order By GiornoAbilitato");
            foreach (DataRow x in TableGiorniAbilitati.Rows) {
                if ((Convert.ToDateTime(x["GiornoAbilitato"])-DateTime.Now).Days<0) continue;
                if ((Convert.ToDateTime(x["GiornoAbilitato"])-DateTime.Now).Days>=Convert.ToInt16(NavettaImpostazioni.LeggiImpostazione("GiorniMaxPrenotazione"))) continue;
                if (Convert.ToDateTime(x["GiornoAbilitato"].ToString()).Month==Mese) RowGiorniMeseAbilitati.Add(x);
            }
        }
        
        

        
        private async void BtnIndietro_Clicked(object sender, EventArgs e) {
            await Navigation.PopAsync();
        }

        private void dayClick(Object label) {
            var DataComponi = new DateTime(AnnoVisualizzato, MeseVisualizzato, Convert.ToInt16(label.ToString()));
            var page = new PageNavettaPrenotaOrario(DataComponi);
            Navigation.PushAsync(page);
        }
        private void CreateCalendar(int year, int month) {
            MeseVisualizzato=month;
            AnnoVisualizzato=year;
            var grid = new Grid ();
            grid.RowDefinitions.Add(new RowDefinition { Height = 33 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 33 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 33 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 33 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 33 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 33 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 33 });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width=GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width=GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width=GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width=GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width=GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width=GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width=GridLength.Star });


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
                dayLabel.FontSize=14;
                dayLabel.Text = currentDay.ToString();
                dayLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
                dayLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
                grid.Children.Add(dayLabel, col, row);
                //add event
                if (RowGiorniMeseAbilitati.Where(row => row.Field<DateTime>("GiornoAbilitato").Day==currentDay).Count()>0) {
                    dayLabel.TextDecorations=TextDecorations.Underline;
                    dayLabel.FontAttributes=FontAttributes.Bold;
                    dayLabel.TextColor=Color.Red;
                    dayLabel.GestureRecognizers.Add(new TapGestureRecognizer() {
                        Command=new Command(dayClick),
                        CommandParameter = dayLabel.Text
                    });
                }
                //Cambiamenti per giorno corrente
                if (month==DateTime.Now.Month && currentDay==DateTime.Now.Day){
                    pallino.BackgroundColor=Color.FromHex("ffffff");
                }

                
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

        static Color ColoreBottoniStandard=Color.Gray;
        static Color ColoreBottoniSelezionato = Color.FromHex("55b7a8");
        private void BtnMesePrec_Clicked(object sender, EventArgs e) {
            Task.Run(() => { RiempiGiorniAbilitatiFromDB(DateTime.Now.Month-1); }).Wait(5000);
            CreateCalendar(DateTime.Now.Year, DateTime.Now.Month-1);
            BtnMesePrec.BackgroundColor=ColoreBottoniStandard; BtnMeseAttuale.BackgroundColor=ColoreBottoniStandard; BtnMeseSucc.BackgroundColor=ColoreBottoniStandard;
            BtnMesePrec.BackgroundColor=ColoreBottoniSelezionato;
        }

        private void BtnMeseAttuale_Clicked(object sender, EventArgs e) {
            Task.Run(() => { RiempiGiorniAbilitatiFromDB(DateTime.Now.Month); }).Wait(5000);
            CreateCalendar(DateTime.Now.Year, DateTime.Now.Month);
            BtnMesePrec.BackgroundColor=ColoreBottoniStandard; BtnMeseAttuale.BackgroundColor=ColoreBottoniStandard; BtnMeseSucc.BackgroundColor=ColoreBottoniStandard;
            BtnMeseAttuale.BackgroundColor=ColoreBottoniSelezionato;
        }

        private void BtnMeseSucc_Clicked(object sender, EventArgs e) {
            Task.Run(() => { RiempiGiorniAbilitatiFromDB(DateTime.Now.Month+1); }).Wait(5000);
            CreateCalendar(DateTime.Now.Year, DateTime.Now.Month+1);
            BtnMesePrec.BackgroundColor=ColoreBottoniStandard; BtnMeseAttuale.BackgroundColor=ColoreBottoniStandard; BtnMeseSucc.BackgroundColor=ColoreBottoniStandard;
            BtnMeseSucc.BackgroundColor=ColoreBottoniSelezionato;
        }
    }
}