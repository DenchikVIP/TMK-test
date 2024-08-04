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
using System.Windows.Forms.VisualStyles;

namespace ProductionOrdersManagement
{
    public partial class Form1 : Form
    {
        private SQLiteConnection connection;
        public Form1()
        {
            InitializeComponent();
            string connectionString = @"Data Source=D:\NeuroPractice\TMK-test\BDfor3.db;Version=3;";
            connection = new SQLiteConnection(connectionString);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadOrders();
        }
        private void LoadOrders()
        {
            try
            {
                connection.Open();
                string query = "SELECT order_id, workshop, start_date, end_date, status FROM OrderHeader";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заказов: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        private void buttonAddOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxWorkshop.Text) || string.IsNullOrWhiteSpace(comboBoxStatus.Text))
                {
                    MessageBox.Show("Заполните все обязательные поля!");
                    return;
                }

                connection.Open();
                string query = "INSERT INTO OrderHeader (workshop, start_date, end_date, status) VALUES (@workshop, @start_date, @end_date, @status)";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@workshop", textBoxWorkshop.Text);
                cmd.Parameters.AddWithValue("@start_date", dateTimePickerStartDate.Value);
                cmd.Parameters.AddWithValue("@end_date", dateTimePickerEndDate.Value);
                cmd.Parameters.AddWithValue("@status", comboBoxStatus.Text);
                cmd.ExecuteNonQuery();
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении заказа: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        private void buttonEditOrder_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заказ для редактирования!");
                return;
            }

            try
            {
                int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["order_id"].Value);
                if (string.IsNullOrWhiteSpace(textBoxWorkshop.Text) || string.IsNullOrWhiteSpace(comboBoxStatus.Text))
                {
                    MessageBox.Show("Заполните все обязательные поля!");
                    return;
                }

                connection.Open();
                string query = "UPDATE OrderHeader SET workshop = @workshop, start_date = @start_date, end_date = @end_date, status = @status WHERE order_id = @order_id";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@workshop", textBoxWorkshop.Text);
                cmd.Parameters.AddWithValue("@start_date", dateTimePickerStartDate.Value);
                cmd.Parameters.AddWithValue("@end_date", dateTimePickerEndDate.Value);
                cmd.Parameters.AddWithValue("@status", comboBoxStatus.Text);
                cmd.Parameters.AddWithValue("@order_id", orderId);
                cmd.ExecuteNonQuery();
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании заказа: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        private void buttonDeleteOrder_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заказ для удаления!");
                return;
            }

            try
            {
                int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["order_id"].Value);
                connection.Open();
                string query = "DELETE FROM OrderHeader WHERE order_id = @order_id";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.Parameters.AddWithValue("@order_id", orderId);
                cmd.ExecuteNonQuery();
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении заказа: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                string query = "SELECT order_id, workshop, start_date, end_date, status FROM OrderHeader WHERE workshop LIKE @workshop";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@workshop", $"%{textBoxSearchWorkshop.Text}%");
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске заказов: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        private void dataGridViewOrders_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["order_id"].Value);
                LoadOrderItems(orderId);
            }
        }

        private void LoadOrderItems(int orderId)
        {
            // Код для загрузки позиций заказа, связанного с выбранным заказом
        }
    }
}
