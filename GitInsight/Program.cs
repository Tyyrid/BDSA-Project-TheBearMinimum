// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

namespace GitInsight
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Please input the path of a valid git project:");
            string? pathInput = "";
            var validPathFound = false;
            while(!validPathFound)
            {
                pathInput = Console.ReadLine();
                if (!Directory.Exists(pathInput))
                {
                    Console.WriteLine("Path does not exist. Please enter a valid path");
                }
                else if (!Directory.Exists(pathInput + "/.git"))
                {
                    Console.WriteLine("Could not find .git folder. Please enter a path connected to git");
                }
                else if (!LibGit2Sharp.Repository.IsValid(pathInput))
                {
                    Console.WriteLine();
                }
                else
                {
                    
                    validPathFound = true;
                }
            }
            
            
            string? modeInput = "";
            while (modeInput != "q")
            {
                Console.Write("Commit frequency(f) or commit author(a) mode or quit(q)?: ");
                modeInput = Console.ReadLine();

                if (modeInput == "f")
                {
                    Console.WriteLine("Running in frequency mode");
                    frequencyMode(new Repository(pathInput).Commits);
                    break;
                }
                else if (modeInput == "a")
                {
                    Console.WriteLine("Running in author mode");
                    authorMode(new Repository(pathInput).Commits);
                    break;
                }
                else if (modeInput == "q")
                {
                    Console.WriteLine("Program closing");
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }
        }

        public static void frequencyMode(IEnumerable<Commit> commits) {
            var dates = commits.GroupBy(c => c.Author.When.Date);   
            string temp = "";
            foreach (var date in dates)
            {
                temp += date.Count() + " " + date.First().Author.When.Date + "\n";
            }
                Console.Write(temp);
        }

        public static void authorMode(IEnumerable<Commit> commits) {
            Console.WriteLine(commits.GroupBy(c => c.Author.Name, c => c.Author.When.Date));
            //var authors = commits.GroupBy(c => c.
        
        }
    }
}