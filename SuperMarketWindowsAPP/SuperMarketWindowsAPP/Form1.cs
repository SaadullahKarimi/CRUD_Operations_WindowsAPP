using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace SuperMarketWindowsAPP
{
    public partial class Form1 : Form
    {
        int bindCounter = 0;
        SqlDataAdapter da;
        DataSet ds;
        BindingSource bsTable;
        public Form1()
        {
            da = new SqlDataAdapter();
            ds = new DataSet();
            bsTable = new BindingSource();

            InitializeComponent();
        }

        private void btnsavedata_Click(object sender, EventArgs e)
        {
            {
                da.InsertCommand = new SqlCommand("Insert into Customer (Cust_ID, F_Name, L_Name,Phone_Number, Address, Gender, Email, Age) values('" + txtid.Text + "','" + txtfname.Text + "', '" + txtlname.Text + "', '" + txtphoneno.Text + "','" + txtadd.Text + "','" + txtgender.Text + "', '" + txtEmail.Text + "','" + txtage.Text + "'); ",
                Connectivity.cn);
                Connectivity.Connect();
                da.InsertCommand.ExecuteNonQuery();
                Connectivity.Disconnect();
            }
        }
        void showData(String Query)
        {
            da.SelectCommand = new SqlCommand(Query, Connectivity.cn);
            ds.Clear();
            da.Fill(ds);
            DGVcust.DataSource = ds.Tables[0];
            bsTable.DataSource = ds.Tables[0];
            if (bindCounter == 0)
            {
                txtid.DataBindings.Add(new Binding("Text", bsTable, "Cust_ID"));
                txtfname.DataBindings.Add(new Binding("Text", bsTable, "F_Name"));
                txtlname.DataBindings.Add(new Binding("Text", bsTable, "L_Name"));
                txtphoneno.DataBindings.Add(new Binding("Text", bsTable, "Phone_Number"));
                txtadd.DataBindings.Add(new Binding("Text", bsTable, "Address"));
                txtgender.DataBindings.Add(new Binding("Text", bsTable, "Gender"));
                txtEmail.DataBindings.Add(new Binding("Text", bsTable, "Email"));
                txtage.DataBindings.Add(new Binding("Text", bsTable, "Age"));

                bindCounter = 1;

            }
            selectrow();
        }
        void selectrow()
        {

            try
            {
                DGVcust.ClearSelection();
                DGVcust.Rows[bsTable.Position].Selected = true;
                DGVcust.CurrentCell = DGVcust.Rows[bsTable.Position].Cells[0];

            }
            catch (Exception ex)
            {

            }



        }












        private void button1_Click(object sender, EventArgs e)
        {
            showData("Select * from Customer");


        }

        private void txtadd_TextChanged(object sender, EventArgs e)
        {

        }

        private void DGVcust_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bsTable.Position = e.RowIndex;
            selectrow();
        }

        private void txtphoneno_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            // Delete a customer here 


            if (string.IsNullOrEmpty(txtid.Text))
            {
                // Show a message dialog indicating that the customer ID is missing
                MessageBox.Show("Please enter a valid customer ID.", "Missing Customer ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit the method without further processing
            }
            int customerId = int.Parse(txtid.Text); // Parse the customer ID from the input


            if (!int.TryParse(txtid.Text, out customerId))
            {
                // Show a message dialog if the input is not a valid integer
                MessageBox.Show("Invalid customer ID. Please enter a numeric value.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit the method
            }

            // Assuming 'Cust_ID' is the primary key for identifying customers

            using (SqlConnection connection = new SqlConnection("Data source = nazari-ibrahim;  initial catalog=homework; user =ibrahim; password=12345"))
            {
                connection.Open();

                // Construct the SQL query
                string deleteQuery = "DELETE FROM Customer WHERE Cust_ID = @CustomerId";

                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    // Execute the query
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Customer deleted successfully
                        // You can add any additional logic or UI updates here
                    }
                    else
                    {
                        // Customer with the specified ID was not found
                        // Handle this case as needed
                    }
                }
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            // connection according to sql server credential in my pc

            using (SqlConnection connection = new SqlConnection("Data source = MYADMIN;  initial catalog=SuperMarket; user =rasouli; password=1234@asdf"))
            {
                connection.Open();

                SqlCommand searchCommand = new SqlCommand("SELECT * FROM Customer WHERE Cust_ID = @CustID", Connectivity.cn);
                searchCommand.Parameters.AddWithValue("@CustID", txtid.Text);// you enter id for customer and click on search to show

                Connectivity.Connect();
                SqlDataReader reader = searchCommand.ExecuteReader();

                if (reader.Read())
                {
                    txtfname.Text = reader["F_Name"].ToString();
                    txtlname.Text = reader["L_Name"].ToString();
                    txtphoneno.Text = reader["Phone_Number"].ToString();
                    txtadd.Text = reader["Address"].ToString();
                    txtgender.Text = reader["Gender"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                    txtage.Text = reader["Age"].ToString();
                }
                else
                {
                    MessageBox.Show("Customer not found!");
                }

                reader.Close();
                Connectivity.Disconnect();

            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtid.Text))
            {
                MessageBox.Show("Please enter a valid Customer ID to update.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection("Data source = MYADMIN; initial catalog=SuperMarket; user=rasouli; password=1234@asdf"))
                {
                    connection.Open();

                    string updateQuery = @"UPDATE Customer 
                                   SET F_Name = @FName, 
                                       L_Name = @LName, 
                                       Phone_Number = @PhoneNumber, 
                                       Address = @Address, 
                                       Gender = @Gender, 
                                       Email = @Email, 
                                       Age = @Age 
                                   WHERE Cust_ID = @CustID";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CustID", txtid.Text);
                        command.Parameters.AddWithValue("@FName", txtfname.Text);
                        command.Parameters.AddWithValue("@LName", txtlname.Text);
                        command.Parameters.AddWithValue("@PhoneNumber", txtphoneno.Text);
                        command.Parameters.AddWithValue("@Address", txtadd.Text);
                        command.Parameters.AddWithValue("@Gender", txtgender.Text);
                        command.Parameters.AddWithValue("@Email", txtEmail.Text);
                        command.Parameters.AddWithValue("@Age", txtage.Text);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            showData("SELECT * FROM Customer"); // refresh grid
                        }
                        else
                        {
                            MessageBox.Show("Customer not found or no changes made.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
