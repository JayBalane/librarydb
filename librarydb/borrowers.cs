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
    public partial class borrowers : Form
    {
        string connectionString = "server=localhost;database=LibraryDB;uid=root;pwd=sanantonio6;";
        private bool _isProgrammaticChange = false;

        public borrowers()
        {
            InitializeComponent();
            // Wire up KeyPress for user_id textbox to allow only integers
            this.user_id.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IntegerOnly_KeyPress);
            LoadBorrowersData();
            borrower_id.ReadOnly = true;
        }

        private void IntegerOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, the backspace key, and other control characters.
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Suppress the key press
            }
        }

        private int GetFirstVisibleColumnIndex(DataGridView dgv)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible) return col.Index;
            }
            return 0;
        }

        private void LoadBorrowersData()
        {
            this.borrowerstable.SelectionChanged -= borrowerstable_SelectionChanged;
            _isProgrammaticChange = true;

            DataTable dataTable = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            br.borrower_id AS 'Borrower ID', 
                            br.user_id AS 'User ID', 
                            u.name AS 'User Name', 
                            u.email AS 'User Email'
                        FROM Borrowers br
                        JOIN Users u ON br.user_id = u.user_id
                        ORDER BY br.borrower_id ASC;";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                    borrowerstable.DataSource = dataTable;

                    borrowerstable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    if (borrowerstable.Columns.Contains("Borrower ID"))
                        borrowerstable.Columns["Borrower ID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    if (borrowerstable.Columns.Contains("User ID"))
                        borrowerstable.Columns["User ID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    if (borrowerstable.Columns.Contains("User Name"))
                        borrowerstable.Columns["User Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    if (borrowerstable.Columns.Contains("User Email"))
                        borrowerstable.Columns["User Email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error while loading borrowers: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ClearInputFields(false);
            _isProgrammaticChange = false;
            this.borrowerstable.SelectionChanged += borrowerstable_SelectionChanged;

            if (borrowerstable.Rows.Count > 0)
            {
                borrowerstable.CurrentCell = borrowerstable.Rows[0].Cells[GetFirstVisibleColumnIndex(borrowerstable)];
                borrowerstable_SelectionChanged(borrowerstable, EventArgs.Empty);
            }
        }

        private void ClearInputFields(bool clearGridSelection = true)
        {
            _isProgrammaticChange = true;
            borrower_id.Clear();
            user_id.Clear();
            borrower_id.ReadOnly = true;

            if (clearGridSelection && borrowerstable.Rows.Count > 0)
            {
                borrowerstable.ClearSelection();
            }
            _isProgrammaticChange = false;
        }

        private void borrowerstable_SelectionChanged(object sender, EventArgs e)
        {
            _isProgrammaticChange = true;
            if (borrowerstable.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = borrowerstable.SelectedRows[0];
                borrower_id.Text = selectedRow.Cells["Borrower ID"].Value?.ToString() ?? "";
                user_id.Text = selectedRow.Cells["User ID"].Value?.ToString() ?? "";
            }
            else
            {
                ClearInputFields(false); // Don't clear selection if this event is due to it
            }
            _isProgrammaticChange = false;
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(user_id.Text, out int currentUserId) || currentUserId <= 0)
            {
                MessageBox.Show("Please enter a valid User ID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                user_id.Focus(); return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Check if User ID exists in the Users table
                    MySqlCommand checkUserCmd = new MySqlCommand("SELECT COUNT(*) FROM Users WHERE user_id = @UserID", conn);
                    checkUserCmd.Parameters.AddWithValue("@UserID", currentUserId);
                    if (Convert.ToInt32(checkUserCmd.ExecuteScalar()) == 0)
                    {
                        MessageBox.Show($"User ID {currentUserId} does not exist in the Users table.", "Invalid User ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Check if User ID is already a borrower
                    MySqlCommand checkBorrowerCmd = new MySqlCommand("SELECT COUNT(*) FROM Borrowers WHERE user_id = @UserID", conn);
                    checkBorrowerCmd.Parameters.AddWithValue("@UserID", currentUserId);
                    if (Convert.ToInt32(checkBorrowerCmd.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show($"User ID {currentUserId} is already registered as a borrower.", "Duplicate Borrower", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string query = "INSERT INTO Borrowers (user_id) VALUES (@UserID)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", currentUserId);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Borrower added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadBorrowersData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add borrower. No rows affected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void user_id_TextChanged(object sender, EventArgs e)
        {
            if (_isProgrammaticChange || borrowerstable.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(borrower_id.Text))
                return;

            if (int.TryParse(borrower_id.Text, out int currentBorrowerId))
            {
                // Only attempt to update if the user_id textbox contains a valid positive integer
                if (int.TryParse(user_id.Text, out int newUserId) && newUserId > 0)
                {
                    UpdateBorrowerUserIdInDatabase(currentBorrowerId, newUserId);
                }
                else if (!string.IsNullOrWhiteSpace(user_id.Text))
                {
                    // User might be typing, or has entered non-numeric/non-positive value.
                    // Validation will happen more strictly in UpdateBorrowerUserIdInDatabase if it gets called.
                    Debug.WriteLine("User ID TextChanged: User ID is not a valid positive integer yet for update.");
                }
            }
        }

        private void UpdateBorrowerUserIdInDatabase(int borrowerIdToUpdate, int newUserId)
        {
            // Store cursor position from the active textbox (user_id textbox)
            int originalCursorPosition = user_id.SelectionStart;
            int originalSelectionLength = user_id.SelectionLength;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // 1. Check if the new User ID exists in the Users table
                    MySqlCommand checkUserCmd = new MySqlCommand("SELECT COUNT(*) FROM Users WHERE user_id = @NewUserID", conn);
                    checkUserCmd.Parameters.AddWithValue("@NewUserID", newUserId);
                    if (Convert.ToInt32(checkUserCmd.ExecuteScalar()) == 0)
                    {
                        Debug.WriteLine($"UpdateBorrowerUserIdInDatabase: New User ID {newUserId} does not exist. Update skipped.");
                        MessageBox.Show($"User ID {newUserId} does not exist in the Users table. Please enter a valid User ID.", "Invalid User ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Revert textbox change by reloading data for the current selection
                        int tempCurrentRowIndex = -1;
                        if (borrowerstable.SelectedRows.Count > 0) tempCurrentRowIndex = borrowerstable.SelectedRows[0].Index;
                        LoadBorrowersData();
                        if (tempCurrentRowIndex != -1 && borrowerstable.Rows.Count > tempCurrentRowIndex)
                        {
                            _isProgrammaticChange = true;
                            borrowerstable.ClearSelection();
                            borrowerstable.Rows[tempCurrentRowIndex].Selected = true;
                            _isProgrammaticChange = false;
                            borrowerstable_SelectionChanged(borrowerstable, EventArgs.Empty); // Repopulate textboxes
                        }
                        return;
                    }

                    // 2. Check if the new User ID is already assigned to ANOTHER borrower
                    MySqlCommand checkBorrowerCmd = new MySqlCommand("SELECT COUNT(*) FROM Borrowers WHERE user_id = @NewUserID AND borrower_id != @CurrentBorrowerID", conn);
                    checkBorrowerCmd.Parameters.AddWithValue("@NewUserID", newUserId);
                    checkBorrowerCmd.Parameters.AddWithValue("@CurrentBorrowerID", borrowerIdToUpdate);
                    if (Convert.ToInt32(checkBorrowerCmd.ExecuteScalar()) > 0)
                    {
                        Debug.WriteLine($"UpdateBorrowerUserIdInDatabase: New User ID {newUserId} is already assigned to another borrower. Update skipped.");
                        MessageBox.Show($"User ID {newUserId} is already assigned to another borrower. Please choose a different User ID.", "User ID In Use", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        int tempCurrentRowIndex = -1;
                        if (borrowerstable.SelectedRows.Count > 0) tempCurrentRowIndex = borrowerstable.SelectedRows[0].Index;
                        LoadBorrowersData();
                        if (tempCurrentRowIndex != -1 && borrowerstable.Rows.Count > tempCurrentRowIndex)
                        {
                            _isProgrammaticChange = true;
                            borrowerstable.ClearSelection();
                            borrowerstable.Rows[tempCurrentRowIndex].Selected = true;
                            _isProgrammaticChange = false;
                            borrowerstable_SelectionChanged(borrowerstable, EventArgs.Empty);
                        }
                        return;
                    }

                    string query = "UPDATE Borrowers SET user_id = @NewUserID WHERE borrower_id = @BorrowerID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NewUserID", newUserId);
                    cmd.Parameters.AddWithValue("@BorrowerID", borrowerIdToUpdate);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Debug.WriteLine($"SUCCESS: Borrower ID {borrowerIdToUpdate} User ID updated to {newUserId}.");
                        int currentRowIndex = -1;
                        if (borrowerstable.SelectedRows.Count > 0) currentRowIndex = borrowerstable.SelectedRows[0].Index;

                        LoadBorrowersData();

                        if (currentRowIndex != -1 && borrowerstable.Rows.Count > currentRowIndex)
                        {
                            _isProgrammaticChange = true;
                            borrowerstable.ClearSelection();
                            borrowerstable.Rows[currentRowIndex].Selected = true;
                            borrowerstable.CurrentCell = borrowerstable.Rows[currentRowIndex].Cells[GetFirstVisibleColumnIndex(borrowerstable)];
                            if (!borrowerstable.Rows[currentRowIndex].Displayed)
                                borrowerstable.FirstDisplayedScrollingRowIndex = currentRowIndex;
                            _isProgrammaticChange = false;
                        }
                        user_id.Focus();
                        if (originalCursorPosition <= user_id.Text.Length) user_id.SelectionStart = originalCursorPosition;
                        else user_id.SelectionStart = user_id.Text.Length;
                        user_id.SelectionLength = originalSelectionLength;
                    }
                    else
                    {
                        Debug.WriteLine($"INFO: Update for Borrower ID {borrowerIdToUpdate} did not affect any rows. User ID might be unchanged.");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error while updating borrower: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadBorrowersData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadBorrowersData();
                }
            }
        }

        private void edit_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Fields cleared. Edits to User ID are saved automatically as you type for a selected borrower.");
            ClearInputFields(true);
            user_id.Focus();
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            if (borrowerstable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a borrower to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(borrower_id.Text, out int borrowerIdToDelete))
            {
                MessageBox.Show("No valid borrower selected for deletion (Borrower ID is missing).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirmResult = MessageBox.Show($"Are you sure you want to delete Borrower ID: {borrowerIdToDelete}?\nThis will also delete all associated borrow records (due to ON DELETE CASCADE in DB).", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Borrowers WHERE borrower_id = @BorrowerID";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@BorrowerID", borrowerIdToDelete);
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Borrower deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadBorrowersData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete borrower. Record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Database error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An unexpected error occurred: " + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void search_button_Click(object sender, EventArgs e)
        {
            string searchTerm = searchbox.Text.Trim();
            this.borrowerstable.SelectionChanged -= borrowerstable_SelectionChanged;
            _isProgrammaticChange = true;

            DataTable dataTable = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string queryBase = @"
                        SELECT 
                            br.borrower_id AS 'Borrower ID', 
                            br.user_id AS 'User ID', 
                            u.name AS 'User Name', 
                            u.email AS 'User Email'
                        FROM Borrowers br
                        JOIN Users u ON br.user_id = u.user_id ";
                    string queryCondition = "";
                    MySqlCommand cmd = new MySqlCommand { Connection = conn };

                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        queryCondition = @"WHERE u.name LIKE @SearchTerm 
                                           OR u.email LIKE @SearchTerm
                                           OR br.borrower_id LIKE @SearchTermNumericEquivalent
                                           OR br.user_id LIKE @SearchTermNumericEquivalent ";
                        cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                        cmd.Parameters.AddWithValue("@SearchTermNumericEquivalent", searchTerm);
                    }
                    cmd.CommandText = queryBase + queryCondition + " ORDER BY br.borrower_id ASC;";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                    borrowerstable.DataSource = dataTable;

                    if (dataTable.Rows.Count == 0 && !string.IsNullOrWhiteSpace(searchTerm))
                    {
                        MessageBox.Show("No borrowers found matching your search.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (MySqlException ex) { MessageBox.Show("Database error during search: " + ex.Message); }
                catch (Exception ex) { MessageBox.Show("Unexpected error during search: " + ex.Message); }
            }
            ClearInputFields(true);
            _isProgrammaticChange = false;
            this.borrowerstable.SelectionChanged += borrowerstable_SelectionChanged;
            if (borrowerstable.Rows.Count > 0)
            {
                borrowerstable.CurrentCell = borrowerstable.Rows[0].Cells[GetFirstVisibleColumnIndex(borrowerstable)];
                borrowerstable_SelectionChanged(borrowerstable, EventArgs.Empty);
            }
        }

        private void save2Excel_button_Click(object sender, EventArgs e)
        {
            if (borrowerstable.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true, FileName = "BorrowersExport.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            DataTable dt = (DataTable)borrowerstable.DataSource;
                            var worksheet = workbook.Worksheets.Add(dt, "Borrowers");
                            worksheet.Columns().AdjustToContents();
                            workbook.SaveAs(sfd.FileName);
                        }
                        MessageBox.Show("Data exported successfully to Excel!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error exporting data to Excel: " + ex.Message); }
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
            MessageBox.Show("Users button clicked - Navigating to Users page.");
            try
            {
                this.Hide();
                using (users users_form = new users())
                {
                    users_form.ShowDialog(this);
                }
                this.Show();
                this.Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Users form: {ex.ToString()}", "Navigation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
                this.Activate();
            }
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
            MessageBox.Show("Borrowers button clicked - Refreshing data.");
            LoadBorrowersData();
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
                            // Application.Exit(); 
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

        // Placeholder event handlers from your borrowers.Designer.cs.
        // If they are not actually used for specific logic, they can be removed
        // from borrowers.cs, and the designer wiring should also be removed to avoid confusion.
        private void label3_Click(object sender, EventArgs e)
        {
            // Likely the "borrower_id" label
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Likely the "user_id" label
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            // Panel paint event, usually not needed for custom logic unless doing custom drawing.
        }
    }
}
