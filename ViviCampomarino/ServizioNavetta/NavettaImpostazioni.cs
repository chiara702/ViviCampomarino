using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ViviCampomarino.ServizioNavetta {
    internal class NavettaImpostazioni {
        private static DataTable TableImpostazioni=null;
        private static void InizializzaImpostazioni() {
            if (TableImpostazioni != null) return;
            var Db = new MySqlvc();
            TableImpostazioni=Db.EseguiQuery("Select * From NavettaImpostazioni");



        }
        public static String LeggiImpostazione(String Chiave) {
            InizializzaImpostazioni();
            foreach (DataRow x in TableImpostazioni.Rows) {
                if (x["Impostazione"].ToString()==Chiave) return x["Valore"].ToString();
            }
            return "";
        }
    }
}
