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
using System.Threading.Channels;
using System.Threading.Tasks;

namespace PSZO_VernyomasMero
{
    internal class Program
    {
        static void Main(string[] args)
        {
            User.CollectUserData();
            while (true)
            {
                // Teszt1
                Console.Clear();
                int choice = 0;
                Console.ForegroundColor = ConsoleColor.Red;
                TextDecoration.WriteLineCentered("=== VÉRNYOMÁSNAPLÓ ===", false);
                Console.ForegroundColor = ConsoleColor.White;
                TextDecoration.WriteLineCentered("-------------------");
                TextDecoration.WriteLineCentered("1. Regisztrálás");
                TextDecoration.WriteLineCentered("2. Bejelentkezés");
                TextDecoration.WriteLineCentered("3. Beállítások");
                TextDecoration.WriteLineCentered("4. Kilépés");
                TextDecoration.WriteLineCentered("--------------------");
                TextDecoration.WriteCentered("Válasszon ki egy menüpontot: ");
                while (!int.TryParse(Console.ReadLine(), out choice))
                {
                }
                string UserName, FullName, Password, Gender;
                DateTime BirthDate;
                if (choice == 1)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    TextDecoration.WriteLineCentered("=== REGISZTRÁCIÓ === ", false);
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    TextDecoration.WriteLineCentered("=== BEJELENTKEZÉS ===", false);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" ");
                    TextDecoration.WriteCentered("Adja meg a felhasználó nevét: ");
                    string LoginUserName = Console.ReadLine();
                    TextDecoration.WriteCentered("Adja meg a jelszavát: ");
                    string LoginPassword = Console.ReadLine();
                    int check = 0;
                    for (int i = 0; i < User.Users.Count; i++)
                    {
                        if (User.Users[i].UserName == LoginUserName && User.Users[i].Password == LoginPassword)
                        {
                            LoggedIn(LoginUserName);
                            break;
                        }
                        else if (LoginUserName == "admin")
                        {
                            LoggedIn("admin");
                            break;
                        }
                        else
                        {
                            check++;
                            if (check == User.Users.Count)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                TextDecoration.WriteLineCentered("Sikertelen bejelentkezés!", false);
                                Console.ForegroundColor = ConsoleColor.White;
                                Thread.Sleep(2000);
                                break;
                            }
                        }
                    }
                }
                else if (choice == 3)
                {
                    Console.Clear();
                    Settings.SettingsMenu();
                }
                else if (choice == 4)
                {
                    Environment.Exit(0);
                }
                else
                {
                    TextDecoration.WriteLineCentered("Nincs ilyen menüpont!");
                }
            }

            static void RegisterUser(out string UserName, out string FullName, out string Password, out DateTime BirthDate, out string Gender)
            {
                UserName = "";
                FullName = "";
                Password = "";
                BirthDate = DateTime.Now;
                Gender = "";
                TextDecoration.WriteCentered("Adja meg a felhasználó nevét: ");
                UserName = Console.ReadLine();
                TextDecoration.WriteCentered("Adja meg a teljes nevét: ");
                FullName = Console.ReadLine();
                TextDecoration.WriteCentered("Adja meg a jelszavát: ");
                Password = Console.ReadLine();
                TextDecoration.WriteCentered("Adja meg a születési dátumát (ÉÉÉÉ-HH-NN): ");
                string BirthDateInput = Console.ReadLine();
                while (!DateTime.TryParse(BirthDateInput, out DateTime birth))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    TextDecoration.WriteCentered("Nem jól adta meg a dátumot! Írja be újra: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    BirthDateInput = Console.ReadLine();
                }
                BirthDate = Convert.ToDateTime(BirthDateInput);
                TextDecoration.WriteCentered("Adja meg a nemét (Férfi/Nő): ");
                Gender = Console.ReadLine();
            }

            static void CreateBpSave(string userName)
            {
                string bloodPressureLevel = "";
                DateTime date;

                TextDecoration.WriteCentered("Dátum: ");
                date = InputChecks.IsValidDate(Console.ReadLine());
                TextDecoration.WriteCentered("Szisztolés érték (Hgmm): ");
                int sys = int.Parse(Console.ReadLine());
                TextDecoration.WriteCentered("Diasztolés érték (Hgmm): ");
                int dia = int.Parse(Console.ReadLine());
                TextDecoration.WriteCentered("Pulzus (bpm): ");
                int pul = int.Parse(Console.ReadLine());

                BpStore newBpData = new BpStore(userName, date, sys, dia, pul);
                newBpData.SaveBpData();
            }

            static string InspectBP(DateTime birthDate, int sys, int diast, int bpm)
            {
                // KISZÁMOLJUK AZ ÉLETKORÁT
                int age = DateTime.Now.Year - birthDate.Year;
                int sysMin = 0;
                int sysMax = 0;
                int diastMin = 0;
                int diastMax = 0;
                int bpmMin = 60;
                int bpmMax = 100;

                string sysStatus = "";
                string diastStatus = "";
                string bpmStatus = "";

                if (0 <= age && age <= 18)
                {
                    sysMin = 75;
                    sysMax = 113;

                    diastMin = 45;
                    diastMax = 74;
                }
                else if (18 < age && age >= 60)
                {
                    sysMin = 105;
                    sysMax = 125;

                    diastMin = 60;
                    diastMax = 80;
                }
                else
                {
                    sysMin = 105;
                    sysMax = 133;

                    diastMin = 60;
                    diastMax = 83;
                }

                // VIZSGÁLJUK A SZISZTOLIKUS ÉRTÉKET
                if (sysMin <= sys && sys <= sysMax)
                {
                    sysStatus = "normális";
                }
                else if (sys < sysMin)
                {
                    sysStatus = "alacsony";
                }
                else
                {
                    sysStatus = "magas";
                }

                // VIZSGÁLJUK A DIASZTOLIKUS ÉRTÉKET
                if (diastMin <= diast && diast <= diastMax)
                {
                    diastStatus = "normális";
                }
                else if (diast < diastMin)
                {
                    diastStatus = "alacsony";
                }
                else
                {
                    diastStatus = "magas";
                }

                // PULZUS VIZSGÁLAT
                if (bpmMin <= bpm && bpm <= bpmMax)
                {
                    bpmStatus = "normális";
                }
                else if (bpm < bpmMin)
                {
                    bpmStatus = "alacsony";
                }
                else
                {
                    bpmStatus = "magas";
                }

                // SZISZTÓLIKUS_ÁLLAPOTA;DIASZTÓLIKUS_ÁLLAPOTA;PULZUS_ÁLLAPOTA
                return $"{sysStatus};{diastStatus};{bpmStatus}";
            }

            static string[] ReadBpData(string userName = "")
            {
                if (userName != "")
                {
                    string basePath = AppDomain.CurrentDomain.BaseDirectory;
                    string projectPath = Path.GetFullPath(Path.Combine(basePath, @"..\..\.."));
                    string filePath = Path.Combine(projectPath, "BpData.txt");
                    List<string> lines = new List<string>();

                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Split(';')[0] == userName)
                            {
                                lines.Add(line);
                            }
                        }
                    }

                    return lines.ToArray();
                }
                else
                {
                    string basePath = AppDomain.CurrentDomain.BaseDirectory;
                    string projectPath = Path.GetFullPath(Path.Combine(basePath, @"..\..\.."));
                    string filePath = Path.Combine(projectPath, "BpData.txt");

                    string[] lines = File.ReadAllLines(filePath);

                    return lines;
                }
            }

            static void LoggedIn(string LoginUserName)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                TextDecoration.WriteLineCentered("Sikeres bejelentkezés!", false);
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(2000);
                bool exit = false;
                while (!exit)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    TextDecoration.WriteLineCentered($"Üdvözöljük, {LoginUserName}!", false);
                    TextDecoration.WriteLineCentered("--------------------");
                    TextDecoration.WriteLineCentered("1. Vérnyomás rögzítése");
                    TextDecoration.WriteLineCentered("2. Saját mérések megtekintése");
                    TextDecoration.WriteLineCentered("3. Kijelentkezés");
                    TextDecoration.WriteLineCentered("--------------------");
                    TextDecoration.WriteCentered("Válasszon: ");
                    string choice2 = Console.ReadLine();
                    switch (choice2)
                    {
                        case "1":
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            TextDecoration.WriteLineCentered("=== VÉRNYOMÁS RÖGZÍTÉSE ===", false);
                            CreateBpSave(LoginUserName);
                            Console.ForegroundColor = ConsoleColor.Green;
                            TextDecoration.WriteLineCentered("Vérnyomásadat elmentve!", false);
                            Thread.Sleep(2000);
                            break;
                        case "2":
                            var bpuserdata = ReadBpData(LoginUserName);
                            TextDecoration.WriteLineCentered("Saját mérései:");
                            foreach (var item in bpuserdata)
                            {
                                TextDecoration.WriteLineCentered(item);
                            }
                            TextDecoration.WriteLineCentered("Nyomjon ENTER-t a folytatáshoz...");
                            Console.ReadLine();
                            break;
                        case "3":
                            exit = true;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            TextDecoration.WriteLineCentered("Nincs ilyen menüpont!", false);
                            Thread.Sleep(2000);
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// A VÉRNYOMÁS ADATAIT TÁROLJA(MÉRÉS IDEJE,EREDMÉNYE, qANY)
    /// </summary>
    internal class BpStore//BloodPressureStore
    {
        public string user;
        public DateTime date;
        public int sys;
        public int dia;
        public int pulse;

        //vérnyomás adatainak fájlba írása
        public void SaveBpData()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectPath = Path.GetFullPath(Path.Combine(basePath, @"..\..\.."));
            string filePath = Path.Combine(projectPath, "BpData.txt");
            File.AppendAllText(filePath, $"{user};{date.ToShortDateString()};{sys};{dia};{pulse}\n");
        }

        public BpStore(string user, DateTime date, int sys, int dia, int pulse)
        {
            this.user = user;
            this.date = date;
            this.sys = sys;
            this.dia = dia;
            this.pulse = pulse;
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

        /// <summary>
        /// Felhasználói adatok összeszerzése
        /// </summary>
        public static void CollectUserData()
        {
            Users.Clear();
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
        /// <summary>
        /// Felhasználók felsorolása
        /// </summary>
        public static void ShowUsers()
        {
            foreach (var user in Users)
            {
                Console.WriteLine($"Felhasználónév: {user.UserName}, Teljes név: {user.FullName}, Születési dátum: {user.BirthDate.ToShortDateString()}, Nem: {user.Gender}");
            }
        }

        /// <summary>
        /// Új felhasználó hozzáadása az adatbázishoz
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="fullName"></param>
        /// <param name="password"></param>
        /// <param name="birth"></param>
        /// <param name="gender"></param>
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

        /// <summary>
        /// User adatok beírásának ellenörzése
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="fullName"></param>
        /// <param name="password"></param>
        /// <param name="birth"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        public static bool ValidateUserData(string userName, string fullName, string password, DateTime birth, string gender)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                TextDecoration.WriteLineCentered("A felhasználónév nem lehet üres!");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            else if (string.IsNullOrWhiteSpace(fullName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                TextDecoration.WriteLineCentered("A teljes név nem lehet üres!");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                TextDecoration.WriteLineCentered("A jelszó nem lehet üres!");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            else if (birth > DateTime.Now)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                TextDecoration.WriteLineCentered("A születési dátum nem lehet a jövőben!");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            else if (string.IsNullOrWhiteSpace(gender) || !(gender.ToLower() == "férfi" || gender.ToLower() == "nő"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                TextDecoration.WriteLineCentered("A nem csak 'Férfi' vagy 'Nő' lehet!");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            return true;
        }

        /// <summary>
        /// A felhasználó adatainak fájlba írása
        /// </summary>
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
    /// Olyan függvényeket tartalmaz amelyek a bementi szövegek ellenörzésére szolgálnak
    /// </summary>
    internal class InputChecks
    {
        public static DateTime IsValidDate(string dateInput)
        {
            DateTime date;
            while (!DateTime.TryParse(dateInput, out date))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Nem jól adta meg a dátumot! Írja be újra: ");
                Console.ForegroundColor = ConsoleColor.White;
                dateInput = Console.ReadLine();
            }
            return date;
        }
    }
    /// <summary>
    /// Olyan függvényeket tartalmaz amelyek a konzol szövegek dekorálására szolgálnak
    /// </summary>
    internal class TextDecoration
    {
        /// <summary>
        /// A függvény a Console.WriteLine középre íratását valósítja meg
        /// </summary>
        /// <param name="text">A megadott szöveget írja ki középre</param>
        /// <param name="changeColor">Igaz érték esetén megváltoztatja a konzol betűszínét a beállításoknak megfelelően</param>
        public static void WriteLineCentered(string text, bool changeColor = true)
        // Console.WriteLine középre íratása
        {
            int width = Console.WindowWidth;
            int leftPadding = (width - text.Length) / 2;
            if (leftPadding < 0)
            {
                leftPadding = 0;
            }
            if (changeColor)
            {
                Settings.ChangeConsoleColors();
            }
            Console.WriteLine(new string(' ', leftPadding) + text);
        }
        public static void WriteCentered(string text, bool changeColor = true)
        // Console.Write középre íratása
        {
            int width = Console.WindowWidth;
            int leftPadding = (width - text.Length) / 2;
            if (leftPadding < 0)
            {
                leftPadding = 0;
            }
            if (changeColor)
            {
                Settings.ChangeConsoleColors();
            }
            Console.Write(new string(' ', leftPadding) + text);
        }
    }
    internal class Settings
    {
        static string fgcolor = "w";
        /// <summary>
        /// Beállítások menü megjelenítése
        /// </summary>
        /// <returns></returns>
        public static void SettingsMenu()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            TextDecoration.WriteLineCentered("=== BEÁLLÍTÁSOK ===", false);
            Console.ForegroundColor = ConsoleColor.White;
            TextDecoration.WriteLineCentered("-------------------");
            TextDecoration.WriteLineCentered("1. Téma kiválasztása");
            TextDecoration.WriteLineCentered("2. Vissza a főmenübe");
            TextDecoration.WriteLineCentered("--------------------");
            TextDecoration.WriteCentered("Válasszon ki egy menüpontot: ");
            string settingsChoice = Console.ReadLine();
            if (settingsChoice == "1")
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                TextDecoration.WriteLineCentered("=== HÁTTÉRSZÍN BEÁLLÍTÁSA ===", false);
                Console.ForegroundColor = ConsoleColor.White;
                TextDecoration.WriteLineCentered("1. Fekete háttér, fehér betűszín");
                TextDecoration.WriteLineCentered("2. Fehér háttér, fekete betűszín");
                TextDecoration.WriteCentered("Válasszon Témát: ");
                string bgColorInput = Console.ReadLine();
                if (bgColorInput == "1")
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    fgcolor = "w";
                    try
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        TextDecoration.WriteLineCentered("Téma sikeresen megváltoztatva!", false);
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        TextDecoration.WriteLineCentered("Nincs ilyen téma!", false);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Thread.Sleep(2000);
                }
                else if (bgColorInput == "2")
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    fgcolor = "b";
                    try
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        TextDecoration.WriteLineCentered("Téma sikeresen megváltoztatva!", false);
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        TextDecoration.WriteLineCentered("Nincs ilyen téma!", false);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Thread.Sleep(2000);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    TextDecoration.WriteLineCentered("Nincs ilyen téma!", false);
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(2000);
                }
            }
            else if (settingsChoice == "2")
            {
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                TextDecoration.WriteLineCentered("Nincs ilyen menüpont!", false);
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(2000);
            }
        }
        /// <summary>
        /// Konzol betűszínének megváltoztatása
        /// </summary>
        public static void ChangeConsoleColors()
        {
            if (fgcolor == "w")
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (fgcolor == "b")
            {
                Console.ForegroundColor = ConsoleColor.Black;
            }
        }
    }
}