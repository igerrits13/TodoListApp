using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace TodoListApp
{
    public partial class ToDoList : Form
    {
        public ToDoList()
        {
            InitializeComponent();
        }

        DataTable todoList = new DataTable();
        bool isEditing = false;
        DateTime tempDate = DateTime.Today;
        DataRow tempRow = null;

        private void ToDoList_Load(object sender, EventArgs e)
        {
            // Create columns
            todoList.Columns.Add("Title");
            todoList.Columns.Add("Description");
            todoList.Columns.Add("Created");
            todoList.Columns.Add("Due");

            // Point our datagridview to our datasource
            toDoListView.DataSource = todoList;
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            textBoxTitle.Text = "";
            textBoxDescription.Text = "";
            timePickerDueDate.Value = DateTime.Today;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            isEditing = true;

            // Fill text field with data from table
            textBoxTitle.Text = todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[0].ToString();
            textBoxDescription.Text = todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[1].ToString();
            timePickerDueDate.Value = DateTime.Parse(todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[3].ToString());

            tempRow = todoList.Rows[toDoListView.CurrentCell.RowIndex];
            tempDate = timePickerDueDate.Value;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                removeDate(DateTime.Parse(todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[3].ToString()),
                    todoList.Rows[toDoListView.CurrentCell.RowIndex]);
                todoList.Rows[toDoListView.CurrentCell.RowIndex].Delete();
            }

            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if(!testDesctiption(textBoxTitle.Text))
            {
                MessageBox.Show("Please make sure all tasks have unique descriptions.");
                return;
            }

            else if(isEditing) 
            {
                tempRow["Title"] = textBoxTitle.Text;
                tempRow["Description"] = textBoxDescription.Text;
                tempRow["Created"] = DateTime.Now.ToString("MM/dd/yyyy");
                tempRow["Due"] = timePickerDueDate.Value.ToString("MM/dd/yyyy");
                removeDate(tempDate, tempRow);
                monthCalendar.AddBoldedDate(timePickerDueDate.Value);
                monthCalendar.UpdateBoldedDates();
            }

            else
            {
                todoList.Rows.Add(textBoxTitle.Text,
                    textBoxDescription.Text,
                    DateTime.Now.ToString("MM/dd/yyyy"),
                    timePickerDueDate.Value.ToString("MM/dd/yyyy"));
                monthCalendar.AddBoldedDate(timePickerDueDate.Value);
                monthCalendar.UpdateBoldedDates();
            }

            // Clear all fields
            textBoxTitle.Text = "";
            textBoxDescription.Text = "";
            timePickerDueDate.Value = DateTime.Today;
            isEditing = false;

            // Sort based on sorting
            switch (boxSort.SelectedIndex)
            {
                case 0:
                    sortByNone();
                    break;
                case 1:
                    sortByTitle();
                    break;

                case 2:
                    sortByCreated();
                    break;

                case 3:
                    sortByDue();
                    break;
            }
        }

        // Confirm that title unique
        private bool testDesctiption(String title)
        {
            foreach (DataRow item in todoList.Rows)
            {
                if (item.ItemArray[0].ToString() == title)
                {
                    return false;
                }
            }

            return true;
        }

        private void timePickerDueDate_ValueChanged(object sender, EventArgs e)
        {
            if (timePickerDueDate.Value <  DateTime.Today) 
            {
                timePickerDueDate.Value = DateTime.Today;
                MessageBox.Show("Please select a future due date.");
            }
        }

        /*
         * Helper method to remove bolding from the date if there are no more tasks due on that date.
         */
        private void removeDate(DateTime date, DataRow row)
        {
            // Only remove bolding if there are no other tasks due on the same date
            foreach (DataRow item in todoList.Rows)
            {
                if(item != row)
                {
                    if(DateTime.Parse(item.ItemArray[3].ToString()) == date)
                    {
                        return;
                    }
                }
            }

            monthCalendar.RemoveBoldedDate(date);
            monthCalendar.UpdateBoldedDates();
            return;
        }

        private void boxSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(boxSort.SelectedIndex)
            {
                case 0:
                    sortByNone();
                    break;
                case 1:
                    sortByTitle();
                    break;
                
                case 2:
                    sortByCreated();
                    break;

                case 3:
                    sortByDue();
                    break;
            }
        }

        // Apply no sorting
        private void sortByNone()
        {
            Console.WriteLine("Sorting None");
            foreach (DataGridViewColumn column in toDoListView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        // Accomplish with selectionsort
        private void sortByTitle()
        {
            Console.WriteLine("Sorting Title");
            toDoListView.Sort(toDoListView.Columns["Title"], ListSortDirection.Ascending);
        }

        // Accomplish with mergesort
        private void sortByCreated()
        {
            Console.WriteLine("Sorting Created");
            toDoListView.Sort(toDoListView.Columns["Created"], ListSortDirection.Ascending);
        }

        // Accomplish with quicksort
        private void sortByDue()
        {
            Console.WriteLine("Sorting Due");
            toDoListView.Sort(toDoListView.Columns["Due"], ListSortDirection.Ascending);
        }
    }
}
