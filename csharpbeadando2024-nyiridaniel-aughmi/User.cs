using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpbeadando2024_nyiridaniel_aughmi
{
    internal class User
    {
        public static void UserMain(string nickname)
        {
            List<Item> items = new List<Item>();

            bool exit = false;

            do
            {
                loadShop(nickname, items);
                Console.Write("\n$ ");
                string[] prompt = Console.ReadLine().Split(' ');

                switch (prompt[0])
                {
                    case "help":
                    case "h":
                        Help();
                        break;


                    case "buy":
                    case "b":
                        if (prompt.Length == 3)
                        {
                            Buy(prompt[1], prompt[2], items);
                        }
                        else if (prompt.Length == 2)
                        {
                            Buy(prompt[1], "1", items);
                        }
                        break;


                    case "info":
                    case "i":
                        break;


                    case "storage":
                    case "st":
                        break;


                    case "password":
                    case "pw":
                        Password(nickname);
                        break;
                        

                    case "logout":
                    case "lo":
                        exit = true;
                        Console.Clear();
                        break;


                    default:
                        Program.Warning($"Invalid command: '{prompt[0]}'! Try the 'help' command.\n");
                        Console.ReadKey();
                        break;
                }
            } while (!exit);

            Program.Main();
        }


        // ~ LOAD SHOP
        public static void loadShop(string nickname, List<Item> items)
        {
            items.Clear();
            int balance = 0;

            // ~ READ EMPLOYEE DATAS
            try
            {
                string[] files = Directory.GetFiles("../../employees/", "*.txt");

                foreach (string file in files)
                {
                    if (nickname == Path.GetFileNameWithoutExtension(file))
                    {
                        string[] lines = File.ReadAllLines(file);
                        balance = Convert.ToInt32(lines[2]);
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Employees directory not found!");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Employee file(s) not found!");
            }

            // ~ READ TOOLS FROM ITEMS.TXT
            // ~ FILL UP ITEMS ARRAY
            try
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($" {balance} \n");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("Welcome to the Company store.");
                Console.WriteLine("Use words BUY and INFO on any item.");
                Console.WriteLine("Order tools in bulk by typing a number.");
                Console.WriteLine("-----------------------------\n");

                string[] itemDatas = File.ReadAllLines("../../items.txt");

                foreach (string data in itemDatas)
                {
                    if (data.Length == 0)  // ENTER
                    {
                        continue;
                    }
                    else if (data.EndsWith(":"))  // HEADER
                    {
                        Console.WriteLine($"\n{data}");
                    }
                    else  // ITEM
                    {
                        string[] item = data.Split(new string[] { ", " }, StringSplitOptions.None);
                        Item newItem = new Item(item[0], Convert.ToInt32(item[1]));
                        items.Add(newItem);
                        Console.WriteLine(newItem.ToString());
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File: 'items.txt' not found!\n");
            }
        }


        // ~ PROPMPT: HELP
        public static void Help()
        {
            Console.Clear();
            string help = File.ReadAllText("../../helpuser.txt");
            Console.WriteLine(help);
            Console.ReadKey();
        }


        public static void Buy(string chosenTool, string chosenAmount, List<Item> items)
        {
            bool found = false;

            foreach (Item item in items)
            {
                string[] tools = item.ToString().ToLower().Split(new string[] { "//" }, StringSplitOptions.None);
                string tool = tools[0].TrimStart('*', ' ').TrimEnd(' ');
                int price = Convert.ToInt32(tools[1].Replace(" price: ", ""));
                int amount = Convert.ToInt32(chosenAmount);

                //int price = Convert.ToInt32(tools[1]);

                if (tool.StartsWith(chosenTool))
                {
                    Confirm(tool, price, amount);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Program.Warning("Invalid tool! Plese check the shop.\n");
                Console.ReadKey();
            }
        }


        // ~ CONFIRM PURCHASE
        public static void Confirm(string tool, int price, int amount)
        {
            Console.Clear();
            Console.WriteLine($"You have requested to order {tool}. Amount: {amount}");
            Console.WriteLine($"Total cost of items: {price * amount}\n");
            Console.WriteLine("Please CONFIRM or DENY.\n");
            string choice = Console.ReadLine();
        }


        // ~ PROMPT: PASSWORD
        public static void Password(string nickname)
        {
            
            Console.Write("Old password: ");
            string oldPassword = Console.ReadLine();
            Console.Write("New password: ");
            string newPassword = Console.ReadLine();
            Console.Write("Confirm password: ");
            string cnfPassword = Console.ReadLine();


            // ~ LOAD USER'S FILE
            string[] employeeDatas = File.ReadAllLines($"../../auth.txt");

            for (int i = 0; i < employeeDatas.Length; i++)
            {
                string[] data = employeeDatas[i].Split(new string[] { ", " }, StringSplitOptions.None);
                if (nickname == data[0])
                {
                    if (oldPassword == data[1])
                    {
                        if (newPassword == cnfPassword)
                        {
                            data[1] = newPassword;
                            employeeDatas[i] = $"{data[0]}, {data[1]}";
                            File.WriteAllLines($"../../auth.txt", employeeDatas);
                            Console.WriteLine("Password updated successfully.");
                        }

                        else
                        {
                            Program.Warning("New and confirm password are not matching!");
                        }
                    }

                    else
                    {
                        Program.Warning("Wrong old password!");
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
