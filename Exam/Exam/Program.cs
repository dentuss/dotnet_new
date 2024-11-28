using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static List<User> users = new List<User>();
    static List<Quiz> quizzes = new List<Quiz>();
    static User currentUser;

    static void Main(string[] args)
    {
        SeedQuizzes();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Quiz!");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");
            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Login();
                    break;
                case "2":
                    Register();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void Register()
    {
        Console.Clear();
        Console.WriteLine("Registration");
        Console.Write("Enter a username: ");
        string login = Console.ReadLine();

        if (users.Any(u => u.Login == login))
        {
            Console.WriteLine("A user with this username already exists.");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter a password: ");
        string password = Console.ReadLine();

        Console.Write("Enter your date of birth (yyyy-mm-dd): ");
        DateTime birthDate;
        if (!DateTime.TryParse(Console.ReadLine(), out birthDate))
        {
            Console.WriteLine("Invalid date format.");
            Console.ReadKey();
            return;
        }

        users.Add(new User(login, password, birthDate));
        Console.WriteLine("Registration completed!");
        Console.ReadKey();
    }

    static void Login()
    {
        Console.Clear();
        Console.WriteLine("Login");
        Console.Write("Enter your username: ");
        string login = Console.ReadLine();
        Console.Write("Enter your password: ");
        string password = Console.ReadLine();

        currentUser = users.FirstOrDefault(u => u.Login == login && u.Password == password);

        if (currentUser == null)
        {
            Console.WriteLine("Invalid username or password.");
            Console.ReadKey();
            return;
        }

        UserMenu();
    }

    static void UserMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Welcome, {currentUser.Login}!");
            Console.WriteLine("1. Start a new quiz");
            Console.WriteLine("2. View my results");
            Console.WriteLine("3. View Top-20 scores");
            Console.WriteLine("4. Settings");
            Console.WriteLine("5. Logout");
            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    StartQuiz();
                    break;
                case "2":
                    ShowResults();
                    break;
                case "3":
                    ShowTopScores();
                    break;
                case "4":
                    UpdateSettings();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void StartQuiz()
    {
        Console.Clear();
        Console.WriteLine("Select a quiz:");
        for (int i = 0; i < quizzes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {quizzes[i].Name}");
        }
        Console.Write("Enter the quiz number: ");
        int quizIndex;
        if (!int.TryParse(Console.ReadLine(), out quizIndex) || quizIndex < 1 || quizIndex > quizzes.Count)
        {
            Console.WriteLine("Invalid choice.");
            Console.ReadKey();
            return;
        }

        Quiz selectedQuiz = quizzes[quizIndex - 1];
        int score = selectedQuiz.Start();
        currentUser.AddResult(selectedQuiz.Name, score);

        Console.WriteLine($"You scored {score} points!");
        Console.ReadKey();
    }

    static void ShowResults()
    {
        Console.Clear();
        Console.WriteLine("Your results:");
        foreach (var result in currentUser.Results)
        {
            Console.WriteLine($"{result.Key}: {result.Value} points");
        }
        Console.ReadKey();
    }

    static void ShowTopScores()
    {
        Console.Clear();
        Console.WriteLine("Top-20 is not available in this version.");
        Console.ReadKey();
    }

    static void UpdateSettings()
    {
        Console.Clear();
        Console.WriteLine("Update settings");
        Console.Write("New password: ");
        currentUser.Password = Console.ReadLine();
        Console.Write("New date of birth (yyyy-mm-dd): ");
        DateTime birthDate;
        if (DateTime.TryParse(Console.ReadLine(), out birthDate))
        {
            currentUser.BirthDate = birthDate;
        }
        Console.WriteLine("Settings updated.");
        Console.ReadKey();
    }

    static void SeedQuizzes()
    {
        quizzes.Add(new Quiz("History", new List<Question>
        {
            new Question("Who was the first president of the USA?", new List<string> { "George Washington" }),
            new Question("When did World War II start?", new List<string> { "1939" })
        }));
        quizzes.Add(new Quiz("Geography", new List<Question>
        {
            new Question("What is the capital of France?", new List<string> { "Paris" }),
            new Question("What is the longest river in the world?", new List<string> { "Amazon", "Nile" })
        }));
    }
}

class User
{
    public string Login { get; set; }
    public string Password { get; set; }
    public DateTime BirthDate { get; set; }
    public Dictionary<string, int> Results { get; set; }

    public User(string login, string password, DateTime birthDate)
    {
        Login = login;
        Password = password;
        BirthDate = birthDate;
        Results = new Dictionary<string, int>();
    }

    public void AddResult(string quizName, int score)
    {
        Results[quizName] = score;
    }
}

class Quiz
{
    public string Name { get; set; }
    public List<Question> Questions { get; set; }

    public Quiz(string name, List<Question> questions)
    {
        Name = name;
        Questions = questions;
    }

    public int Start()
    {
        int score = 0;
        foreach (var question in Questions)
        {
            Console.Clear();
            Console.WriteLine(question.Text);
            Console.WriteLine("Enter your answers separated by commas: ");
            string[] answers = Console.ReadLine().Split(',');

            if (question.IsCorrect(answers))
                score++;
        }
        return score;
    }
}

class Question
{
    public string Text { get; set; }
    public List<string> CorrectAnswers { get; set; }

    public Question(string text, List<string> correctAnswers)
    {
        Text = text;
        CorrectAnswers = correctAnswers;
    }

    public bool IsCorrect(string[] answers)
    {
        return CorrectAnswers.All(a => answers.Contains(a)) && answers.All(a => CorrectAnswers.Contains(a));
    }
}
