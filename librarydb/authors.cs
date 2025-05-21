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
    public partial class authors : Form
    {
        // IMPORTANT: Ensure this connection string is correct for your MySQL setup.
        // It should be the same as in your home.cs and login.cs
        string connectionString = "server=localhost;database=LibraryDB;uid=root;pwd=sanantonio6;";

        // Flag to prevent TextChanged events from firing updates when textboxes
        // are being populated programmatically.
        private bool _isProgrammaticChange = false;

        public authors()
        {
            InitializeComponent();
            LoadAuthorsData();
            authorID.ReadOnly = true; // Author ID is auto-generated and not directly editable
        }

        private int GetFirstVisibleColumnIndex(DataGridView dgv)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible) return col.Index;
            }
            return 0; // Fallback
        }

        private void LoadAuthorsData()
        {
            this.authorstable.SelectionChanged -= authorstable_SelectionChanged; // Detach to prevent issues during reload
            _isProgrammaticChange = true;

            DataTable dataTable = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT author_id AS 'Author ID', author_name AS 'Author Name' FROM Authors ORDER BY author_id ASC;";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                    authorstable.DataSource = dataTable;

                    authorstable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    if (authorstable.Columns.Contains("Author ID"))
                    {
                        authorstable.Columns["Author ID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error while loading authors: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred while loading authors: " + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ClearInputFields(false); // Clear input fields but don't clear grid selection
            _isProgrammaticChange = false;
            this.authorstable.SelectionChanged += authorstable_SelectionChanged; // Re-attach

            if (authorstable.Rows.Count > 0)
            {
                authorstable.CurrentCell = authorstable.Rows[0].Cells[GetFirstVisibleColumnIndex(authorstable)];
                authorstable_SelectionChanged(authorstable, EventArgs.Empty); // Manually trigger to populate
            }
        }

        private void ClearInputFields(bool clearGridSelection = true)
        {
            _isProgrammaticChange = true;
            authorID.Clear();
            authorName.Clear();
            authorID.ReadOnly = true; // Author ID is always read-only

            if (clearGridSelection && authorstable.Rows.Count > 0)
            {
                authorstable.ClearSelection();
            }
            _isProgrammaticChange = false;
        }

        private void authorstable_SelectionChanged(object sender, EventArgs e)
        {
            _isProgrammaticChange = true;
            if (authorstable.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = authorstable.SelectedRows[0];
                authorID.Text = selectedRow.Cells["Author ID"].Value != DBNull.Value ? selectedRow.Cells["Author ID"].Value.ToString() : "";
                authorName.Text = selectedRow.Cells["Author Name"].Value != DBNull.Value ? selectedRow.Cells["Author Name"].Value.ToString() : "";
            }
            else
            {
                authorID.Clear();
                authorName.Clear();
            }
            _isProgrammaticChange = false;
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            string currentAuthorName = authorName.Text.Trim();

            if (string.IsNullOrWhiteSpace(currentAuthorName))
            {
                MessageBox.Show("Author Name cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                authorName.Focus();
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Check if author name already exists 
                    string checkQuery = "SELECT COUNT(*) FROM Authors WHERE author_name = @AuthorName";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@AuthorName", currentAuthorName);
                    if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show($"An author with the name '{currentAuthorName}' already exists.", "Duplicate Author", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string query = "INSERT INTO Authors (author_name) VALUES (@AuthorName)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@AuthorName", currentAuthorName);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Author added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAuthorsData(); // Refresh
                    }
                    else
                    {
                        MessageBox.Show("Failed to add author.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error while adding author: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Event handler for authorName TextBox TextChanged
        private void authorName_TextChanged(object sender, EventArgs e)
        {
            if (_isProgrammaticChange || authorstable.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(authorID.Text))
                return;

            if (int.TryParse(authorID.Text, out int currentAuthorId))
            {
                // Call UpdateAuthorName method when text changes
                UpdateAuthorNameInDatabase(currentAuthorId, authorName.Text.Trim());
            }
        }

        // Method to update author's name in the database
        private void UpdateAuthorNameInDatabase(int authorIdToUpdate, string newAuthorName)
        {
            if (string.IsNullOrWhiteSpace(newAuthorName))
            {
                Debug.WriteLine("UpdateAuthorNameInDatabase: Author Name cannot be empty. Update skipped.");
                // Optionally, you could provide non-blocking feedback to the user here (e.g., change textbox border color)
                // For now, we just don't proceed with the update.
                return;
            }

            // Optional: Check if the new name would create a duplicate (excluding the current author)
            // This can be complex for on-text-changed due to rapid firing.
            // A simpler approach is to rely on database unique constraints if `author_name` is unique.
            // If `author_name` is NOT unique in DB, this check might be desired.
            // For now, we'll proceed without this specific duplicate check during on-change edit for simplicity.
            // The ADD button already checks for duplicates.

            int originalCursorPosition = authorName.SelectionStart;
            int originalSelectionLength = authorName.SelectionLength;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Authors SET author_name = @NewAuthorName WHERE author_id = @AuthorID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NewAuthorName", newAuthorName);
                    cmd.Parameters.AddWithValue("@AuthorID", authorIdToUpdate);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Debug.WriteLine($"SUCCESS: Author ID {authorIdToUpdate} name updated to '{newAuthorName}'.");

                        int currentRowIndex = -1;
                        if (authorstable.SelectedRows.Count > 0)
                            currentRowIndex = authorstable.SelectedRows[0].Index;

                        // Store current cell to try and restore focus later
                        DataGridViewCell currentDgvCellBeforeLoad = authorstable.CurrentCell;

                        LoadAuthorsData(); // Refresh the grid

                        // Attempt to re-select the row and restore cell focus
                        if (currentRowIndex != -1 && authorstable.Rows.Count > currentRowIndex)
                        {
                            _isProgrammaticChange = true;
                            authorstable.ClearSelection();
                            authorstable.Rows[currentRowIndex].Selected = true;

                            // Try to restore the DataGridView's current cell if it was on the updated row
                            if (currentDgvCellBeforeLoad != null && currentDgvCellBeforeLoad.RowIndex == currentRowIndex &&
                                currentDgvCellBeforeLoad.ColumnIndex >= 0 && currentDgvCellBeforeLoad.ColumnIndex < authorstable.ColumnCount &&
                                authorstable.Columns[currentDgvCellBeforeLoad.ColumnIndex].Visible)
                            {
                                authorstable.CurrentCell = authorstable.Rows[currentRowIndex].Cells[currentDgvCellBeforeLoad.ColumnIndex];
                            }
                            else if (authorstable.Rows[currentRowIndex].Cells.Count > 0) // Fallback
                            {
                                authorstable.CurrentCell = authorstable.Rows[currentRowIndex].Cells[GetFirstVisibleColumnIndex(authorstable)];
                            }

                            if (!authorstable.Rows[currentRowIndex].Displayed) // Ensure row is visible
                                authorstable.FirstDisplayedScrollingRowIndex = currentRowIndex;
                            _isProgrammaticChange = false;
                        }

                        // Restore focus and cursor position to the authorName textbox
                        authorName.Focus();
                        if (originalCursorPosition <= authorName.Text.Length)
                            authorName.SelectionStart = originalCursorPosition;
                        else
                            authorName.SelectionStart = authorName.Text.Length; // Fallback to end
                        authorName.SelectionLength = originalSelectionLength;
                    }
                    else
                    {
                        // This might happen if the new name is the same as the old name.
                        Debug.WriteLine($"INFO: Update for Author ID {authorIdToUpdate} did not affect any rows. Name might be unchanged.");
                    }
                }
                catch (MySqlException ex)
                {
                    // Handle potential duplicate entry error if author_name has a UNIQUE constraint in DB
                    if (ex.Number == 1062) // MySQL error code for duplicate entry
                    {
                        MessageBox.Show($"Error: An author with the name '{newAuthorName}' already exists.", "Duplicate Author Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Database error while updating author: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadAuthorsData(); // Revert on error by reloading original data
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadAuthorsData(); // Revert
                }
            }
        }

        private void edit_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Edit button clicked - Clearing fields. Edits are saved automatically as you type in 'Author Name'.");
            ClearInputFields(true);
            authorName.Focus();
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            if (authorstable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an author to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(authorID.Text, out int authorIdToDelete))
            {
                MessageBox.Show("No valid author selected for deletion.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string currentAuthorName = authorName.Text;
            DialogResult confirmResult = MessageBox.Show($"Are you sure you want to delete '{currentAuthorName}' (ID: {authorIdToDelete})?\nNote: Books by this author will have their author field set to NULL (due to ON DELETE SET NULL in DB).", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Authors WHERE author_id = @AuthorID";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@AuthorID", authorIdToDelete);
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Author deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadAuthorsData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete author. Author not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Database error while deleting author: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            this.authorstable.SelectionChanged -= authorstable_SelectionChanged;
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
                        query = "SELECT author_id AS 'Author ID', author_name AS 'Author Name' FROM Authors ORDER BY author_id ASC;";
                    }
                    else
                    {
                        query = "SELECT author_id AS 'Author ID', author_name AS 'Author Name' FROM Authors " +
                                "WHERE author_name LIKE @SearchTerm OR author_id LIKE @SearchTermNumericEquivalent ORDER BY author_id ASC;";
                        cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                        cmd.Parameters.AddWithValue("@SearchTermNumericEquivalent", searchTerm);
                    }
                    cmd.CommandText = query;
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                    authorstable.DataSource = dataTable;

                    if (dataTable.Rows.Count == 0 && !string.IsNullOrWhiteSpace(searchTerm))
                    {
                        MessageBox.Show("No authors found matching your search.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            this.authorstable.SelectionChanged += authorstable_SelectionChanged;
            if (authorstable.Rows.Count > 0)
            {
                authorstable.CurrentCell = authorstable.Rows[0].Cells[GetFirstVisibleColumnIndex(authorstable)];
                authorstable_SelectionChanged(authorstable, EventArgs.Empty);
            }
        }

        private void save2Excel_button_Click(object sender, EventArgs e)
        {
            if (authorstable.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true, FileName = "AuthorsExport.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            DataTable dt = (DataTable)authorstable.DataSource;
                            var worksheet = workbook.Worksheets.Add(dt, "Authors");
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
            MessageBox.Show("Authors button clicked - Refreshing authors data.");
            LoadAuthorsData();
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

        // Placeholder event handlers from your authors.Designer.cs.
        // If they are not actually used for specific logic, they can be removed
        // from authors.cs, and the designer wiring should also be removed to avoid confusion.
        private void label1_Click(object sender, EventArgs e)
        {
            // This is likely the "Library Management" label in the side panel.
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // This is likely the "Authors" title label in panel2.
        }
    }
}
