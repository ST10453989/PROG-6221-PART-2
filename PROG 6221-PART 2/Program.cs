using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using NAudio.Wave;

class Program
{
    static Dictionary<string, string> responses = new Dictionary<string, string>
    {
        { "how are you", "I'm just a program, but I'm here to help you!" },
        { "what's your purpose", "I'm here to provide information about cybersecurity, passwords, phishing, and safe browsing." },
        { "what can i ask you about", "You can ask me about password safety, phishing scams, malware, and social engineering." },
        { "what is phishing", "Phishing is a cyberattack where scammers trick you into revealing sensitive information, often through fake emails or websites." },
        { "how can i create a strong password", "Use a mix of uppercase, lowercase, numbers, and symbols. Avoid using common words or personal info." },
        { "what are safe browsing habits", "Avoid clicking unknown links, enable two-factor authentication, and always verify website URLs before entering personal info." },
        { "what is malware", "Malware is malicious software designed to harm, exploit, or otherwise compromise your computer or network." },
        { "what is social engineering", "Social engineering is a tactic used by hackers to manipulate individuals into divulging confidential information." }
    };

    static Dictionary<string, string> userMemory = new Dictionary<string, string>();
    static string[] randomResponses = new string[]
    {
        "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organizations.",
        "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.",
        "Review the security settings on your accounts regularly to ensure your information is protected."
    };

    static string currentTopic = "";

    static void Main(string[] args)
    {
        PlayWelcomeAudio();
        DisplayAsciiArt();

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("==============================================");
        Console.WriteLine("          Welcome to NETA GPT                ");
        Console.WriteLine("==============================================");
        Thread.Sleep(1000);
        Console.ResetColor();

        string userName;
        do
        {
            Console.Write("Please enter your name (at least 5 characters with a special character): ");
            userName = Console.ReadLine();
        } while (!IsValidName(userName));

        Console.WriteLine($"\nHello, {userName}! How can I assist you today?");

        Console.WriteLine("\nHere are the questions you can ask:");
        foreach (var question in responses.Keys)
        {
            Console.WriteLine($"- {question}");
        }

        while (true)
        {
            Console.Write("\nYou: ");
            string userInput = Console.ReadLine()?.ToLower();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Respond("It looks like you didn't type anything. Try asking about cybersecurity topics, like 'What is phishing?'", ConsoleColor.Red);
                continue;
            }

            if (userInput == "exit")
            {
                Respond("Goodbye! Stay safe online!", ConsoleColor.Green);
                break;
            }

            // Process Part 1 style responses
            if (responses.TryGetValue(userInput, out string response))
            {
                Respond(response, ConsoleColor.Green);
                continue;
            }

            // Part 2: Keyword Recognition
            CheckKeyword(userInput);

            // Part 2: Random Tips
            if (userInput.Contains("tips") || userInput.Contains("tip"))
            {
                Respond(GetRandomResponse(), ConsoleColor.Cyan);
            }

            // Part 2: Conversation Flow
            if (userInput == "more" && !string.IsNullOrEmpty(currentTopic))
            {
                Respond("Continuing with the current topic: " + currentTopic, ConsoleColor.Yellow);
            }

            // Part 2: Sentiment Detection
            if (userInput.Contains("worried") || userInput.Contains("frustrated") || userInput.Contains("curious"))
            {
                Respond("I understand how you feel. Let me provide some information to help.", ConsoleColor.Magenta);
            }

            // Memory Check
            if (!userMemory.ContainsKey(userInput))
            {
                Respond("I'm not sure I understand. Can you try rephrasing?", ConsoleColor.DarkYellow);
            }
        }
    }

    static void PlayWelcomeAudio()
    {
        string filePath = @"C:\Users\RC_Student_lab\source\repos\PROG 6221-PART 1\audio\welcome.wav";

        try
        {
            using (var audioFile = new AudioFileReader(filePath))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();

                Console.WriteLine("Playing audio... Press any key to continue.");
                Console.ReadKey();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error playing audio: {ex.Message}");
        }
    }

    static void DisplayAsciiArt()
    {
        Console.WriteLine(@"
     .-') _   ('-.   .-') _      ('-.                        _ (`-.  .-') _    
    ( OO ) )_(  OO) (  OO) )    ( OO ).-.                   ( (OO  )(  OO) )   
,--./ ,--,'(,------./     '._   / . --. /        ,----.    _.`     \/     '._  
|   \ |  |\ |  .---'|'--...__)  | \-.  \        '  .-./-')(__...--''|'--...__) 
|    \|  | )|  |    '--.  .--'.-'-'  |  |       |  |_( O- )|  /  | |'--.  .--' 
|  .     |/(|  '--.    |  |    \| |_.'  |       |  | .--, \|  |_.' |   |  |    
|  |\    |  |  .--'    |  |     |  .-.  |      (|  | '. (_/|  .___.'   |  |    
|  | \   |  |  `---.   |  |     |  | |  |       |  '--'  | |  |        |  |    
`--'  `--'  `------'   `--'     `--' `--'        `------'  `--'        `--'    
        ");
    }

    static void Respond(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        foreach (char letter in message)
        {
            Console.Write(letter);
            Thread.Sleep(30);
        }
        Console.WriteLine();
        Console.ResetColor();
    }

    static bool IsValidName(string name)
    {
        return name.Length >= 5 && Regex.IsMatch(name, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
    }

    static void CheckKeyword(string userInput)
    {
        if (userInput.Contains("password"))
        {
            Respond("Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.", ConsoleColor.Cyan);
            currentTopic = "Password Safety";
        }
        else if (userInput.Contains("privacy"))
        {
            Respond("Protecting your privacy online is crucial. Be cautious about sharing personal information.", ConsoleColor.Cyan);
            currentTopic = "Privacy Protection";
        }
        else if (userInput.Contains("scam"))
        {
            Respond("Be careful of online scams. Always verify the source before providing any personal information.", ConsoleColor.Cyan);
            currentTopic = "Avoiding Scams";
        }
    }

    static string GetRandomResponse()
    {
        Random rand = new Random();
        return randomResponses[rand.Next(randomResponses.Length)];
    }
}