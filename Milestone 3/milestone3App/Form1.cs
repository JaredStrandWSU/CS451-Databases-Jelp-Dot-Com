using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace milestone3App
{
    public partial class YelpApp : Form
    {
        public YelpApp()
        {
            InitializeComponent();
            addStates();
            addColumnsToGrid();
        }
        private string BuildConnString()
        {
            return "Host=localhost; Username=postgres; Password=admin; Database=yelp";
        }

        public void addStates()
        {
            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT state FROM businesses ORDER BY state;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }

        public void addCities()
        {
            cityListBox.Items.Clear();
            if (!comboBox1.SelectedIndex.Equals(-1))
            {
                using (var conn = new NpgsqlConnection(BuildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT DISTINCT city FROM businesses WHERE state = '" + 
                            comboBox1.SelectedItem.ToString().ToUpper() + "' ORDER BY city; ";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cityListBox.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }

        public void addZipcodes()
        {
            zipcodeListBox.Items.Clear();
            if (!comboBox1.SelectedIndex.Equals(-1))
            {
                using (var conn = new NpgsqlConnection(BuildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT DISTINCT postal_code FROM businesses WHERE city = '" + 
                            cityListBox.SelectedItem.ToString() + "' ORDER BY postal_code; ";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                zipcodeListBox.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }
        public void addBusinesses()
        {
            dataGridView1.Rows.Clear();
            if (!comboBox1.SelectedIndex.Equals(-1))
            {
                using (var conn = new NpgsqlConnection(BuildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT business_id, name, state, city, postal_code, address, " +
                            "longitude, latitude, stars, review_count, review_rating, num_checkins, " +
                            "open_status FROM businesses WHERE postal_code = '" +
                            zipcodeListBox.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            var i = 0;
                            while (reader.Read())
                            {
                                dataGridView1.Rows.Add();
                                dataGridView1.Rows[i].Cells[0].Value = reader.GetString(0);
                                dataGridView1.Rows[i].Cells[1].Value = reader.GetString(1);
                                dataGridView1.Rows[i].Cells[2].Value = reader.GetString(2);
                                dataGridView1.Rows[i].Cells[3].Value = reader.GetString(3);
                                dataGridView1.Rows[i].Cells[4].Value = reader.GetString(4);
                                dataGridView1.Rows[i].Cells[5].Value = reader.GetString(5);
                                dataGridView1.Rows[i].Cells[6].Value = reader.GetDouble(6);
                                dataGridView1.Rows[i].Cells[7].Value = reader.GetDouble(7);
                                dataGridView1.Rows[i].Cells[8].Value = reader.GetDouble(8);
                                dataGridView1.Rows[i].Cells[9].Value = reader.GetInt32(9);
                                dataGridView1.Rows[i].Cells[10].Value = reader.GetDouble(10);
                                dataGridView1.Rows[i].Cells[11].Value = reader.GetInt32(11);
                                dataGridView1.Rows[i].Cells[12].Value = reader.GetBoolean(12);
                                i++;
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }


        public void getCategories()
        {
            categoriesListBox.Items.Clear();
            if (!comboBox1.SelectedIndex.Equals(-1))
            {
                using (var conn = new NpgsqlConnection(BuildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT DISTINCT category FROM BusinessCategories WHERE business_id IN (SELECT business_id " +
                            "FROM Businesses WHERE postal_code = '" + zipcodeListBox.SelectedItem.ToString() + "') ORDER BY category;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categoriesListBox.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }

        public void addColumnsToGrid()
        {
            DataGridViewColumn col0 = new DataGridViewColumn();
            col0.HeaderText = "Business ID";
            col0.Width = 100;
            col0.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(col0);

            DataGridViewColumn col1 = new DataGridViewColumn();
            col1.HeaderText = "Business Name";
            col1.Width = 225;
            col1.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(col1);

            DataGridViewColumn col2 = new DataGridViewColumn();
            col2.HeaderText = "State";
            col2.Width = 60;
            col2.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(col2);

            DataGridViewColumn col3 = new DataGridViewColumn();
            col3.HeaderText = "City";
            col3.Width = 100;
            col3.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(col3);
            DataGridViewColumn col4 = new DataGridViewColumn();
            col4.HeaderText = "Zipcode";
            col4.Width = 100;
            col4.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(col4);
            DataGridViewColumn col5 = new DataGridViewColumn();
            col5.HeaderText = "Address";
            col5.Width = 100;
            col5.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(col5);
            DataGridViewColumn col6 = new DataGridViewColumn();
            col6.HeaderText = "Longitude";
            col6.Width = 100;
            col6.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(col6);
            DataGridViewColumn col7 = new DataGridViewColumn();
            col7.HeaderText = "Latitude";
            col7.Width = 100;
            col7.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(col7);
            DataGridViewColumn col7half = new DataGridViewColumn();
            col7half.HeaderText = "Stars";
            col7half.Width = 100;
            col7half.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(col7half);
            DataGridViewColumn col8 = new DataGridViewColumn();
            col8.HeaderText = "Review Count";
            col8.Width = 100;
            col8.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(col8);
            DataGridViewColumn col9 = new DataGridViewColumn();
            col9.HeaderText = "Review Rating";
            col9.Width = 100;
            col9.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(col9);
            DataGridViewColumn col10 = new DataGridViewColumn();
            col10.HeaderText = "Num Checkins";
            col10.Width = 100;
            col10.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(col10);
            DataGridViewColumn col11 = new DataGridViewColumn();
            col11.HeaderText = "Open";
            col11.Width = 100;
            col11.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView1.Columns.Add(col11);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            cityListBox.Items.Clear();
            zipcodeListBox.Items.Clear();
            categoriesListBox.Items.Clear();
            addCities();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            zipcodeListBox.Items.Clear();
            categoriesListBox.Items.Clear();
            addZipcodes();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            categoriesListBox.Items.Clear();
            addBusinesses();
            getCategories();
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (!comboBox1.SelectedIndex.Equals(-1))
            {
                using (var conn = new NpgsqlConnection(BuildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT business_id, name, state, city, postal_code, address, " +
                            "longitude, latitude, stars, review_count, review_rating, num_checkins, " +
                            "open_status FROM businesses WHERE postal_code = '" +
                            zipcodeListBox.SelectedItem.ToString() + "' AND business_id IN (SELECT business_id FROM " +
                            "businessCategories WHERE category = '" + categoriesListBox.SelectedItem.ToString() + "');";
                        using (var reader = cmd.ExecuteReader())
                        {
                            var i = 0;
                            while (reader.Read())
                            {
                                dataGridView1.Rows.Add();
                                dataGridView1.Rows[i].Cells[0].Value = reader.GetString(0);
                                dataGridView1.Rows[i].Cells[1].Value = reader.GetString(1);
                                dataGridView1.Rows[i].Cells[2].Value = reader.GetString(2);
                                dataGridView1.Rows[i].Cells[3].Value = reader.GetString(3);
                                dataGridView1.Rows[i].Cells[4].Value = reader.GetString(4);
                                dataGridView1.Rows[i].Cells[5].Value = reader.GetString(5);
                                dataGridView1.Rows[i].Cells[6].Value = reader.GetDouble(6);
                                dataGridView1.Rows[i].Cells[7].Value = reader.GetDouble(7);
                                dataGridView1.Rows[i].Cells[8].Value = reader.GetDouble(8);
                                dataGridView1.Rows[i].Cells[9].Value = reader.GetInt32(9);
                                dataGridView1.Rows[i].Cells[10].Value = reader.GetDouble(10);
                                dataGridView1.Rows[i].Cells[11].Value = reader.GetInt32(11);
                                dataGridView1.Rows[i].Cells[12].Value = reader.GetBoolean(12);
                                i++;
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            comboBox1_SelectedIndexChanged(sender, e);
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //this is where a user enters their name, once entered, we need to write a query to search for their name, when we find all names like "tyler" we shoudl load
            //them into the combobox below. Once the user selects their id from the list, we can use the selected ID to populate the user information boxes, and use the user ID to
            //populate the Friends category with a cross query, then we can use the returned friends query of user ID to pull the latest review from each person and add it to the
            //review box.
            /*
             *
                A. User Information:
                User can view the list of his/her friends and the latest reviews each friend has provided.
                Use Case:
                1. The user enters his name and chooses his/her own user id (among the users who has the same
                name). The system retrieves user’s profile information (including, his/her name, average stars, the
                date he/she joined yelp, number of fans, average stars, and count of votes) from the database and
                displays it on the screen. The list of the user’s friends and the latest review(s) that each of those
                friends posted are displayed.
                The user may remove a friend from his friend list. (See Figure-1)
             */

            //get input from box and set as user var
            string username = null;
            username = this.nameTextBox.Text;
            Console.WriteLine("Testing: " + username);

            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT user_id FROM users WHERE name=" + username + ";";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //put all user ID's into the box, not working don't know why
                            userIDlistBox.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }

        private void userIDlistBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Once a user has been selected by their user_id, populate their profile information, populate friends Data Grid, and populate reviews section
        }

        private void friendsDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void cityListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1_SelectedIndexChanged(sender, e);
        }

        private void zipcodeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2_SelectedIndexChanged(sender, e);
        }
    }
}
