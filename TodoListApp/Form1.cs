using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private void ToDoList_Load(object sender, EventArgs e)
        {
            // Create columns
            todoList.Columns.Add("Title");
            todoList.Columns.Add("Description");

            // Point our datagridview to our datasource
            toDoListView.DataSource = todoList;
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            textBoxTitle.Text = "";
            textBoxDescription.Text = "";
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            isEditing = true;

            // Fill text field with data from table
            textBoxTitle.Text = todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[0].ToString();
            textBoxDescription.Text = todoList.Rows[toDoListView.CurrentCell.RowIndex].ItemArray[1].ToString();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                todoList.Rows[toDoListView.CurrentCell.RowIndex].Delete();
            }

            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if(isEditing) 
            {
                todoList.Rows[toDoListView.CurrentCell.RowIndex]["Title"] = textBoxTitle.Text;
                todoList.Rows[toDoListView.CurrentCell.RowIndex]["Description"] = textBoxDescription.Text;
            }

            else
            {
                todoList.Rows.Add(textBoxTitle.Text, textBoxDescription.Text);
            }

            // Clear all fields
            textBoxTitle.Text = "";
            textBoxDescription.Text = "";
            isEditing = false;
        }
    }
}
