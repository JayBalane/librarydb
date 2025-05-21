using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace librarydb
{
    public partial class newpassword : Form
    {
        string connectionString = "server=localhost;database=librarydb;uid=root;pwd=sanantonio6;"; // Ensure this matches
        private string _emailForPasswordReset;

        // Constructor to accept the email
        public newpassword(string email)
        {
            InitializeComponent();
            _emailForPasswordReset = email;
            // Optionally display the email on the form if you have a label for it
            // lblEmailForReset.Text = "Resetting password for: " + _emailForPasswordReset;

            // Ensure password fields use PasswordChar (can also be set in designer)
            txtNewPassword.PasswordChar = '*';
            txtConfirmPassword.PasswordChar = '*';
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string newPass = txtNewPassword.Text;
            string confirmPass = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(newPass))
            {
                MessageBox.Show("New password cannot be empty.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("Passwords do not match. Please re-enter.", "Password Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtConfirmPassword.Clear();
                txtNewPassword.Clear(); // Also clear new password for re-entry
                txtNewPassword.Focus();
                return;
            }

            // !!! SECURITY WARNING: Updating with plain text password!
            // In a real application, hash the newPass before saving.
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Users SET password = @NewPassword WHERE email = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NewPassword", newPass); // Storing plain text
                    cmd.Parameters.AddWithValue("@Email", _emailForPasswordReset);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password has been reset successfully. You can now log in with your new password.", "Password Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK; // Signal success
                        this.Close(); // Close this form
                    }
                    else
                    {
                        // This case should ideally not happen if email was verified correctly
                        MessageBox.Show("Failed to reset password. User email might no longer exist or an unknown error occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error while resetting password: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelNewPass_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Make sure your TextBoxes are txtNewPassword, txtConfirmPassword
        // and buttons are btnResetPassword, btnCancelNewPass in the designer.
        // You might also have a label named lblEmailForReset if you want to display the email.
    }
}
