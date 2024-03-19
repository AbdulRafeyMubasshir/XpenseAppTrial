using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Budget_Tracking_App
{
    // Interface for ExpenseTracker
    public interface IExpenseTracker
    {
        void ShowRecentTransactions();
        void AddTransaction();
        void EditTransaction();
        void DeleteTransaction();
        void ShowCategories();
        void AddCategory();
        void SetBudgetForCategory();
        void ShowSpendingByCategories();
        void ShowOverallSpending();
    }

    // ExpenseTracker class implementing the IExpenseTracker interface
    public class ExpenseTracker : IExpenseTracker
    {
        private IDatabase database;

        public ExpenseTracker(IDatabase database)
        {
            this.database = database;
        }

        public void ShowRecentTransactions()
        {
            try
            {
                // Retrieve recent transactions from the database
                List<Transaction> transactions = database.GetTransactions();

                // Check if there are any transactions
                if (transactions.Count == 0)
                {
                    Console.WriteLine("No recent transactions found.");
                    return; // Exit the method early since there are no transactions to display
                }

                // Display recent transactions
                Console.WriteLine("Recent Transactions:");
                foreach (Transaction transaction in transactions.OrderByDescending(t => t.Time))
                {
                    // Show details of each transaction
                    transaction.ShowDetails();
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        public void AddTransaction()
        {
            try
            {
                // Prompt user to enter amount
                Console.Write("Enter amount: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                // Prompt user to select transaction type
                Console.WriteLine("Select transaction type:");
                Console.WriteLine("1. Debit");
                Console.WriteLine("2. Credit");
                Console.Write("Enter your choice (1/2): ");
                int typeChoice;
                while (!int.TryParse(Console.ReadLine(), out typeChoice) || (typeChoice != 1 && typeChoice != 2))
                {
                    Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                    Console.Write("Enter your choice (1/2): ");
                }

                // Determine transaction type based on user choice
                TransactionType type = typeChoice == 1 ? TransactionType.Debit : TransactionType.Credit;

                // Prompt user to select category
                Console.Write("Enter category: ");
                List<Category> categories = database.GetCategories();
                Console.WriteLine("Categories:");
                for (int i = 0; i < categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories[i].Name}");
                }

                // Select category
                int categoryChoice;
                while (!int.TryParse(Console.ReadLine(), out categoryChoice) || categoryChoice < 1 || categoryChoice > categories.Count)
                {
                    Console.WriteLine("Invalid choice. Please enter a valid category number.");
                    Console.Write("Enter category: ");
                }
                Category selectedCategory = categories[categoryChoice - 1];

                // Prompt user to enter note
                Console.Write("Enter note (optional): ");
                string note = Console.ReadLine();

                // Prompt user to enter recurring choice
                Console.Write("Enter 0 for non-recurring or 1 for recurring transaction: ");
                int recurringChoice;
                while (!int.TryParse(Console.ReadLine(), out recurringChoice) || (recurringChoice != 0 && recurringChoice != 1))
                {
                    Console.WriteLine("Invalid choice. Please enter 0 or 1.");
                    Console.Write("Enter 0 for non-recurring or 1 for recurring transaction: ");
                }

                // Determine if transaction is recurring based on user choice
                bool isRecurring = recurringChoice == 1;

                // Generate a unique ID for the transaction
                int transactionId = database.GetNextTransactionId();

                // Create transaction object
                Transaction transaction = new Transaction(transactionId, amount, type, selectedCategory, DateTime.Now, note, isRecurring);

                // Add transaction to database
                database.AddTransaction(transaction);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter a valid number.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Input out of range. Please enter a valid number.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void EditTransaction()
        {
            try
            {
                // Prompt user to enter the ID of the transaction to edit
                Console.Write("Enter the ID of the transaction you want to edit: ");
                int id = int.Parse(Console.ReadLine());

                // Find the transaction with the provided ID
                Transaction transactionToEdit = database.GetTransactions().FirstOrDefault(t => t.Id == id);

                // Check if transaction with the provided ID exists
                if (transactionToEdit != null)
                {
                    Console.WriteLine("Transaction found. Please provide new details:");

                    // Adjust category expense or budget based on transaction type
                    if (transactionToEdit.Type == TransactionType.Debit)
                    {
                        transactionToEdit.Category.Expense -= transactionToEdit.Amount; // Decrease category expense
                    }
                    else
                    {
                        transactionToEdit.Category.Budget -= transactionToEdit.Amount; // Decrease category budget
                    }

                    // Prompt user to enter new amount
                    Console.Write("Enter new amount: ");
                    decimal newAmount = decimal.Parse(Console.ReadLine());

                    // Prompt user to select new transaction type
                    Console.WriteLine("Select new transaction type:");
                    Console.WriteLine("1. Debit");
                    Console.WriteLine("2. Credit");
                    Console.Write("Enter your choice (1/2): ");
                    int typeChoice;
                    while (!int.TryParse(Console.ReadLine(), out typeChoice) || (typeChoice != 1 && typeChoice != 2))
                    {
                        Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                        Console.Write("Enter your choice (1/2): ");
                    }

                    // Determine new transaction type based on user choice
                    TransactionType newType = typeChoice == 1 ? TransactionType.Debit : TransactionType.Credit;

                    // Prompt user to select new category
                    Console.Write("Enter new category: ");
                    List<Category> categories = database.GetCategories();
                    Console.WriteLine("Categories:");
                    for (int i = 0; i < categories.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {categories[i].Name}");
                    }

                    // Select new category
                    int categoryChoice;
                    while (!int.TryParse(Console.ReadLine(), out categoryChoice) || categoryChoice < 1 || categoryChoice > categories.Count)
                    {
                        Console.WriteLine("Invalid choice. Please enter a valid category number.");
                        Console.Write("Enter new category: ");
                    }
                    Category newCategory = categories[categoryChoice - 1];

                    // Prompt user to enter new note
                    Console.Write("Enter new note (optional): ");
                    string newNote = Console.ReadLine();

                    // Prompt user to confirm the edit
                    Console.WriteLine("Confirm edit (y/n): ");
                    string confirmation = Console.ReadLine();

                    if (confirmation.ToLower() == "y")
                    {
                        // Update transaction details
                        transactionToEdit.Amount = newAmount;
                        transactionToEdit.Type = newType;
                        transactionToEdit.Category = newCategory;
                        transactionToEdit.Note = newNote;

                        // Notify user that transaction has been edited
                        Console.WriteLine("Transaction edited successfully.");

                        // Update the category's expense or budget based on the new transaction type
                        if (transactionToEdit.Type == TransactionType.Debit)
                        {
                            transactionToEdit.Category.Expense += transactionToEdit.Amount; // Increase category's expense
                        }
                        else
                        {
                            transactionToEdit.Category.Budget += transactionToEdit.Amount; // Increase category's budget
                        }
                    }
                    else
                    {
                        // Notify user that edit has been canceled
                        Console.WriteLine("Edit canceled. Transaction remains unchanged.");
                    }
                }
                else
                {
                    // Notify user if transaction with provided ID is not found
                    Console.WriteLine("Transaction with the provided ID not found.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter a valid number.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Input out of range. Please enter a valid number.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }



        public void DeleteTransaction()
        {
            try
            {
                // Prompt user to enter the ID of the transaction to delete
                Console.Write("Enter the ID of the transaction you want to delete: ");
                int id = int.Parse(Console.ReadLine());

                // Delete transaction from the database by ID
                database.DeleteTransaction(id);

            }
            catch (FormatException)
            {
                // Handle the case when the user enters invalid input format
                Console.WriteLine("Invalid input format. Please enter a valid number.");
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }



        public void ShowCategories()
        {
            try
            {
                // Retrieve categories from the database
                List<Category> categories = database.GetCategories();

                // Display categories
                Console.WriteLine("Categories:");
                foreach (Category category in categories)
                {
                    Console.WriteLine("Name: " + category.Name + ", Budget: " + category.Budget);
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        public void AddCategory()
        {
            try
            {
                // Prompt user to enter category name
                Console.WriteLine("Enter Category Name:");
                string name = Console.ReadLine();

                // Prompt user to enter budget
                Console.WriteLine("Enter Budget:");
                decimal budget;

                // Validate user input for budget
                while (!decimal.TryParse(Console.ReadLine(), out budget) || budget < 0)
                {
                    Console.WriteLine("Invalid input. Please enter a valid positive number for the budget.");
                    Console.WriteLine("Enter Budget:");
                }

                // Add category to the database
                database.AddCategory(name, budget);
                Console.WriteLine("Category added successfully.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter a valid number.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Input out of range. Please enter a valid number.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void SetBudgetForCategory()
        {
            try
            {
                // Prompt user to select category
                Console.Write("Enter category: ");
                List<Category> categories = database.GetCategories();

                // Display available categories
                Console.WriteLine("Categories:");
                for (int i = 0; i < categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories[i].Name} Budget: {categories[i].Budget}");
                }

                // Select category
                Console.WriteLine("Select Category");
                int categoryChoice;

                // Validate user input for category choice
                while (!int.TryParse(Console.ReadLine(), out categoryChoice) || categoryChoice < 1 || categoryChoice > categories.Count)
                {
                    Console.WriteLine("Invalid choice. Please enter a valid category number.");
                    Console.Write("Enter category: ");
                }

                Category selectedCategory = categories[categoryChoice - 1];

                // Prompt user to enter new budget
                Console.WriteLine("Enter Budget:");
                decimal newBudget;

                // Validate user input for new budget
                while (!decimal.TryParse(Console.ReadLine(), out newBudget) || newBudget < 0)
                {
                    Console.WriteLine("Invalid input. Please enter a valid positive number for the budget.");
                    Console.WriteLine("Enter Budget:");
                }

                // Set new budget for the selected category
                selectedCategory.Budget = newBudget;
                Console.WriteLine($"Budget for category '{selectedCategory.Name}' updated successfully.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter a valid number.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Input out of range. Please enter a valid number.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void ShowSpendingByCategories()
        {
            try
            {
                // Retrieve categories from the database
                List<Category> categories = database.GetCategories();

                // Display categories along with budget and expenses
                Console.WriteLine("Categories:");
                foreach (Category category in categories)
                {
                    Console.WriteLine($"Name: {category.Name}, Budget: {category.Budget}, Expense: {category.Expense}");
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public void ShowOverallSpending()
        {
            try
            {
                // Retrieve categories from the database
                List<Category> categories = database.GetCategories();

                // Initialize variables to track total budget and total expense
                decimal totalBudget = 0, totalExpense = 0;

                // Iterate over each category to calculate total budget and total expense
                foreach (Category category in categories)
                {
                    totalBudget += category.Budget; // Add category's budget to total budget
                    totalExpense += category.Expense; // Add category's expense to total expense
                }

                // Display total budget and total expense to the console
                Console.WriteLine("Total Budget: " + totalBudget);
                Console.WriteLine("Total Expense: " + totalExpense);
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }

    }
}
