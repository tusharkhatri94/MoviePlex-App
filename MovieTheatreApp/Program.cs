using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp1
{
    static class Globals
    {
        public static Queue Moviequeue = new Queue();
        public static Queue Ratingqueue = new Queue();
        public static int RatingAge;
    }

    class OptionException : ApplicationException
    {
        private string messageDetails;

        public OptionException() { }
        public OptionException(string message)
        {
            messageDetails = message;
        }
        public override string Message => $"Error Message: {messageDetails}";
    }

    class Movie
    {
        public void SelectOption()
        {
            Console.WriteLine();
            Console.WriteLine("Please select from the following options: ");
            Console.WriteLine("1: Administrator");
            Console.WriteLine("2: Guest");
            Console.Write("Your selected option is: ");
            string line = Console.ReadLine();
            Console.WriteLine();
            try
            {
                if (!(int.TryParse(line, out int caseSwitch)))
                {
                    OptionException ex = new OptionException("Invalid input should be number not a string");
                    throw ex;
                }
                else
                {
                    switch (caseSwitch)
                    {
                        case 1:
                            EnterPassword(5);
                            break;
                        case 2:
                            GuestOption();
                            break;
                        default:
                            Console.WriteLine("The option you selected is incorrect. Please try again.");
                            SelectOption();
                            break;
                    }
                }
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine();
                SelectOption();
            }
        }


        public void EnterPassword(int counter)
        {
            Console.Write("Please enter the Admin Password: ");
            string pwd = Console.ReadLine();
            Console.WriteLine();
            if (pwd == "Admin@123")
                EnterMovies();
            else if (pwd == "B")
            {
                Console.WriteLine("You have chosen to go back to previous menu.");
                SelectOption();
            }
            else
            {
                if (counter == 1)
                {
                    Console.WriteLine("You have incorrectly entered the password 5 times. Taking you back to the main menu.");
                    SelectOption();
                }
                else
                {
                    counter--;
                    Console.WriteLine("You have entered an Invalid Password.");
                    Console.WriteLine("You have {0} more attempts to enter the correct password OR press B to go back to previous menu.", counter);
                    Console.WriteLine();
                    EnterPassword(counter);
                }
            }
        }
        public string parseNumToWord(int i)
        {
            switch (i)
            {
                case 1:
                    return "First";
                case 2:
                    return "Second";
                case 3:
                    return "Third";
                case 4:
                    return "Fourth";
                case 5:
                    return "Fifth";
                case 6:
                    return "Sixth";
                case 7:
                    return "Seventh";
                case 8:
                    return "Eighth";
                case 9:
                    return "Ninth";
                case 10:
                    return "Tenth";
                default:
                    return "";
            }
        }

        public void EnterMovies()
        {
            Console.WriteLine("Welcome MoviePlex Administrator");
            Console.WriteLine();
            Console.Write("How many movies are playing today?: ");
            string line = Console.ReadLine();
            Console.WriteLine();
            try
            {
                if (!int.TryParse(line, out int movieCounter))
                {
                    OptionException ex = new OptionException("Invalid input should be number not a string");
                    throw ex;
                }
                else
                {
                    if (movieCounter < 0)
                    {
                        Console.WriteLine("Number of Movies cannot be a negative number.");
                        Console.WriteLine();
                        EnterMovies();
                    }
                    else if (movieCounter > 10)
                    {
                        Console.WriteLine("Please limit the movies to 10. ");
                        Console.WriteLine();
                        EnterMovies();
                    }
                    int i = 1;
                    Globals.Moviequeue = new Queue();
                    Globals.Ratingqueue = new Queue();
                    List<string> ListA = new List<string>
                    {
                        "G",
                        "PG",
                        "PG-13",
                        "R",
                        "NC-17",
                    };
                    for (int k = 0; k <= 120; k++)
                    {
                        ListA.Add(k.ToString());
                    }
                    bool ratingFlag = true;
                    while (i <= movieCounter)
                    {
                        Console.Write("Please Enter the {0} Movie's Name: ", parseNumToWord(i));
                        string movie = Console.ReadLine();
                        Globals.Moviequeue.Enqueue(movie);
                    enterRating: Console.Write("Please Enter the Age Limit or Rating of the Movie Name: ");
                        string rating = Console.ReadLine();
                        foreach (string a in ListA)
                        {
                            if (a == rating.Trim() || a == rating.ToUpper())
                            {
                                Globals.Ratingqueue.Enqueue(a);
                                i++;
                                ratingFlag = true;
                                break;
                            }
                            else
                            {
                                ratingFlag = false;
                            }
                        }
                        if (!ratingFlag)
                        {
                            Console.WriteLine("Invalid rating. Please try again.");
                            goto enterRating;
                        }
                    }
                    printMovies();
                }
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine();
                EnterMovies();
            }

        }

        public void printMovies()
        {
            int a = 1;
            object[] MovieArray = Globals.Moviequeue.ToArray();
            object[] RatingArray = Globals.Ratingqueue.ToArray();
            Console.WriteLine();
            for (int i = 0; i < MovieArray.Length; i++)
            {
                Console.WriteLine("{0}. {1} {{{2}}}", a, Convert.ToString(MovieArray[i]).Trim(), Convert.ToString(RatingArray[i]).Trim());
                a++;
            }
            Console.Write("Your Movies Playing Today Are Listed Above. Are You Satisfied? (Y/N)? ");
            string satFlag = Console.ReadLine();
            if (satFlag == "y" || satFlag == "Y")
            {
                Console.WriteLine("All movies are saved.");
                Console.WriteLine();
                SelectOption();
            }
            else if (satFlag == "n" || satFlag == "N")
            {
                Console.WriteLine("Taking you back to entering movies.");
                Console.WriteLine();
                EnterMovies();
            }
            else
            {
                Console.WriteLine("Entered input is incorrect. Please try again.");
                Console.WriteLine();
                printMovies();
            }
        }

        public void checkRatingAge(string rating)
        {
            if (rating == "G")
                Globals.RatingAge = 0;
            else if (rating == "PG")
                Globals.RatingAge = 10;
            else if (rating == "PG-13")
                Globals.RatingAge = 13;
            else if (rating == "R")
                Globals.RatingAge = 15;
            else if (rating == "NC-17")
                Globals.RatingAge = 17;
            else
                Globals.RatingAge = Convert.ToInt32(rating);
        }

        public void GuestOption()
        {
            if (Globals.Moviequeue.Count == 0)
            {
                Console.WriteLine("No movies available.");
                SelectOption();
            }
            else
            {
                int a = 1;
                object[] MovieArray = Globals.Moviequeue.ToArray();
                object[] RatingArray = Globals.Ratingqueue.ToArray();
                Console.WriteLine();
                for (int i = 0; i < MovieArray.Length; i++)
                {
                    Console.WriteLine("{0}. {1} {{{2}}}", a, Convert.ToString(MovieArray[i]).Trim(), Convert.ToString(RatingArray[i]).Trim());
                    a++;
                }

            movieSelection: Console.Write("Which movie do you like to Watch: ");
                string line = Console.ReadLine();
                try
                {
                    if (!(int.TryParse(line, out int MovieSelection)))
                    {
                        OptionException ex = new OptionException("Invalid input should be number not a string");
                        throw ex;
                    }
                    else
                    {
                        if (MovieSelection > MovieArray.Length || MovieSelection < 0)
                        {
                            Console.WriteLine("Please select correct option.");
                        }
                        else
                        {
                            Console.WriteLine("Selected movie is: {0} {{{1}}}", MovieArray[MovieSelection - 1], RatingArray[MovieSelection - 1]);
                        ageVerification: Console.Write("Please enter your Age for Verification: ");
                            string ageLine = Console.ReadLine();
                            try
                            {
                                if (!(int.TryParse(ageLine, out int age)))
                                {
                                    OptionException ex = new OptionException("Invalid input should be number not a string");
                                    throw ex;
                                }
                                else
                                {
                                    checkRatingAge(RatingArray[MovieSelection - 1].ToString());
                                    if (age < 0 && age >= 120)
                                    {
                                        Console.WriteLine("Invalid Age. Please try again");
                                        goto ageVerification;
                                    }
                                    else if (age >= Globals.RatingAge)
                                    {
                                        Console.WriteLine("Enjoy your movie!!!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("You are underaged to watch this movie. Please choose another option.");
                                        goto movieSelection;
                                    }

                                }
                            }
                            catch (OptionException e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine();
                                goto ageVerification;
                            }

                        menuOption: Console.WriteLine("Press M to go back to Guest Menu.");
                            Console.WriteLine("Press S to go back to Start Page");
                            string menuLine = Console.ReadLine();
                            if (menuLine.ToUpper() == "M")
                            {
                                GuestOption();
                            }
                            else if (menuLine.ToUpper() == "S")
                            {
                                SelectOption();
                            }
                            else
                            {
                                Console.WriteLine("Please choose correct option");
                                goto menuOption;
                            }
                        }
                    }
                }
                catch (OptionException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine();
                    goto movieSelection;
                }
            }



        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string str = "*** Welcome to MoviePlex Theatre ***";
            string str2 = "************************************";
            Console.SetCursorPosition((Console.WindowWidth - str2.Length) / 2, Console.CursorTop);
            Console.WriteLine(str2);
            Console.SetCursorPosition((Console.WindowWidth - str2.Length) / 2, Console.CursorTop);
            Console.WriteLine(str);
            Console.SetCursorPosition((Console.WindowWidth - str2.Length) / 2, Console.CursorTop);
            Console.WriteLine(str2);
            Movie movie = new Movie();
            movie.SelectOption();
        }
    }
}