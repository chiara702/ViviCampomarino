using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using ZXing;

namespace ViviCampomarino.ServizioNavetta {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageNavettaRegistrazione : ContentPage {
        private DateTime DataSelezionata;
        private int Posto;
        private int IdPrenotazione = 0;
        private Boolean isAndata = false;
        private DataTable tableFermate;
        public PageNavettaRegistrazione(DateTime DataSelezionata, int Posto) {
            InitializeComponent();
            this.DataSelezionata=DataSelezionata;
            this.Posto=Posto;
            LblData.Text = DataSelezionata.ToString("dd/MM/yyyy HH:mm");
            var Db = new MySqlvc();
            var rowP = Db.EseguiRow($"Select * from NavettaPrenotazioni where Giorno='{DataSelezionata.ToString("yyyy-MM-dd HH:mm")}' and Posto={Posto}");
            var rowG = Db.EseguiRow($"Select * From NavettaGiorniAbilitati Where GiornoAbilitato='{DataSelezionata.ToString("yyyy-MM-dd")}'");
            if (rowG["OrariAndata"].ToString().Split(",").Contains(DataSelezionata.ToString("HH:mm"))) isAndata=true;
            tableFermate = Db.EseguiQuery($"Select * From NavettaFermate Where Disabili=true order By Valore");
            if (isAndata==false) {
                tableFermate.DefaultView.Sort="Valore desc";
                tableFermate=tableFermate.DefaultView.ToTable();
            }
            foreach (DataRow x in tableFermate.Rows) {
                var orariValidi = Funzioni.Antinull(x["OrariValidi"]).Split(",");
                if (orariValidi.Contains(DataSelezionata.ToString("HH:mm"))==true) {
                    PickIndirizzoPrelievo.Items.Add(x["FermataDescrizioneLunga"].ToString());
                    PickIndirizzoDestinazione.Items.Add(x["FermataDescrizioneLunga"].ToString());
                }
                /*if (isAndata==true) {
                    if ((Convert.ToInt16(x["Valore"])<100)) PickIndirizzoPrelievo.Items.Add(x["FermataDescrizioneLunga"].ToString());
                    if ((Convert.ToInt16(x["Valore"])>100)) PickIndirizzoDestinazione.Items.Add(x["FermataDescrizioneLunga"].ToString());
                }
                if (isAndata==false) {
                    if ((Convert.ToInt16(x["Valore"])>100)) PickIndirizzoPrelievo.Items.Add(x["FermataDescrizioneLunga"].ToString());
                    if ((Convert.ToInt16(x["Valore"])<100)) PickIndirizzoDestinazione.Items.Add(x["FermataDescrizioneLunga"].ToString());
                }*/
            }
            Db.CloseCommit();
            if (rowP != null) {
                if (rowP["GuidDevice"].ToString()==App.Guid) {
                    BtnAnnullaPrenotazione.IsVisible=true;
                    BtnAnnulla.IsVisible=false;
                    IdPrenotazione=(int)rowP["Id"];
                    TxtNome.Text=rowP["Nome"].ToString();
                    TxtTelefono.Text=rowP["Telefono"].ToString();
                    SwitchAccompagnatore.IsToggled=Convert.ToBoolean(rowP["Accompagnatore"]);
                    TxtAccompagnatoreNome.Text=rowP["NomeAccompagnatore"].ToString();
                    TxtAccompagnatoreTelefono.Text=rowP["TelefonoAccompagnatore"].ToString();
                    //TxtIndirizzoPrelievo.Text=rowP["IndirizzoPresa"].ToString();
                    //TxtIndirizzoDestinazione.Text=rowP["IndirizzoRilascio"].ToString();
                    if (PickIndirizzoPrelievo.Items.Contains(rowP["IndirizzoPresa"].ToString())==false) PickIndirizzoPrelievo.Items.Add(rowP["IndirizzoPresa"].ToString());
                    PickIndirizzoPrelievo.SelectedItem=rowP["IndirizzoPresa"].ToString();
                    if (PickIndirizzoDestinazione.Items.Contains(rowP["IndirizzoRilascio"].ToString())==false) PickIndirizzoDestinazione.Items.Add(rowP["IndirizzoRilascio"].ToString());
                    PickIndirizzoDestinazione.SelectedItem=rowP["IndirizzoRilascio"].ToString();
                    TxtNote.Text=rowP["Note"].ToString();
                }
            }
        }

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync();
        }

        private void SwitchAccompagnatore_Toggled(object sender, ToggledEventArgs e) {
            StkAccompagnatore.IsVisible=e.Value;
        }

        private void BtnAnnulla_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync(true);
        }

        private void BtnConferma_Clicked(object sender, EventArgs e) {
            //Verifica dati principali
            if (String.IsNullOrEmpty(TxtNome.Text)==true) {
                DisplayAlert("Errore", "Occorre inserire il nome!", "ok");
                return;
            }
            if (String.IsNullOrEmpty(PickIndirizzoPrelievo.SelectedItem.ToString())==true) {
                DisplayAlert("Errore", "Occorre inserire indirizzo di prelievo/partenza!", "ok");
                return;
            }
            if (String.IsNullOrEmpty(PickIndirizzoDestinazione.SelectedItem.ToString())==true) {
                DisplayAlert("Errore", "Occorre inserire indirizzo di rilascio/destinazione!", "ok");
                return;
            }
            //controllo Valore Fermata
            if (Convert.ToInt16 ( tableFermate.Select($"FermataDescrizioneLunga='{PickIndirizzoPrelievo.SelectedItem.ToString()}'")[0]["Valore"])>=Convert.ToInt16(tableFermate.Select($"FermataDescrizioneLunga='{PickIndirizzoDestinazione.SelectedItem.ToString()}'")[0]["Valore"])) {
                DisplayAlert("Errore", "Direzione corsa errata! Controllare indirizzo di rilascio/destinazione!", "ok");
                return;
            }
            if (SwitchAccompagnatore.IsToggled==true && string.IsNullOrEmpty(TxtAccompagnatoreNome.Text)==true) {
                DisplayAlert("Errore", "Occorre inserire almeno il nome dell'accomapagnatore", "ok");
                return;
            }
            //Verifica posto già occupato
            var Db = new MySqlvc();
            var rowP=Db.EseguiRow($"Select * from NavettaPrenotazioni where Giorno='{DataSelezionata.ToString("yyyy-MM-dd HH:mm")}' and Posto={Posto} and GuidDevice!='{App.Guid}'");
            Db.CloseCommit();
            if (rowP != null) {
                DisplayAlert("Errore", $"Posto già prenotato da {rowP["GuidDevice"]}! Provare con un altro posto o altro orario.", "ok");
                return;
            }
            //Salva prenotazione
            var SalvaOk = 0;
            try { 
                Db = new MySqlvc();
                var bis = new MySqlvc.DBSqlBis(Db, "NavettaPrenotazioni");
                bis.GetParam.AddWithValue("Giorno", DataSelezionata);
                bis.GetParam.AddWithValue("Posto", Posto);
                bis.GetParam.AddWithValue("GuidDevice", App.Guid);
                bis.GetParam.AddWithValue("Nome", Funzioni.Antinull(TxtNome.Text));
                bis.GetParam.AddWithValue("Telefono", Funzioni.Antinull(TxtTelefono.Text));
                bis.GetParam.AddWithValue("Accompagnatore", SwitchAccompagnatore.IsToggled);
                if (SwitchAccompagnatore.IsToggled==false) { TxtAccompagnatoreNome.Text=""; TxtAccompagnatoreTelefono.Text=""; }
                bis.GetParam.AddWithValue("NomeAccompagnatore", Funzioni.Antinull(TxtAccompagnatoreNome.Text));
                bis.GetParam.AddWithValue("TelefonoAccompagnatore", Funzioni.Antinull(TxtAccompagnatoreTelefono.Text));
                bis.GetParam.AddWithValue("IndirizzoPresa", Funzioni.Antinull(PickIndirizzoPrelievo.SelectedItem.ToString()));
                bis.GetParam.AddWithValue("IndirizzoRilascio", Funzioni.Antinull(PickIndirizzoDestinazione.SelectedItem.ToString()));
                bis.GetParam.AddWithValue("Note", Funzioni.Antinull(TxtNote.Text));
                if (IdPrenotazione==0) {
                    bis.GetParam.AddWithValue("DataOraCreazione", DateTime.Now);
                    bis.GetParam.AddWithValue("DataOraModifica", DateTime.Now);
                    bis.GeneraInsert();
                    Db.CloseCommit();
                } else {
                    bis.GetParam.AddWithValue("DataOraModifica", DateTime.Now);
                    bis.GeneraUpdate(IdPrenotazione);
                    Db.CloseCommit();
                }
                SalvaOk=1;
            } catch(Exception ex) {
                DisplayAlert("Errore", "Prenotazione non registrata a causa di un problema di connessione!", "Ok");
            }
            if (SalvaOk==1) { Navigation.PopAsync(true); }
            _=Task.Run(() => {
                if (IdPrenotazione==0) {
                    EmailSender.SendEmail(NavettaImpostazioni.LeggiImpostazione("EmailInvioPrenotazioni"), "Prenotazione Navetta Disabili (da ViviCampomarino)", $"Prenotazione creata: {DataSelezionata.ToString("dd/MM/yyyy HH:mm")}, Posto: {Posto}, Nome: {TxtNome.Text}, Telefono: {TxtTelefono.Text}, Accompagnatore: {SwitchAccompagnatore.IsToggled} {TxtAccompagnatoreNome.Text + " " + TxtAccompagnatoreTelefono.Text}");
                } else {
                    EmailSender.SendEmail(NavettaImpostazioni.LeggiImpostazione("EmailInvioPrenotazioni"), "Prenotazione Navetta Disabili (da ViviCampomarino)", $"Prenotazione aggiornata: {DataSelezionata.ToString("dd/MM/yyyy HH:mm")}, Posto: {Posto}, Nome: {TxtNome.Text}, Telefono: {TxtTelefono.Text}, Accompagnatore: {SwitchAccompagnatore.IsToggled} {TxtAccompagnatoreNome.Text + " " + TxtAccompagnatoreTelefono.Text}");
                }
            });

        }

        private async void BtnAnnullaPrenotazione_Clicked(object sender, EventArgs e) {
            DataRow rowPrenotazione;
            try {
                var Db = new MySqlvc();
                rowPrenotazione=Db.EseguiRow("Select * From NavettaPrenotazioni Where Id=" + IdPrenotazione);
                Db.EseguiScalare("Delete From NavettaPrenotazioni Where Id=" + IdPrenotazione);
                Db.CloseCommit();
            } catch(Exception ex) {
                await DisplayAlert("Errore", "Prenotazione non annullata a causa di un problema di connessione!", "Ok");
                return;
            }
            await DisplayAlert("Prenotazione", "Prenotazione annullata con successo!", "Ok");
            await Navigation.PopAsync(true);
            _=Task.Run(() => { EmailSender.SendEmail(NavettaImpostazioni.LeggiImpostazione("EmailInvioPrenotazioni"), "Prenotazione Navetta Disabili (da ViviCampomarino)", $"Prenotazione annullata: {Convert.ToDateTime(rowPrenotazione["Giorno"]).ToString("dd/MM/yyyy HH:mm")}, Posto: {rowPrenotazione["Posto"]}, Nome: {rowPrenotazione["Nome"]}"); });
        }
    }
}