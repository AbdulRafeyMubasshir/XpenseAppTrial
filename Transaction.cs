using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Budget_Tracking_App
{
    // Enum to represent the type of transaction: Debit or Credit
    public enum TransactionType
    {
        Debit,
        Credit
    }

    // Class to represent a transaction
    public class Transaction
    {
        // Properties
        private int id;                         // Unique identifier for the transaction
        private TransactionType type;           // Type of transaction: Debit or Credit
        private decimal amount;                 // Amount of the transaction
        private Category category;              // Category associated with the transaction
        private DateTime time;                  // Time of the transaction
        private string note;                    // Additional note for the transaction
        private bool isRecurring;               // Flag indicating if the transaction is recurring

        // Properties with getters and setters
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public TransactionType Type
        {
            get { return type; }
            set { type = value; }
        }
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public Category Category
        {
            get { return category; }
            set { category = value; }
        }
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }
        public string Note
        {
            get { return note; }
            set { note = value; }
        }
        public bool IsRecurring
        {
            get { return isRecurring; }
            set { isRecurring = value; }
        }

        // Constructor
        public Transaction(int id, decimal amount, TransactionType type, Category category, DateTime time, string note, bool isRecurring)
        {
            // Initialize properties with provided values
            this.id = id;
            this.amount = amount;
            this.type = type;
            this.category = category;
            this.time = time;
            this.note = note;
            this.isRecurring = isRecurring;
        }

        // Method to display transaction details
        public void ShowDetails()
        {
            // Print transaction details to the console
            Console.WriteLine("{ ID: " + this.Id + ", " + this.Type + ": " + this.Amount + ", Category: " + this.Category.Name + ", Time: " + this.Time +
                ", Note: " + this.Note + ", Is Recurring: " + this.isRecurring + " }");
        }
    }
}
