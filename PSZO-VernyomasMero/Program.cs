// See https://aka.ms/new-console-template for more information

using System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using static PSZO_VernyomasMero.Program;

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
                string[] MainMenu = { "Fiók létrehozása ", "Bejelentkezés", "Beállítások", "Kilépés" };
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
                                Admin.AdminPanel();                         
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
                // Bejelentkezés utáni menü
                static void LoggedIn(string LoginUserName, User CurrentUser)
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
                        int loggedinchoose = MenuControll.ArrowMenu(new string[] { "Vérnyomás rögzítése", "Saját mérések megtekintése", "Statisztikák", "Beállítások", "Kijelentkezés" }, $"Üdvözöljük, {LoginUserName}!");
                        switch (loggedinchoose)
                        {
                            case 0:
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Red;
                                TextDecoration.WriteLineCentered("=== VÉRNYOMÁS RÖGZÍTÉSE ===", false);
                                Console.WriteLine(" ");
                                RegisterBP(LoginUserName, CurrentUser);
                                Console.ForegroundColor = ConsoleColor.Green;
                                TextDecoration.WriteLineCentered("Vérnyomásadat elmentve!", false);
                                TextDecoration.WriteLineCentered("Nyomjon ENTER-t a folytatáshoz...");
                                Console.ReadLine();
                                break;
                            case 1:
                                Console.Clear();
                                string[] bpuserdata = BpStore.ReadBpData(LoginUserName);
                                Console.ForegroundColor = ConsoleColor.Red;
                                TextDecoration.WriteLineCentered("=== SAJÁT MÉRÉSEK ===", false);
                                Console.WriteLine(" ");
                                BpStore.PrintBpTable(LoginUserName);
                                int userdatachoice = MenuControll.ArrowMenuWithTable(new string[] { "Dátom alapján rendezés", "Érték nagyság alapú rendezés","Vissza" }, "Rendezés", () => BpStore.PrintBpTable(LoginUserName));
                                switch(userdatachoice)
                                {
                                    case 0:
                                        TextDecoration.WriteLineCentered("Még nem elérhető!", false);
                                        TextDecoration.WriteLineCentered("Nyomjon ENTER-t a folytatáshoz...");
                                        break;
                                    case 1:
                                        TextDecoration.WriteLineCentered("Még nem elérhető!", false);
                                        TextDecoration.WriteLineCentered("Nyomjon ENTER-t a folytatáshoz...");
                                        break;
                                    case 2:
                                        break;
                                }
                                break;
                            case 2:
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Red;
                                TextDecoration.WriteLineCentered("=== STATISZTIKÁK ===", false);
                                BpStore.PrintMaxMinBpValues(LoginUserName);
                                BpStore.PrintMaxMinBpValuesGlobal();
                                List<string> diffusers = BpStore.GetDifferentBPUser(30);
                                TextDecoration.WriteLineCentered("Felhasználók, akiknél nagyobb mint 30% eltérés van:");
                                foreach (var user in diffusers)
                                {
                                    TextDecoration.WriteLineCentered(user);
                                }
                                TextDecoration.WriteLineCentered("Nyomjon ENTER-t a folytatáshoz...");
                                Console.ReadLine();
                                break;
                            case 3:
                                Console.Clear();
                                Settings.SettingsMenu();
                                continue;
                            case 4:
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
            public static int[] AverageBpData(string Username)
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
                    return [sumsys, sumdia, sumpul];
                }
                else
                {
                    return [];
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
            /// Visszaadja a globális átlagos vérnyomás értékeket
            /// </summary>
            /// <returns></returns>
            public static int[] GetAverageBpValueGlobal()
            {
                string[] users = User.GetUserNames();
                int AverageSys = 0;
                int AverageDia = 0;
                int AveragePul = 0;

                foreach (var user in users)
                {
                    string[] userbpdata = ReadBpData(user);
                    foreach (var line in userbpdata)
                    {
                        string[] split = line.Split(';');
                        int sys = int.Parse(split[2]);
                        int dia = int.Parse(split[3]);
                        int pul = int.Parse(split[4]);
                        AverageSys += sys;
                        AverageDia += dia;
                        AveragePul += pul;
                    }
                }
                return new int[] { AverageSys / users.Length, AverageDia / users.Length, AveragePul / users.Length};
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
                TextDecoration.WriteLineCentered("╚══════════════════╩═══════╩═══════╩═══════╝");
            }

            public static void PrintMaxMinBpValues(string username)
            {
                int[] minmax = GetMaxMinBpValues(username);
                int[] avg = AverageBpData(username);
                int avgsys = avg[0];
                int avgdia = avg[1];
                int avgpul = avg[2];
                int maxSys = minmax[0];
                int minSys = minmax[1];
                int maxDia = minmax[2];
                int minDia = minmax[3];
                int maxPul = minmax[4];
                int minPul = minmax[5];
                Console.WriteLine(" ");
                TextDecoration.WriteLineCentered("Személyes értékek");
                Console.WriteLine(" ");
                TextDecoration.WriteLineCentered("╔══════════════════╦════════╦════════╦═══════╗");
                TextDecoration.WriteLineCentered("║ Érték típusa     ║   MIN  ║   MAX  ║  AVG  ║");
                TextDecoration.WriteLineCentered("╠══════════════════╬════════╬════════╬═══════╣");
                TextDecoration.WriteLineCentered($"║ Szisztolés érték ║ {minSys.ToString().PadLeft(6)} ║ {maxSys.ToString().PadLeft(6)} ║{avgsys.ToString().PadLeft(6)} ║");
                TextDecoration.WriteLineCentered($"║ Diasztolés érték ║ {minDia.ToString().PadLeft(6)} ║ {maxDia.ToString().PadLeft(6)} ║{avgdia.ToString().PadLeft(6)} ║");
                TextDecoration.WriteLineCentered($"║ Pulzus érték     ║ {minPul.ToString().PadLeft(6)} ║ {maxPul.ToString().PadLeft(6)} ║{avgpul.ToString().PadLeft(6)} ║");
                TextDecoration.WriteLineCentered("╚══════════════════╩════════╩════════╩═══════╝");
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
                int[] avg = GetAverageBpValueGlobal();
                int avgSys = avg[0];
                int avgDia = avg[1];
                int avgPul = avg[2];
                Console.WriteLine(" ");
                TextDecoration.WriteLineCentered("Globális értékek");
                Console.WriteLine(" ");
                TextDecoration.WriteLineCentered("╔══════════════════╦════════╦════════╦════════╗");
                TextDecoration.WriteLineCentered("║ Érték típusa     ║   MIN  ║   MAX  ║   AVG  ║");
                TextDecoration.WriteLineCentered("╠══════════════════╬════════╬════════╬════════╣");
                TextDecoration.WriteLineCentered($"║ Szisztolés érték ║ {minSys.ToString().PadLeft(6)} ║ {maxSys.ToString().PadLeft(6)} ║ {avgSys.ToString().PadLeft(6)} ║");
                TextDecoration.WriteLineCentered($"║ Diasztolés érték ║ {minDia.ToString().PadLeft(6)} ║ {maxDia.ToString().PadLeft(6)} ║ {avgDia.ToString().PadLeft(6)} ║");
                TextDecoration.WriteLineCentered($"║ Pulzus érték     ║ {minPul.ToString().PadLeft(6)} ║ {maxPul.ToString().PadLeft(6)} ║ {avgPul.ToString().PadLeft(6)} ║");
                TextDecoration.WriteLineCentered("╚══════════════════╩════════╩════════╩════════╝");
                Console.WriteLine(" ");
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
                    TextDecoration.WriteLineCentered($"Felhasználónév: {user.UserName}, Teljes név: {user.FullName}, Születési dátum: {user.BirthDate.ToShortDateString()}, Nem: {user.Gender}");
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
            public static void LoadingAnimation(string message = "Mentés folyamatban", int durationMs = 1500)
            {
                WriteCentered($"{message}");
                int steps = 5;
                for (int i = 0; i < steps; i++)
                {
                    Console.Write(".");
                    Thread.Sleep(durationMs / steps);
                }
                Console.Write("Kész");
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
                int settingchoose = MenuControll.ArrowMenu(new string[] { "Téma kiválasztása","Program beállítások", "Fiók beállítások", "Vissza"}, "=== BEÁLLÍTÁSOK ===");
                switch (settingchoose)
                {
                    case 0:
                        Console.Clear();
                        int colorswitcher = MenuControll.ArrowMenu(new string[] { "Fekete háttér, fehér betűszín", "Fehér háttér, fekete betűszín", "Sötétkék háttér, cián betűszín", "Vissza"}, "=== HÁTTÉRSZÍN BEÁLLÍTÁSA ===");
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
                                break;
                        }
                        break;
                    case 1:
                        {
                            Console.Clear();
                            TextDecoration.WriteLineCentered("Nincsenek elérhető program beállítások!", false);
                            TextDecoration.WriteLineCentered("Nyomjon ENTER-t a folytatáshoz...");
                            Console.ReadLine();
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            TextDecoration.WriteLineCentered("Nincsenek elérhető fiók beállítások!", false);
                            TextDecoration.WriteLineCentered("Nyomjon ENTER-t a folytatáshoz...");
                            Console.ReadLine();
                            break;
                        }
                    case 3:
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
            }
        }
        /// <summary>
        /// Menü kezelő osztály
        /// </summary>
        internal class MenuControll
        {
            /// <summary>
            /// Menü kezelő függvény
            /// </summary>
            /// <param name="menupoints">Menüpontok amiket létrehoz</param>
            /// <param name="title">A menü címe</param>
            /// <returns></returns>
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
            public static int ArrowMenuWithTable(string[] menupoints, string title,Action TableWriteFunction)
            {
                int currentPoint = 0;
                bool selected = false;
                do
                {
                    Console.Clear();
                    TableWriteFunction();
                    Console.ForegroundColor = ConsoleColor.Red;
                    TextDecoration.WriteLineCentered(title, false);
                    TextDecoration.WriteLineCentered("--------------------");
                    for (int i = 0; i < menupoints.Length; i++)
                    {
                        if (i == currentPoint)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            TextDecoration.WriteLineCentered($"> {menupoints[i]}", false);
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

        /// <summary>
        /// Adminisztrátori funkciókat tartalmazó osztály
        /// </summary>
        internal class Admin
        {
            /// <summary>
            /// Felhasználók listázása táblázatos formában
            /// </summary>
            public static void ShowUsers()
            {
                if (User.Users.Count == 0)
                {
                    TextDecoration.WriteLineCentered("Nincs adat");
                    return;
                }

                TextDecoration.WriteLineCentered("╔════════════════════════════╦════════════════════════════╦═══════════════════╦═══════════════════╦═══════════════════╗");
                TextDecoration.WriteLineCentered("║       Felhasználó név      ║         Teljes név         ║      Jelszó       ║  Születési dátum  ║       Neme        ║");
                TextDecoration.WriteLineCentered("╠════════════════════════════╬════════════════════════════╬═══════════════════╬═══════════════════╬═══════════════════╣");

                foreach (User user in User.Users)
                {
                    string userName = user.UserName.PadRight(26);
                    string fullName = user.FullName.PadRight(26);
                    string password = user.Password.PadRight(17);
                    string birthDate = user.BirthDate.ToShortDateString().PadRight(17);
                    string gender = user.Gender.PadRight(17);

                    TextDecoration.WriteLineCentered($"║ {userName} ║ {fullName} ║ {password} ║ {birthDate} ║ {gender} ║");
                }

                TextDecoration.WriteLineCentered("╚════════════════════════════╩════════════════════════════╩═══════════════════╩═══════════════════╩═══════════════════╝");
            }
            /// <summary>
            /// Admin panel menü
            /// </summary>
            public static void AdminPanel()
            {
                bool exit = false;
                while (!exit)
                {
                    Console.Clear();
                    int choice = MenuControll.ArrowMenu(new string[] { "Felhasználók listázása", "Felhasználó törlése", "Felhasználó adatainak módosítása", "Kilépés" }, "=== ADMIN PANEL ===");
                    switch (choice)
                    {
                        case 0:
                            // Felhasználók listázása
                            Console.Clear();
                            Admin.ShowUsers();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            TextDecoration.WriteLineCentered("Nyomjon ENTER-t a folytatáshoz...", false);
                            Console.ReadLine();
                            break;

                        case 1:
                            // Felhasználó törlése
                            Console.Clear();
                            TextDecoration.WriteCentered("Adja meg a törlendő felhasználó nevét: ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            string delUser = Console.ReadLine();
                            int checker = 0;
                            for (int i = 0; i < User.Users.Count; i++)
                            {
                                // Törlés
                                if (delUser == User.Users[i].UserName)
                                {
                                    TextDecoration.LoadingAnimation("Felhasználó törlése folyamatban", 2000);
                                    User.Users.RemoveAt(i);// Felhasználó törlése a listából
                                    User.SaveUserData();
                                    string[] userbpdata = BpStore.ReadBpData(delUser);
                                    string basePath = AppDomain.CurrentDomain.BaseDirectory;
                                    string projectPath = Path.GetFullPath(Path.Combine(basePath, @"..\..\.."));
                                    string filePath = Path.Combine(projectPath, "BpData.txt");
                                    string[] allbpdata = File.ReadAllLines(filePath);
                                    if (userbpdata.Length == 0)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        for (int j = 0; j < allbpdata.Length; j++)
                                        {
                                            string[] split = allbpdata[j].Split(';');
                                            if (split[0] == delUser)
                                            {
                                                allbpdata[j] = null;// Felhasználó vérnyomás adatainak törlése
                                            }
                                        }
                                        List<string> newbpdata = new List<string>();
                                        foreach (var line in allbpdata)
                                        {
                                            if (line != null)
                                            {
                                                newbpdata.Add(line);
                                            }
                                        }
                                        File.WriteAllLines(filePath, newbpdata);
                                    }
                                }
                                else
                                {
                                    checker++;
                                    if (checker == User.Users.Count)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        TextDecoration.WriteLineCentered("Nincs ilyen felhasználó!", false);
                                        Thread.Sleep(2000);
                                    }
                                }
                            }
                            break;

                        case 2:
                            // Felhasználó adatainak módosítása
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            TextDecoration.WriteLineCentered("=== FELHASZNÁLÓ MÓDOSÍTÁS", false);
                            TextDecoration.WriteCentered("Adja meg a módosítandó felhasználó nevét: ");
                            string editUser = Console.ReadLine();
                            int checker2 = 0;
                            for (int i = 0; i < User.Users.Count; i++)
                            {
                                if (editUser == User.Users[i].UserName)
                                {
                                    TextDecoration.WriteCentered("Adja meg az új felhasználó nevet: ");
                                    string newUserName = Console.ReadLine();
                                    if (newUserName != "")
                                    {
                                        User.Users[i].UserName = newUserName;
                                    }
                                    TextDecoration.WriteCentered("Adja meg az új teljes nevet: ");
                                    string newFullName = Console.ReadLine();
                                    if (newFullName != "")
                                    {
                                        User.Users[i].FullName = newFullName;
                                    }
                                    TextDecoration.WriteCentered("Adja meg az új jelszót: ");
                                    string newPassword = Console.ReadLine();
                                    if (newPassword != "")
                                    {
                                        User.Users[i].Password = newPassword;
                                    }
                                    TextDecoration.WriteCentered("Adja meg az új születési dátumot (ÉÉÉÉ-HH-NN): ");
                                    string newBirthDateInput = Console.ReadLine();
                                    if (newBirthDateInput != "")
                                    {
                                        DateTime newBirthDate = InputChecks.IsValidDate(newBirthDateInput, false);
                                        User.Users[i].BirthDate = newBirthDate;
                                    }
                                    TextDecoration.WriteCentered("Adja meg az új nemet (Férfi/Nő): ");
                                    string newGender = Console.ReadLine();
                                    bool newGenderSpecified = false;
                                    if (newGender != "")
                                    {
                                        User.Users[i].Gender = newGender;
                                    }
                                    User.SaveUserData();
                                }
                                else
                                {
                                    checker2++;
                                    if (checker2 == User.Users.Count)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        TextDecoration.WriteLineCentered("Nincs ilyen felhasználó!", false);
                                        Thread.Sleep(2000);
                                    }
                                }
                            }
                            break;

                        case 3:
                            exit = true;
                            break;
                    }
                }
            }
        }
    }
}