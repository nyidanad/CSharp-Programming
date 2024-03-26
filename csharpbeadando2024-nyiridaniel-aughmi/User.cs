using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace csharpbeadando2024_nyiridaniel_aughmi
{
    internal class User
    {
        public static void UserMain(string nickname)
        {
            List<Item> items = new List<Item>();
            Employee employee;
            bool exit = false;


            do
            {
                bool found = false;
                employee = loadEmployee(nickname);
                loadShop(employee, items);
                Console.Write("\n$ ");
                string[] prompt = Console.ReadLine().Split(' ');


                // ~ BUY, INFO PROMPT
                if (prompt[0].Length > 2)
                {
                    foreach (Item item in items)
                    {
                        if (item.Tool.ToLower().StartsWith(prompt[0].ToLower()))
                        {
                            found = true;

                            if (prompt.Length == 2)
                            {
                                if (prompt[1] == "i" || prompt[1] == "info")
                                {
                                    Info(item.Tool.ToLower());
                                    break;

                                }
                                else
                                {
                                    Buy(prompt[0], prompt[1], nickname, items, employee);
                                    break;
                                }
                            }
                            else if (prompt.Length == 1)
                            {
                                Buy(prompt[0], "1", nickname, items, employee);
                                break;
                            }
                        }
                    }
                }
                

                // ~ OTHER PROMPTS
                if (!found)
                {
                    switch (prompt[0])
                    {
                        case "help":
                        case "h":
                            Help();
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
                            Program.Warning($"Invalid command! Try the 'help' command.");
                            Console.ReadKey();
                            break;
                    }
                }
            } while (!exit);

            Program.Main();
        }

        // ~ LOAD EMPLOYEE DATAS
        public static Employee loadEmployee(string nickname)
        {
            string name = "";
            int balance = 0;

            try
            {
                string[] files = Directory.GetFiles("../../employees/", "*.txt");

                foreach (string file in files)
                {
                    if (nickname == Path.GetFileNameWithoutExtension(file))
                    {
                        string[] lines = File.ReadAllLines(file);
                        name = lines[1];
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

            Employee employee = new Employee(nickname, name, balance);
            return employee;
        }


        // ~ LOAD SHOP
        public static void loadShop(Employee employee, List<Item> items)
        {
            items.Clear();

            // ~ READ TOOLS FROM ITEMS.TXT
            // ~ FILL UP ITEMS ARRAY
            try
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($" {employee.Balance} \n");
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


        // ~ PROMPT: BUY
        public static void Buy(string chosenTool, string chosenAmount, string nickname, List<Item> items, Employee employee)
        {
            bool found = false;

            foreach (Item item in items)
            {
                string[] tools = item.ToString().ToLower().Split(new string[] { "//" }, StringSplitOptions.None);
                string tool = tools[0].TrimStart('*', ' ').TrimEnd(' ');
                int price = Convert.ToInt32(tools[1].Replace(" price: ", ""));
                int amount = Convert.ToInt32(chosenAmount);

                if (tool.StartsWith(chosenTool))
                {
                    found = true;

                    if (employee.Balance >= (price * amount))
                    {
                        Confirm(tool, price, amount, nickname, employee);
                    }
                    else
                    {
                        Program.Warning("Not enough money to purchase!");
                        Console.ReadKey();
                    }
                    
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
        public static void Confirm(string tool, int price, int amount, string nickname, Employee employee)
        {
            bool exit = false;

            do
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($" {employee.Balance} \n");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine($"You have requested to order {tool}. Amount: {amount}");
                Console.WriteLine($"Total cost of items: {price * amount}\n");
                Console.WriteLine("Please CONFIRM or DENY.");
                Console.Write("\n$ ");
                string prompt = Console.ReadLine();

                switch (prompt)
                {
                    case "confirm":
                    case "c":
                        exit = true;

                        try
                        {
                            string[] fileData = File.ReadAllLines($"../../employees/{nickname}.txt");
                            List<string> tmp = new List<string>();

                            for (int i = 0; i < fileData.Length; i++)
                            {
                                if (i == 2)
                                {
                                    tmp.Add((Convert.ToInt32(fileData[i]) - (price * amount)).ToString());
                                    continue;
                                }
                                tmp.Add(fileData[i]);
                                
                            }

                            File.WriteAllLines($"../../employees/{nickname}.txt", tmp);
                            

                            Transfer(tool, amount, nickname);
                        }
                        catch (DirectoryNotFoundException)
                        {
                            Console.WriteLine("Employees directory not found!");
                        }
                        catch (FileNotFoundException)
                        {
                            Console.WriteLine("Employee file(s) not found!");
                        }
                        break;


                    case "deny":
                    case "d":
                        exit = true;
                        break;


                    default:
                        Program.Warning($"Invalid command! Use 'confirm' or 'deny'.");
                        Console.ReadKey();
                        break;
                }
            } while (!exit);
        }


        // ~ ADD PURCHASE TO EMPLOYEE'S TXT
        public static void Transfer(string tool, int amount, string nickname)
        {
            try
            {
                List<string> tmp = new List<string>();
                File.AppendAllText($"../../employees/{nickname}.txt", $"{tool} [{amount}]");

                string[] lines = File.ReadAllLines($"../../employees/{nickname}.txt");
                int sumAmount = 0;
                string sumTool = "";


                // ~ SUM PURCHASED TOOL AMOUNT
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] item = lines[i].Split(' ');

                    if (item.Length == 2 &&item[0].StartsWith(tool))
                    {
                        sumAmount += Convert.ToInt32(item[1].Trim('[', ']'));
                        sumTool = $"{tool} [{sumAmount}]";
                    }
                }


                // ~ REWRITE FILE WITH SUM AMOUNT
                foreach (string line in lines)
                {
                    string[] item = line.Split(' ');

                    if (tmp.Any(tmpLine => tmpLine.StartsWith(item[0])))
                    {
                        continue;
                    }
                    else if (item[0].StartsWith(tool))
                    {
                        tmp.Add(sumTool);
                    }
                    else
                    {
                        tmp.Add(line);
                    }
                }

                File.WriteAllLines($"../../employees/{nickname}.txt", tmp);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Employees directory not found!");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"{nickname} file not found!");
            }
        }


        // ~ PROMPT: PASSWORD
        public static void Password(string nickname)
        {
            
            Console.Write("Old password: ");
            string oldPassword = Login.Password();
            Console.Write("\nNew password: ");
            string newPassword = Login.Password();
            Console.Write("\nConfirm password: ");
            string cnfPassword = Login.Password();


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
                            Program.Success("\nPassword updated successfully.");
                        }

                        else
                        {
                            Program.Warning("\nNew and confirm password are not matching!");
                        }
                    }

                    else
                    {
                        Program.Warning("\nWrong old password!");
                    }
                }
            }
            Console.ReadKey();
        }


        // ~ PROMPT: INFO
        public static void Info(string tool)
        {
            try
            {
                Console.Clear();
                string info = File.ReadAllText($"../../infos/{tool}.txt");
                Console.WriteLine(info);
                
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"{tool} file not found!");
            }
            Console.ReadKey();
        }
    }
}
