using BankApp;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace BankApp
{
    public class RunMenu
    {
        
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Generic GreedyBank");
            Console.WriteLine("Press Enter to countinue...");
            Console.ReadLine();
            Console.Clear();
            MainMenu.MainLogin();

        }

    }
    public static class MainMenu 
    {
        static string[] userNames = { "Robbe", "Kjell", "Leo", "Fibbe", "Mimmi" };
        static string[] pincodes = { "123", "12345", "1337", "13372", "13373" };
        static string user;
        static string pincode;
        public static int activeUser;
        public static void MainLogin()
        {
            Console.WriteLine("Please log in");
            var _accountTrue = new Account();
            bool mainMenu = true;
            //Tries for login
            int tries = 0;

            while (mainMenu)
            {
                Console.WriteLine("Please enter your username.");
                Console.WriteLine("User name:");
                user = Console.ReadLine();
                for (int i = 0; i < userNames.Length; i++)
                {
                    if (userNames[i] == user)
                    {
                        Console.WriteLine("Please enter your pincode");
                        Console.WriteLine("Pincode:");
                        pincode = Console.ReadLine();
                        if (pincodes[i] == pincode)
                        {
                            Console.Clear();
                            Console.WriteLine("Login succsesful");
                            _accountTrue.UserIndex = i;                      
                            // make int index value for account menu
                            _accountTrue.accountTrue();
                        }
                        else
                        {
                            Console.WriteLine("Invalid input, please try again");
                        }
                        if (pincodes[i] != pincode)
                        {
                            tries++;
                        }
                        if (tries >= 3)
                        {
                            Console.WriteLine("Too Many tries, account locked for 3 mins");
                            Thread.Sleep(6000);
                            Console.WriteLine("Please try again in 2 mins..");
                            Thread.Sleep(6000);
                            Console.WriteLine("Please try again in 1 min...");
                            Thread.Sleep(6000);
                            tries = 0;
                            Console.WriteLine("You can now try to log in again");
                            Console.WriteLine("Press Any key to continue");
                            Console.ReadLine();
                            Console.Clear();
                            MainLogin();
                        }
                    }
                }


            }

        }
    }

    public class Account
    { 
        

        protected int userIndex;

        public int UserIndex
        {
            get { return userIndex; }
            set { userIndex = value; }
        }
      
       public void accountTrue()
       {
            var _assets = new assets();
            _assets.AssetsIndex = userIndex;

        bool accountMenu = true;
        while (accountMenu)
        {
            Console.WriteLine("What would you like to do ?");
            Console.WriteLine("[1]: Look over your accounts and funds. ");
            Console.WriteLine("[2]: Deposit funds between accounts. ");
            Console.WriteLine("[3]: Withdraw funds.");
            Console.WriteLine("[4]: Log out.");
            string command = Console.ReadLine();


            switch (command)
            {
                case "1":
                        //make function prints out accoouts and funds
                        printAcc();
                    break;
                case "2":
                        exchange();
                    break;
                case "3":
                    //Make function to withdaraw funds
                    break;
                case "4":
                        Console.WriteLine("You are logging out.");
                        Console.WriteLine("Press Any key to continue");
                        Console.Read();
                        Console.Clear();
                        MainMenu.MainLogin();
                    break;
                default:
                        // write invalid input 3 
                        Console.WriteLine("Invalid input");
                    break;
            }
        }
       }
        public void printAcc()
        {
            int count = 0;
            for (int i = 0; i < assets.accounts[userIndex].Length; i++)
            {
                Console.Write("|" + i + "|" +assets.accounts[userIndex][i]);

                for (int j = 0; j < 1; j++)
                {
                    Console.Write(assets.funds[userIndex][count++] + " Sek\n");
                }
            }                  
        }
        public void exchange()
        {
            int count = 0;
            //Prints out accounts and funds
            Console.WriteLine("Choose accout to depposit money.");
            for (int i = 0; i < assets.accounts[userIndex].Length; i++)
            {
                Console.Write("|" + i + "|"+assets.accounts[userIndex][i]);
                for (int j = 0; j < 1; j++)
                {
                    Console.WriteLine(assets.funds[userIndex][count++] + " Sek\n");
                }
            }
            count= 0;
            int choice1 = int.Parse(Console.ReadLine());
            Console.WriteLine("Choose accout to exchange money from.");
            for (int i = 0; i < assets.accounts[userIndex].Length; i++)
            {

                if (choice1 == i)
                {
                    count++;
                }
                else
                {
                    Console.Write("|" + i + "|" + assets.accounts[userIndex][i]);
                    for (int j = 0; j < 1; j++)
                    {
                        Console.WriteLine(assets.funds[userIndex][count++] + " Sek\n");
                    }
                }

            }
            int choice2 = int.Parse(Console.ReadLine());
            Console.WriteLine("How much money do you want transfer?\n");
            Console.WriteLine("From: " + assets.accounts[userIndex][choice1] + assets.funds[userIndex][choice1] + " Sek");
            Console.WriteLine("To: " + assets.accounts[userIndex][choice2] + assets.funds[userIndex][choice2] + " Sek\n");
        }
    }

    public class assets
    {
        protected int assetsIndex;
        public int AssetsIndex
        {
            get { return assetsIndex; }
            set { assetsIndex = value; }
        }
        public static string[][] accounts =
        {
            new string[] {"Savings: ", "Salary: "},
            new string[] {"Savings: ", "Salary: ","Z-tv Salary: "},
            new string[] {"Savings: ", "Salary: ","Computer Parts: ","Goods: "},
            new string[] {"Savings: ", "Salary: ","Cash for snacks: ","Vet Savings: ","Toy Account: "},
            new string[] {"Savings: ", "Salary: ", "Knitting Salary: ","Cosplay funds: ","Fibbes Xmas present: ","Gaming Gear: "},
        };
        public static decimal[][] funds =
        {
            new decimal[] {1000,20000}
            , new decimal[] {1000,20004,12354}
            , new decimal[] {1000,12312,41231,123451}
            , new decimal[] {1000,12394,12349,123094,129394}
            , new decimal[] {1000,12394,12341,123412,451234,124512}
        };
    }
}
