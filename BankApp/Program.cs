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
                        if (pincodes[i] != pincode)
                        {
                            tries++;

                            Console.WriteLine("Invalid input, please try again");
                            Console.Write(".");
                            Thread.Sleep(1000);
                            Console.Write(".");
                            Thread.Sleep(1000);
                            Console.Write(".");
                            Thread.Sleep(1000);
                            Console.Clear();

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
                Console.WriteLine("Invalid input, please try again");
                delay();
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
                    Console.Write("\n" + "[" + i + "]" + assets.accounts[userIndex][i]);

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
                Console.WriteLine("Choose accout to deposit money to.\n");

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
                        Console.WriteLine("Choose accout to exchange money from.\n");
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
                            int choice2 = int.Parse(Console.ReadLine());
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
                                while (transfersucc)
                                {
                                    if (transfer < assets.funds[userIndex][choice1] && transfer > 0)
                                    {
                                        assets.funds[userIndex][choice1] -= transfer;
                                        assets.funds[userIndex][choice2] += transfer;
                                        Console.WriteLine("You exchanges money from: " + assets.accounts[userIndex][choice1] + "\nTo: " + assets.accounts[userIndex][choice2]);
                                        Console.WriteLine("Your new balance is.");
                                        Console.WriteLine(assets.accounts[userIndex][choice1] + ": " + assets.funds[userIndex][choice1] + " Sek");
                                        Console.WriteLine(assets.accounts[userIndex][choice2] + ": " + assets.funds[userIndex][choice2] + " Sek\n");
                                        Console.WriteLine("Press [Enter] to continue");
                                        Console.ReadLine();
                                        break;
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
                                Console.WriteLine("You cant transfer money to the same account");
                                Console.ReadLine();
                                Console.Clear();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No account found  with that number. Please try again");
                        Console.ReadLine();
                        Console.Clear();
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
                //FAST PÅ BARA SAVING FIX IT
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
            new string[] {"Savings", "Salary"},
            new string[] {"Savings", "Salary","Z-tv Salary"},
            new string[] {"Savings", "Salary","Computer Parts","Goods"},
            new string[] {"Savings", "Salary","Cash for snacks","Vet Savings","Toy Account"},
            new string[] {"Savings", "Salary", "Knitting Salary","Cosplay funds","Fibbes Xmas present","Gaming Gear"},
        };
            public static decimal[][] funds =
            {
            new decimal[] {1000,20000}
            , new decimal[] {1000,20000,12345}
            , new decimal[] {1000,20000,12345,12341}
            , new decimal[] {1000,12394,12349,123494,129394}
            , new decimal[] {1000,12394,12341,12412,451234,12512}
        };
        }

        public static void delay()
        {
            int delay = 0;
            for (int i = 0; delay < 15; i++)
            {
                delay++;
                Console.Write(".");
                Thread.Sleep(200);
            }
            Console.Clear();
        }

    }
}
