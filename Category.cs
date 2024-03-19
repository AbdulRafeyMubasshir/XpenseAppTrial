using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget_Tracking_App
{
    // Class to represent a category
    public class Category
    {
        // Properties
        private int id;             // Unique identifier for the category
        private string name;        // Name of the category
        private decimal budget;     // Budget allocated for the category
        private decimal expense;    // Total expense incurred in the category

        // Properties with getters and setters
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public decimal Budget
        {
            get { return budget; }
            set { budget = value; }
        }
        public decimal Expense
        {
            get { return expense; }
            set { expense = value; }
        }

        // Constructor with only name parameter
        public Category(string name)
        {
            // Initialize category with provided name
            this.name = name;
        }

        // Constructor with name and budget parameters
        public Category(string name, decimal budget)
        {
            // Initialize category with provided name and budget
            this.name = name;
            this.budget = budget;
        }
    }
}
