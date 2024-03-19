using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget_Tracking_App
{
    // Interface for the database
    public interface IDatabase
    {
        int GetNextTransactionId();                 // Method to get the next transaction ID
        void AddTransaction(Transaction transaction);  // Method to add a transaction
        void DeleteTransaction(int id);            // Method to delete a transaction
        void AddCategory(string name, decimal budget);  // Method to add a category
        List<Transaction> GetTransactions();       // Method to get all transactions
        List<Category> GetCategories();            // Method to get all categories
    }

    // Database class implementing the IDatabase interface
    public class DummyDatabase : IDatabase
    {
        private List<Transaction> transactions;
        private List<Category> categories;
        private int nextTransactionId = 1; // Starting from 1 for the first transaction

        // Constructor
        public DummyDatabase()
        {
            transactions = new List<Transaction>();
            categories = new List<Category>();

            // Preset categories
            categories.Add(new Category("Food"));
            categories.Add(new Category("Transport"));
            categories.Add(new Category("Entertainment"));
            categories.Add(new Category("Kids"));
            categories.Add(new Category("Gifts"));
            categories.Add(new Category("Clothes"));
        }

        // Method to get the next transaction ID
        public int GetNextTransactionId()
        {
            // Return the current nextTransactionId and increment it for the next call
            return nextTransactionId++;
        }

        // Method to add a transaction
        public void AddTransaction(Transaction transaction)
        {
            try
            {
                // Add the transaction to the list of transactions
                transactions.Add(transaction);

                // Update the category's expense or budget based on the transaction type
                if (transaction.Type == TransactionType.Debit)
                {
                    transaction.Category.Expense += transaction.Amount; // Increase category's expense
                }
                else
                {
                    transaction.Category.Budget += transaction.Amount; // Increase category's budget
                }
                Console.WriteLine("Transaction added successfully.");

            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"An error occurred while adding transaction: {ex.Message}");
            }
        }

        // Method to delete a transaction by ID
        public void DeleteTransaction(int id)
        {
            try
            {
                // Find the transaction with the provided ID
                Transaction transactionToDelete = transactions.FirstOrDefault(t => t.Id == id);

                if (transactionToDelete != null)
                {
                    // Adjust category expense or budget based on transaction type
                    if (transactionToDelete.Type == TransactionType.Debit)
                    {
                        transactionToDelete.Category.Expense -= transactionToDelete.Amount; // Decrease category expense
                    }
                    else
                    {
                        transactionToDelete.Category.Budget -= transactionToDelete.Amount; // Decrease category budget
                    }

                    // Remove the transaction from the list
                    transactions.Remove(transactionToDelete);

                    // Notify user that transaction has been deleted
                    Console.WriteLine("Transaction deleted successfully.");
                }
                else
                {
                    // If the transaction with the provided ID is not found, display a message
                    Console.WriteLine("Transaction with the provided ID not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"An error occurred while deleting transaction: {ex.Message}");
            }
        }

        // Method to add a category
        public void AddCategory(string name, decimal budget)
        {
            try
            {
                // Add a new category to the list of categories
                categories.Add(new Category(name, budget));
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"An error occurred while adding category: {ex.Message}");
            }
        }

        // Method to get all transactions
        public List<Transaction> GetTransactions()
        {
            return transactions;
        }

        // Method to get all categories
        public List<Category> GetCategories()
        {
            return categories;
        }

    }
}
