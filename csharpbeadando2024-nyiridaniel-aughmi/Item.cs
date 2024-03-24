using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpbeadando2024_nyiridaniel_aughmi
{
    internal class Item
    {
		private string Tool { get; set; }
		private int Price { get; set; }

        public Item(string tool, int price)
        {
            Tool = tool;
            Price = price;
        }

        public override string ToString()
        {
            return $"* {Tool} // Price: {Price}";
        }
	}
}
