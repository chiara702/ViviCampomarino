using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ViviCampomarino {
    [SQLite.Table("Versione")]
    public class SqlLiteVersione {
        [PrimaryKey]
        public int ID { get; set; }
        public String Versione { get; set; }
    }
    [SQLite.Table("Notifiche")]
    public class SqlLiteNotifiche {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime Data { get; set; }
        public string Descrizione { get; set; }
        public bool Letta { get; set; }
    }
    public class SqlLiteDatabase {
        private String percorso = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/SqlLite.db3";
        public static SQLiteConnection Connessione=null;


        public SqlLiteDatabase() {
            if (Connessione != null) return;
            Connessione = new SQLiteConnection(percorso, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache);
            Connessione.CreateTable<SqlLiteNotifiche>();
            var TableVer = Connessione.Table<SqlLiteVersione>();
            if (TableVer == null) {
                Connessione.CreateTable<SqlLiteVersione>();
                var v = new SqlLiteVersione();
                v.ID = 1; v.Versione = "1.0";
                Connessione.Update(v);
            }
        }
        public SQLiteConnection GetConn() {
            return Connessione;
        }


        

        
    }
}
