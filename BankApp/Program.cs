using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;

namespace BankApp
{
    public class RunMenu
    {

        public static void Main(string[] args)
        {
            

            bool runMenu = true;
            while (runMenu)
            {
                Console.Clear();
                //The if else is there because of console and powershell reads from diffrent dir..
                string text;
                if (File.Exists("../../../welcome.txt"))
                {
                    text = File.ReadAllText("../../../welcome.txt");
                }
                else
                {
                    text = File.ReadAllText(".\\welcome.txt");

                }
                
                Console.WriteLine(text);
                Console.WriteLine("Press Enter to countinue...");
                Console.ReadLine();
                Console.Clear();
                MainMenu.MainLogin();
            }


        }

    }
    public static class MainMenu
    {
        //Acount in main menu for comparing index valus, if index valus are the same run function AccountTrue
        static string[] userNames = { "Robbe", "Kjell", "Leo", "Fibbe", "Mimmi" };
        static string[] pincodes = { "123", "12345", "1337", "13372", "13373" };
        //Save login tries to lock the user out //WORKING ON IT//-------------------------------------------
        static int[] logins = { 0, 0, 0, 0, 0, };
        static string user;
        static string pincode;
        static int userTries;
        static DateTime lockoutTime;
        //activeUser sends value in to  class Account ot know what account to show, activUser gets its value if the login is a succses
        //it gets the same value  as index value from userNames and pincodes
        public static int activeUser;
        public static async void MainLogin()
        {
            var _accountTrue = new Account();
            bool mainMenu = true;
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
                        if (logins[i] < 3)
                        {
                            Console.WriteLine("Please enter your pincode");
                            Console.Write("Pincode:");
                            logins[i] = 0;
                            pincode = Console.ReadLine();
                            Console.WriteLine(logins[i]);
                            Console.ReadLine();
                            if (pincodes[i] == pincode)
                            {
                                Console.Clear();
                                Console.WriteLine("Login succsesful");
                                _accountTrue.UserIndex = i;
                                _accountTrue.accountTrue(user);
                                Console.WriteLine("Logout succssesful. Have a great day " + user + "!");
                                Console.WriteLine("----RETURNING TO LOGIN-----");
                                delay();
                                Console.Clear();
                                mainMenu = false;
                                return;
                            }
                            if (pincodes[i] != pincode)
                            {
                                logins[i] += 1;
                            }
                        }
                        else if (logins[i] == 3)
                        {
                            logins[i] += 1;                      
                            lockoutTime = DateTime.Now.AddMinutes(1);
                        }
                        else
                        {
                            Console.WriteLine(logins[i]);
                        }

                    if (logins[i] == 4)
                    {
                            Console.WriteLine("User is locked out, please try again in 3 mins");
                        if (DateTime.Now >= lockoutTime)
                        {
                            logins[i] = 0;
                            lockoutTime = DateTime.Now;
                        }
                    }
                    }
                }
                Console.WriteLine("Invalid input, please try again.");
                Console.WriteLine(lockoutTime);
            }
           
        }

        //public static void watchTimer(int userTries)
        //{
        //    if (userTries == 1)
        //    {

        //      DateTime start = DateTime.UtcNow;
        //      for (int i = 0; i < logins.Length; i++)
        //      {
        //        if (logins[i] > 3)
        //        {
        //            if (DateTime.UtcNow > start.AddMinutes(1))
        //            {
        //                logins[i] = 0;
        //                    userTries = 0;
        //            }
        //            else
        //            {

        //            }
        //        }
               
        //      }
        //    }

        //}

        public class Account
        {
            protected int userIndex;

            public int UserIndex
            {
                get { return userIndex; }
                set { userIndex = value; }
            }

            public void accountTrue(string user)
            {
                var _assets = new assets();
                _assets.AssetsIndex = userIndex;

                bool accountMenu = true;
                while (accountMenu)
                {
                    Console.WriteLine("\nWelcome " + user + "! What would you like to do ?\n");
                    Console.WriteLine("[1]: Look over your accounts and funds. ");
                    Console.WriteLine("[2]: Deposit funds between accounts. ");
                    Console.WriteLine("[3]: Withdraw funds.");
                    Console.WriteLine("[4]: Deposit money.");
                    Console.WriteLine("[5]: Log out.");
                    string command = Console.ReadLine();


                    switch (command)
                    {
                        //Case 1 prints out the account for the activc user
                        case "1":
                            Console.Clear();
                            printAcc(user);
                            Console.Clear();
                            break;
                        //Case 2 is the option to deposit funds between the users OWN accounts
                        case "2":
                            Console.Clear();
                            exchange();
                            Console.Clear();
                            break;
                        //Case 3 is the method for withdrawing funds ------------------------------------------FIXA LOGUT------------------------------------------------------------------------------
                        case "3":
                            Console.Clear();
                            withdraw();
                            Console.Clear();
                            break;
                        //Deposit money
                        case "4":
                            Console.Clear();
                            deposit();
                            Console.Clear();    
                            break;
                        //Case 5 lets the user log out and back to the start screen
                        case "5":
                            Console.Clear();
                            Console.WriteLine("You are logging out.");
                            delay();
                            Console.Clear();
                            accountMenu = false;
                            break;
                        //If the string command dose not mean the requirements this prints out an error message
                        default:
                            Console.WriteLine("Invalid input,press [Enter] to countinue");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                    }
                }
            }

            //A super simple nesteed for loop to print out the jagged array in the assets class for the active use
            //MAKE OPTION TO GO BACK WORKING ON IT - LOW PRIO
            public void printAcc(string user)
            {
                Console.WriteLine("\nUser: " + user);
                int count = 0;
                for (int i = 0; i < assets.accounts[userIndex].Length; i++)
                {
                    Console.Write("\n" + "[" + i + "]" + assets.accounts[userIndex][i] + ": ");

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
                int choice1;
                int choice2;
                Console.WriteLine("\nChoose accout to deposit money to.\n");

                for (int i = 0; i < assets.accounts[userIndex].Length; i++)
                {
                    Console.Write("[" + i + "]" + assets.accounts[userIndex][i] + ": ");
                    for (int j = 0; j < 1; j++)
                    {
                        Console.WriteLine(assets.funds[userIndex][count++] + " Sek\n");
                    }
                }

                count = 0;
                Console.WriteLine("Choose from the list above");
                Console.Write("Enter Your choice :");
                bool success = int.TryParse(Console.ReadLine(), out choice1);
                Console.Write("\n");

                if (success)
                {
                    if (choice1 < assets.accounts[userIndex].Length)
                    {
                        Console.WriteLine("Choose account to deposit money from.\n");
                        for (int i = 0; i < assets.accounts[userIndex].Length; i++)
                        {

                            if (choice1 == i)
                            {
                                count++;
                            }
                            else
                            {
                                Console.Write("[" + i + "]" + assets.accounts[userIndex][i] + ": ");
                                for (int j = 0; j < 1; j++)
                                {
                                    Console.WriteLine(assets.funds[userIndex][count++] + " Sek\n");
                                }
                            }
                        }
                        bool succ = true;
                        while (succ)
                        {
                            Console.Write("Enter Your choice: ");
                            bool succ2 = int.TryParse(Console.ReadLine(), out choice2);
                            if (succ2)
                            {
                                if (choice1 != choice2)
                                {
                                    Console.Clear();
                                    succ = false;
                                    Console.WriteLine("How much money do you want transfer?\n");
                                    Console.WriteLine("From:" + assets.accounts[userIndex][choice1] + "  " + assets.funds[userIndex][choice1] + " Sek");
                                    Console.WriteLine("--------");
                                    Console.WriteLine("To:" + assets.accounts[userIndex][choice2] + " " + assets.funds[userIndex][choice2] + " Sek\n");
                                    Console.Write("Enter the amout you want to transfer: ");
                                    decimal transfer = 0;
                                    bool transfersucc = decimal.TryParse(Console.ReadLine(), out transfer);
                                    Console.WriteLine("Checking balance, please hold.");
                                    delay();
                                    while (transfersucc)
                                    {
                                        if (transfer <= assets.funds[userIndex][choice1] && transfer > 0)
                                        {
                                            assets.funds[userIndex][choice1] -= transfer;
                                            assets.funds[userIndex][choice2] += transfer;
                                            Console.WriteLine("You exchanges money from: " + assets.accounts[userIndex][choice1] + "\nTo: " + assets.accounts[userIndex][choice2]);
                                            Console.WriteLine("Your new balance is.");
                                            Console.WriteLine(assets.accounts[userIndex][choice1] + ": " + assets.funds[userIndex][choice1] + " Sek");
                                            Console.WriteLine(assets.accounts[userIndex][choice2] + ": " + assets.funds[userIndex][choice2] + " Sek\n");
                                            Console.WriteLine("Press [Enter] to continue.");
                                            Console.ReadLine();
                                            Console.Clear();
                                            return;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid amount to transfer");
                                            Console.WriteLine("Sending you back to deposit menu");
                                            delay();
                                            exchange();
                                        }
                                    }
                                    Console.WriteLine("Invalid input \nSending you back to deposit menu");
                                    delay();
                                    exchange();
                                }
                                else
                                {
                                    Console.WriteLine("You cant transfer money to the same account.");
                                    Console.WriteLine("Press [Enter] to continue.");
                                    Console.ReadLine();
                                    Console.Clear();
                                    exchange();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid account number");
                                Console.ReadLine();

                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No account found  with that number. Please try again");
                        delay();
                        exchange();
                    }
                }
                else
                {
                    Console.WriteLine("Input a [NUMBER] from the list above.");
                    Console.ReadLine();
                    Console.Clear();
                    exchange();
                }
            }

            public void withdraw()
            {
                string[] pin = MainMenu.pincodes;
                int count = 0;
                int choice;
                Console.WriteLine("Choose account to withdraw money from.\n");
                for (int i = 0; i < assets.accounts[userIndex].Length; i++)
                {
                    Console.Write("[" + i + "]" + assets.accounts[userIndex][i] + ": ");

                    for (int j = 0; j < 1; j++)
                    {
                        Console.Write(assets.funds[userIndex][count++] + " Sek\n");

                    }

                }
                Console.Write("\nEnter account number: ");
                bool success = int.TryParse(Console.ReadLine(), out choice);

                while (success)
                {
                    if (choice < assets.funds[userIndex].Length)
                    {
                        decimal withdrawM;
                        Console.WriteLine("Enter the amount you want to withdraw from:" + assets.accounts[userIndex][choice]);
                        Console.Write("Amount: ");
                        bool transferM = decimal.TryParse(Console.ReadLine(), out withdrawM);

                        int pintries = 0;
                        while (transferM)
                        {
                            Console.WriteLine("Enter your pincode to confirm.");
                            Console.Write("Pincode:");
                            pincode = Console.ReadLine();

                            if (pincode == pin[userIndex])
                            {
                                if (withdrawM <= assets.funds[userIndex][choice] && withdrawM > 0)
                                {
                                    assets.funds[userIndex][choice] -= withdrawM;
                                    Console.WriteLine("Amount withdrawn: " + assets.funds[userIndex][choice]);
                                    Console.WriteLine("Your new account balance is.");
                                    Console.WriteLine(assets.accounts[userIndex][choice] + ": " + assets.funds[userIndex][choice] + " Sek") ;
                                    Console.WriteLine("Press [Enter] to continue.");
                                    Console.ReadLine();
                                    return;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid amount to withdraw");
                                    Console.WriteLine("Sending you back to the wtihdraw menu");
                                    delay();
                                    withdraw();
                                }
                            }
                            else if (pintries >= 2)
                            {
                                Console.WriteLine("To many tries,please log in again to countinue.");
                                delay();
                                transferM = false;
                                success = false;
                                Console.Clear();
                                MainMenu.MainLogin();
                                return;

                            }
                            else
                            {
                                Console.WriteLine("Wrong pincode please try again");
                                Console.WriteLine(pintries);
                                pintries++;
                            }

                        }
                        Console.WriteLine("Invalid input");
                        Console.WriteLine("Press [ENTER] to continue");
                    }
                    else
                    {
                        Console.WriteLine("No account found with that number.");
                        Console.WriteLine("Enter a NUMBER from the list above.");
                        delay();
                        success = false;
                        withdraw();
                    }
                }
                if (!success)
                {
                    Console.WriteLine("Invalid input, please enter a NUMBER.");
                    delay();
                    Console.Clear();
                    withdraw();
                }
                /*
                OLD WITHDRAW METHOD , LOGIC DINT WORK JUST RE DID IT.
                try
                {
                    Console.Write("Enter account number: ");
                    int choice1;
                    choice1 = int.Parse(Console.ReadLine());
                    if (choice1 <  assets.funds[userIndex].Length) {
                        Console.WriteLine("How much money do you want to withdraw?");
                        Console.Write("Enter the amount: ");
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
                }
                catch
                {
                    Console.WriteLine("Invalid input, choose from the list above.");
                    Console.ReadLine();
                    Console.Clear();
                    withdraw();
            }*/
            }

            public void deposit()
            {
                {
                    int count = 0;
                    int choice;
                    Console.WriteLine("Choose account to deposit money to.\n");
                    for (int i = 0; i < assets.accounts[userIndex].Length; i++)
                    {
                        Console.Write("[" + i + "]" + assets.accounts[userIndex][i] + ": ");

                        for (int j = 0; j < 1; j++)
                        {
                            Console.Write(assets.funds[userIndex][count++] + " Sek\n");

                        }

                    }
                    Console.Write("\nEnter account number: ");
                    bool success = int.TryParse(Console.ReadLine(), out choice);

                    while (success)
                    {
                        if (choice < assets.funds[userIndex].Length)
                        {
                            decimal depositM;
                            Console.WriteLine("Enter the amount you want to deposit to: " + assets.accounts[userIndex][choice]);
                            Console.Write("Amount: ");
                            bool transferM = decimal.TryParse(Console.ReadLine(), out  depositM);
                            while (transferM)
                            {
                                if (depositM! > 0)
                                {
                                    assets.funds[userIndex][choice] += depositM;
                                    Console.WriteLine("Amount deposit: " + depositM + " Sek");
                                    Console.WriteLine("Your new account balance is.");
                                    Console.WriteLine(assets.accounts[userIndex][choice] + ": " + assets.funds[userIndex][choice] + " Sek");
                                    Console.Write("Press [Enter] to continue.");
                                    Console.ReadLine();
                                    return;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid amount to deposit");
                                    Console.WriteLine("Sending you back to the wtihdraw menu");
                                    delay();
                                    withdraw();
                                }
                            }

                        }
                        else
                        {
                            Console.WriteLine("No account found with that number.");
                            Console.WriteLine("Enter a NUMBER from the list above.");
                            delay();
                            success = false;
                            withdraw();
                        }
                        if (!success)
                        {
                            Console.WriteLine("Invalid input, please enter a NUMBER.");
                            delay();
                            withdraw();
                        }
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
            new string[] {"Savings", "Salary"},
            new string[] {"Savings", "Salary","Z-tv Salary"},
            new string[] {"Savings", "Salary","Computer Parts","Goods"},
            new string[] {"Savings", "Salary","Cash for snacks","Vet Savings","Toy Account"},
            new string[] {"Savings", "Salary", "Knitting Salary","Cosplay funds","Fibbes Xmas present","Gaming Gear"},
             };
                public static decimal[][] funds =
                {
            new decimal[] {1000.50M,20000}
            , new decimal[] {1000,2000.05M,12345}
            , new decimal[] {1000,20000,123.45M,12341}
            , new decimal[] {1000,12394,12349,1234,94M,129394}
            , new decimal[] {1000,12394,1234.1M,12412,451234,12512}
             };
            }

            //Made a method for the delays for cleaner code
            //Simple yet efective

            //Would be fun, sounded annoying
            /*public static void welcomeTune()
            {
                Console.Beep(369, 200);
                Console.Beep(369, 200);
                Console.Beep(369, 200);
                Console.Beep(293, 200);
                Console.Beep(246, 200);
                Console.Beep(329, 200);
                Console.Beep(329, 200);
                Console.Beep(329, 200);
                Console.Beep(415, 200);
                Console.Beep(415, 200);
                Console.Beep(440, 200);
                Console.Beep(493, 200);
                Console.Beep(440, 200);
                Console.Beep(440, 200);
                Console.Beep(440, 200);
                Console.Beep(329, 200);
                Console.Beep(293, 200);
                Console.Beep(369, 200);
                Console.Beep(369, 200);
                Console.Beep(369, 200);
                Console.Beep(329, 200);
                Console.Beep(329, 200);
                Console.Beep(369, 200);
                Console.Beep(329, 200);
                Console.Beep(369, 200);
                Console.Beep(369, 200);
                Console.Beep(369, 200);
                Console.Beep(293, 200);
                Console.Beep(246, 200);
                Console.Beep(329, 200);
                Console.Beep(329, 200);
                Console.Beep(329, 200);
                Console.Beep(415, 200);
                Console.Beep(415, 200);
                Console.Beep(440, 200);
                Console.Beep(493, 200);
                Console.Beep(440, 200);
                Console.Beep(440, 200);
                Console.Beep(440, 200);
                Console.Beep(329, 200);
                Console.Beep(293, 200);
                Console.Beep(369, 200);
                Console.Beep(369, 200);
                Console.Beep(369, 200);
                Console.Beep(329, 200);
                Console.Beep(329, 200);
                Console.Beep(369, 200);
                Console.Beep(329, 200);
            }*/

        }
            public static void delay()
            {
                int delay = 0;
                for (int i = 0; delay < 15; i++)
                {
                    delay++;
                    Console.Write(".");
                    Thread.Sleep(175);
                    if (delay > 10)
                    {
                        delay++;
                        Console.Write(".");
                        Thread.Sleep(75);
                    }
                }
                Console.Write('\u2713');
                Thread.Sleep(600);
            }
    }
}
