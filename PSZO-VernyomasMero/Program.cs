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
            string[] files = { "UserData.txt", "BpData.txt" };
            foreach (var item in files)
            {
                if (!File.Exists(item))
                {
                    Console.WriteLine("UserData vagy BpData nem létezik!");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
            User.CollectUserData();
            while (true)
            {
                string[] MainMenu = { "Regisztrálás", "Bejelentkezés", "Beállítások", "Kilépés" };
                int mainmenuchoice = MenuControll.ArrowMenu(MainMenu, "=== VÉRNYOMÁSNAPLÓ ===");
                string UserName, FullName, Password, Gender;
                DateTime BirthDate;
                switch (mainmenuchoice)
                {
                    case 0:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        TextDecoration.WriteLineCentered("=== REGISZTRÁCIÓ ===", false);
                        Console.ForegroundColor = ConsoleColor.White;
                        RegisterUser(out UserName, out FullName, out Password, out BirthDate, out Gender);
                        User.AddUser(UserName, FullName, Password, BirthDate, Gender);
                        User.ShowUsers();
                        User.SaveUserData();
                        break;
                    case 1:
                        User LoggedInUser = new User();
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
                                LoggedIn(LoginUserName, LoggedInUser);
                                LoggedInUser = User.Users[i];
                                break;
                            }
                            else if (LoginUserName == "admin")
                            {
                                LoggedIn("admin", LoggedInUser);
                                LoggedInUser = User.Users[i];
                                break;
                            }
                            else
                            {
                                check++;
                                if (check == User.Users.Count)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    TextDecoration.WriteLineCentered("Sikertelen bejelentkezés!", false);
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Thread.Sleep(2000);
                                    break;
                                }
                            }
                        }
                        break;
                    case 2:
                        Console.Clear();
                        Settings.SettingsMenu();
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                }
                //    Console.Clear();
                //    int choice = 0;
                //    Console.ForegroundColor = ConsoleColor.Red;
                //    TextDecoration.WriteLineCentered("=== VÉRNYOMÁSNAPLÓ ===", false);
                //    Console.ForegroundColor = ConsoleColor.White;
                //    TextDecoration.WriteLineCentered("-------------------");
                //    TextDecoration.WriteLineCentered("1. Regisztrálás");
                //    TextDecoration.WriteLineCentered("2. Bejelentkezés");
                //    TextDecoration.WriteLineCentered("3. Beállítások");
                //    TextDecoration.WriteLineCentered("4. Kilépés");
                //    TextDecoration.WriteLineCentered("--------------------");
                //    TextDecoration.WriteCentered("Válasszon ki egy menüpontot: ");
                //    while (!int.TryParse(Console.ReadLine(), out choice))
                //    {
                //    }
                //    string UserName, FullName, Password, Gender;
                //    DateTime BirthDate;
                //    if (choice == 1)
                //    {
                //        Console.Clear();
                //        Console.ForegroundColor = ConsoleColor.Red;
                //        TextDecoration.WriteLineCentered("=== REGISZTRÁCIÓ === ", false);
                //        Console.ForegroundColor = ConsoleColor.White;
                //        RegisterUser(out UserName, out FullName, out Password, out BirthDate, out Gender);
                //        User.AddUser(UserName, FullName, Password, BirthDate, Gender);
                //        User.ShowUsers();
                //        User.SaveUserData();
                //    }
                //    else if (choice == 2)
                //    {
                //        User LoggedInUser = new User();
                //        Console.Clear();
                //        Console.ForegroundColor = ConsoleColor.Red;
                //        TextDecoration.WriteLineCentered("=== BEJELENTKEZÉS ===", false);
                //        Console.ForegroundColor = ConsoleColor.White;
                //        Console.WriteLine(" ");
                //        TextDecoration.WriteCentered("Adja meg a felhasználó nevét: ");
                //        string LoginUserName = Console.ReadLine();
                //        TextDecoration.WriteCentered("Adja meg a jelszavát: ");
                //        string LoginPassword = Console.ReadLine();
                //        int check = 0;
                //        for (int i = 0; i < User.Users.Count; i++)
                //        {
                //            if (User.Users[i].UserName == LoginUserName && User.Users[i].Password == LoginPassword)
                //            {
                //                LoggedIn(LoginUserName, LoggedInUser);
                //                LoggedInUser = User.Users[i];
                //                break;
                //            }
                //            else if (LoginUserName == "admin")
                //            {
                //                LoggedIn("admin", LoggedInUser);
                //                LoggedInUser = User.Users[i];
                //                break;
                //            }
                //            else
                //            {
                //                check++;
                //                if (check == User.Users.Count)
                //                {
                //                    Console.ForegroundColor = ConsoleColor.DarkRed;
                //                    TextDecoration.WriteLineCentered("Sikertelen bejelentkezés!", false);
                //                    Console.ForegroundColor = ConsoleColor.White;
                //                    Thread.Sleep(2000);
                //                    break;
                //                }
                //            }
                //        }
                //    }
                //    else if (choice == 3)
                //    {
                //        Console.Clear();
                //        Settings.SettingsMenu();
                //    }
                //    else if (choice == 4)
                //    {
                //        Environment.Exit(0);
                //    }
                //    else
                //    {
                //        Console.ForegroundColor = ConsoleColor.Red;
                //        TextDecoration.WriteLineCentered("Nincs ilyen menüpont!", false);
                //    }
                //}

                // Felhasználó regisztráció
                static void RegisterUser(out string UserName, out string FullName, out string Password, out DateTime BirthDate, out string Gender)
                {
                    // Felhasználónév
                    do
                    {
                        TextDecoration.WriteCentered("Adja meg a felhasználó nevét: ");
                        UserName = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(UserName))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            TextDecoration.WriteLineCentered("A felhasználónév nem lehet üres!", false);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    } while (string.IsNullOrWhiteSpace(UserName));

                    //Teljes nev
                    do
                    {
                        TextDecoration.WriteCentered("Adja meg a teljes nevét: ");
                        FullName = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(FullName))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            TextDecoration.WriteLineCentered("A teljes név nem lehet üres!", false);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    } while (string.IsNullOrWhiteSpace(FullName));

                    //Jelszo
                    do
                    {
                        TextDecoration.WriteCentered("Adja meg a jelszavát: ");
                        Password = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(Password))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            TextDecoration.WriteLineCentered("A jelszó nem lehet üres!", false);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    } while (string.IsNullOrWhiteSpace(Password));

                    //Szuldat
                    do
                    {
                        TextDecoration.WriteCentered("Adja meg a születési dátumát (ÉÉÉÉ-HH-NN): ");
                        string BirthDateInput = Console.ReadLine();
                        if (!DateTime.TryParse(BirthDateInput, out BirthDate) || BirthDate > DateTime.Now)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            TextDecoration.WriteLineCentered("Érvénytelen dátum!", false);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            break;
                        }
                    } while (true);

                    //Nem
                    do
                    {
                        TextDecoration.WriteCentered("Adja meg a nemét (Férfi/Nő): ");
                        Gender = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(Gender) || !(Gender.ToLower() == "férfi" || Gender.ToLower() == "nő"))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            TextDecoration.WriteLineCentered("A nem csak 'Férfi' vagy 'Nő' lehet!", false);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            break;
                        }
                    } while (true);
                }
                // Vérnyomás adatainak rögzítése
                static void RegisterBP(string userName, User CurrentUser)
                {
                    string bloodPressureLevel = "";
                    DateTime date;

                TextDecoration.WriteCentered("Dátum: ");
                date = InputChecks.IsValidDate(Console.ReadLine(), true);
                TextDecoration.WriteCentered("Szisztolés érték (Hgmm): ");
                int sys = int.Parse(Console.ReadLine());
                TextDecoration.WriteCentered("Diasztolés érték (Hgmm): ");
                int dia = int.Parse(Console.ReadLine());
                TextDecoration.WriteCentered("Pulzus (bpm): ");
                int pul = int.Parse(Console.ReadLine());
                TextDecoration.WriteLineCentered(BpStore.InspectBP(CurrentUser.BirthDate, sys, dia, pul));
                    
                BpStore newBpData = new BpStore(userName, date, sys, dia, pul);
                newBpData.SaveBpData();
            }

            static void LoggedIn(string LoginUserName,User CurrentUser)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                TextDecoration.WriteLineCentered("Sikeres bejelentkezés!", false);
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(2000);
                bool exit = false;
                while (!exit)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
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
                            RegisterBP(LoginUserName, CurrentUser);
                            Console.ForegroundColor = ConsoleColor.Green;
                            TextDecoration.WriteLineCentered("Vérnyomásadat elmentve!", false);
                            Thread.Sleep(4000);
                            break;
                        case "2":
                            var bpuserdata = BpStore.ReadBpData(LoginUserName);
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

            /// <summary>
            /// Vérnyomás adatainak fájlba mentése
            /// </summary>
            public void SaveBpData()
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string projectPath = Path.GetFullPath(Path.Combine(basePath, @"..\..\.."));
                string filePath = Path.Combine(projectPath, "BpData.txt");
                File.AppendAllText(filePath, $"{user};{date.ToShortDateString()};{sys};{dia};{pulse}\n");
            }


            /// <summary>
            /// Vérnyomás adatainak összeállítása
            /// </summary>
            /// <param name="user"></param>
            /// <param name="date"></param>
            /// <param name="sys"></param>
            /// <param name="dia"></param>
            /// <param name="pulse"></param>
            public BpStore(string user, DateTime date, int sys, int dia, int pulse)
            {
                this.user = user;
                this.date = date;
                this.sys = sys;
                this.dia = dia;
                this.pulse = pulse;
            }
            /// <summary>
            /// Vérnyomás értékek vizsgálata és állapot visszaadása
            /// </summary>
            /// <param name="birthDate"></param>
            /// <param name="sys"></param>
            /// <param name="diast"></param>
            /// <param name="bpm"></param>
            /// <returns></returns>

            public static string InspectBP(DateTime birthDate, int sys, int diast, int bpm)
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
                return $"Szisztólikus: {sysStatus}, Diasztólikus: {diastStatus}, Pulzus: {bpmStatus}";
            }

            /// <summary>
            /// Átlagos vérnyomás adatok számítása
            /// </summary>
            /// <returns></returns>
            public static string AverageBpData(string Username)
            {
                string[] Data = ReadBpData(Username);
                int sumsys = 0;
                int sumdia = 0;
                int sumpul = 0;
                double avgsys = 0;
                double avgdia = 0;
                double avgpul = 0;

                for (int i = 0; i < Data.Length; i++)
                {
                    string[] dataLine = Data[i].Split(';');
                    sumsys += Convert.ToInt32(dataLine[2]);
                    sumdia += Convert.ToInt32(dataLine[3]);
                    sumpul += Convert.ToInt32(dataLine[4]);
                }
                if (Data.Length > 0)
                {
                    sumsys /= Data.Length;
                    sumdia /= Data.Length;
                    sumpul /= Data.Length;
                    return $"Átlagos értékek - Szisztolés: {sumsys} Hgmm, Diasztolés: {sumdia} Hgmm, Pulzus: {sumpul} bpm.";
                }
                else
                {
                    return "Nincs elég adat az átlag számításhoz.";
                }
            }

            /// <summary>
            /// Visszaadja azon felhasználók listáját, akiknél a mérések meghatározott százaléka eltér a normálistól.
            /// </summary>
            /// <param name="maxDiff">A megengedett maximum eltérés százalékban</param>
            /// <returns>Felhasználónevek listája</returns>
            public static List<string> GetDifferentBPUser(double maxDiff)
            {
                int diffNum = 0;

                double diffPercent = 0;

                string[] users = User.GetUserNames();
                string[] cUserData = { };
                string[] lineSplit = { };
                string[] inspected = { };

                List<string> diffUsers = new List<string> { };

                foreach (string user in users)
                {
                    cUserData = BpStore.ReadBpData(user);
                    diffNum = 0;

                    if (cUserData.Length != 0)
                    {
                        foreach (string line in cUserData)
                        {
                            lineSplit = line.Split(';');

                            inspected = BpStore.InspectBP(DateTime.Parse(lineSplit[1]), int.Parse(lineSplit[2]), int.Parse(lineSplit[3]), int.Parse(lineSplit[4])).Split(';');


                            if (lineSplit[0] == user)
                            {
                                if (inspected[0] != "normális" || inspected[1] != "normális" || inspected[2] != "normális")
                                {
                                    diffNum++;
                                }
                            }
                        }

                        diffPercent = (double)diffNum / cUserData.Length * 100;

                        if (diffPercent < maxDiff || diffPercent > maxDiff)
                        {
                            diffUsers.Add(user);
                        }
                    }
                }

                return diffUsers;
            }

            /// <summary>
            /// max és minimum vérnyomás értékek összegyűjtése
            /// </summary>
            /// <param name="username"></param>
            /// <returns></returns>
            public static int[] GetMaxMinBpValues(string username)
            {
                string[] userbpdata = ReadBpData(username);
                int maxSys = 0;
                int minSys = 0;
                int maxDia = 0;
                int minDia = 0;
                int maxPul = 0;
                int minPul = 0;

                foreach (var line in userbpdata)
                {
                    string[] split = line.Split(';');
                    int sys = int.Parse(split[2]);
                    int dia = int.Parse(split[3]);
                    int pul = int.Parse(split[4]);
                    if (sys > maxSys)
                    {
                        maxSys = sys;
                    }
                    if (sys < minSys || minSys == 0)
                    {
                        minSys = sys;
                    }
                    if (dia > maxDia)
                    {
                        maxDia = dia;
                    }
                    if (dia < minDia || minDia == 0)
                    {
                        minDia = dia;
                    }
                    if (pul > maxPul)
                    {
                        maxPul = pul;
                    }
                    if (pul < minPul || minPul == 0)
                    {
                        minPul = pul;
                    }
                }
                return new int[] { maxSys, minSys, maxDia, minDia, maxPul, minPul };
            }

            /// <summary>
            /// Visszaadja a globális maximum és minimum vérnyomás értékeket
            /// </summary>
            /// <returns></returns>
            public static int[] GetMaxMinBpValuesGlobal()
            {
                string[] users = User.GetUserNames();
                int AverageMaxSys = 0;
                int AverageMinSys = 0;
                int AverageMaxDia = 0;
                int AverageMinDia = 0;
                int AverageMaxPul = 0;
                int AverageMinPul = 0;

                foreach (var user in users)
                {
                    string[] userbpdata = ReadBpData(user);
                    int maxSys = 0;
                    int minSys = 0;
                    int maxDia = 0;
                    int minDia = 0;
                    int maxPul = 0;
                    int minPul = 0;
                    foreach (var line in userbpdata)
                    {
                        string[] split = line.Split(';');
                        int sys = int.Parse(split[2]);
                        int dia = int.Parse(split[3]);
                        int pul = int.Parse(split[4]);
                        if (sys > maxSys)
                        {
                            maxSys = sys;
                        }
                        if (sys < minSys || minSys == 0)
                        {
                            minSys = sys;
                        }
                        if (dia > maxDia)
                        {
                            maxDia = dia;
                        }
                        if (dia < minDia || minDia == 0)
                        {
                            minDia = dia;
                        }
                        if (pul > maxPul)
                        {
                            maxPul = pul;
                        }
                        if (pul < minPul || minPul == 0)
                        {
                            minPul = pul;
                        }
                        AverageMaxDia += maxDia;
                        AverageMinDia += minDia;
                        AverageMaxSys += maxSys;
                        AverageMinSys += minSys;
                        AverageMaxPul += maxPul;
                        AverageMinPul += minPul;
                    }
                }
                return new int[] { AverageMaxSys/users.Length, AverageMinSys/users.Length, AverageMaxDia/ users.Length, AverageMinDia/ users.Length, AverageMaxPul/ users.Length, AverageMinPul/ users.Length };
            }
            /// <summary>
            /// Vérnyomás adatok kiírása táblázatos formában
            /// </summary>
            /// <param name="username"></param>
            public static void PrintBpTable(string username)
            {
                string[] userbpdata = ReadBpData(username);
                if (userbpdata.Length == 0)
                {
                    TextDecoration.WriteLineCentered("Nincs adat");
                    return;
                }
                TextDecoration.WriteLineCentered("╔══════════════════╦═══════╦═══════╦═══════╗");
                TextDecoration.WriteLineCentered("║ Dátum            ║  SYS  ║  DIA  ║  PUL  ║");
                TextDecoration.WriteLineCentered("╠══════════════════╬═══════╬═══════╬═══════╣");
                foreach (var line in userbpdata)
                {
                    string[] split = line.Split(';');
                    string date = split[1].PadRight(16);
                    string sys = split[2].PadLeft(5);
                    string dia = split[3].PadLeft(5);
                    string pul = split[4].PadLeft(5);
                    TextDecoration.WriteLineCentered($"║ {date} ║ {sys} ║ {dia} ║ {pul} ║");
                }
                TextDecoration.WriteLineCentered("╚══════════════════════════════════════════╝");
            }

            public static void PrintMaxMinBpValues(string username)
            {
                int[] minmax = GetMaxMinBpValues(username);
                int maxSys = minmax[0];
                int minSys = minmax[1];
                int maxDia = minmax[2];
                int minDia = minmax[3];
                int maxPul = minmax[4];
                int minPul = minmax[5];
                TextDecoration.WriteLineCentered("Személyes értékek");
                TextDecoration.WriteLineCentered("╔══════════════════╦════════╦════════╗");
                TextDecoration.WriteLineCentered("║ Érték típusa     ║   MIN  ║   MAX  ║");
                TextDecoration.WriteLineCentered("╠══════════════════╬════════╬════════╣");
                TextDecoration.WriteLineCentered($"║ Szisztolés érték ║ {minSys.ToString().PadLeft(6)} ║ {maxSys.ToString().PadLeft(6)} ║");
                TextDecoration.WriteLineCentered($"║ Diasztolés érték ║ {minDia.ToString().PadLeft(6)} ║ {maxDia.ToString().PadLeft(6)} ║");
                TextDecoration.WriteLineCentered($"║ Pulzus érték     ║ {minPul.ToString().PadLeft(6)} ║ {maxPul.ToString().PadLeft(6)} ║");
                TextDecoration.WriteLineCentered("╚══════════════════╩════════╩════════╝");
            }

            public static void PrintMaxMinBpValuesGlobal()
            {
                int[] minmax = GetMaxMinBpValuesGlobal();
                int maxSys = minmax[0];
                int minSys = minmax[1];
                int maxDia = minmax[2];
                int minDia = minmax[3];
                int maxPul = minmax[4];
                int minPul = minmax[5];
                TextDecoration.WriteLineCentered("Globális értékek");
                TextDecoration.WriteLineCentered("╔══════════════════╦════════╦════════╗");
                TextDecoration.WriteLineCentered("║ Érték típusa     ║   MIN  ║   MAX  ║");
                TextDecoration.WriteLineCentered("╠══════════════════╬════════╬════════╣");
                TextDecoration.WriteLineCentered($"║ Szisztolés érték ║ {minSys.ToString().PadLeft(6)} ║ {maxSys.ToString().PadLeft(6)} ║");
                TextDecoration.WriteLineCentered($"║ Diasztolés érték ║ {minDia.ToString().PadLeft(6)} ║ {maxDia.ToString().PadLeft(6)} ║");
                TextDecoration.WriteLineCentered($"║ Pulzus érték     ║ {minPul.ToString().PadLeft(6)} ║ {maxPul.ToString().PadLeft(6)} ║");
                TextDecoration.WriteLineCentered("╚══════════════════╩════════╩════════╝");
            }


            /// <summary>
            /// 
            /// </summary>
            /// <param name="userName"></param>
            /// <returns></returns>
            public static string[] ReadBpData(string userName = "")
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

        }

        /// <summary>
        /// A FELHASZNÁLÓ ÉS ANNAK ADATAaaI
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

            public static string[] GetUserNames()
            {
                string[] Usernames = new string[Users.Count];
                for (int i = 0; i < Users.Count; i++)
                {
                    Usernames[i] = Users[i].UserName;
                }
                return Usernames;
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
            /// <summary>
            /// A dátum érvényességének ellenörzése
            /// </summary>
            /// <param name="dateInput">A dátum</param>
            /// <param name="IfTrueThenToday">Ha igaz akkor ha a felhasználó nem ír be semmit akkor a mai dátumot veszi alapul</param>
            /// <returns>a dátumot</returns>
            public static DateTime IsValidDate(string dateInput, bool IfTrueThenToday)
            {
                DateTime date;
                while (!DateTime.TryParse(dateInput, out date))
                {
                    if (IfTrueThenToday && string.IsNullOrWhiteSpace(dateInput))
                    {
                        date = DateTime.Now;
                        return date;
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    TextDecoration.WriteLineCentered("Nem jól adta meg a dátumot! Írja be újra: ", false);
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
        /// <summary>
        /// Olyan függvényeket tarlamaz amelyek a konzol beállításaira szolgálnak
        /// </summary>
        internal class Settings
        {
            static string fgcolor = "w";
            /// <summary>
            /// Beállítások menü megjelenítése
            /// </summary>
            /// <returns></returns>
            public static void SettingsMenu()
            {
                int settingchoose = MenuControll.ArrowMenu(new string[] { "Téma kiválasztása", "Vissza"}, "=== BEÁLLÍTÁSOK ===");
                switch (settingchoose)
                {
                    case 0:
                        Console.Clear();
                        int colorswitcher = MenuControll.ArrowMenu(new string[] { "Fekete háttér, fehér betűszín", "Fehér háttér, fekete betűszín", "Sötétkék háttér, cián betűszín"}, "=== HÁTTÉRSZÍN BEÁLLÍTÁSA ===");
                        switch (colorswitcher)
                        {
                            case 0:
                                Console.BackgroundColor = ConsoleColor.Black;
                                fgcolor = "w";
                                break;
                            case 1:
                                Console.BackgroundColor = ConsoleColor.White;
                                fgcolor = "b";
                                break;
                            case 2:
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                                fgcolor = "c";
                                break;
                            case 3:
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                                fgcolor = "y";
                                break;
                        }
                        break;
                    case 1:
                        break;
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
                else if (fgcolor == "c")
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else if (fgcolor == "dr")
                {
                    //Console.ForegroundColor = ConsoleColor.;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal class MenuControll
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="menupoints">Menüpontok amiket létrehoz a választáshoz</param>
            /// <param name="title">A cím</param>
            /// <returns>Egy menüt</returns>
            public static int ArrowMenu(string[] menupoints, string title)
            {
                int currentPoint = 0;
                bool selected = false;
                do
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    TextDecoration.WriteLineCentered(title,false);
                    TextDecoration.WriteLineCentered("--------------------");
                    for (int i = 0; i < menupoints.Length; i++)
                    {
                        if (i == currentPoint)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            TextDecoration.WriteLineCentered($"> {menupoints[i]}",false);
                        }
                        else
                        {
                            TextDecoration.WriteLineCentered($"  {menupoints[i]}");
                        }
                    }
                    TextDecoration.WriteLineCentered("--------------------");
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Enter:
                            selected = true;
                            break;
                        case ConsoleKey.UpArrow:
                            if (currentPoint > 0) currentPoint--;
                            break;
                        case ConsoleKey.DownArrow:
                            if (currentPoint < menupoints.Length - 1) currentPoint++;
                            break;
                        default:
                            Console.Beep();
                            break;
                    }
                } while (!selected);
                return currentPoint;
            }
        }
    }
}