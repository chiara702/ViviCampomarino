using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViviCampomarino.ServizioNavetta {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAmministratoreSettings : ContentPage {
        private DataTable NavettaImpostazioni;
        public PageAmministratoreSettings() {
            InitializeComponent();
            var Db = new MySqlvc();
            NavettaImpostazioni=Db.EseguiQuery("Select * From NavettaImpostazioni");
            Db.CloseCommit();
            foreach (DataRow x in  NavettaImpostazioni.Rows) {
                PickerSettings.Items.Add(x["Impostazione"].ToString());
            }
            
        }

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync(true);
        }

        private void PickerSettings_SelectedIndexChanged(object sender, EventArgs e) {
            BtnSave.IsEnabled=false;
            TxtValue.Text=NavettaImpostazioni.Select($"Impostazione='{PickerSettings.SelectedItem}'")[0]["Valore"].ToString();
            LblDescrizione.Text=NavettaImpostazioni.Select($"Impostazione='{PickerSettings.SelectedItem}'")[0]["Descrizione"].ToString();
        }

        private void TxtValue_TextChanged(object sender, TextChangedEventArgs e) {
            BtnSave.IsEnabled=true;
        }

        private void BtnSave_Clicked(object sender, EventArgs e) {
            var Db = new MySqlvc();
            Db.EseguiCommand($"Update NavettaImpostazioni Set Valore='{TxtValue.Text}' Where Impostazione='{PickerSettings.SelectedItem}'");
            NavettaImpostazioni=Db.EseguiQuery("Select * From NavettaImpostazioni");
            Db.CloseCommit();
            BtnSave.IsEnabled=false;
        }
    }
}