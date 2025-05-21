using System;
using System.Data;
using System.Diagnostics; // For Debug.WriteLine
using System.Drawing;
using System.Linq; // Required for OfType<T>()
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Required for MySQL connection
using ClosedXML.Excel;       // Required for Excel export

namespace librarydb
{
    public partial class users : Form
    {
        // IMPORTANT: Ensure this connection string is correct for your MySQL setup.
        string connectionString = "server=localhost;database=LibraryDB;uid=root;pwd=sanantonio6;";

        // Flag to prevent TextChanged events from firing updates when textboxes
        // are being populated programmatically.
        private bool _isProgrammaticChange = false;

        public users()
        {
            InitializeComponent();
            password.PasswordChar = '*'; // Mask password input
            LoadUsersData();
            userID.ReadOnly = true; // User ID is auto-generated and not directly editable
        }

        private int GetFirstVisibleColumnIndex(DataGridView dgv)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible) return col.Index;
            }
            return 0; // Fallback
        }

        private void LoadUsersData()
        {
            this.userstable.SelectionChanged -= userstable_SelectionChanged;
            _isProgrammaticChange = true;

            DataTable dataTable = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // DO NOT select the 'password' column to display in the grid for security.
                    string query = "SELECT user_id AS 'User ID', name AS 'Name', email AS 'Email', role AS 'Role' FROM Users ORDER BY user_id ASC;";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                    userstable.DataSource = dataTable;

                    userstable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    if (userstable.Columns.Contains("User ID"))
                    {
                        userstable.Columns["User ID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error while loading users: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred while loading users: " + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ClearInputFields(false); // Clear input fields but don't clear grid selection initially
            _isProgrammaticChange = false;
            this.userstable.SelectionChanged += userstable_SelectionChanged;

            if (userstable.Rows.Count > 0)
            {
                userstable.CurrentCell = userstable.Rows[0].Cells[GetFirstVisibleColumnIndex(userstable)];
                userstable_SelectionChanged(userstable, EventArgs.Empty); // Manually trigger to populate textboxes
            }
        }

        private void ClearInputFields(bool clearGridSelection = true)
        {
            _isProgrammaticChange = true;
            userID.Clear();
            name.Clear();
            email.Clear();
            role.Clear();
            password.Clear(); // Clear password field

            userID.ReadOnly = true; // User ID is always read-only
            password.ReadOnly = false; // Allow password input for new users (when fields are cleared)
            name.ReadOnly = false;
            email.ReadOnly = false;
            role.ReadOnly = false;


            if (clearGridSelection && userstable.Rows.Count > 0)
            {
                userstable.ClearSelection();
            }
            _isProgrammaticChange = false;
        }

        private void userstable_SelectionChanged(object sender, EventArgs e)
        {
            _isProgrammaticChange = true;
            if (userstable.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = userstable.SelectedRows[0];
                userID.Text = selectedRow.Cells["User ID"].Value != DBNull.Value ? selectedRow.Cells["User ID"].Value.ToString() : "";
                name.Text = selectedRow.Cells["Name"].Value != DBNull.Value ? selectedRow.Cells["Name"].Value.ToString() : "";
                email.Text = selectedRow.Cells["Email"].Value != DBNull.Value ? selectedRow.Cells["Email"].Value.ToString() : "";
                role.Text = selectedRow.Cells["Role"].Value != DBNull.Value ? selectedRow.Cells["Role"].Value.ToString() : "";

                password.Clear(); // DO NOT load existing password into the textbox
                password.ReadOnly = true; // Password cannot be edited here directly for existing users.
                                          // A separate "Change Password" feature would be needed.
            }
            else
            {
                // If selection is cleared, prepare for potentially adding a new user
                userID.Clear(); // User ID will be auto-generated by DB
                name.Clear();
                email.Clear();
                role.Clear();
                password.Clear();
                password.ReadOnly = false; // Enable password field for new entry
            }
            _isProgrammaticChange = false;
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            string currentName = name.Text.Trim();
            string currentEmail = email.Text.Trim();
            string currentRole = role.Text.Trim();
            string currentPassword = password.Text; // Plain text, needs hashing in real app

            if (string.IsNullOrWhiteSpace(currentName) ||
                string.IsNullOrWhiteSpace(currentEmail) ||
                string.IsNullOrWhiteSpace(currentRole) ||
                string.IsNullOrWhiteSpace(currentPassword)) // Password is required for new users
            {
                MessageBox.Show("Name, Email, Role, and Password are required for new users.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate Role
            if (currentRole != "Librarian" && currentRole != "Member")
            {
                MessageBox.Show("Role must be either 'Librarian' or 'Member'.", "Input Error - Role", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                role.Focus();
                return;
            }

            // Basic email format validation
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(currentEmail);
            }
            catch
            {
                MessageBox.Show("Invalid email format.", "Input Error - Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                email.Focus();
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Check if email already exists (as email is UNIQUE)
                    string checkEmailQuery = "SELECT COUNT(*) FROM Users WHERE email = @Email";
                    MySqlCommand checkCmd = new MySqlCommand(checkEmailQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Email", currentEmail);
                    if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show($"The email '{currentEmail}' is already registered.", "Duplicate Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        email.Focus();
                        return;
                    }

                    // !!! SECURITY WARNING: Storing plain text passwords! HASH passwords in a real application.
                    string query = "INSERT INTO Users (name, email, role, password) VALUES (@Name, @Email, @Role, @Password)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", currentName);
                    cmd.Parameters.AddWithValue("@Email", currentEmail);
                    cmd.Parameters.AddWithValue("@Role", currentRole);
                    cmd.Parameters.AddWithValue("@Password", currentPassword); // Storing plain text
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUsersData(); // Refresh grid and clear fields
                    }
                    else
                    {
                        MessageBox.Show("Failed to add user. No rows were affected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error while adding user: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred while adding user: " + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateUserField(string fieldName, object value, int userIdToUpdate)
        {
            // Password field is not updatable through this method for existing users.
            if (fieldName.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Debug.WriteLine("UpdateUserField: Password updates require a separate dedicated function. Update skipped.");
                return;
            }

            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                Debug.WriteLine($"UpdateUserField: {fieldName} cannot be empty. Update skipped.");
                return;
            }

            // Specific validations
            if (fieldName.Equals("email", StringComparison.OrdinalIgnoreCase))
            {
                try { var m = new System.Net.Mail.MailAddress(value.ToString()); }
                catch { Debug.WriteLine("UpdateUserField: Invalid email format. Update skipped."); return; }

                using (MySqlConnection tempConn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        tempConn.Open();
                        string checkEmailQuery = "SELECT COUNT(*) FROM Users WHERE email = @Email AND user_id != @UserId";
                        MySqlCommand checkCmd = new MySqlCommand(checkEmailQuery, tempConn);
                        checkCmd.Parameters.AddWithValue("@Email", value.ToString());
                        checkCmd.Parameters.AddWithValue("@UserId", userIdToUpdate);
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                        {
                            Debug.WriteLine($"UpdateUserField: Email '{value}' already exists for another user. Update skipped.");
                            MessageBox.Show($"The email '{value}' is already registered by another user. Reverting change.", "Duplicate Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            int tempCurrentRowIndex = -1;
                            if (userstable.SelectedRows.Count > 0) tempCurrentRowIndex = userstable.SelectedRows[0].Index;
                            LoadUsersData();
                            if (tempCurrentRowIndex != -1 && userstable.Rows.Count > tempCurrentRowIndex)
                            {
                                _isProgrammaticChange = true;
                                userstable.ClearSelection();
                                userstable.Rows[tempCurrentRowIndex].Selected = true;
                                _isProgrammaticChange = false;
                                userstable_SelectionChanged(userstable, EventArgs.Empty);
                            }
                            return;
                        }
                    }
                    catch (Exception ex) { Debug.WriteLine($"Error checking email uniqueness: {ex.Message}"); return; }
                }
            }

            if (fieldName.Equals("role", StringComparison.OrdinalIgnoreCase))
            {
                string roleValue = value.ToString();
                if (roleValue != "Librarian" && roleValue != "Member")
                {
                    Debug.WriteLine($"UpdateUserField: Invalid role '{roleValue}'. Must be 'Librarian' or 'Member'. Update skipped.");
                    MessageBox.Show("Role must be 'Librarian' or 'Member'. Reverting change.", "Invalid Role", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    int tempCurrentRowIndex = -1;
                    if (userstable.SelectedRows.Count > 0) tempCurrentRowIndex = userstable.SelectedRows[0].Index;
                    LoadUsersData();
                    if (tempCurrentRowIndex != -1 && userstable.Rows.Count > tempCurrentRowIndex)
                    {
                        _isProgrammaticChange = true;
                        userstable.ClearSelection();
                        userstable.Rows[tempCurrentRowIndex].Selected = true;
                        _isProgrammaticChange = false;
                        userstable_SelectionChanged(userstable, EventArgs.Empty);
                    }
                    return;
                }
            }

            TextBox activeTextBox = null;
            int originalSelectionStart = 0;
            int originalSelectionLength = 0;
            Control currentFocusedControl = this.ActiveControl;
            if (currentFocusedControl is TextBox tb && (tb == name || tb == email || tb == role))
            {
                activeTextBox = tb;
                originalSelectionStart = activeTextBox.SelectionStart;
                originalSelectionLength = activeTextBox.SelectionLength;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = $"UPDATE Users SET `{fieldName}` = @Value WHERE user_id = @UserId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Value", value.ToString());
                    cmd.Parameters.AddWithValue("@UserId", userIdToUpdate);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Debug.WriteLine($"SUCCESS: User ID {userIdToUpdate} field '{fieldName}' updated to '{value}'.");
                        int currentRowIndex = -1;
                        if (userstable.SelectedRows.Count > 0) currentRowIndex = userstable.SelectedRows[0].Index;

                        LoadUsersData();

                        if (currentRowIndex != -1 && userstable.Rows.Count > currentRowIndex)
                        {
                            _isProgrammaticChange = true;
                            userstable.ClearSelection();
                            userstable.Rows[currentRowIndex].Selected = true;
                            userstable.CurrentCell = userstable.Rows[currentRowIndex].Cells[GetFirstVisibleColumnIndex(userstable)];
                            if (!userstable.Rows[currentRowIndex].Displayed)
                                userstable.FirstDisplayedScrollingRowIndex = currentRowIndex;
                            _isProgrammaticChange = false;
                        }
                        if (activeTextBox != null)
                        {
                            activeTextBox.Focus();
                            if (originalSelectionStart <= activeTextBox.Text.Length) activeTextBox.SelectionStart = originalSelectionStart;
                            else activeTextBox.SelectionStart = activeTextBox.Text.Length;
                            activeTextBox.SelectionLength = originalSelectionLength;
                        }
                    }
                    else
                    {
                        Debug.WriteLine($"INFO: Update for User ID {userIdToUpdate} ({fieldName}) did not affect any rows. Value might be unchanged.");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Database error while updating user: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadUsersData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadUsersData();
                }
            }
        }

        private void name_TextChanged(object sender, EventArgs e)
        {
            if (_isProgrammaticChange || userstable.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(userID.Text)) return;
            if (int.TryParse(userID.Text, out int currentUserId))
            {
                UpdateUserField("name", name.Text, currentUserId);
            }
        }

        private void email_TextChanged(object sender, EventArgs e)
        {
            if (_isProgrammaticChange || userstable.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(userID.Text)) return;
            if (int.TryParse(userID.Text, out int currentUserId))
            {
                // Basic check to avoid updating on every char if email is clearly invalid early
                // More robust validation (like regex or just trying to parse) happens in UpdateUserField
                if (email.Text.Contains("@") && email.Text.Contains("."))
                {
                    UpdateUserField("email", email.Text, currentUserId);
                }
                else if (email.Text.Length > 3) // Heuristic to avoid too many checks on very short invalid strings
                {
                    Debug.WriteLine("Email TextChanged: Format still looks incomplete for update check.");
                }
            }
        }

        private void role_TextChanged(object sender, EventArgs e)
        {
            if (_isProgrammaticChange || userstable.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(userID.Text)) return;
            if (int.TryParse(userID.Text, out int currentUserId))
            {
                // Validate role before attempting update
                string currentRole = role.Text.Trim();
                // We call UpdateUserField, and it will do the strict 'Librarian'/'Member' check.
                // This allows the user to type and only validates fully when UpdateUserField is called.
                UpdateUserField("role", currentRole, currentUserId);
            }
        }

        private void edit_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Edit button clicked - Fields cleared for new user entry or re-selection. Edits for Name, Email, and Role are saved automatically as you type for selected users. Password for existing users must be changed via a dedicated feature (not implemented here).");
            ClearInputFields(true);
            name.Focus();
            password.ReadOnly = false; // Ensure password field is enabled for a potential new entry
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            if (userstable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(userID.Text, out int userIdToDelete))
            {
                MessageBox.Show("No valid user selected for deletion (User ID is missing or invalid).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string currentUserName = name.Text;
            DialogResult confirmResult = MessageBox.Show($"Are you sure you want to delete '{currentUserName}' (ID: {userIdToDelete})?\nThis will also delete related borrower and review records (due to ON DELETE CASCADE in DB).", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Users WHERE user_id = @UserId";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@UserId", userIdToDelete);
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("User deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadUsersData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete user. User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Database error while deleting user: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An unexpected error occurred while deleting user: " + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void search_button_Click(object sender, EventArgs e)
        {
            string searchTerm = searchbox.Text.Trim();
            this.userstable.SelectionChanged -= userstable_SelectionChanged;
            _isProgrammaticChange = true;

            DataTable dataTable = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query;
                    MySqlCommand cmd = new MySqlCommand { Connection = conn };

                    if (string.IsNullOrWhiteSpace(searchTerm))
                    {
                        query = "SELECT user_id AS 'User ID', name AS 'Name', email AS 'Email', role AS 'Role' FROM Users ORDER BY user_id ASC;";
                    }
                    else
                    {
                        query = "SELECT user_id AS 'User ID', name AS 'Name', email AS 'Email', role AS 'Role' FROM Users " +
                                "WHERE name LIKE @SearchTerm OR email LIKE @SearchTerm OR role LIKE @SearchTerm OR user_id LIKE @SearchTermNumericEquivalent ORDER BY user_id ASC;";
                        cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                        cmd.Parameters.AddWithValue("@SearchTermNumericEquivalent", searchTerm);
                    }
                    cmd.CommandText = query;
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                    userstable.DataSource = dataTable;

                    if (dataTable.Rows.Count == 0 && !string.IsNullOrWhiteSpace(searchTerm))
                    {
                        MessageBox.Show("No users found matching your search.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error during search: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected error during search: " + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ClearInputFields(true);
            _isProgrammaticChange = false;
            this.userstable.SelectionChanged += userstable_SelectionChanged;
            if (userstable.Rows.Count > 0)
            {
                userstable.CurrentCell = userstable.Rows[0].Cells[GetFirstVisibleColumnIndex(userstable)];
                userstable_SelectionChanged(userstable, EventArgs.Empty);
            }
        }

        private void save2Excel_button_Click(object sender, EventArgs e)
        {
            if (userstable.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true, FileName = "UsersExport.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            DataTable dt = (DataTable)userstable.DataSource;
                            var worksheet = workbook.Worksheets.Add(dt, "Users");
                            worksheet.Columns().AdjustToContents();
                            workbook.SaveAs(sfd.FileName);
                        }
                        MessageBox.Show("Data exported successfully to Excel!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting data to Excel: " + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Navigation Event Handlers (from side panel) ---
        private void books_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Books button clicked - Navigating to Home (Books).");
            try
            {
                this.Hide();
                home homeForm = Application.OpenForms.OfType<home>().FirstOrDefault();
                if (homeForm == null || homeForm.IsDisposed)
                {
                    homeForm = new home();
                    homeForm.FormClosed += (s, args) => this.Close();
                    homeForm.Show();
                }
                else
                {
                    homeForm.Show();
                    homeForm.Activate();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to Books: {ex.ToString()}", "Navigation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
                this.Activate();
            }
        }

        private void authors_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Authors button clicked - Navigating to Authors page.");
            try
            {
                this.Hide();
                using (authors authors_form = new authors())
                {
                    authors_form.ShowDialog(this);
                }
                this.Show();
                this.Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Authors form: {ex.ToString()}", "Navigation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
                this.Activate();
            }
        }

        private void users_button_Click(object sender, EventArgs e)
        {
            // This is the current form, so just refresh data.
            MessageBox.Show("Users button clicked - Refreshing users data.");
            LoadUsersData();
        }

        private void borrowedbooks_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Borrowed Books button clicked - Navigating to Borrowed Books page.");
            try
            {
                this.Hide();
                using (borrowedbooks borrowed_form = new borrowedbooks())
                {
                    borrowed_form.ShowDialog(this);
                }
                this.Show();
                this.Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Borrowed Books form: {ex.ToString()}", "Navigation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
                this.Activate();
            }
        }

        private void borrowers_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Borrowers button clicked - Navigating to Borrowers page.");
            try
            {
                this.Hide();
                using (borrowers borrowers_form = new borrowers())
                {
                    borrowers_form.ShowDialog(this);
                }
                this.Show();
                this.Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Borrowers form: {ex.ToString()}", "Navigation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
                this.Activate();
            }
        }

        private void logout_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Logout button clicked - Returning to Login.");
            try
            {
                this.Hide();
                login login_form = Application.OpenForms.OfType<login>().FirstOrDefault();

                if (login_form == null || login_form.IsDisposed)
                {
                    login_form = new login();
                    login_form.FormClosed += (s, args) => {
                        if (Application.OpenForms.Count == 0)
                        {
                            // Application.Exit(); // Consider if this is desired if login is closed by user
                        }
                    };
                    login_form.Show();
                }
                else
                {
                    login_form.Show();
                    login_form.Activate();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during logout: {ex.ToString()}", "Logout Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
                this.Activate();
            }
        }
    }
}
