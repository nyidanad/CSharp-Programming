using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace csharpbeadando2024_nyiridaniel_aughmi
{
    internal class Admin
    {
        public static void AdminMain()
        {
            ListDictionary employees = new ListDictionary();
            employees = LoadEmployees(employees);
            bool exit = false;

            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("admin:~$ ");
                Console.ForegroundColor = ConsoleColor.White;
                string[] prompt = Console.ReadLine().Split(' ');

                switch (prompt[0])
                {
                    case "help":
                    case "h":
                        Help();
                        break;


                    case "list":
                    case "ls":
                        List(employees);
                        break;


                    case "add":
                    case "a":
                        if (prompt.Length == 4)
                        {
                            string name = prompt[1] + " " + prompt[2];
                            AddEmployee(name, prompt[3], employees);
                        }
                        else
                        {
                            Program.Warning("Invalid syntax! Try 'help' command.\n");
                        }
                        break;


                    case "delete":
                    case "d":
                        if (prompt.Length == 2)
                        {
                            if (employees.Contains(Convert.ToInt32(prompt[1])))
                            {
                                Delete(prompt[1], employees);
                            }
                            else
                            {
                                Program.Warning("Invalid id! Try $ 'list' command.\n");
                            }
                        }
                        else
                        {
                            Program.Warning("Invalid syntax! Try 'help' command.\n");
                        }
                        break;


                    case "balance":
                    case "b":
                        if (prompt.Length == 3)
                        {
                            if (employees.Contains(Convert.ToInt32(prompt[1])))
                            {
                                Balance(prompt[1], prompt[2], employees);
                            }
                            else
                            {
                                Program.Warning("Invalid id! Try $ 'list' command.\n");
                            }
                        }
                        else
                        {
                            Program.Warning("Invalid syntax! Try 'help' command.\n");
                        }
                        break;


                    case "clear":
                    case "c":
                        Console.Clear();
                        break;


                    case "logout":
                    case "lo":
                        exit = true;
                        Console.Clear();
                        break;
                        

                    default:
                        Program.Warning($"Invalid command: '{prompt[0]}'! Try the 'help' command.\n");
                        break;
                }
            } while (!exit);

            Program.Main();
        }


        // ~ LOAD EMPLOYEES INTO LISTDICTIONARY
        public static ListDictionary LoadEmployees(ListDictionary employees)
        {
            employees.Clear();
            try
            {
                string[] files = Directory.GetFiles("../../employees/", "*.txt");

                foreach (string file in files)
                {
                    string nickname = Path.GetFileNameWithoutExtension(file);

                    string[] lines = File.ReadAllLines(file);
                    int id = Convert.ToInt32(lines[0]);
                    string name = lines[1];
                    int balance = Convert.ToInt32(lines[2]);

                    employees.Add(id, new Employee(nickname, name, balance));
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

            return employees;
        }


        // ~ PROMPT: HELP
        public static void Help()
        {
            string help = File.ReadAllText("../../helpadmin.txt");
            Console.WriteLine(help);
        }


        // ~ PROMPT: List
        public static void List(ListDictionary employees)
        {
            foreach (DictionaryEntry employee in employees)
            {
                Console.WriteLine($"{(employee.Key).ToString().PadLeft(3, '0')} | {employee.Value}");
            }
        }


        // ~ PROMPT: ADD
        public static void AddEmployee(string name, string password, ListDictionary employees)
        {
            try
            {
                string[] tmp = name.Split(' ');
                string lastName = tmp[0].ToLower().Substring(0, 3);
                string firstName = tmp[1].ToLower().Substring(0, 3);
                List<int> ids = new List<int>();
                List<int> keys = new List<int>();

                // ~ CREATE NICKNAME
                string nickname = $"{lastName}{firstName}";

                foreach (DictionaryEntry employee in employees)
                {
                    if (employee.Value.ToString().Substring(1, 6) == nickname)
                    {
                        string[] value = employee.Value.ToString().Split(' ');
                        ids.Add(Convert.ToInt32((value[0][7]).ToString()));
                    }
                    keys.Add(Convert.ToInt32(employee.Key));
                }

                int key =  keys.Count > 0 ? keys.Max() + 1 : 0;
                int id = ids.Count > 0 ? ids.Max() + 1 : 1;

                nickname = $"{lastName}{firstName}{id}";


                // ~ADD THE NEW EMPLOYEE TO AUTH.TXT
                string auth = $"{nickname}, {password}\n";
                File.AppendAllText("../../auth.txt", auth);
                
                //// ~ ADD THE NEW EMPLOYEE TO EMPLOYEES
                string text = $"{key}\n{name}\n{0}\n";
                File.WriteAllText($"../../employees/{nickname}.txt", text);
                Program.Success($"User: {nickname} added successfully!\n");

                // ~ RELOADING THE EMPLOYEES
                LoadEmployees(employees);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Employees directory not found!");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File: 'auth.txt' not found!\n");
            }
        }


        // ~ PROMPT: DELETE
        public static void Delete(string key, ListDictionary employees)
        {
            try
            {
                string value = "";

                foreach (DictionaryEntry employee in employees)
                {
                    if (employee.Key.Equals(Convert.ToInt32(key)))
                    {
                        value = employee.Value.ToString();
                    }
                }

                string[] datas = value.Split(' ');
                string nickname = datas[0].Trim('[', ']');


                // ~ DELETE FROM EMPLOYEES FOLDER
                File.Delete($"../../employees/{nickname}.txt");


                // ~ DELETE LINE FROM AUTH.TXT
                string[] employeeDatas = File.ReadAllLines("../../auth.txt");
                List<string> tmp = new List<string>();

                foreach (string employeeData in employeeDatas)
                {
                    string[] data = employeeData.Split(new string[] { ", " }, StringSplitOptions.None);
                    if (nickname != data[0])
                    {
                        tmp.Add(employeeData);
                    }
                }

                File.WriteAllLines("../../auth.txt", tmp);
                Program.Success($"{nickname} deleted successfully!\n");


                // ~ RELOADING THE EMPLOYEES
                LoadEmployees(employees);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Employees directory not found!");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Employee file(s) not found!");
            }
        }


        // ~ PROMPT: BALANCE
        public static void Balance(string key, string price, ListDictionary employees)
        {
            try
            {
                string value = "";

                foreach (DictionaryEntry employee in employees)
                {
                    if (employee.Key.Equals(Convert.ToInt32(key)))
                    {
                        value = employee.Value.ToString();
                    }
                }

                string[] datas = value.Split(' ');
                string nickname = datas[0].Trim('[', ']');
                string name = $"{datas[1]} {datas[2]}";
                int balance = Convert.ToInt32(datas[4]) + Convert.ToInt32(price);

                string text = $"{key}\n{name}\n{balance}\n";
                File.WriteAllText($"../../employees/{nickname}.txt", text);
                Program.Success($"{nickname} balance updated successfully!\n");

                // ~ RELOADING THE EMPLOYEES
                LoadEmployees(employees);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Employees directory not found!");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Employee file(s) not found!");
            }
        }
    }
}
