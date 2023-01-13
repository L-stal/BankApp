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
                //The if else needs to be here because of console and powershell reads from diffrent dir..
                string text;
                if (File.Exists("../../../welcome.txt"))
                {
                    text = File.ReadAllText("../../../welcome.txt");
                }
                else
                {
                    text = File.ReadAllText(".\\welcome.txt");
                }
                //Prints out the Askii art 
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
        //Account in main menu for comparing index valus, if index valus are the same run function AccountTrue
        static string[] userNames = { "Robbe", "Kjell", "Leo", "Fibbe", "Mimmi" };
        static string[] pincodes = { "123", "12345", "1337", "13372", "13373" };
        //Made and array for logins, if the index value hits 0 freez out the user
        static int[] logins = { 3, 3, 3, 3, 3, };
        static string user;
        static string pincode;
        //A simple timestamp to check if 3 mins has passed to reset the index values in the logins array
        static DateTime lockoutTime;
        //activeUser sends value in to  class Account to know what account to show, activUser gets its value if the login is a succses
        //it gets the same value  as index value from userNames[] and pincodes[]
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

                // A for loop that compare user input to the names in userNames[]
                for (int i = 0; i < userNames.Length; i++)
                {
                    //If the user exist in userName[] and the login index value [i] is higher or  0 let the user input the pincode
                    if (userNames[i] == user)
                    {
                        if (logins[i] > 0)
                        {
                            Console.WriteLine("Please enter your pincode");
                            Console.Write("Pincode:");
                            pincode = Console.ReadLine();
                            if (pincodes[i] == pincode)
                            {
                                logins[i] = 3;//Resets the value for the user if they login
                                Console.Clear();
                                Console.WriteLine("Login succsesful");
                                _accountTrue.UserIndex = i;//Sets the value of userIndex to the same value as the userNames and pincodes
                                //UserIndex is used to know what accounts and funds to print out (Check assets class)
                                _accountTrue.accountTrue(user);
                                Console.WriteLine("Logout succssesful. Have a great day " + user + "!");
                                Console.WriteLine("----RETURNING TO LOGIN-----");
                                delay();
                                Console.Clear();
                                mainMenu = false;
                                return;
                            }
                            //If the pincode is wrong , take away 1 from logins[]
                            if (pincodes[i] != pincode)
                            {
                                logins[i] -= 1;
                                Console.WriteLine("Pincode is wrong " + logins[i] + " tries left\n");
                            }
                        }
                        //If logins[i] hits 0 take away 1 and "time stamp" the index, i did this to not loop through it again
                        else if (logins[i] == 0)
                        {
                            logins[i] -= 1;                      
                            lockoutTime = DateTime.Now.AddMinutes(3);
                        }
                    //Checks the time from when the user faild to input correct pincode and if 3 minuts have passed give 3 tries back to the user
                    if (logins[i] == -1)
                    {
                        Console.WriteLine("User is locked out, please try again in 3 mins from " + lockoutTime);
                        if (DateTime.Now >= lockoutTime)
                        {
                            logins[i] = 3;
                            lockoutTime = DateTime.Now;
                            break;
                        }
                    }
                }
            }

        }
           
    }


    //A simple menu prints out when the user is log in
    //simple switch case takes imputs from user and runs methonds coresponding to input
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
                        
                        case "1":
                            Console.Clear();
                            printAcc(user);
                            Console.Clear();
                            break;
                      
                        case "2":
                            Console.Clear();
                            exchange();
                            Console.Clear();
                            break;
                    
                        case "3":
                            Console.Clear();
                            withdraw();
                            Console.Clear();
                            break;
                        
                        case "4":
                            Console.Clear();
                            deposit();
                            Console.Clear();    
                            break;
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
                Console.Write("\nPress Enter to continue");
                Console.ReadLine();
            }


            //Method to exchange money between user accounts
            public void exchange()
            {
                int count = 0;
                int choice1;
                int choice2;
                Console.WriteLine("\nChoose accout to deposit money to.\n");

                //This for loop prints out the user accounts
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
                //If the TryParse is successful prints out the other accounts except the one the user choose
                if (success)
                {
                    if (choice1 <= assets.accounts[userIndex].Length && choice1 >= 0)
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
                        while (succ)
                        {
                            Console.Write("Enter Your choice: ");
                            bool succ2 = int.TryParse(Console.ReadLine(), out choice2);
                            if (succ2 && choice2 <= assets.accounts[userIndex].Length && choice2 >= 0)
                            {
                                //If the choice is not the same and the one about let the user exchange money
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
                                    Console.WriteLine("\n");
                                    while (transfersucc)
                                    {
                                        //If the exchange is succsesful, adds the amount chosen to the account and take away from the other
                                        if (transfer <= assets.funds[userIndex][choice1] && transfer > 0)
                                        {
                                            assets.funds[userIndex][choice1] -= transfer;
                                            assets.funds[userIndex][choice2] += transfer;
                                            Console.WriteLine("You exchange money from: " + assets.accounts[userIndex][choice1] + "\nTo: " + assets.accounts[userIndex][choice2]);
                                            Console.WriteLine("Your new balance is.");
                                            Console.WriteLine(assets.accounts[userIndex][choice1] + ": " + assets.funds[userIndex][choice1] + " Sek");
                                            Console.WriteLine(assets.accounts[userIndex][choice2] + ": " + assets.funds[userIndex][choice2] + " Sek\n");
                                            Console.WriteLine("Press [Enter] to continue.");
                                            Console.ReadLine();
                                            Console.Clear();
                                            return;
                                        }
                                        // All these Else are just bug catchers
                                        else
                                        {
                                            Console.WriteLine("Invalid amount to transfer");
                                            Console.WriteLine("Sending you back to deposit menu");
                                            delay();
                                            Console.Clear();
                                            exchange();
                                        }
                                    }
                                    Console.WriteLine("Invalid input \nSending you back to deposit menu");
                                    delay();
                                    Console.Clear();
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
                        Console.Clear();
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
                // Yes , all of these ...
            }

            //The withdraw method below is very similar to deposet except you only use one account
            public void withdraw()
            {
                // This array is used to check the user pincode
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
                    if (choice <= assets.funds[userIndex].Length && choice >= 0 )
                    {
                        decimal withdrawM;
                        Console.WriteLine("Enter the amount you want to withdraw from:" + assets.accounts[userIndex][choice]);
                        Console.Write("Amount: ");
                        bool transferM = decimal.TryParse(Console.ReadLine(), out withdrawM);

                        int pinTries = 0;//How many times the user input the wrong pincode

                        //if the amount exist lets the user write in the pincode to the account to withdraw the money
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
                                //Same here, alot of else that catches bugs *Bzz BBzzz*
                                else
                                {
                                    Console.WriteLine("Invalid amount to withdraw");
                                    Console.WriteLine("Sending you back to the wtihdraw menu");
                                    delay();
                                    Console.Clear();
                                    withdraw();
                                }
                            }
                            //Sends the user back to the login menu if the user inputs to many tries
                            else if (pinTries >= 2)
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
                                Console.WriteLine(pinTries);
                                pinTries++;
                            }

                        }
                        Console.WriteLine("Invalid input");
                        Console.WriteLine("Press [ENTER] to continue");
                        Console.ReadLine();
                        Console.Clear();
                        withdraw();

                    }
                    else
                    {
                        Console.WriteLine("No account found with that number.");
                        Console.WriteLine("Enter a NUMBER from the list above.");
                        delay();
                        Console.Clear();
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
                //*Bzz *BzzZZ * *Slap* Got'em ....
            }

            //Deposit method, very similar to the ones above,same for loops to pring accounts
            //And the same TryParse for bug catching
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
                        if (choice <= assets.funds[userIndex].Length && choice >= 0)
                        {
                            decimal depositM;
                            Console.WriteLine("Enter the amount you want to deposit to: " + assets.accounts[userIndex][choice]);
                            Console.Write("Amount: ");
                            bool transferM = decimal.TryParse(Console.ReadLine(), out  depositM);
                            while (transferM)
                            {
                                if (depositM !> 0)
                                {
                                    assets.funds[userIndex][choice] += depositM;
                                    Console.WriteLine("Amount deposit: " + depositM + " Sek");
                                    Console.WriteLine("Your new account balance is.");
                                    Console.WriteLine(assets.accounts[userIndex][choice] + ": " + assets.funds[userIndex][choice] + " Sek");
                                    Console.Write("Press [Enter] to continue.");
                                    Console.ReadLine();
                                    return;
                                }
                                // Same here, Else to catch bugs
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

            //Here i use jagged arrays with the UserIndex value to easily know waht arrays to print out
            public class assets
            {
                protected int assetsIndex;
                public int AssetsIndex
                {
                    get { return assetsIndex; }
                    set { assetsIndex = value; }
                }
                //Jagged array for accounts, if the user inputs a username and logs in
                //set the value in the array to the same value as the array for userNames[]
                public static string[][] accounts =
                {
            new string[] {"Savings", "Salary"},
            new string[] {"Savings", "Salary","Z-tv Salary"},
            new string[] {"Savings", "Salary","Computer Parts","Goods"},
            new string[] {"Savings", "Salary","Cash for snacks","Vet Savings","Toy Account"},
            new string[] {"Savings", "Salary", "Knitting Salary","Cosplay funds","Fibbes Xmas present","Gaming Gear"},
             };
                //Same as aboove but for the funds in the accounts
                public static decimal[][] funds =
                {
            new decimal[] {1000.50M,20000}
            , new decimal[] {1000,2000.05M,12345}
            , new decimal[] {1000,20000,123.45M,12341}
            , new decimal[] {1000,12394,12349,1234,94M,129394}
            , new decimal[] {1000,12394,1234.1M,12412,451234,12512}
             };
            }

            //Would have be fun, sounded annoying
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

        //Made a method for the delays for cleaner code
        //Simple yet effective
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
