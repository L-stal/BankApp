using BankApp;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Xml.Serialization;

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
        //Acount in main menu for comparing index valus, if index valus are the same run function AccountTrue
        static string[] userNames = { "Robbe", "Kjell", "Leo", "Fibbe", "Mimmi" };
        static string[] pincodes = { "123", "12345", "1337", "13372", "13373" };
        static string user;
        static string pincode;
        //activeUser sends value in to  class Account ot know what account to show, activUser gets its value if the login is a succses
        //it gets the same value  as index value from userNames and pincodes
        public static int activeUser;
        public static void MainLogin()
        {
            var _accountTrue = new Account();
            bool mainMenu = true;
            //Tries for login
            int tries = 0;

            while (mainMenu)
            {
                    Console.WriteLine("Please enter your user name.");
                    Console.Write("User name:");
                    user = Console.ReadLine();
                // A for loops that compare user input to the names in userNames[]
                for (int i = 0; i < userNames.Length; i++)
                {
                    //IF the input matches a userName login , user gets to input password
                    if (userNames[i] == user)
                    {
                        Console.WriteLine("Please enter your pincode");
                        Console.Write("Pincode:");
                        pincode = Console.ReadLine();
                        if (pincodes[i] == pincode)
                        {
                            Console.Clear();
                            Console.WriteLine("Login succsesful");
                            _accountTrue.UserIndex = i;                      
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
            Console.WriteLine("What would you like to do ?\n");
            Console.WriteLine("[1]: Look over your accounts and funds. ");
            Console.WriteLine("[2]: Deposit funds between accounts. ");
            Console.WriteLine("[3]: Withdraw funds.");
            Console.WriteLine("[4]: Log out.");
            string command = Console.ReadLine();


            switch (command)
            {
                case "1":
                        //make function prints out accoouts and funds
                        Console.Clear();
                        printAcc();
                        Console.Clear();
                        break;
                case "2":
                        //functction 
                        Console.Clear();
                        exchange();
                        Console.Clear();
                    break;
                case "3":
                        //Make function to withdaraw funds
                        Console.Clear();
                        withdraw();
                        Console.Clear();
                        break;
                case "4":
                        //logging out
                        Console.Clear();
                        Console.WriteLine("You are logging out.");
                        Console.WriteLine("Press Any key to continue");
                        Console.Read();
                        Console.Clear();
                        accountMenu = false;
                    break;
                default:
                        // If the user inputs a wrong number print this
                        Console.WriteLine("Invalid input,press [Enter] to countinue");
                        Console.ReadLine();
                        Console.Clear();
                    break;
            }
        }
       }
        public void printAcc()
        {
            int count = 0;
            for (int i = 0; i < assets.accounts[userIndex].Length; i++)
            {
                Console.Write("\n" + "|" + i + "|" +assets.accounts[userIndex][i]);

                for (int j = 0; j < 1; j++)
                {
                    Console.Write(assets.funds[userIndex][count++] + " Sek\n");
                }
            }  
            Console.WriteLine("\nPress Enter to continue");
            Console.ReadLine();
        }
        public void exchange()
        {
            int count = 0;
            Console.WriteLine("Choose accout to depposit money to.\n");
            for (int i = 0; i < assets.accounts[userIndex].Length; i++)
            {
                Console.Write("|" + i + "|"+assets.accounts[userIndex][i]);
                for (int j = 0; j < 1; j++)
                {
                    Console.WriteLine(assets.funds[userIndex][count++] + " Sek\n");
                }
            }
            count= 0;
            try
            {
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
                Console.Clear();
                Console.WriteLine("How much money do you want transfer?\n");
                Console.WriteLine("From: " + assets.accounts[userIndex][choice1] + assets.funds[userIndex][choice1] + " Sek");
                Console.WriteLine("To: " + assets.accounts[userIndex][choice2] + assets.funds[userIndex][choice2] + " Sek\n");
                decimal transfer = decimal.Parse(Console.ReadLine());
                assets.funds[userIndex][choice1] -= transfer;
                assets.funds[userIndex][choice2] += transfer;
                Console.WriteLine("test: " + assets.accounts[userIndex][choice1] + assets.funds[userIndex][choice1] + " Sek");
                Console.WriteLine("test new: " + assets.accounts[userIndex][choice2] + assets.funds[userIndex][choice2] + " Sek\n");
                Console.ReadLine();

            }
            catch
            {
                Console.WriteLine("No account found with that number,try again and choose from the list above");
                Console.ReadLine();
                Console.Clear() ;
                exchange();

            }
       
        }

        public void withdraw()
        {
            //FAST PÅ BAR SAVING FIX IT
                int count = 0;
                Console.WriteLine("Choose account to withdraw money from\n");
                for (int i = 0; i < assets.accounts[userIndex].Length; i++)
                {
                    Console.Write("|" + i + "|" + assets.accounts[userIndex][i]);

                    for (int j = 0; j < 1; j++)
                    {
                        Console.Write(assets.funds[userIndex][count++] + " Sek\n");
                    }
                }
            try
            {
                int choice1;
                choice1 = int.Parse(Console.ReadLine());
                    Console.WriteLine("How much money do you want to withdraw?");
                    decimal withdrawM = decimal.Parse(Console.ReadLine());
                    //Fixa så man inte kan skriva tillexemple 1001 och välja konto
                    if (withdrawM <= choice1)
                    {
                        assets.funds[userIndex][choice1] -= withdrawM;
                        Console.WriteLine("You took out:" + withdrawM + "\n Your new balance is:" + choice1);
                        Console.WriteLine("Press [Enter] to continue");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("There is not enough money to make the transfer. Please choose another account");
                        Console.ReadLine();
                        Console.Clear();
                        withdraw();

                    }
            }
            catch 
            {
                Console.WriteLine("Invalid input, choose from the list above.");
                Console.ReadLine();
                Console.Clear();
                withdraw();
            }
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
