namespace PotatoOnTaskes
{
    class Program
    {
        static void Main() // gather and run everything!
        {
            InfoText("Welcome to the todo app!");
            MainMenu(); // Run main program
        }

        static void GreenText(string text) // Green text -> friendly text
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        static void RedText(string text) // Red text -> error / bad text
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        static void InfoText(string text) // Blue text -> Good to know text
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(text);
            Console.ResetColor();

        }

        static void worker1(string? user_choice)
        {
            switch (user_choice) // Goes through the user choice 
            {
                case "1":
                    InfoText("You choosed: Add task");
                    string file_name = IDGenerator(10); // Generate a random file name
                    var file_path = Path.Combine(Directory.GetCurrentDirectory(), "todo", file_name + ".txt"); // Combine the file name with the current directory
                    GreenText("Enter the title of the task: "); // Ask the title
                    string? title = Console.ReadLine(); // Read the title
                    GreenText("Enter the task: "); // Ask the task
                    string? task = Console.ReadLine(); // Read the task
                    AddTask(title, task, file_path, file_path); // Run the AddTask function
                    break;
                case "2":
                    InfoText("You choosed: View task");
                    bool checker = ReadTitle();
                    if (checker is true)
                    {
                        Console.Write("Choose the task: ");
                        Int32 task_choosen = Convert.ToInt32(Console.ReadLine());
                        ListTask(task_choosen);
                        break;
                    }
                    else
                    { RedText("No task on the list!\n"); break; }
                case "3":
                    InfoText("You choosed: Delete task");
                    bool checker2 = ReadTitle();
                    if (checker2 is true)
                    {
                        Console.Write("Choose the task: ");
                        Int32 task_to_delete = Convert.ToInt32(Console.ReadLine());
                        DeleteTask(task_to_delete);
                        break;
                    }
                    else
                    { RedText("No task to delete!\n"); break; }
                case "4":
                    InfoText("You choosed: Exit");
                    Environment.Exit(0);
                    break;
            }
        }

        static string MainMenu()
        {
            while (1 == 1)
            {
                GreenText("\nChoose a option:\n1. Add task\n2. View tasks\n3. Delete task\n4. Exit\n");
                Console.Write("\nEnter your choice: ");

                string? choice = Console.ReadLine();

                if (choice == "1" || choice == "2" || choice == "3" || choice == "4")
                {
                    worker1(choice); // Run the worker1 method
                }

                else
                {
                    RedText("Invalid choice. Please try again!");
                }
            }

        }

        static void AddTask(string? title, string? task, string filepath, string? task_path)
        {

            try
            {
                var tasks_path = Path.Combine(Directory.GetCurrentDirectory(), "todo", "tasks.txt");
                var _f = Path.Combine(Directory.GetCurrentDirectory(), "todo", "paths.txt");
                using (StreamWriter sw = new StreamWriter(tasks_path, true))
                {
                    sw.WriteLine(title); // tasks titles
                    sw.Dispose();
                }
                using (StreamWriter sw = new StreamWriter(_f, true))
                {
                    sw.WriteLine(task_path); // tasks path
                    sw.Dispose();
                }
                using (StreamWriter sw = new StreamWriter(filepath))
                {
                    sw.WriteLine("\nContent:\nTitle: " + title + "\ntask\n" + task + "\n"); // Easy!
                    sw.Dispose();
                }
            }

            catch (Exception e)
            {
                RedText("An error occured while adding task: " + e.Message);
            }
        }

        static void DeleteLine(Int32 choice, string path)
        {
            string[] lines = File.ReadAllLines(path);

            if (choice >= 1 && choice <= lines.Length)
            {
                // Create a new file without the line to delete
                using (StreamWriter writer = new StreamWriter(path))
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        // Skip the line to delete (lineToDelete - 1 because array index is 0-based)
                        if (i != choice - 1)
                        {
                            writer.WriteLine(lines[i]);
                        }
                    }
                    writer.Dispose();
                }
            }
        }

        static void DeleteTask(Int32 choice)
        {
            var tasks_path = Path.Combine(Directory.GetCurrentDirectory(), "todo", "tasks.txt");
            var _path = Path.Combine(Directory.GetCurrentDirectory(), "todo", "paths.txt");

            DeleteLine(choice, tasks_path); // Delete the line in the tasks.txt file


            using (StreamReader reader = new StreamReader(_path))
            {
                string? line;
                int lineNumber = 0; // To keep track of the line number

                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++; // Counter
                    if (choice == lineNumber)  // If the line number is equal to the line choosen by the user
                    {
                        try
                        {
                            // Check if the file exists before deleting
                            if (File.Exists(line))
                            {
                                File.Delete(line);
                                InfoText("File deleted successfully.");
                            }
                            else
                            {
                                InfoText("The file does not exist.");
                            }
                        }
                        catch (IOException ex)
                        {
                            RedText($"An error occurred while deleting the file: {ex.Message}");
                        }
                    }
                }
                reader.Dispose();
            }
            DeleteLine(choice, _path); // Delete the line in the paths.txt file
        }

        static bool ReadTitle()
        {
            InfoText("Tasks list:");
            var tasks_path = Path.Combine(Directory.GetCurrentDirectory(), "todo", "tasks.txt"); // task.txt path
            bool foo = false; // idk why but it works
            // Read lines from file
            using (StreamReader reader = new StreamReader(tasks_path))
            {
                string? line;
                int lineNumber = 0; // To keep track of the line number

                while ((line = reader.ReadLine()) is not null)
                {
                    lineNumber++; // Counter
                    // Process each line here
                    Console.WriteLine($"{lineNumber}- {line}");
                    if (lineNumber == 0)
                    {
                        foo = false;
                    }
                    else
                    {
                        foo = true;
                    }
                }
            }
            return foo;
        }



        static void ListTask(Int32 liney)
        {
            var tasks_path = Path.Combine(Directory.GetCurrentDirectory(), "todo", "paths.txt");
            // Get the path of the choosen task
            using (StreamReader reader = new StreamReader(tasks_path))
            {
                string? line;
                int lineNumber = 0; // To keep track of the line number
                //
                while ((line = reader.ReadLine()) is not null)
                {
                    lineNumber++; // Counter
                    if (liney == lineNumber)  // If the line number is equal to the line choosen by the user
                    {
                        using (StreamReader reader2 = new StreamReader(line))
                        {
                            string? line2;
                            while ((line2 = reader2.ReadLine()) is not null)
                            {
                                Console.WriteLine(line2);
                            }
                        }
                    }
                }
            }
        }

        static string IDGenerator(int length) // Check out Password Generator Repo! ;)
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random(); //Initialize random generator 
            var id = new char[length];
            // add each character randomly choosen in the string
            for (int i = 0; i < length; i++)
            {
                id[i] = characters[random.Next(characters.Length)];
            }
            return new string(id); // return the string
        }
    }
}

