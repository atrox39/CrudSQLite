using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudSQLite
{
    public class Column
    {
        private string Name;
        private dynamic Value;

        public Column(string Name, dynamic Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        public string GetName()
        {
            return Name;
        }

        public dynamic GetValue()
        {
            return Value;
        }
    }
}
