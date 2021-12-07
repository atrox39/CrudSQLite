using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace CrudSQLite
{
    public class Crud
    {
        private SQLiteConnection link; // Conexion de la base de datos
        private string DATABASE_PATH = null; // Path donde esta la base de datos
        
        public Crud(string DATABASE_PATH) // Iniciamos el objeto Crud con la base de datos
        {
            this.DATABASE_PATH = DATABASE_PATH; // Agregamos el Path
            Connect(); // Conectamos a la base de datos
        }

        public void Connect()
        {
            link = new SQLiteConnection(DATABASE_PATH); // Creamos la conexion
            link.Open(); // Abrimos la conexion
        }

        public void CommandNoData(string cmd)
        {
            if (link.State == ConnectionState.Open)
            {
                using(var cmd_ = new SQLiteCommand(cmd, getLink()))
                {
                    cmd_.ExecuteNonQuery(); // Ejecutamos un comando cualquiera de SQLite
                }                
            }
        }

        public void CommandData(string cmd)
        {
            if (link.State == ConnectionState.Open)
            {
                using (var cmd_ = new SQLiteCommand(cmd, getLink()))
                {
                    cmd_.ExecuteReaderAsync();
                }
            }
        }

        public SQLiteConnection getLink()
        {
            return link;
        }
    }
}
