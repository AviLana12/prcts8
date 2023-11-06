using System.Diagnostics;
using System.Net.NetworkInformation;

namespace ConsoleApp11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RecordsTable.Table();
        }
    }
}

namespace ConsoleApp11
{
    internal class Typing
    {
        static string[] texts = new string[10];
        public static int TypingTest(string textsFile)
        {
            textsFile = "C:\\C#\\ConsoleApp11\\texts.txt";

            if (File.Exists(textsFile))
            {
                texts = File.ReadAllLines(textsFile);
            }

            else
            {
                Console.WriteLine("произошла ошибка чтения текста с файла! введите вручную путь: ");
                textsFile = Console.ReadLine();
                Typing.TypingTest(textsFile);
            }

            Random text = new Random();
            int choice = text.Next(0, texts.Length);

            Console.WriteLine(texts[choice]);
            Console.WriteLine(new string('=', 31));
            Console.WriteLine("начать - Enter");
            Console.SetCursorPosition(0, 0);

            ConsoleKeyInfo key = Console.ReadKey();

            int index = 0;

            if (key.Key == ConsoleKey.Enter)
            {
                Thread timer = new((_) =>
                {
                    Stopwatch stopwatch = new();
                    Stopwatch timer = stopwatch;
                    timer.Start();
                    TimeSpan ts;

                    do
                    {
                        ts = timer.Elapsed;
                        string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                        Console.SetCursorPosition(0, texts[choice].Length / Console.WindowWidth + 4);
                        Console.WriteLine("Время: " + elapsedTime);
                        Thread.Sleep(1000);

                    } while (ts.Seconds <= 59);

                    Console.SetCursorPosition(0, texts[choice].Length / Console.WindowWidth + 5);
                    Console.WriteLine("Стоп!");
                });

                timer.Start();

                do
                {
                    ConsoleKeyInfo letter = Console.ReadKey(true);

                    if (letter.KeyChar == texts[choice][index])
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.SetCursorPosition(index, index / Console.WindowWidth);
                        Console.Write(letter.KeyChar);
                        Console.ForegroundColor = ConsoleColor.White;

                        index++;
                    }

                } while (timer.IsAlive);

                Thread.Sleep(1000);
                Console.SetCursorPosition(0, texts[choice].Length / Console.WindowWidth + 5);
                Console.WriteLine("нажмите любую кнопку для продолжения");

                Console.ReadKey();

                Console.Clear();
            }

            return index;
        }
    }
}
namespace ConsoleApp11
{
    internal class RecordsTable
    {
        public static void Table()
        {
            Console.WriteLine("напишите имя для рекордной таблицы: ");
            string name = Console.ReadLine();
            Console.Clear();

            int index = Typing.TypingTest("");

            List<User> users = User.Serializing(name, index);

            Console.WriteLine("рекордная таблица: ");
            Console.WriteLine(new string('=', 18));

            foreach (User item in users)
            {
                Console.WriteLine($"{item.name}\t{item.charsPerMinute} символов в минуту\t{item.charsPerSecond} символ в секунду");
            }

            Console.WriteLine("начать заново - Enter");
            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter)
            {
                Table();
            }
        }
    }
}

﻿using Newtonsoft.Json;

namespace ConsoleApp11
{
    public class User
    {
        public string name;
        public int charsPerMinute; public float charsPerSecond;

        public User(string nameArg, int charsPerMinuteArg)
        {
            name = nameArg;
            charsPerMinute = charsPerMinuteArg;
            charsPerSecond = (float)charsPerMinuteArg / 60;
        }

        public static List<User> Serializing(string name, int index)
        {
            string json = File.ReadAllText("C:\\C#\\ConsoleApp11\\record.json");
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
            User user = new User(name, index);
            users.Add(user);

            json = JsonConvert.SerializeObject(users);
            File.WriteAllText("C:\\C#\\ConsoleApp11\\record.json", json);

            return users;
        }
    }
}