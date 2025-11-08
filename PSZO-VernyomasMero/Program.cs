// See https://aka.ms/new-console-template for more information

using System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
 
namespace PSZO_VernyomasMero
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                int choice = 0;
                Console.WriteLine("1.Regisztrálás");
                Console.WriteLine("2. Bejelentkezés");
                choice = int.Parse(Console.ReadLine());
                User.CollectUserData();
                string UserName, FullName, Password, Gender;
                DateTime BirthDate;
                if (choice == 1)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Regisztráció");
                    Console.ForegroundColor = ConsoleColor.White;
                    do
                    {
                        RegisterUser(out UserName, out FullName, out Password, out BirthDate, out Gender);
                    } while (!User.ValidateUserData(UserName, FullName, Password, BirthDate, Gender));
                    User.AddUser(UserName, FullName, Password, BirthDate, Gender);
                    User.ShowUsers();
                    User.SaveUserData();
                }
                else if (choice == 2)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Bejelentkezés");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Adja meg a felhasználó nevét: ");
                    string LoginUserName = Console.ReadLine();
                    Console.Write("Adja meg a jelszavát: ");
                    string LoginPassword = Console.ReadLine();
                    int check = 0;
                    for (int i = 0; i < User.Users.Count; i++)
                    {
                        if (User.Users[i].UserName == LoginUserName && User.Users[i].Password == LoginPassword)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Sikeres bejelentkezés!");
                            Console.ForegroundColor = ConsoleColor.White;
                            Thread.Sleep(2000);
                            break;
                        }
                        else
                        {
                            check++;
                            if (check == User.Users.Count)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Sikertelen bejelentkezés!");
                                Console.ForegroundColor = ConsoleColor.White;
                                Thread.Sleep(2000);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Nincs ilyen menüpont!");
                }
            }

            static void RegisterUser(out string UserName, out string FullName, out string Password, out DateTime BirthDate, out string Gender)
            {
                UserName = "";
                FullName = "";
                Password = "";
                BirthDate = DateTime.Now;
                Gender = "";
                Console.Write("Adja meg a felhasználó nevét: ");
                UserName = Console.ReadLine();
                Console.Write("Adja meg a teljes nevét: ");
                FullName = Console.ReadLine();
                Console.Write("Adja meg a jelszavát: ");
                Password = Console.ReadLine();
                Console.Write("Adja meg a születési dátumát (ÉÉÉÉ-HH-NN): ");
                string BirthDateInput = Console.ReadLine();
                while (!DateTime.TryParse(BirthDateInput, out DateTime birth))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Nem jól adta meg a dátumot! Írja be újra: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    BirthDateInput = Console.ReadLine();
                }
                BirthDate = Convert.ToDateTime(BirthDateInput);
                Console.Write("Adja meg a nemét (Férfi/Nő): ");
                Gender = Console.ReadLine();
            }
        }
    }
    /// <summary>
    /// A FELHASZNÁLÓ ÉS ANNAK ADATAI
    /// </summary>
    internal class User
    {
        public string UserName;
        public string FullName;
        public string Password;
        public string Gender;
        public DateTime BirthDate;
        static public List<User> Users = new List<User>();

        //User data összeszedése
        public static void CollectUserData()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectPath = Path.GetFullPath(Path.Combine(basePath, @"..\..\.."));
            string filePath = Path.Combine(projectPath, "UserData.txt");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Nincs UserData.txt fájl a projekt mappában!");
                return;
            }
            string[] UserData = File.ReadAllLines(filePath);
            for (int i = 0; i < UserData.Length; i++)
            {
                string[] Data = UserData[i].Split(';');
                User NewUser = new User();
                NewUser.UserName = Data[0];
                NewUser.FullName = Data[1];
                NewUser.Password = Data[2];
                NewUser.BirthDate = DateTime.Parse(Data[3]);
                NewUser.Gender = Data[4];
                Users.Add(NewUser);
            }
        }
        //user adatok kiíratása
        public static void ShowUsers()
        {
            foreach (var user in Users)
            {
                Console.WriteLine($"Felhasználónév: {user.UserName}, Teljes név: {user.FullName}, Születési dátum: {user.BirthDate.ToShortDateString()}, Nem: {user.Gender}");
            }
        }

        //új user hozzáadása
        public static void AddUser(string userName, string fullName, string password, DateTime birth, string gender)
        {
            User NewUser = new User();
            NewUser.UserName = userName;
            NewUser.FullName = fullName;
            NewUser.Password = password;
            NewUser.BirthDate = birth;
            NewUser.Gender = gender;
            Users.Add(NewUser);
        }

        //user adatok ellenörzése hogy nem semmit írt e be
        public static bool ValidateUserData(string userName, string fullName, string password, DateTime birth, string gender)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("A felhasználónév nem lehet üres!");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            if (string.IsNullOrWhiteSpace(fullName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("A teljes név nem lehet üres!");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("A jelszó nem lehet üres!");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            if (birth > DateTime.Now)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("A születési dátum nem lehet a jövőben!");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            if (string.IsNullOrWhiteSpace(gender) || !(gender.ToLower() == "férfi" || gender.ToLower() == "nő"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("A nem csak 'Férfi' vagy 'Nő' lehet!");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            return true;
        }

        //user adatainak fájlba írása
        public static void SaveUserData()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectPath = Path.GetFullPath(Path.Combine(basePath, @"..\..\.."));
            string filePath = Path.Combine(projectPath, "UserData.txt");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Nincs UserData.txt fájl a projekt mappában!");
                return;
            }
            List<string> UsersOutput = new List<string>();
            foreach (var user in Users)
            {
                UsersOutput.Add($"{user.UserName};{user.FullName};{user.Password};{user.BirthDate.ToShortDateString()};{user.Gender}");
            }
            File.WriteAllLines(filePath, UsersOutput);
        }
    }
    /// <summary>
    /// A VÉRNYOMÁS ADATAIT TÁROLJA(MÉRÉS IDEJE,EREDMÉNYE, qANY)
    /// </summary>
    internal class BpStore//BloodPressureStore
    {

    }

    
}