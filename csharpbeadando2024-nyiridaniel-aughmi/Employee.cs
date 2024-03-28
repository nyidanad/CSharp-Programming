using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpbeadando2024_nyiridaniel_aughmi
{
    internal class Employee
    {
        private string eid;
        private string name;
        private int balance;

        public Employee(string eid, string name, int balance)
        {
            this.eid = eid;
            this.name = name;
            this.balance = balance;
        }

        public override string ToString()
        {
            return $"[{Eid}] {Name} // {Balance}";
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Eid
        {
            get { return eid; }
            set { eid = value; }
        }

        public int Balance
        {
            get { return balance; }
            set { balance = value; }
        }
    }
}
