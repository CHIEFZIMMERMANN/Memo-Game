using System;
using System.Diagnostics;
using System.Globalization;

namespace ConsoleApp2
{
    public class Program
    {
        
        public static Tuple<int, int, string> chooseLevel()
        {
            string numInput1 = "";
            int chances = 0;
            int wordsy = 0;
            string level = "";


            while (chances == 0)
            {
                Console.Write("Enter 1 for Level Easy or 2 for Level Hard: ");
                numInput1 = Console.ReadLine();
                switch (numInput1)
                {
                    case "1":
                        chances = 10;
                        wordsy = 4;
                        level = "Easy";
                        break;

                    case "2":
                        chances = 15;
                        wordsy = 8;
                        level = "Hard";
                        break;

                    default:
                        Console.Write("This is not valid input. Please enter an integer value. ");
                        
                        break;
                }
            }
            
          
            return Tuple.Create(chances, wordsy, level);
            
        }

        private static Random rnd = new Random();

        public static List<string> Shuffle(List<string> x)
        {
            List<string> wordsLoc = x.ToList();

            int n = wordsLoc.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                string value = wordsLoc[k];
                wordsLoc[k] = wordsLoc[n];
                wordsLoc[n] = value;
            }

            return wordsLoc;
        }

        public static List<string> getWords(int wordsQuantity, string[] wordsLines)
        {
            List<string> wordsList = new List<string>();

            while (wordsList.Count < wordsQuantity)
            {

                int mIndex = rnd.Next(wordsLines.Length);
                string word = wordsLines[mIndex];
                if (!wordsList.Any(s => s == word))
                {
                    wordsList.Add(word);
                }
            }

            return wordsList;
        }


        public static List<string> Repeated(int qty, string sign)
        {
            List<string> listR = new List<string>();
            for(int i=0; i< qty; i++)
            {
                listR.Add(sign);
            }

            return listR;

        }

        public static List<string> Growing(int qty, string sign)
        {
            List<string> listR = new List<string>();
            for (int i = 0; i < qty; i++)
            {
                string numbR = (i+1).ToString();
                listR.Add(sign + numbR);
            }

            return listR;

        }


        public static string GetLocation(string sign, List<string> acceptable)
        {
            int flag = 1;
            string loc = "";
            string joined = string.Join(",", acceptable);

            Console.Write($"Enter word position from list: {joined}: ");

            while (flag != 0)
            {
                
                loc = Console.ReadLine();

                if (acceptable.Contains(loc))
                {
                    flag = 0;
                    break;

                }
                else {

                    Console.Write($"Wrong position, select from: {joined}: ");
                    //update screen function

                }
                
                    
                
            }

            return loc;
        }

        public static void consoleUpdate(string lvl,int chances, List<string> BoxA, List<string> BoxB, int highScore, string highScoreValue)
        {
            Console.Clear();
            Console.WriteLine("Level: " + lvl);
            Console.WriteLine("Guess chances: " + chances);
            Console.WriteLine($"Highscore (less is more here):{highScoreValue} with score: "+ highScore);
            Console.WriteLine(string.Format("A: {0}", string.Join(" ", BoxA)));
            Console.WriteLine(string.Format("B: {0}", string.Join(" ", BoxB)));

        }

        static void Restart()
        {
            String[] n = new String[10];
            n[0] = "hi";
            Main(n);
        }

        public static void endGame(string msg, bool win = true, int chances = 0, long seconds = 0)
        {
            Console.Clear();
            Console.WriteLine(msg);
            if (win == true)
            {
                Console.WriteLine($"You solved the memory game after {chances} chances. It took you {seconds} seconds.");
                Console.WriteLine("Enter you name to save game results to highscores");
                string name = Console.ReadLine();
                DateTime localDate = DateTime.Now;
                var culture = new CultureInfo("en-US");
                string date = localDate.ToString(culture);

                string highscore = name + " | " + date + " | " + seconds + " | " + chances;
                Console.WriteLine(highscore);
                using (StreamWriter st = new StreamWriter(@"..\Task\Highscores.txt", true))
                {
                    st.WriteLine(highscore);
                }
                
                //File.WriteAllTextAsync(@"..\Coding Task Motorola Academy C#\Highscores.txt", highscore);

            }
            Console.WriteLine("Do you want to play again? If yes pick Y, if no, pick any different button.");
            string decision = Console.ReadLine();
            if (decision == "Y")
            {
                Console.Clear();
                Restart();
            }
            else
            {

                Environment.Exit(0);

            }
        }

        



        static void Main(string[] args)
        {
            Stopwatch sw;
            sw = Stopwatch.StartNew();

            //READ DATA FROM TXT FILE
            string[] lines = File.ReadAllLines(@"..\Task\Words.txt");
            string[] highScores = File.ReadAllLines(@"..\Task\Highscores.txt");

            int highScVal = 0;
            string highSc = "No highscore yet.";



            List<int> highvalues = new List<int>();
            if(highScores.Length > 0)
            {
                foreach (string hi in highScores)
                {
                    string[] subs = hi.Split(" | ");

                    int timerNumber = Int32.Parse(subs[^2]);
                    int chancesNumber = Int32.Parse(subs[^1]);

                    int hiScore = timerNumber * chancesNumber;

                    highvalues.Add(hiScore);
                }

                int highScIndex = highvalues.IndexOf(highvalues.Min());
                highScVal = highvalues.Min();
                highSc = highScores[highScIndex];
            }
            



            //ASSIGN GAME SETTINGS
            (int chancesQty, int wordsQty, string chosenLevel) = chooseLevel();

            //CREATE LIST OF WORDS       
            List<string> gameWordsA= getWords(wordsQty,lines);

            //CREATE SECONS (SHUFFLED) LIST OF WORDS
            List<string> gameWordsB = Shuffle(gameWordsA);


            List<string> gameBoxA = Repeated(wordsQty, "X");
            List<string> gameBoxB = Repeated(wordsQty, "X");

            List<string> acceptableA = Growing(wordsQty, "A");
            List<string> acceptableB = Growing(wordsQty, "B");

            Console.WriteLine(acceptableA[0]);
            Console.WriteLine(acceptableB[0]);

            int chancesDone = 0;


            while (chancesQty > 0)
            {
                chancesDone ++;

                consoleUpdate(chosenLevel, chancesQty, gameBoxA, gameBoxB, highScVal, highSc);

                string locationA = GetLocation("A", acceptableA);
                int aIndex = (int)Char.GetNumericValue(locationA[1]) - 1;
                string showA = gameWordsA[aIndex];
                gameBoxA[aIndex] = showA;


                consoleUpdate(chosenLevel, chancesQty, gameBoxA, gameBoxB, highScVal, highSc);
                
                string locationB = GetLocation("B", acceptableB);
                int bIndex = (int)Char.GetNumericValue(locationB[1]) - 1;
                string showB = gameWordsB[bIndex];
                gameBoxB[bIndex] = showB;


                if (gameBoxA.Contains("X") && gameBoxB.Contains("X")) { }
                else
                {
                    chancesQty--;
                    endGame("Congratulations!! You won!!", chances : chancesDone, seconds : (sw.ElapsedMilliseconds)/1000);   
                }


                consoleUpdate(chosenLevel, chancesQty, gameBoxA, gameBoxB, highScVal, highSc);



                if (gameBoxA.Contains(showB))
                {
                    acceptableA.Remove(locationA);
                    acceptableB.Remove(locationB);
                }
                else
                {
                    Thread.Sleep(1500);
                    gameBoxB[bIndex] = "X";
                    gameBoxA[aIndex] = "X";
                }


                chancesQty--;

            }


            if(chancesQty == 0)
            {
                endGame("You Lost.");
            }





        }
    }
}