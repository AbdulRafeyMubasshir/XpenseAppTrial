namespace Budget_Tracking_App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize dummy database and expense tracker
            DummyDatabase database = new DummyDatabase();
            ExpenseTracker tracker = new ExpenseTracker(database);

            // Main menu loop
            while (true)
            {
                try
                {
                    // Display the expense tracker menu
                    Console.WriteLine("\n===== Expense Tracker Menu =====");
                    Console.WriteLine("1. Show Recent Transactions");
                    Console.WriteLine("2. Add Transaction");
                    Console.WriteLine("3. Edit Transaction");
                    Console.WriteLine("4. Delete Transaction");
                    Console.WriteLine("5. Manage Categories");
                    Console.WriteLine("6. Spending Analysis");
                    Console.WriteLine("7. Exit");

                    // Prompt user for choice
                    Console.Write("Enter your choice: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            tracker.ShowRecentTransactions();
                            break;
                        case "2":
                            tracker.AddTransaction();
                            break;
                        case "3":
                            tracker.EditTransaction();
                            break;
                        case "4":
                            tracker.DeleteTransaction();
                            break;
                        case "5":
                            // Sub-menu for managing categories
                            Console.WriteLine("Enter your choice: ");
                            Console.WriteLine("1. Show Categories");
                            Console.WriteLine("2. Add Category");
                            Console.WriteLine("3. Set Budget for Categories");
                            string catChoice = Console.ReadLine();
                            switch (catChoice)
                            {
                                case "1":
                                    tracker.ShowCategories();
                                    break;
                                case "2":
                                    tracker.AddCategory();
                                    break;
                                case "3":
                                    tracker.SetBudgetForCategory();
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice. Please try again.");
                                    break;
                            }
                            break;
                        case "6":
                            // Sub-menu for spending analysis
                            Console.WriteLine("Enter your choice: ");
                            Console.WriteLine("1. Show overall Spending");
                            Console.WriteLine("2. Show spending by category");
                            string analysisChoice = Console.ReadLine();
                            switch (analysisChoice)
                            {
                                case "1":
                                    tracker.ShowOverallSpending();
                                    break;
                                case "2":
                                    tracker.ShowSpendingByCategories();
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice. Please try again.");
                                    break;
                            }
                            break;
                        case "7":
                            // Exit the program
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Handle any unexpected errors
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
