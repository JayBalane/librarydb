using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace librarydb
{
    public partial class forgotpassword : Form // Ensure class name matches file name
    {
        string connectionString = "server=localhost;database=librarydb;uid=root;pwd=sanantonio6;";

        // Control names used in this code:
        // txtEmail (TextBox for email input)
        // btnVerifyEmail (Button to verify email)
        // btnCancelForgot (Button to cancel)
        // Ensure these match your forgotpassword.Designer.cs control names.

        public forgotpassword()
        {
            InitializeComponent();
        }

        private void btnVerifyEmail_Click(object sender, EventArgs e)
        {
            string emailToVerify = txtEmail.Text.Trim(); // Using txtEmail

            if (string.IsNullOrWhiteSpace(emailToVerify))
            {
                MessageBox.Show("Please enter your email address.", "Email Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus(); // Using txtEmail
                return;
            }

            try
            {
                var mail = new System.Net.Mail.MailAddress(emailToVerify);
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid email format. Please enter a valid email address.", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus(); // Using txtEmail
                txtEmail.SelectAll(); // Using txtEmail
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE email = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", emailToVerify);

                    int userCount = Convert.ToInt32(cmd.ExecuteScalar());

                    if (userCount > 0)
                    {
                        this.Hide();
                        using (newpassword newPasswordForm = new newpassword(emailToVerify))
                        {
                            DialogResult result = newPasswordForm.ShowDialog(this.Owner);

                            if (result == DialogResult.OK)
                            {
                                this.DialogResult = DialogResult.OK;
                            }
                            else
                            {
                                this.DialogResult = DialogResult.Cancel;
                            }
                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Email address not found in our records. Returning to login.", "Email Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // txtEmail.Focus(); // Not strictly needed as form will close
                        // txtEmail.SelectAll();
                        this.DialogResult = DialogResult.Cancel; // Indicate failure/cancellation to login form
                        this.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error during email verification: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelForgot_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // This was a placeholder from your original file.
        // If label2 is not interactive, remove this event handler and its wiring in the designer.
        private void label2_Click(object sender, EventArgs e)
        {
            // Placeholder
        }
    }
}
