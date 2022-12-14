using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;
using AlgoritmoTest.Models.Tools;

namespace AlgoritmoTest.Models.Entity
{
    public class HuellasDB 
    {
        public SQLiteConnection sqlite_conn;
        public Configuracion Configuracion { get; set; }

        public HuellasDB() {
            sqlite_conn = CreateConnection();
            CreateTable();
        }

        public SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = new SQLiteConnection("Data Source=HuelasDB.db; Version = 3; New = True; Compress = True; ");
            try{
                sqlite_conn.Open();
            }catch (Exception ex)
            {
            }
            return sqlite_conn;
        }

        public void CreateTable()
        {
            SQLiteCommand sqlite_cmd;

            
            
            sqlite_cmd = sqlite_conn.CreateCommand();

            string CreateTableSubCluster = "CREATE TABLE IF NOT EXISTS SUB_CLUSTER(Id INT, Estatus VARCHAR(20), Col2 INT)";
            sqlite_cmd.CommandText = CreateTableSubCluster;
            sqlite_cmd.ExecuteNonQuery();

            string CreateTableClientes = "CREATE TABLE IF NOT EXISTS CLIENTES(C VARCHAR(20), CINT)";
            sqlite_cmd.CommandText = CreateTableClientes;
            sqlite_cmd.ExecuteNonQuery();

            string CreateTableHuellas = "CREATE TABLE IF NOT EXISTS HUELLAS(C VARCHAR(20), CINT)";
            sqlite_cmd.CommandText = CreateTableHuellas;
            sqlite_cmd.ExecuteNonQuery();

            string CreateTableConfig = "CREATE TABLE IF NOT EXISTS CONFIGURACION(Id INT, Nombre_Archivo VARCHAR(20), StatusProceso INT,CONSTRAINT constraint_name PRIMARY KEY (Id))";
            sqlite_cmd.CommandText = CreateTableConfig;
            sqlite_cmd.ExecuteNonQuery();
        }



        public void InsertSubCluster(SubClusters data)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable(Col1, Col2) VALUES('Test Text ', 1); ";
            sqlite_cmd.ExecuteNonQuery();
        }

        public void InsertClientes(Clientes data)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable(Col1, Col2) VALUES('Test Text ', 1); ";
            sqlite_cmd.ExecuteNonQuery();
        }

        public void InsertHuellas(Huellas data)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable(Col1, Col2) VALUES('Test Text ', 1); ";
            sqlite_cmd.ExecuteNonQuery();
        }

        public void getListaClientesFaltantes(Object data)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM SampleTable";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                Console.WriteLine(myreader);
            }
            sqlite_conn.Close();
        }

        public void getListaSubClusters(Object data)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM SampleTable";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                Console.WriteLine(myreader);
            }
            sqlite_conn.Close();
        }

        public async void ReadConfiguracion()
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = this.sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM CONFIGURACION";
            var response = new Configuracion();
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {  
                response.StatusProceso= sqlite_datareader.GetInt32(0);
                response.Nombre_Archivo = sqlite_datareader.GetString(1);
                this.sqlite_conn.Close();
            }
            this.Configuracion = response;
        }

        public void InsertConfiguracion()
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = this.sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT OR REPLACE INTO CONFIGURACION(Id,Nombre_Archivo, StatusProceso) VALUES(1,'data.json ', 2); ";
            sqlite_cmd.ExecuteNonQuery();
        }
    }
}
