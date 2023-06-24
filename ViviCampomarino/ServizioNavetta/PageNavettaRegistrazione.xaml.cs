﻿using System;
using System.Collections.Generic;
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
        public PageNavettaRegistrazione(DateTime DataSelezionata, int Posto) {
            InitializeComponent();
            this.DataSelezionata=DataSelezionata;
            this.Posto=Posto;
            LblData.Text = DataSelezionata.ToString("dd/MM/yyyy HH:mm");
            var Db = new MySqlvc();
            var rowP = Db.EseguiRow($"Select * from NavettaPrenotazioni where Giorno='{DataSelezionata.ToString("yyyy-MM-dd HH:mm")}' and Posto={Posto}");
            Db.CloseCommit();
            if (rowP != null) {
                if (rowP["GuidDevice"].ToString()==App.Guid) {
                    BtnAnnullaPrenotazione.IsVisible=true;
                    BtnAnnulla.IsVisible=false;
                    IdPrenotazione=(int)rowP["Id"];
                    TxtNome.Text=rowP["Nome"].ToString();
                    TxtTelefono.Text=rowP["Telefono"].ToString();

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

        }

        private void BtnConferma_Clicked(object sender, EventArgs e) {
            //Verifica dati principali
            if (String.IsNullOrEmpty(TxtNome.Text)==true) {
                DisplayAlert("Errore", "Occorre inserire il nome!", "ok");
                return;
            }
            if (String.IsNullOrEmpty(TxtIndirizzoPrelievo.Text)==true) {
                DisplayAlert("Errore", "Occorre inserire indirizzo di prelievo/partenza!", "ok");
                return;
            }
            if (String.IsNullOrEmpty(TxtIndirizzoDestinazione.Text)==true) {
                DisplayAlert("Errore", "Occorre inserire indirizzo di rilascio/destinazione!", "ok");
                return;
            }
            if (SwitchAccompagnatore.IsToggled==true && string.IsNullOrEmpty(TxtAccompagnatoreNome.Text)==true) {
                DisplayAlert("Errore", "Occorre inserire almeno il nome dell'accomapagnatore", "ok");
                return;
            }
            //Verifica posto già occupato
            var Db = new MySqlvc();
            var rowP=Db.EseguiRow($"Select * from NavettaPrenotazioni where Giorno='{DataSelezionata.ToString("yyyy-MM-dd HH:mm")}' and Posto={Posto}");
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
                bis.GetParam.AddWithValue("GuidDevice", App.Guid);
                bis.GetParam.AddWithValue("Nome", Funzioni.Antinull(TxtNome.Text));
                bis.GetParam.AddWithValue("Telefono", Funzioni.Antinull(TxtTelefono.Text));
                bis.GetParam.AddWithValue("Accompagnatore", SwitchAccompagnatore.IsToggled);
                if (SwitchAccompagnatore.IsToggled==false) { TxtAccompagnatoreNome.Text=""; TxtAccompagnatoreTelefono.Text=""; }
                bis.GetParam.AddWithValue("NomeAccompagnatore", Funzioni.Antinull(TxtAccompagnatoreNome.Text));
                bis.GetParam.AddWithValue("TelefonoAccompagnatore", Funzioni.Antinull(TxtAccompagnatoreTelefono.Text));
                bis.GetParam.AddWithValue("IndirizzoPresa", Funzioni.Antinull(TxtIndirizzoPrelievo.Text));
                bis.GetParam.AddWithValue("IndirizzoRilascio", Funzioni.Antinull(TxtIndirizzoDestinazione.Text));
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
            if (SalvaOk==1) { Navigation.PopAsync(); }

        }

        private void BtnAnnullaPrenotazione_Clicked(object sender, EventArgs e) {
            try {
                var Db = new MySqlvc();
                Db.EseguiScalare("Delete From NavettaPrenotaqzioni Where Id=" + IdPrenotazione);
                Db.CloseCommit();
            } catch(Exception ex) {
                DisplayAlert("Errore", "Prenotazione non annullata a causa di un problema di connessione!", "Ok");
            }
            DisplayAlert("Prenotazione", "Prenotazione annullata con successo!", "Ok");
            Navigation.PopAsync();
        }
    }
}