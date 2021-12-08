using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Collections;
using System.Globalization;
using System.IO;

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
            InitPath();
        }

        private void InitPath()
        {
            if (!Directory.Exists("Scripts")) Directory.CreateDirectory("Scripts");
        }

        public void Connect()
        {
            link = new SQLiteConnection("Data Source="+DATABASE_PATH); // Creamos la conexion
            link.Open(); // Abrimos la conexion
        }

        public void ExecuteScript(string script_name)
        {
            script_name += ".sql";
            string script_path = Directory.GetCurrentDirectory() + "\\Scripts\\" + script_name;
            if (File.Exists(script_path))
            {
                using(var fs = new FileStream(script_path, FileMode.Open, FileAccess.Read))
                {
                    using(var read = new StreamReader(fs))
                    {
                        string line = null;
                        string command = "";
                        while ((line = read.ReadLine()) != null)
                        {
                            command += line;
                            if (line.Contains(";"))
                            {
                                command = command.Replace(";", "");
                                CommandNoData(command);
                                command = "";
                            }
                        }
                    }
                }
            }
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

        public DataTable CommandData(string cmd)
        {
            DataTable datatable = new DataTable();
            SQLiteDataAdapter adapter = null;
            if (link.State == ConnectionState.Open)
            {
                using (var cmd_ = new SQLiteCommand(cmd, getLink()))
                {
                    adapter = new SQLiteDataAdapter(cmd_);
                    adapter.Fill(datatable);
                }
            }
            return datatable;
        }

        public string InsertData(string tbname, Column[] columns)
        {
            try
            {
                string command_ = "INSERT INTO " + tbname + " (";
                for (int i = 0; i < columns.Length; i++)
                {
                    if (i == columns.Length - 1) command_ += columns[i].GetName() + ") VALUES (";
                    else command_ += columns[i].GetName() + ", ";
                }
                for (int i = 0; i < columns.Length; i++)
                {
                    string type = columns[i].GetValue().GetType().Name;
                    if (i == columns.Length - 1)
                    {
                        if (type == "Int32" || type == "Int64" || type == "Double")
                        {
                            command_ += columns[i].GetValue().ToString(CultureInfo.CreateSpecificCulture("en-US")) + ")";
                        }
                        else
                            command_ += "'" + columns[i].GetValue() + "')";
                    }
                    else
                    {
                        if (type == "Int32" || type == "Int64" || type == "Double")
                        {
                            command_ += columns[i].GetValue().ToString(CultureInfo.CreateSpecificCulture("en-US")) + ", ";
                        }
                        else
                            command_ += "'" + columns[i].GetValue() + "', ";
                    }
                }
                CommandNoData(command_);
                return command_;
            }
            catch(SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public DataTable GetDataById(string tbname, int id)
        {
            return CommandData("SELECT * FROM "+tbname+" WHERE _id=" + tbname.ToString());
        }

        public string DeleteData(string tbname, string condition)
        {
            try
            {
                string command_ = "DELETE FROM " + tbname + " WHERE " + condition;
                CommandNoData(command_);
                return command_;
            }
            catch(SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public SQLiteConnection getLink()
        {
            return link;
        }
    }
}
