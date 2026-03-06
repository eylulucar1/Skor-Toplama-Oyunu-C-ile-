using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;

namespace HomeWork
{
    class Game
    {
        class DusenNesne
        {
            public int X;
            public int Y;
            public char Sembol;
        }
        static void writeLog(string message){
            try{
            using(StreamWriter writer = File.AppendText("log.txt"))
            {
                writer.WriteLine(DateTime.Now + " " +message);
            }
            }
            catch
            {
                
            }
        }
        static void Main(string[] args)
        {
            Console.CursorVisible = false; //imleci yok et.
            int playerX = 20;
            int playerY = Console.WindowHeight - 2; // Oyuncu en altta dursun
            int score = 0;

            List<DusenNesne> nesneler = new List<DusenNesne>();
            Random rnd = new Random();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.LeftArrow && playerX > 0) {
                        playerX--;
                        writeLog($"Input--> key = {key} X: {playerX} Y: {playerY} ");
                    }
                    if (key == ConsoleKey.RightArrow && playerX < Console.WindowWidth - 1) {
                        writeLog($"Input--> key = {key} X: {playerX} Y: {playerY} ");
                        playerX++;
                    }
                    if(key == ConsoleKey.Escape){
                        writeLog("Oyun bitti. Oynadiginiz icin tesekkurler!");
                        break; 
                    }
                }

                if (rnd.Next(0, 100) < 15) // %15 ihtimalle yeni nesne düşer
                {
                    var YeniNesne = new DusenNesne
                    {
                        X = rnd.Next(0, Console.WindowWidth),
                        Y = 0,
                        Sembol = rnd.Next(0, 2) == 0 ? '*' : 'O',
                    };
                    nesneler.Add(YeniNesne);
                    writeLog($"UPDATE → itemSpawned x= {YeniNesne.X} y= {YeniNesne.Y}");
                }
                foreach (var n in nesneler) {
                    n.Y++; // Nesneleri aşağı kaydır
                    writeLog($"OBJE_HAREKET: Obje({n.Sembol}) yeni konuma düştü -> X:{n.X} Y:{n.Y}");
                }

                // Oyuncu nesneyi yakaladı mı?
                for (int i = nesneler.Count - 1; i >= 0; i--)
                {
                    if (nesneler[i].X == playerX && nesneler[i].Y == playerY)
                    {
                        score++;
                        writeLog($"COLLISION → score= {score}");
                        nesneler.RemoveAt(i);
                    }
                    else if (nesneler[i].Y >= Console.WindowHeight)
                    {
                        nesneler.RemoveAt(i);
                    }
                }

                Console.Clear();
                
                // Skor Yazdır
                Console.SetCursorPosition(0, 0);
                Console.Write($"Skor: {score}");

                // Nesneleri Çiz
                foreach (var n in nesneler)
                {
                    if (n.Y < Console.WindowHeight)
                    {
                        Console.SetCursorPosition(n.X, n.Y);
                        Console.Write(n.Sembol);
                    }
                }

                // Oyuncuyu Çiz
                Console.SetCursorPosition(playerX, playerY);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("&");
                Console.ResetColor();

                Thread.Sleep(50); // ms kontrol
            }
        }
    }
}