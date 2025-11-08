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

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            User.CollectUserData();
            string UserName, FullName, Password, Gender;
            DateTime BirthDate;
            RegisterUser(out UserName, out FullName, out Password, out BirthDate, out Gender);
            User.AddUser(UserName, FullName, Password, BirthDate, Gender);
            User.ShowUsers();
            User.SaveUserData();

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
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(fullName) && string.IsNullOrEmpty(password) && string.IsNullOrEmpty(birth.ToString()) && string.IsNullOrEmpty(gender))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Nem adott meg minden adatot!");
                return false;
            }
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
            for (int i = 0; i < Users.Count; i++)
            {
                string Output = $"{Users[i].UserName};{Users[i].FullName};{Users[i].Password};{Users[i].BirthDate.ToShortDateString()};{Users[i].Gender}";
                File.AppendAllText(filePath,Output + Environment.NewLine);
            }
        }
    }
    /// <summary>
    /// A VÉRNYOMÁS ADATAIT TÁROLJA(MÉRÉS IDEJE,EREDMÉNYE, qANY)
    /// </summary>
    internal class BpStore//BloodPressureStore
    {

    }

    
}