using System;
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
        public static void UserMain(string name)
        {
            bool exit = false;
            string nickname = name;

            do
            {
                loadShop();
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

                    //case "add":
                    //case "a":
                    //    if (prompt.Length == 4)
                    //    {
                    //        string name = prompt[1] + " " + prompt[2];
                    //        AddEmployee(name, prompt[3], employees);
                    //    }
                    //    else
                    //    {
                    //        Program.Warning("Invalid syntax! Try 'help' command.\n");
                    //    }
                    //    break;
                        

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
        public static void loadShop()
        {
            List<Item> items = new List<Item>();
            List<string> headers = new List<string>();


            // ~READ TOOLS FROM ITEMS.TXT
            // ~FILL UP ITEMS ARRAY
            try
            {
                Console.Clear();
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
