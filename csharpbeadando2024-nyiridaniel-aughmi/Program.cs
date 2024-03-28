using System;
using System.Media;

namespace csharpbeadando2024_nyiridaniel_aughmi
{
    internal class Program
    {
        public static void Main()
        {
            Login.LoginMain();
        }


        // ~ SUCCESS MESSAGE
        public static void Success(string message)
        {
            SoundPlayer warning = new SoundPlayer("../../sounds/success.wav");
            warning.Play();
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
        }


        // ~ WARNING MESSAGE
        public static void Warning(string message)
        {
            SoundPlayer warning = new SoundPlayer("../../sounds/warning.wav");
            warning.Play();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
        }


        // ~ PURCHASE SOUND
        public static void Purchase()
        {
            SoundPlayer purchase = new SoundPlayer("../../sounds/purchase.wav");
            purchase.Play();
        }
    }
}
