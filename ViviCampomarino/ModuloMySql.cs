using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ViviCampomarino {
    public class MySqlvc {
        public static string ServerSql = "185.58.119.77";
        // Public Transazione As SqlTransaction
        public MySqlConnection Connessione;

        public MySqlvc() {
            ApriConnessione();
        }
        public MySqlConnection ApriConnessione() {
            // Dim Conn As SqlConnection
            Connessione = new MySqlConnection("Server=212.237.39.62;Database=y95qsb0vrv;Uid=spbkm3j6f6;Pwd=Fabio123456!; convert zero datetime=True; Allow User Variables=True");
            try {
                Connessione.Open();
            } catch (Exception ex) {
                return null/* TODO Change to default(_) if this is not a reference type */;
            }


            return Connessione;
        }
        public void CloseCommit() {
            // Transazione.Commit()
            Connessione.Close();
        }
        public void CloseRollback() {
            // Transazione.Rollback()
            Connessione.Close();
        }
        public object EseguiScalare(string Query, object NullReturn = null) {
            MySqlConnection Conn = null;
            try {
                if (Connessione == null)
                    Conn = ApriConnessione();
                else
                    Conn = Connessione;
                MySqlCommand Cmd = new MySqlCommand(Query, Conn);
                // If Transazione IsNot Nothing Then Cmd.Transaction = Transazione
                object ritorno = Cmd.ExecuteScalar();
                if (Convert.IsDBNull(ritorno) == true)
                    ritorno = NullReturn;
                if (Connessione == null)
                    Conn.Close();
                return ritorno;
            } catch (Exception ex) {
                // If Transazione IsNot Nothing Then Transazione.Rollback()
                if (Conn != null)
                    Conn.Close();
                return null;
            }
        }
        public object EseguiCommand(string Comando) {
            MySqlConnection Conn = null;
            try {
                if (Connessione != null)
                    Conn = Connessione;
                else
                    Conn = ApriConnessione();
                MySqlCommand Cmd = new MySqlCommand(Comando, Conn);
                // If Transazione IsNot Nothing Then Cmd.Transaction = Transazione
                object ritorno = Cmd.ExecuteNonQuery();
                return ritorno;
            } catch (Exception ex) {
                // If Transazione IsNot Nothing Then Transazione.Rollback()
                if (Conn != null)
                    Conn.Close();
                return null;
            }
        }
        public static string DataTimeSql(DateTime Data) {
            return "'" + Data.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        }
        public DataTable EseguiQuery(string Query) {
            MySqlDataAdapter Adattatore = new MySqlDataAdapter(Query, Connessione);
            Adattatore.SelectCommand.CommandTimeout = 30;
            // Adattatore.InsertCommand.Transaction = Transazione
            // Adattatore.UpdateCommand.Transaction = Transazione
            // Adattatore.SelectCommand.Transaction = Transazione
            DataTable Tabella = new DataTable();
            Adattatore.Fill(Tabella);
            return Tabella;
        }
        public DataRow EseguiRow(string Query) {
            var Table = EseguiQuery(Query);
            if (Table.Rows.Count == 0)
                return null/* TODO Change to default(_) if this is not a reference type */;
            return Table.Rows[0];
        }

        public static string GeneraInsert(string Tabella, MySqlParameterCollection Parameters) {
            string Sql = "INSERT INTO " + Tabella + " (";
            foreach (MySqlParameter x in Parameters)
                Sql += x.ParameterName + ",";
            Sql = Sql.TrimEnd(',');
            Sql += ") VALUES(";
            foreach (MySqlParameter x in Parameters)
                // Sql &= "?,"
                Sql += "@" + x.ParameterName + ",";
            Sql = Sql.TrimEnd(',');
            Sql += ")";
            return Sql;
        }
        public static string GeneraUpdate(string Tabella, long Id, MySqlParameterCollection parameters) {
            string Sql = "UPDATE " + Tabella + " SET ";
            foreach (MySqlParameter x in parameters)
                // Sql &= x.ParameterName & "=?,"
                Sql += x.ParameterName + "=@" + x.ParameterName + ",";
            Sql = Sql.TrimEnd(',');
            Sql += " WHERE Id=" + Id;
            return Sql;
        }
        public static string AntiAp(string input) {
            return input.Replace("'", "''");
        }
        public static string AntiNull(object Input) {
            if (Input == null)
                return "";
            if (Convert.IsDBNull(Input))
                return "";
            return Input.ToString();
        }

        public static long AntiNullNum(object Inp) {
            if (Inp == null)
                return 0;
            if (Convert.IsDBNull(Inp) == true)
                return 0;
            return Convert.ToInt32(Inp);
        }

        public void UpdateRapido(string Tabella, long Id, string Campo, object Valore) {
            MySqlConnection Conn = null/* TODO Change to default(_) if this is not a reference type */;
            try {
                MySqlCommand Command = new MySqlCommand("" + Tabella, Connessione);
                MySqlParameterCollection Par = Command.Parameters;
                MySqlParameter Par1 = Par.AddWithValue(Campo, Valore);
                string Query = GeneraUpdate(Tabella, Id, Par);
                Command.CommandText = Query;
                Command.ExecuteNonQuery();
            } catch (Exception ex) {
                if (Conn != null)
                    Conn.Close();
                throw;
            }
        }

        public static void WriteLog(String Operazione) {
            Task.Run(() => {
                var Db = new MySqlvc();
                var Bis = new MySqlvc.DBSqlBis(Db, "Log");
                var Par = Bis.GetParam;
                Par.AddWithValue("Data", DateTime.Now);
                Par.AddWithValue("Piattaforma", "Mobile");
                if (App.login != null) Par.AddWithValue("LoginId", App.login["Id"].ToString());
                Par.AddWithValue("Operazione", Funzioni.LimitLength(Operazione,100));
                Bis.GeneraInsert();
                Db.CloseCommit();
            });
        }



        public class DBSqlBis {
            private MySqlvc Db;
            private MySqlConnection Conn;
            private MySqlCommand Comm;
            private MySqlParameterCollection Param;
            private string Tabella;
            public DBSqlBis(MySqlvc Db, string Tabella) {
                this.Db = Db;
                Conn = Db.Connessione;
                Comm = new MySqlCommand("", Conn);
                Param = Comm.Parameters;
                Param.Clear();
                this.Tabella = Tabella;
            }

            public MySqlParameterCollection GetParam {
                get {
                    return Param;
                }
            }

            public void DeleteParam() {
                Param.Clear();
            }

            public long GeneraInsert() {
                var Query = MySqlvc.GeneraInsert(Tabella, Param);
                Comm.CommandText = Query;
                // Comm.Transaction = Db.Transazione
                Comm.ExecuteNonQuery();
                Comm.CommandText = "SELECT @@IDENTITY";
                var Identity = Comm.ExecuteScalar();
                // Conn.Close()
                if (Convert.IsDBNull(Identity) == true)
                    return 0;
                return Convert.ToInt32(Identity);
            }
            public void GeneraUpdate(long Id) {
                var Query = MySqlvc.GeneraUpdate(Tabella, Id, Param);
                // Comm.Transaction = Db.Transazione
                Comm.CommandText = Query;
                var RigheModificate = Comm.ExecuteNonQuery();
                if (RigheModificate == 0)
                    Debug.Print("Righe modificate 0");
            }
        }

    }
}
