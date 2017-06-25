using System;
using System.IO;

namespace AeroPort
{
    #region Task
    // Написать программу - табло аэропорта.
    // Рейсы записаны в текстовом файле в произвольном порядке в формате

    //Depart
    //QR2130  : Paris : 14.23  
    //DF2130  : Istambul : 11.50
    //R1122 : London : 15.30

    //Arrive
    //QR2131  : Paris : 13.10  
    //DF1212  : Istambul : 10.00
    //R1121 : London : 14.30
    //T1122 : Milan : 13.50

    //Загрузить все записи, каждую из них записать в отдельную структуру.Создать отдельно массив структур для прилета и для отлета.Отсортировать его. 
    //Создать текстовое меню с возможностью выбора одного из пунктов.

    // Программа имеет меню со следующими позициями
    //   1. Показать таблицу прилетов и отлетов.
    //   2. Показать только таблицу прилетов
    //   3. Показать только таблицу отлетов
    //   4. Добавить рейс прилета
    //   5. Добавить рейс отлета
    //   6. Добавить время и пересчитать статус (улетел, посадка(за пол часа), регистрация(за 2 часа), ожидается) 
    //   7. Таблицу прилетов и отлетов записать в отдельный файл.
    //   Esc - выход
    #endregion
    class Program
    {
        struct Time                   //структура Тайм введена для хранения времени, указанного в табло
        {
            int second;                              //для хранения мы вибераем секунды
            public void setTime(string v)
            {
                string[] tmp = v.Split('.');                                 //строку времени, переданую из структуры рейса, мы переводим в секунды. для этого мы создаем масив строк, 
                second = int.Parse(tmp[0]) * 3600 + int.Parse(tmp[1]) * 60;     //Сплитом с символом "." мы разделяем строку часы и минуты и переводим их в секунды
            }
            public int sec
            {
                get { return second; }                                 //вводим свойство 
            }
            public string TimeToString()                                             //создае метод для перевода секунд в читаемый формат времени: час.минута
            {
                int h = second / 3600;                                              //вычитаем часы, они образуются делением числа секунд на 3600, поскольку у нас инт, то дробная часть остается
                int m = second % 3600 / 60;                           //тоже самое для секунд,но они образуются делением остатка числа от деления на 3600 и потом делем на секунды

                string mH = h.ToString();
                if (mH.Length == 1) mH = ' ' + mH;
                string mS = m.ToString();
                if (mS.Length == 1) mS = '0' + mS;

                string s = mH + '.' + mS;
                return s;
            }
        }
        struct rowTable              //структура рейса
        {
            string rname;
            string city;
            Time time;                      //время будем обрабатывать в структуре Тайм
            public int sec
            {
                get { return time.sec; }
            }
            public void SetValue(string v)         //метод получения инфы из строки.
            {
                string[] tmp = v.Split(':');             //мы создаем массив строк и входной строки. символ ":" будет разделять члены массива
                for (int i = 0; i < tmp.Length; i++)
                    tmp[i] = tmp[i].Trim(' ');                 // тримом обрезаем все пустые символы в элементах членов массива
                rname = tmp[0];
                city = tmp[1];
                time.setTime(tmp[2]);               //время забрасываем в структуры времени
            }
            public string Name
            {
                get { return rname; }
                set { Name = value; }
            }
            public string City
            {
                get { return city; }
                set { City = value; }
            }

            public string TimeOfFl
            {
                get { return time.TimeToString(); }
                set { TimeOfFl = value; }
            }
            public void Print()                        //метод вывода на экран
            {
                Console.WriteLine("{0,7}|{1,10}|{2,5}", rname, city, time.TimeToString());
            }
        }
        static string[] LoadTablesFromFile(string fileName)         // создаесм метод по загрузке строк из файла, он являетмся массивом строк и должен возвращать таблицу
        {
            string[] tableFromFile = File.ReadAllLines(fileName);      //Загружаем файл из дебага. имя  файла передается в метод
            int countNotEmptyLines = 0;

            for (int i = 0; i < tableFromFile.Length; i++)
            {
                if (tableFromFile[i] != "") countNotEmptyLines++;          //подсчитываем колличество не пустых строк. если в файле были устые строки мы их не учитываем
            }

            string[] tableWithEmptyString = new string[countNotEmptyLines];      //создаем новый массив, с колличеством строк, равным колличеству строк без пробелов

            for (int i = 0, j = 0; i < tableFromFile.Length; i++)
            {
                if (tableFromFile[i] != "") { tableWithEmptyString[j] = tableFromFile[i]; j++; }
            }
            return tableWithEmptyString;
        }
        static void CreateTable(string fileName, out rowTable[] departTable, out rowTable[] arriveTable)
        {

            string[] tableString = LoadTablesFromFile(fileName);

            int countArrive = 0;
            int countDepart = 0;
            int action = 0;        // o - not action, 1- depart, 2 - arrave

            for (int i = 0; i < tableString.Length; i++)
            {
                if (tableString[i] == "Depart") { action = 1; continue; }
                if (tableString[i] == "Arrive") { action = 2; continue; }
                switch (action)
                {
                    case 1:
                        countDepart++;
                        break;
                    case 2:
                        countArrive++;
                        break;
                }
            }

            departTable = new rowTable[countDepart];
            arriveTable = new rowTable[countArrive];

            countArrive = 0;
            countDepart = 0;
            action = 0;
            for (int i = 0; i < tableString.Length; i++)
            {
                if (tableString[i] == "Depart") { action = 1; continue; }
                if (tableString[i] == "Arrive") { action = 2; continue; }
                switch (action)
                {
                    case 1:
                        departTable[countDepart].SetValue(tableString[i]);
                        countDepart++;
                        break;
                    case 2:
                        arriveTable[countArrive].SetValue(tableString[i]);
                        countArrive++;
                        break;
                }
            }
        }
        static void PrintTable(string nameTable, rowTable[] anytable)
        {
            Console.WriteLine(nameTable);
            for (int i = 0; i < anytable.Length; i++)
                anytable[i].Print();
        }
        static void PrintMenu()
        {
            Console.WriteLine("        Aeroport Table Menu");
            Console.WriteLine("1. Arrive  and Depart Tables");
            Console.WriteLine("2. Depart Tables");
            Console.WriteLine("3. Arrive Tables");
            Console.WriteLine("4. Imput new flight to Arrive Tables");
            Console.WriteLine("5. Imput new flight to Depart Tables");
            Console.WriteLine("6. Status flightes");
            Console.WriteLine("7. Write Tables to new file");
            Console.WriteLine("Esc - exit");
        }
        static void SortTable(rowTable[] table)
        {
            rowTable tmp;
            for (int i = 0; i < table.Length - 1; i++)
                for (int j = 0; j < table.Length - 1; j++)
                {
                    if (table[j].sec > table[j + 1].sec)
                    {
                        tmp = table[j];
                        table[j] = table[j + 1];
                        table[j + 1] = tmp;
                    }
                }
        }
        static rowTable[] AddFlight(rowTable[] oldTable, string s)
        {
            rowTable[] newTable = new rowTable[oldTable.Length + 1];
            for (int i = 0; i < oldTable.Length; i++) { newTable[i] = oldTable[i]; }
            newTable[oldTable.Length].SetValue(s);
            SortTable(newTable);
            return newTable;
        }
        // пункт 6
        static void StatusFlA(rowTable[] aTable)
        {
            int timeNow;
            timeNow = DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600;

            for (int i = 0; i < aTable.Length; i++)
            {
                if ((aTable[i].sec - timeNow) > 0 && (aTable[i].sec - timeNow) < 1800)
                {
                    // (улетел, посадка(за пол часа), регистрация(за 2 часа), ожидается)
                    Console.WriteLine("\tLands flight: ");
                    aTable[i].Print();
                }
                else if ((aTable[i].sec - timeNow) > 0 && (aTable[i].sec - timeNow) >= 1800)
                {
                    Console.WriteLine("\tFlight waiting: ");
                    aTable[i].Print();
                }
            }
        }
        static void StatusFlD(rowTable[] dTable)
        {
            int timeNow;
            timeNow = DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600;
            
            for (int i = 0; i < dTable.Length; i++)
            {
                //    (улетел, посадка(за пол часа), регистрация(за 2 часа), ожидается)
                if ((timeNow - dTable[i].sec) > 0 && (timeNow - dTable[i].sec) < 1800)
                {
                    // (улетел, посадка(за пол часа), регистрация(за 2 часа), ожидается)
                    Console.WriteLine("\tDeparted: ");
                    dTable[i].Print();
                }
                else if ((dTable[i].sec - timeNow) > 0 && (dTable[i].sec - timeNow) < 3600)
                {
                    Console.WriteLine("\tRegistration flight: ");
                    dTable[i].Print();
                }
            }
        }
        static void ArreyToText(rowTable[] dTable, rowTable[] aTable)
        {
            FileInfo f = new FileInfo("NewTables.txt");
            StreamWriter w = f.CreateText();
            w.WriteLine("Depart");
            for (int i = 0; i < dTable.Length; i++)
                w.WriteLine("{0,7}|{1,10}|{2,8}", dTable[i].Name, dTable[i].City, dTable[i].TimeOfFl);

            w.WriteLine("Arrive");
            for (int i = 0; i < aTable.Length; i++)
                w.WriteLine("{0,7}|{1,10}|{2,8}", aTable[i].Name, aTable[i].City, aTable[i].TimeOfFl);
            w.Close();
        }
        static void Main(string[] args)
        {
            rowTable[] dTable, aTable;
            CreateTable("Tables.txt", out dTable, out aTable);
            
            SortTable(dTable);
            SortTable(aTable);

            ConsoleKeyInfo k = new ConsoleKeyInfo();
            do
            {
                PrintMenu();
                k = Console.ReadKey();
                Console.Clear();
                switch (k.Key)
                {
                    case ConsoleKey.D1:
                        PrintTable("\tDepart Table", dTable);
                        PrintTable("\tArrive Table", aTable);
                        break;
                    case ConsoleKey.D2:
                        PrintTable("\tDepart Table", dTable);
                        break;
                    case ConsoleKey.D3:
                        PrintTable("\tArrive Table", aTable);
                        break;

                    case ConsoleKey.D4:
                        Console.Write("Imput new flight to Arrive table: ");
                        string s = Console.ReadLine();
                        aTable = AddFlight(aTable, s);
                        break;
                    case ConsoleKey.D5:
                        Console.Write("Imput new flight to Depart Table: ");
                        string s1 = Console.ReadLine();
                        dTable = AddFlight(dTable, s1);
                        break;
                    case ConsoleKey.D6:
                        Console.WriteLine("Current time - {0}", DateTime.Now);
                        Console.WriteLine(new string('*',36));
                        StatusFlD(dTable);
                        Console.WriteLine(new string('*', 36));
                        StatusFlA(aTable);
                        break;

                    case ConsoleKey.D7:
                        ArreyToText(dTable, aTable);

                        break;
                }

                Console.WriteLine(new string('-', 36));
            } while (k.Key != ConsoleKey.Escape);
        }
    }
}

