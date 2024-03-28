using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpbeadando2024_nyiridaniel_aughmi
{
    internal class Item
    {
		private string tool;
		private int price;

        public Item(string tool, int price)
        {
            this.tool = tool;
            this.price = price;
        }

        public override string ToString()
        {
            return $"* {Tool} // Price: {Price}";
        }

        public int Price
		{
			get { return price; }
			set { price = value; }
		}

		public string Tool
		{
			get { return tool; }
			set { tool = value; }
		}
	}
}
