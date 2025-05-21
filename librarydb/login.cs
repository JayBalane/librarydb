using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data; // Required for ConnectionState
using System.Linq; // Required for Application.OpenForms.OfType<T>()

namespace librarydb
{
    public partial class login : Form
    {
        string connectionString = "server=localhost;database=librarydb;uid=root;pwd=sanantonio6;"; // Ensure this is correct

        public login()
        {
            InitializeComponent();
            // Ensure password_textBox is the correct name from your login.Designer.cs for the password input
            // And email_textBox is the correct name for the email input.
            if (this.Controls.Find("password_textBox", true).FirstOrDefault() is TextBox passBox)
            {
                passBox.PasswordChar = '*';
            }
            // Add similar check for email_textBox if its name is different in your designer
        }

        private void loginbutton_Click(object sender, EventArgs e)
        {
            string email = email_textBox.Text.Trim(); //
            string inputPassword = password_textBox.Text; //

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(inputPassword)) //
            {
                MessageBox.Show("Please enter both email and password.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning); //
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT user_id, name, email, role, password FROM Users WHERE email = @Email"; //
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", email); //
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) //
                    {
                        string dbStoredPassword = reader["password"].ToString(); //
                        string name = reader["name"].ToString(); //
                        string userRole = reader["role"].ToString(); //

                        if (inputPassword == dbStoredPassword) // PLAIN TEXT COMPARISON - INSECURE
                        {
                            MessageBox.Show($"Login successful!\nWelcome, {name} (Role: {userRole})", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information); //

                            // Create a new instance of the home form
                            home newHomeForm = new home();

                            // When the newHomeForm closes, then close this login form.
                            // This keeps the application alive via the (hidden) login form
                            // until the home form is intentionally closed.
                            newHomeForm.FormClosed += (s, args) => {
                                if (!this.IsDisposed)
                                {
                                    this.Close();
                                }
                            };

                            this.Hide(); // Hide the login form
                            newHomeForm.Show(); // Show the home form
                                                // Do NOT call this.Close() here directly if login form is the main application form.
                        }
                        else
                        {
                            MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error); //
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error); //
                    }
                    reader.Close(); //
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("A database error occurred. Please try again later.\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred. Please try again later.\n" + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //
                }
            }
        }

        private void signupbutton_Click(object sender, EventArgs e)
        {
            this.Hide();
            register registerForm = Application.OpenForms.OfType<register>().FirstOrDefault() ?? new register();
            if (registerForm.IsDisposed) registerForm = new register();

            registerForm.ShowDialog(this);

            if (!this.IsDisposed)
            {
                this.Show();
                this.Activate();
            }
        }

        private void forgotpass_link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide(); // Hide the login form while forgot password process is active
            using (forgotpassword forgotPassForm = new forgotpassword())
            {
                DialogResult result = forgotPassForm.ShowDialog(this); // Show forgot password form as a dialog

                // After forgotPassForm (and potentially newPasswordForm) closes, we return here.
                if (result == DialogResult.OK)
                {
                    // This means the password was successfully reset in the newpassword form.
                    MessageBox.Show("Password has been successfully reset. Please log in with your new password.", "Password Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    email_textBox.Clear();
                    password_textBox.Clear();
                    email_textBox.Focus();
                }
                // Whether OK or Cancel (or if email wasn't found and forgotPassForm closed), we re-show the login form.
                if (!this.IsDisposed) // Check if login form hasn't been closed by other means
                {
                    this.Show();
                    this.Activate();
                }
            }
        }

        private void login_Load(object sender, EventArgs e)
        {
            if (email_textBox != null)
            {
                email_textBox.Select();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            signupbutton_Click(sender, e);
        }

        private void email_textBox_TextChanged(object sender, EventArgs e)
        {
            // Logic for email textbox change if any
        }

        private void password_textBox_TextChanged(object sender, EventArgs e)
        {
            // Logic for password textbox change if any
        }
    }
}
