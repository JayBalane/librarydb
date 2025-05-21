using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data; // Required for ConnectionState

namespace librarydb
{
    public partial class register : Form
    {
        string connectionString = "server=localhost;database=LibraryDB;uid=root;pwd=sanantonio6;"; // Ensure this is correct

        public register()
        {
            InitializeComponent();
            // Wire up the click event for the signup button directly here
            signup_button.Click += new EventHandler(SignUpButton_Click);
            password_textBox.PasswordChar = '*';
            confirmpass_textBox.PasswordChar = '*';
        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            string firstName = fname_textBox.Text.Trim();
            string lastName = lname_textBox.Text.Trim();
            string email = email_textBox.Text.Trim();
            string password = password_textBox.Text; // Plain text password
            string confirmPassword = confirmpass_textBox.Text;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("All fields are required.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match!", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fullName = firstName + " " + lastName;

            // !!! SECURITY WARNING: STORING PLAIN TEXT PASSWORD !!!
            // You should HASH the password before storing it.
            // Example (conceptual, use a proper library like BCrypt.Net):
            // string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Check if email already exists
                    string checkEmailQuery = "SELECT COUNT(*) FROM Users WHERE email = @Email";
                    MySqlCommand checkCmd = new MySqlCommand(checkEmailQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Email", email);
                    int emailExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (emailExists > 0)
                    {
                        MessageBox.Show("This email address is already registered.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close(); // Close connection before returning
                        return;
                    }

                    // Insert new user WITH password
                    // IMPORTANT: Replace @password with the HASHED password in a production system
                    string query = "INSERT INTO Users (name, email, role, password) VALUES (@name, @email, 'Member', @password)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", fullName);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password); // Store plain text password (NOT RECOMMENDED FOR PRODUCTION)
                    // cmd.Parameters.AddWithValue("@password", hashedPassword); // When using hashing

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User registered successfully! Please login.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();
                    login loginForm = new login();
                    loginForm.Show();
                    this.Close(); // Close the register form
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error during registration: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during registration: " + ex.Message, "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        // This method might be linked to the label4 click event in the designer.
        // If not needed, you can also remove its linkage in the designer's event properties for label4.
        private void label4_Click(object sender, EventArgs e)
        {
            // Typically, labels don't require click events unless for specific UI interactions.
        }
    }
}