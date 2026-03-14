using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Channels;

//Написать консольный Task Manager:
//1. Get List all Process
//2. Start process by Name
//3. Kill process by Id
//4. Kill processes by Name
//5. Add Black list
//6. Remove from Black list
//7. Exit

List<string> Blacklist = new List<string>();
int boolBlackList(string processName)
{
    foreach (var item in Blacklist)
        if (processName == item) return 1;
    return -1;
}

bool fr = true;
string choice = null;
while (fr)
{
    Console.WriteLine("    Task Manager    ");
    Console.WriteLine("1. Get List all Process");
    Console.WriteLine("2. Start process by Name");
    Console.WriteLine("3. Kill process by Id");
    Console.WriteLine("4. Kill processes by Name");
    Console.WriteLine("5. Add Black list");
    Console.WriteLine("6. Remove from Black list");
    Console.WriteLine("7. Exit");
    Console.Write(" Choice operation: ");
    choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.WriteLine("1. Get List all Process");
            var processes = Process.GetProcesses();
            Console.WriteLine("All processes:");
            foreach (var item in processes)
                Console.WriteLine($"ID: {item.Id} - Name: {item.ProcessName} - Thread count: {item.Threads.Count}");
            break;

        case "2":
            Console.WriteLine("2. Start process by Name");
            Console.Write("Enter process name for starting: ");
            string process = Console.ReadLine();
            Process.Start(process);
            Console.ReadKey();
            var check = boolBlackList(process);
            if (check == 1)
            {
                Console.WriteLine("PROCESS IS IN BLACK LIST!");
                List <Process> p = Process.GetProcessesByName(process).ToList();
                foreach (var item in p)
                    item.Kill();
                break;
            }
            Console.WriteLine("Process has already started.");
            break;

        case "3":
            Console.WriteLine("3. Kill process by Id");
            Console.Write("Input process ID for killing: ");
            int IDpr = int.Parse(Console.ReadLine());
            var killID = Process.GetProcessById(IDpr);
            killID.Kill();
            Console.WriteLine("Process has killed by ID.");
            break;

        case "4":
            Console.WriteLine("4. Kill processes by Name\n");
            Console.Write("Enter Process Name for killing: ");
            string procesName = Console.ReadLine();
            var process1 = Process.GetProcessesByName(procesName);
            foreach (var item in process1) item.Kill();
            Console.WriteLine("Process has killed by NAME.");
            break;
            

        case "5":
            Console.WriteLine("5. Add Black list");
            Console.Write("Enter process name for adding: ");
            string Addname = Console.ReadLine();
            Blacklist.Add(Addname);
            Console.WriteLine("Process added.");
            break;

        case "6":
            Console.WriteLine("6. Remove from Black list");
            Console.Write("Enter process name for removing: ");
            string Revname = Console.ReadLine();
            Blacklist.Remove(Revname);
            Console.WriteLine("Process removed from Black List.");
            break;

        case "7":
            Console.WriteLine("7. Exit");
            fr = false;
            break;
        default:
            Console.WriteLine("Invalid input. Try again.");
            break;



    }
}

