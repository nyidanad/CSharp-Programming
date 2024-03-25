using System;
using System.IO;

namespace csharpbeadando2024_nyiridaniel_aughmi
{
    internal class Login
    {
        public static void LoginMain()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            string name = "";
            string password = "";
            bool authentication = false;


            // ~ LOGIN
            // ~ CHECK AUTHENTICAION FROM AUTH.TXT
            do
            {
                try
                {
                    Console.WriteLine("~~~~~~~~ LOGIN ~~~~~~~~");
                    Console.Write("name: ");
                    name = Console.ReadLine();
                    Console.Write("password: ");


                    // ~ CHECK EVERY KEY THAT'VE BEEN PRESSED
                    // ~ PRINT * INSTEAD OF PASSWORD
                    ConsoleKeyInfo key;
                    do
                    {
                        key = Console.ReadKey(true);

                        if (!char.IsControl(key.KeyChar) && key.Key != ConsoleKey.Enter)
                        {
                            password += key.KeyChar;
                            Console.Write("*");
                        }
                        else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                        {
                            password = password.Substring(0, password.Length - 1);
                            Console.Write("\b \b");
                        }
                    } while (key.Key != ConsoleKey.Enter);


                    // ~ CHECK AUTHENTICATION
                    string auth = name + ", " + password;
                    string[] authDatas = File.ReadAllLines("../../auth.txt");
                    foreach (string data in authDatas)
                    {
                        if (auth == data)
                        {
                            authentication = true;
                            Console.Clear();
                        }

                    }

                    if (!authentication)
                    {
                        password = "";
                        Program.Warning("\n\nUsername or password are not correct!\n" +
                                "Please check your datas!             ");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("File: 'auth.txt' not found!\n");
                }
            } while (!authentication);


            // ~ HANDLE MAINS
            switch (name)
            {
                case "admin":
                    Admin.AdminMain();
                    break;
                default:
                    User.UserMain(name);
                    break;
            }
        }
    }
}
