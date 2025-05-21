using System;
using System.Data;
using System.Diagnostics; // For Debug.WriteLine
using System.Drawing;
using System.Linq; // Required for OfType<T>()
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Required for MySQL connection
using ClosedXML.Excel;       // Required for Excel export
using System.Globalization; // Required for DateTime parsing

namespace librarydb
{
    public partial class borrowedbooks : Form
    {
        string connectionString = "server=localhost;database=LibraryDB;uid=root;pwd=sanantonio6;";
        private bool _isProgrammaticChange = false; // Still useful for selection changes

        public borrowedbooks()
        {
            InitializeComponent();
            LoadBorrowedBooksData();
            borrow_id.ReadOnly = true; // Borrow ID is auto-generated and not directly editable
            SetInputFieldsReadOnly(true); // Initially, fields are read-only
        }

        private void SetInputFieldsReadOnly(bool isReadOnly)
        {
            book_id.ReadOnly = isReadOnly;
            borrower_id.ReadOnly = isReadOnly;
            borrow_date.ReadOnly = isReadOnly;
            return_date.ReadOnly = isReadOnly;
        }

        private int GetFirstVisibleColumnIndex(DataGridView dgv)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible) return col.Index;
            }
            return 0; // Fallback
        }

        private void LoadBorrowedBooksData()
        {
            this.b_bookstable.SelectionChanged -= b_bookstable_SelectionChanged;
            _isProgrammaticChange = true;

            DataTable dataTable = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            bb.borrow_id AS 'Borrow ID', 
                            bb.book_id AS 'BookID_Hidden', 
                            b.title AS 'Book Title', 
                            bb.borrower_id AS 'BorrowerID_Hidden', 
                            u.name AS 'Borrower Name', 
                            DATE_FORMAT(bb.borrow_date, '%Y-%m-%d') AS 'Borrow Date', 
                            DATE_FORMAT(bb.return_date, '%Y-%m-%d') AS 'Return Date'
                        FROM Borrowed_Books bb
                        JOIN Books b ON bb.book_id = b.book_id
                        JOIN Borrowers bor ON bb.borrower_id = bor.borrower_id
                        JOIN Users u ON bor.user_id = u.user_id
                        ORDER BY bb.borrow_id ASC;";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                    b_bookstable.DataSource = dataTable;

                    if (b_bookstable.Columns.Contains("BookID_Hidden"))
                        b_bookstable.Columns["BookID_Hidden"].Visible = false;
                    if (b_bookstable.Columns.Contains("BorrowerID_Hidden"))
                        b_bookstable.Columns["BorrowerID_Hidden"].Visible = false;

                    b_bookstable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    if (b_bookstable.Columns.Contains("Borrow ID"))
                        b_bookstable.Columns["Borrow ID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    if (b_bookstable.Columns.Contains("Book Title"))
                        b_bookstable.Columns["Book Title"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    if (b_bookstable.Columns.Contains("Borrower Name"))
                        b_bookstable.Columns["Borrower Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error while loading borrowed books: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ClearInputFields(false); // Clear input fields but don't clear grid selection
            SetInputFieldsReadOnly(true); // After loading, if no row is selected, fields are for display/new add
            _isProgrammaticChange = false;
            this.b_bookstable.SelectionChanged += b_bookstable_SelectionChanged;

            if (b_bookstable.Rows.Count > 0)
            {
                b_bookstable.CurrentCell = b_bookstable.Rows[0].Cells[GetFirstVisibleColumnIndex(b_bookstable)];
                b_bookstable_SelectionChanged(b_bookstable, EventArgs.Empty); // Manually trigger to populate
            }
        }

        private void ClearInputFields(bool clearGridSelection = true)
        {
            _isProgrammaticChange = true;
            borrow_id.Clear();
            book_id.Clear();
            borrower_id.Clear();
            borrow_date.Clear();
            return_date.Clear();
            borrow_id.ReadOnly = true; // Always read-only

            // When clearing for a new entry (typically after edit_button_Click), fields become writable.
            // If just clearing due to deselection, they might remain read-only until edit_button is clicked.
            // This is handled by SetInputFieldsReadOnly in Load and SelectionChanged.

            if (clearGridSelection && b_bookstable.Rows.Count > 0)
            {
                b_bookstable.ClearSelection();
            }
            _isProgrammaticChange = false;
        }

        private void b_bookstable_SelectionChanged(object sender, EventArgs e)
        {
            _isProgrammaticChange = true;
            if (b_bookstable.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = b_bookstable.SelectedRows[0];
                borrow_id.Text = selectedRow.Cells["Borrow ID"].Value?.ToString() ?? "";
                book_id.Text = selectedRow.Cells["BookID_Hidden"].Value?.ToString() ?? "";
                borrower_id.Text = selectedRow.Cells["BorrowerID_Hidden"].Value?.ToString() ?? "";
                borrow_date.Text = selectedRow.Cells["Borrow Date"].Value?.ToString() ?? "";
                return_date.Text = selectedRow.Cells["Return Date"].Value?.ToString() ?? "";
                SetInputFieldsReadOnly(true); // Make fields read-only when displaying existing record
            }
            else
            {
                ClearInputFields(false);
                SetInputFieldsReadOnly(true); // If no selection, keep read-only until "Edit" (clear) or "Add"
            }
            _isProgrammaticChange = false;
        }

        private bool IsBookAvailable(int bookIdToCheck, MySqlConnection conn, int? currentBorrowIdToExclude = null)
        {
            string availabilityQuery = "SELECT COUNT(*) FROM Borrowed_Books WHERE book_id = @BookID AND return_date IS NULL";
            if (currentBorrowIdToExclude.HasValue)
            {
                availabilityQuery += " AND borrow_id != @CurrentBorrowID";
            }
            MySqlCommand availabilityCmd = new MySqlCommand(availabilityQuery, conn);
            availabilityCmd.Parameters.AddWithValue("@BookID", bookIdToCheck);
            if (currentBorrowIdToExclude.HasValue)
            {
                availabilityCmd.Parameters.AddWithValue("@CurrentBorrowID", currentBorrowIdToExclude.Value);
            }
            return Convert.ToInt32(availabilityCmd.ExecuteScalar()) == 0;
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            // Ensure fields are writable for adding
            SetInputFieldsReadOnly(false); // Allow input for new record

            if (!int.TryParse(book_id.Text, out int currentBookId) || currentBookId <= 0)
            {
                MessageBox.Show("Please enter a valid Book ID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                book_id.Focus(); SetInputFieldsReadOnly(false); return; // Keep editable
            }
            if (!int.TryParse(borrower_id.Text, out int currentBorrowerId) || currentBorrowerId <= 0)
            {
                MessageBox.Show("Please enter a valid Borrower ID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                borrower_id.Focus(); SetInputFieldsReadOnly(false); return;
            }
            if (!DateTime.TryParseExact(borrow_date.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime currentBorrowDate))
            {
                MessageBox.Show("Borrow Date is required and must be in YYYY-MM-DD format.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                borrow_date.Focus(); SetInputFieldsReadOnly(false); return;
            }

            DateTime? currentReturnDate = null;
            if (!string.IsNullOrWhiteSpace(return_date.Text))
            {
                if (!DateTime.TryParseExact(return_date.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedReturnDate))
                {
                    MessageBox.Show("Return Date, if provided, must be in YYYY-MM-DD format.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return_date.Focus(); SetInputFieldsReadOnly(false); return;
                }
                if (parsedReturnDate < currentBorrowDate)
                {
                    MessageBox.Show("Return Date cannot be earlier than Borrow Date.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return_date.Focus(); SetInputFieldsReadOnly(false); return;
                }
                currentReturnDate = parsedReturnDate;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    MySqlCommand checkCmd = new MySqlCommand("SELECT COUNT(*) FROM Books WHERE book_id = @ID", conn);
                    checkCmd.Parameters.AddWithValue("@ID", currentBookId);
                    if (Convert.ToInt32(checkCmd.ExecuteScalar()) == 0)
                    {
                        MessageBox.Show($"Book ID {currentBookId} does not exist.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        SetInputFieldsReadOnly(false); return;
                    }

                    checkCmd.CommandText = "SELECT COUNT(*) FROM Borrowers WHERE borrower_id = @ID";
                    checkCmd.Parameters.Clear();
                    checkCmd.Parameters.AddWithValue("@ID", currentBorrowerId);
                    if (Convert.ToInt32(checkCmd.ExecuteScalar()) == 0)
                    {
                        MessageBox.Show($"Borrower ID {currentBorrowerId} does not exist.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        SetInputFieldsReadOnly(false); return;
                    }

                    if (!IsBookAvailable(currentBookId, conn))
                    {
                        MessageBox.Show($"Book ID {currentBookId} is currently borrowed and not yet returned.", "Book Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        SetInputFieldsReadOnly(false); return;
                    }

                    string query = "INSERT INTO Borrowed_Books (book_id, borrower_id, borrow_date, return_date) VALUES (@BookID, @BorrowerID, @BorrowDate, @ReturnDate)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@BookID", currentBookId);
                    cmd.Parameters.AddWithValue("@BorrowerID", currentBorrowerId);
                    cmd.Parameters.AddWithValue("@BorrowDate", currentBorrowDate.ToString("yyyy-MM-dd"));
                    if (currentReturnDate.HasValue)
                        cmd.Parameters.AddWithValue("@ReturnDate", currentReturnDate.Value.ToString("yyyy-MM-dd"));
                    else
                        cmd.Parameters.AddWithValue("@ReturnDate", DBNull.Value);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Borrowed book record added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadBorrowedBooksData(); // This will also call ClearInputFields and set fields to read-only
                    }
                    else
                    {
                        MessageBox.Show("Failed to add borrowed book record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        SetInputFieldsReadOnly(false); // Keep editable on failure
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetInputFieldsReadOnly(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetInputFieldsReadOnly(false);
                }
            }
        }

        // Edit button will now primarily clear fields to prepare for an Add operation or reset view.
        // Direct editing of existing records via TextChanged is disabled for Borrowed_Books.
        private void edit_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Fields cleared for new entry. To modify an existing record (e.g., mark as returned), delete the old record and add a new one, or use a dedicated 'Return Book' feature (not implemented here).", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearInputFields(true);
            SetInputFieldsReadOnly(false); // Make fields writable for new entry
            book_id.Focus();
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            if (b_bookstable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a borrow record to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(borrow_id.Text, out int borrowIdToDelete))
            {
                MessageBox.Show("No valid borrow record selected for deletion (Borrow ID is missing).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirmResult = MessageBox.Show($"Are you sure you want to delete borrow record ID: {borrowIdToDelete}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Borrowed_Books WHERE borrow_id = @BorrowID";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@BorrowID", borrowIdToDelete);
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Borrow record deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadBorrowedBooksData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete borrow record. Record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            this.b_bookstable.SelectionChanged -= b_bookstable_SelectionChanged;
            _isProgrammaticChange = true;

            DataTable dataTable = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string queryBase = @"
                        SELECT 
                            bb.borrow_id AS 'Borrow ID', 
                            bb.book_id AS 'BookID_Hidden', 
                            b.title AS 'Book Title', 
                            bb.borrower_id AS 'BorrowerID_Hidden', 
                            u.name AS 'Borrower Name', 
                            DATE_FORMAT(bb.borrow_date, '%Y-%m-%d') AS 'Borrow Date', 
                            DATE_FORMAT(bb.return_date, '%Y-%m-%d') AS 'Return Date'
                        FROM Borrowed_Books bb
                        JOIN Books b ON bb.book_id = b.book_id
                        JOIN Borrowers bor ON bb.borrower_id = bor.borrower_id
                        JOIN Users u ON bor.user_id = u.user_id ";
                    string queryCondition = "";
                    MySqlCommand cmd = new MySqlCommand { Connection = conn };

                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        queryCondition = @"WHERE b.title LIKE @SearchTerm 
                                           OR u.name LIKE @SearchTerm 
                                           OR bb.borrow_id LIKE @SearchTermNumericEquivalent
                                           OR bb.book_id LIKE @SearchTermNumericEquivalent
                                           OR bb.borrower_id LIKE @SearchTermNumericEquivalent
                                           OR DATE_FORMAT(bb.borrow_date, '%Y-%m-%d') LIKE @SearchTerm
                                           OR DATE_FORMAT(bb.return_date, '%Y-%m-%d') LIKE @SearchTerm ";
                        cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                        cmd.Parameters.AddWithValue("@SearchTermNumericEquivalent", searchTerm);
                    }
                    cmd.CommandText = queryBase + queryCondition + " ORDER BY bb.borrow_id ASC;";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                    b_bookstable.DataSource = dataTable;

                    if (b_bookstable.Columns.Contains("BookID_Hidden")) b_bookstable.Columns["BookID_Hidden"].Visible = false;
                    if (b_bookstable.Columns.Contains("BorrowerID_Hidden")) b_bookstable.Columns["BorrowerID_Hidden"].Visible = false;

                    if (dataTable.Rows.Count == 0 && !string.IsNullOrWhiteSpace(searchTerm))
                    {
                        MessageBox.Show("No borrowed book records found matching your search.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (MySqlException ex) { MessageBox.Show("Database error during search: " + ex.Message); }
                catch (Exception ex) { MessageBox.Show("Unexpected error during search: " + ex.Message); }
            }
            ClearInputFields(true);
            SetInputFieldsReadOnly(true);
            _isProgrammaticChange = false;
            this.b_bookstable.SelectionChanged += b_bookstable_SelectionChanged;
            if (b_bookstable.Rows.Count > 0)
            {
                b_bookstable.CurrentCell = b_bookstable.Rows[0].Cells[GetFirstVisibleColumnIndex(b_bookstable)];
                b_bookstable_SelectionChanged(b_bookstable, EventArgs.Empty);
            }
        }

        private void save2Excel_button_Click(object sender, EventArgs e)
        {
            if (b_bookstable.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true, FileName = "BorrowedBooksExport.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            DataTable dtSource = (DataTable)b_bookstable.DataSource;
                            DataTable dtExport = new DataTable("BorrowedBooks");
                            dtExport.Columns.Add("Borrow ID", typeof(int));
                            dtExport.Columns.Add("Book Title", typeof(string));
                            dtExport.Columns.Add("Borrower Name", typeof(string));
                            dtExport.Columns.Add("Borrow Date", typeof(string));
                            dtExport.Columns.Add("Return Date", typeof(string));
                            dtExport.Columns.Add("Book ID", typeof(int));
                            dtExport.Columns.Add("Borrower ID", typeof(int));

                            foreach (DataRow row in dtSource.Rows)
                            {
                                dtExport.Rows.Add(
                                    row["Borrow ID"],
                                    row["Book Title"],
                                    row["Borrower Name"],
                                    row["Borrow Date"],
                                    row["Return Date"],
                                    row["BookID_Hidden"],
                                    row["BorrowerID_Hidden"]
                                );
                            }
                            var worksheet = workbook.Worksheets.Add(dtExport);
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
            MessageBox.Show("Borrowed Books button clicked - Refreshing data.");
            LoadBorrowedBooksData();
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

        // Placeholder event handlers from your borrowedbooks.Designer.cs.
        // Remove if not used and also unwire from designer.
        private void label1_Click(object sender, EventArgs e)
        {
            // Likely "Library Management" label
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Likely "Borrowed Books" title label
        }
    }
}
