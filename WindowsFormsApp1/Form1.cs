using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        string connectionString = @"Data Source=D:\NeuroPractice\TMK-test\BD.db;Version=3;";

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            string query = @"
                SELECT Book.book_name, Author.author_name, Book.publish_date
                FROM Book
                JOIN Author ON Book.author_id = Author.author_id;
                ";

            using (var connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Connection opened successfully");

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var adapter = new SQLiteDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            MessageBox.Show($"Number of rows fetched: {dataTable.Rows.Count}");

                            foreach (DataRow row in dataTable.Rows)
                            {
                                Console.WriteLine($"{row["book_name"]}, {row["author_name"]}, {row["publish_date"]}");
                            }

                            dataGridView1.DataSource = dataTable;

                            if (dataTable.Rows.Count > 0)
                            {
                                MessageBox.Show("Data successfully loaded into DataGridView");
                            }
                            else
                            {
                                MessageBox.Show("No data found");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            string query = "SELECT Book.book_id, Book.book_name, Book.author_id, Author.author_name, Book.publish_date " +
                           "FROM Book " +
                           "INNER JOIN Author ON Book.author_id = Author.author_id";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dataTable; // Привязка данных к DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Нет данных для отображения.");
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            string query = @"
                SELECT Book.book_name, Reader.reader_name, Event.date_event AS issue_date
                FROM Event
                INNER JOIN Book ON Event.book_id = Book.book_id
                INNER JOIN Author ON Book.author_id = Author.author_id
                INNER JOIN Reader ON Event.reader_id = Reader.reader_id
                WHERE Author.author_name = 'Иванов И.И.'
                AND Event.typ_event = 'Выдача'
                AND Event.date_event BETWEEN '2022-01-12' AND '2022-02-12'
                ORDER BY Reader.reader_name ASC, Event.date_event DESC;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dataTable; // Привязка данных к DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Нет данных для отображения.");
                    }
                }
            }
        }
    }
}
