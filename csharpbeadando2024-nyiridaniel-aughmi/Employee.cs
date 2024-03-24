using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpbeadando2024_nyiridaniel_aughmi
{
    internal class Employee
    {
        private string Eid { get; set; }
        private string Name { get; set; }
        private int Balance { get; set; }

        public Employee(string eid, string name, int balance)
        {
            Eid = eid;
            Name = name;
            Balance = balance;
        }

        public override string ToString()
        {
            return $"[{Eid}] {Name} // {Balance}";
        }




    }
}
