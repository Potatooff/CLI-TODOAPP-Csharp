using System;
using System.Globalization;
using System.IO;


namespace wassup
{
    class Program
    {
        static void Main() // gather everything and run!
        {
            
            Console.WriteLine("Hello World!");
            MainMenu();
        }

        static void worker1(string? user_choice)
        {
            switch (user_choice) // Goes through the user choice 
            {
                case "1":
                    Console.WriteLine("You choosed: Add task");
                    string file_name = IDGenerator(10); // Generate a random file name
                    var file_path = Path.Combine(Directory.GetCurrentDirectory(),"todo" , file_name + ".txt"); // Combine the file name with the current directory
                    Console.Write("Enter the title of the task: "); // Ask the title
                    string? title = Console.ReadLine(); // Read the title
                    Console.WriteLine("Enter the task: "); // Ask the task
                    string? task = Console.ReadLine(); // Read the task
                    AddTask(title, task, file_path, file_path); // Run the AddTask function
                    //MainMenu();
                    break;

                case "2":
                    Console.WriteLine("You choosed: View task");
                    bool checker = ReadTitle();
                    if (checker == true)
                    {
                        Console.Write("Choose the task: ");
                        Int32 task_choosen = Convert.ToInt32(Console.ReadLine());
                        ListTask(task_choosen);
                        break;
                    }
                    else
                    {break;}

                case "3":
                    Console.WriteLine("You choosed: Delete task");
                    bool checker2 = ReadTitle();
                    if (checker2 == true)
                    {
                        Console.Write("Choose the task: ");
                        Int32 task_to_delete = Convert.ToInt32(Console.ReadLine());
                        DeleteTask(task_to_delete);
                        break;
                    }
                    else
                    {break;}

                case "4":
                    Console.WriteLine("You choosed: Exit");
                    Environment.Exit(0);
                    break;
            }

        }

        static string MainMenu()
        {
            Console.WriteLine("1. Add task\n2. View tasks\n3. Delete task\n4. Exit");
            while (true)
            {
                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine(); 

                if (choice == "1" || choice == "2" || choice == "3" || choice == "4")
                {
                    worker1(choice); // Run the worker1 method
                    
                }

                else
                {
                    Console.WriteLine("Invalid choice. Please try again!");
                }
                
            }
           
        }

        static void AddTask(string? title, string? task, string filepath, string? task_path)
        {

            try
            {
                var tasks_path = Path.Combine(Directory.GetCurrentDirectory(),"todo" , "tasks.txt");
                var _f = Path.Combine(Directory.GetCurrentDirectory(),"todo" , "paths.txt");
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
                    sw.WriteLine("Title: " + title + "\n\n" + task+ "\n"); // Easy!
                    sw.Dispose();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("An error occured while adding task: " + e.Message);
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
            var tasks_path = Path.Combine(Directory.GetCurrentDirectory(),"todo" , "tasks.txt");
            var _path = Path.Combine(Directory.GetCurrentDirectory(),"todo" , "paths.txt");

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
                                Console.WriteLine("File deleted successfully.");
                            }
                            else
                            {
                                Console.WriteLine("The file does not exist.");
                            }
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine($"An error occurred while deleting the file: {ex.Message}");
                        }
                    }
                }
                reader.Dispose();
            }
            DeleteLine(choice, _path); // Delete the line in the paths.txt file
        }

        static bool ReadTitle()
        {
            var tasks_path = Path.Combine(Directory.GetCurrentDirectory(),"todo" , "tasks.txt");
            bool foo = false;
            using (StreamReader reader = new StreamReader(tasks_path))
            {
                string? line;
                int lineNumber = 0; // To keep track of the line number

                while ((line = reader.ReadLine()) != null)
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
            var tasks_path = Path.Combine(Directory.GetCurrentDirectory(),"todo" , "paths.txt");
            using (StreamReader reader = new StreamReader(tasks_path))
            {
                string? line;
                int lineNumber = 0; // To keep track of the line number

                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++; // Counter
                    if (liney == lineNumber)  // If the line number is equal to the line choosen by the user
                    {
                        using (StreamReader reader2 = new StreamReader(line))
                        {
                            string? line2;
                            while ((line2 = reader2.ReadLine()) != null)
                            {
                                Console.WriteLine(line2);
                            }
                        }
                    }
                }
            }

        }

        static string IDGenerator(int length) 
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random(); //Initialize random generator 
            var id = new char[length];

            for (int i =0; i < length; i++) // Loop for the password length
            {
                id[i] = characters[random.Next(characters.Length)];
            }

            return new string(id);
        }
    }
}