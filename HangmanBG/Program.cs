using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanBG
{
    class Program
    {

        static string word;
        static char[] guess;
        static int lives;
        static ArrayList usedLetters = new ArrayList();
        static List<string> arrayWithCountries = new List<string>();
        static List<string> arrayWithCapitals = new List<string>();
        static int tempOfRowCapital;
        static DateTime dt;
        static int guessingCount;
        static int guessing_time;
        static string score;
        static string pathToScore;
        static string pathToCauntriesAndCapitals;
        static List<ScoreBoard> scoreBoardList = new List<ScoreBoard>();


        static string[] status = new string[]
        {
     "  _______    \n"+
     " |/      |   \n"+
     " |      (_)  \n"+
     " |      \\|/  \n"+
     " |       |   \n"+
     " |      / \\  \n"+
     " |           \n"+
     "_|___        \n",

     "  _______    \n"+
     " |/      |   \n"+
     " |      (_)  \n"+
     " |      \\|/  \n"+
     " |       |   \n"+
     " |           \n"+
     " |           \n"+
     "_|___        \n",

     "  _______    \n"+
     " |/      |   \n"+
     " |      (_)  \n"+
     " |      \\|/  \n"+
     " |           \n"+
     " |           \n"+
     " |           \n"+
     "_|___        \n",

     "  _______    \n"+
     " |/      |   \n"+
     " |      (_)  \n"+
     " |           \n"+
     " |           \n"+
     " |           \n"+
     " |           \n"+
     "_|___        \n",

     "  _______    \n"+
     " |/          \n"+
     " |           \n"+
     " |           \n"+
     " |           \n"+
     " |           \n"+
     " |           \n"+
     "_|___        \n",

        };

        static void Main(string[] args)
        {
            pathToCauntriesAndCapitals = @"C:\Users\bartlomiej\Desktop\Motorola Academy - Recruitment Task\HangmanBG\HangmanBG\Resources\countries_and_capitals.txt";
            pathToScore = @"C:\Users\bartlomiej\Desktop\Motorola Academy - Recruitment Task\HangmanBG\HangmanBG\Resources\score_board.txt";

                LoadDataToArraysFromFile(pathToCauntriesAndCapitals);
                Launch();
        
        }
        static void Addlives()
        {
            lives = 5;
        }
        static void Start()
        {
            Console.Clear();
            guessingCount = 0;
            dt = DateTime.Now;
            word = PickWord();
            BlankLetters();
        }
        private static void Launch()
        {
            Addlives();        
            Start();
            while (lives > 0)
            {

                PrintWord();
                PromptPlayer();
                string input = GetInput();
                TableAddUsedLetters(input);
                CheckInput(input);
                CheckGameOver();

            }
        }
        static string PickWord()
        {
            Random random = new Random();

            int index = random.Next(0, arrayWithCapitals.Count());
            tempOfRowCapital = index;
            return arrayWithCapitals[index].ToUpper();
        }
        static void BlankLetters()
        {
            guess = new char[word.Length];
            for (int i = 0; i < guess.Length; i++)
            {
                guess[i] = '_';
            }
        }
        static void PrintWord()
        {
            Console.WriteLine(
" _  \n" +
"| | \n" +
"| |__   __ _ _ __   __ _ _ __ ___   __ _ _ __ \n" +
"| '_ \\ / _` | '_ \\ / _` | '_ ` _ \\ / _` | '_ \\ \n" +
"| | | | (_| | | | | (_| | | | | | | (_| | | | | \n" +
"|_| |_|\\__,_|_| |_|\\__, |_| |_| |_|\\__,_|_| |_|  by Bartlomiej Gluch\n" +
"                   __ / | \n" +
"                   |___/ \n"
                );

            Console.WriteLine("Guess the letter or the whole word \n");

            for (int i = 0; i < guess.Length; i++)
            {
                Console.Write(guess[i] + " ");
            }

            Console.WriteLine(" ");
            TableShowUsedLetters();
            Console.WriteLine(" ");
        }
        static void PromptPlayer()
        {
            if (lives < 5)
            {
                Console.WriteLine(status[lives]);
                Console.WriteLine($"You have {lives} life points");
                if (lives < 2)
                {
                    Console.WriteLine("Hint: The capital of " + arrayWithCountries[tempOfRowCapital]);
                }
            }
        }
        static string GetInput()
        {
            string input = Console.ReadLine();
            input = input.ToUpper();
            guessingCount++;
            return input;
        }
        static void CheckInput(string input)
        {
            bool correct = false;

            if (input.Length == 1)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    if (word[i] == input[0])
                    {
                        guess[i] = input[0];
                        correct = true;
                    }
                }

                if (!correct)
                {
                    lives--;
                }
                Console.Clear();
            }
            else if (input.Length > 1)
            {
                if (word == input)
                {
                    for (int i = 0; i < word.Length; i++)
                    {
                        guess[i] = input[i];
                    }
                }
                else
                {
                    if (lives >= 2)
                    {
                        lives -= 2;
                    }
                    else
                    {
                        lives = 0;
                    }
                }
                Console.Clear();
            }
            Console.Clear();
        }
        static void CheckGameOver()
        {
            if (lives == 0)
            {
                Console.WriteLine("Game Over !\n");
                Console.WriteLine($"The word was {word}");
                Console.WriteLine(" ");
                Console.WriteLine(status[lives]);
                ShowEndScreen();
            }
            else if (!guess.Contains('_'))
            {
                Console.WriteLine("You Win!\n");
                Console.WriteLine($"The word was {word}");
                guessing_time = dt.Second;
                Console.WriteLine($"You guessed the capital after {guessingCount} entries. It took you {guessing_time} seconds");
                Console.WriteLine(" ");
                Console.WriteLine(status[lives]);
                AddScore(pathToScore);
                ShowEndScreen();
            }
        }

        static void ShowEndScreen()
        {
            showTenBestResult();
            Console.WriteLine("Press R to restart, press any different key to quit");
            ConsoleKeyInfo k = Console.ReadKey();

            if (k.Key == ConsoleKey.R)
            {
                Addlives();
                TableClearUsedLetters();
                Start();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        static void TableAddUsedLetters(string usedletter)
        {
            usedLetters.Add(usedletter);
        }
        static void TableShowUsedLetters()
        {
            Console.WriteLine(" ");
            Console.WriteLine("Used letters:");
            foreach (string str in usedLetters)
            {
                if (str.Length == 1)
                    Console.Write(str + ",");
            }
        }

        static void TableClearUsedLetters()
        {
            usedLetters.Clear();
        }
        static internal void LoadDataToArraysFromFile(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split('|');
                arrayWithCountries.Add(line[0].Trim());
                arrayWithCapitals.Add(line[1].Trim());

            }
        }
        static void AddScore(string path)
        {
            Console.WriteLine("Enter your result :");
           
            Console.Write("Enter name: ");
            string name = Console.ReadLine();              
         
            DateTime dateDT = DateTime.Now;
            string date = dateDT.ToString();

         ScoreBoard temp = new ScoreBoard(name, date, guessing_time, guessingCount, word);

                scoreBoardList.Add(temp);
            score = temp.ToString();

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(score);              
            }
        }

        static void showTenBestResult() {

           string[] temp  = System.IO.File.ReadAllLines(pathToScore);
            int result1=0;
            int result2 = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                string[] line = temp[i].Split('|');

                try
                {
                    result1 = Int32.Parse(line[2]);
                     result2 = Int32.Parse(line[3]);
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Unable to parse '{result1}'");
                }

                ScoreBoard tempBest = new ScoreBoard(line[0], line[1], result1, result2, line[4]);

                scoreBoardList.Add(tempBest);
            }

            //      scoreBoardList.Sort(delegate (ScoreBoard x, ScoreBoard y)
            //         {
            //               if (x.Name == null && y.Name == null) return 0;
            //              else if (x.Name == null) return -1;
            //               else if (y.Name == null) return 1;
            //              else return x.Name.CompareTo(y.Name);
            //          });

            scoreBoardList.Sort();

            Console.WriteLine("Score board sorted by the lowest numbers of guessing tries");
            int place = 0;    
            foreach (ScoreBoard aScore in scoreBoardList)
            {
                if (place < 10)
                {                
                    Console.WriteLine($"{place+1}. "+ aScore);
                    place++;
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine();

        }
    }
    public class ScoreBoard : IEquatable<ScoreBoard>, IComparable<ScoreBoard>
    {
        public string Name { get; set; }

        public string Date { get; set; }

        public int GuessingTime { get; set; }

        public int GuessingTries { get; set; }

        public string GuessedWord { get; set; }

        public ScoreBoard(string name, string date, int guessingTime, int guessingTries, string guessedWord)
        {
            Name = name;
            Date = date;
            GuessingTime = guessingTime;
            GuessingTries = guessingTries;
            GuessedWord = guessedWord;
        }

        public override string ToString()
        {
            return Name + " | " + Date  + " | " + GuessingTime + " | " + GuessingTries + " | " + GuessedWord;
        }
       


        public int SortByNameAscending(string name1, string name2)
        {

            return name1.CompareTo(name2);
        }

        public override bool Equals(object obj)
        {
            return obj is ScoreBoard part &&
                   Name == part.Name &&
                   Date == part.Date &&
                   GuessingTime == part.GuessingTime &&
                   GuessingTries == part.GuessingTries &&
                   GuessedWord == part.GuessedWord;
        }

        public override int GetHashCode()
        {
            int hashCode = 1534529556;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Date);
            hashCode = hashCode * -1521134295 + GuessingTime.GetHashCode();
            hashCode = hashCode * -1521134295 + GuessingTries.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(GuessedWord);
            return hashCode;
        }

        public int CompareTo(ScoreBoard compareGuessingTries)
        {
            if (compareGuessingTries == null)
                return 1;

            else
                return this.GuessingTries.CompareTo(compareGuessingTries.GuessingTries);
        }

        public bool Equals(ScoreBoard other)
        {
            if (other == null) return false;
            return (this.GuessingTries.Equals(other.GuessingTries));
        }
    }
    }


