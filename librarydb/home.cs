using System;
using System.Data;
using System.Diagnostics; // For Debug.WriteLine
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Required for MySQL connection
using ClosedXML.Excel;       // Required for Excel export

namespace librarydb
{
    public partial class home : Form
    {
        // IMPORTANT: Ensure this connection string is correct for your MySQL setup.
        string connectionString = "server=localhost;database=LibraryDB;uid=root;pwd=sanantonio6;";

        // Flag to prevent TextChanged events from firing updates when textboxes
        // are being populated programmatically.
        private bool _isProgrammaticChange = false;

        public home()
        {
            InitializeComponent();

            // Wire up KeyPress event for integer-only textboxes
            this.textBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IntegerOnly_KeyPress);
            this.textBox4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IntegerOnly_KeyPress);

            LoadBooksData();
            textBox1.ReadOnly = true;
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
            return 0; // Fallback
        }

        private void LoadBooksData()
        {
            this.bookstable.SelectionChanged -= bookstable_SelectionChanged;
            _isProgrammaticChange = true;

            DataTable dataTable = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT
                            b.book_id AS 'Book ID', b.title AS 'Title',
                            a.author_name AS 'Author', c.category_name AS 'Category',
                            b.category_id AS 'CategoryID_Hidden', b.author_id AS 'AuthorID_Hidden'
                        FROM Books b
                        LEFT JOIN Authors a ON b.author_id = a.author_id
                        LEFT JOIN Categories c ON b.category_id = c.category_id
                        ORDER BY b.book_id ASC;";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                    bookstable.DataSource = dataTable;

                    if (bookstable.Columns.Contains("CategoryID_Hidden"))
                        bookstable.Columns["CategoryID_Hidden"].Visible = false;
                    if (bookstable.Columns.Contains("AuthorID_Hidden"))
                        bookstable.Columns["AuthorID_Hidden"].Visible = false;
                    bookstable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error while loading books: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred while loading books: " + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ClearInputFields(false);
            _isProgrammaticChange = false;
            this.bookstable.SelectionChanged += bookstable_SelectionChanged;

            if (bookstable.Rows.Count > 0)
            {
                bookstable.CurrentCell = bookstable.Rows[0].Cells[GetFirstVisibleColumnIndex(bookstable)];
                bookstable_SelectionChanged(bookstable, EventArgs.Empty);
            }
        }

        private void ClearInputFields(bool clearGridSelection = true)
        {
            _isProgrammaticChange = true;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox4.ReadOnly = false;
            if (clearGridSelection && bookstable.Rows.Count > 0)
            {
                bookstable.ClearSelection();
            }
            _isProgrammaticChange = false;
        }

        private void bookstable_SelectionChanged(object sender, EventArgs e)
        {
            _isProgrammaticChange = true;
            if (bookstable.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = bookstable.SelectedRows[0];
                textBox1.Text = selectedRow.Cells["Book ID"].Value != DBNull.Value ? selectedRow.Cells["Book ID"].Value.ToString() : "";
                textBox2.Text = selectedRow.Cells["Title"].Value != DBNull.Value ? selectedRow.Cells["Title"].Value.ToString() : "";
                textBox3.Text = selectedRow.Cells["CategoryID_Hidden"].Value != DBNull.Value ? selectedRow.Cells["CategoryID_Hidden"].Value.ToString() : "";
                textBox4.Text = selectedRow.Cells["AuthorID_Hidden"].Value != DBNull.Value ? selectedRow.Cells["AuthorID_Hidden"].Value.ToString() : "";
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = false;
                textBox3.ReadOnly = false;
                textBox4.ReadOnly = false;
            }
            else
            {
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox1.ReadOnly = true;
            }
            _isProgrammaticChange = false;
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            string title = textBox2.Text;
            string categoryIdText = textBox3.Text;
            string authorIdText = textBox4.Text;

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(categoryIdText) || string.IsNullOrWhiteSpace(authorIdText))
            {
                MessageBox.Show("Title, Category ID, and Author ID are required.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(categoryIdText, out int categoryId) || categoryId <= 0)
            {
                MessageBox.Show($"Category ID '{categoryIdText}' must be a valid positive number.", "Input Error - Category ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(authorIdText, out int authorId) || authorId <= 0)
            {
                MessageBox.Show($"Author ID '{authorIdText}' must be a valid positive number.", "Input Error - Author ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string categoryCheckQuery = "SELECT COUNT(*) FROM Categories WHERE category_id = @categoryIdParam";
                    MySqlCommand categoryCmd = new MySqlCommand(categoryCheckQuery, conn);
                    categoryCmd.Parameters.AddWithValue("@categoryIdParam", categoryId);
                    if (Convert.ToInt32(categoryCmd.ExecuteScalar()) == 0)
                    {
                        MessageBox.Show($"Category ID {categoryId} does not exist.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close(); return;
                    }
                    string authorCheckQuery = "SELECT COUNT(*) FROM Authors WHERE author_id = @authorIdParam";
                    MySqlCommand authorCmd = new MySqlCommand(authorCheckQuery, conn);
                    authorCmd.Parameters.AddWithValue("@authorIdParam", authorId);
                    if (Convert.ToInt32(authorCmd.ExecuteScalar()) == 0)
                    {
                        MessageBox.Show($"Author ID {authorId} does not exist.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close(); return;
                    }
                    string query = "INSERT INTO Books (title, category_id, author_id) VALUES (@titleParam, @categoryIdParam, @authorIdParam)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@titleParam", title);
                    cmd.Parameters.AddWithValue("@categoryIdParam", categoryId);
                    cmd.Parameters.AddWithValue("@authorIdParam", authorId);
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Book added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadBooksData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add book.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    if (ex.Message.Contains("Duplicate book entry detected!"))
                        MessageBox.Show("Database error: This book might already exist.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show($"DATABASE ERROR during Add Book: {ex.Message}", "MySqlException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"UNEXPECTED ERROR during Add Book: {ex.ToString()}", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateBookField(string fieldName, object value, int bookId)
        {
            if (fieldName.Equals("title", StringComparison.OrdinalIgnoreCase) && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
            {
                Debug.WriteLine("UpdateBookField: Title cannot be empty. Update skipped.");
                return;
            }
            if (fieldName.Equals("category_id", StringComparison.OrdinalIgnoreCase) || fieldName.Equals("author_id", StringComparison.OrdinalIgnoreCase))
            {
                if (value == null || !int.TryParse(value.ToString(), out int idVal) || idVal <= 0)
                {
                    Debug.WriteLine($"UpdateBookField: {fieldName} '{value}' is not a valid positive ID. Update skipped.");
                    return;
                }
                string checkTable = fieldName.Contains("category") ? "Categories" : "Authors";
                string checkColumn = fieldName.Contains("category") ? "category_id" : "author_id";
                using (MySqlConnection tempConn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        tempConn.Open();
                        string checkQuery = $"SELECT COUNT(*) FROM {checkTable} WHERE {checkColumn} = @id";
                        MySqlCommand checkCmd = new MySqlCommand(checkQuery, tempConn);
                        checkCmd.Parameters.AddWithValue("@id", idVal);
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) == 0)
                        {
                            Debug.WriteLine($"UpdateBookField: {(fieldName.Contains("category") ? "Category" : "Author")} ID {idVal} does not exist. Update skipped.");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"UpdateBookField: Error validating ID {value}: {ex.Message}. Update skipped.");
                        return;
                    }
                }
            }

            TextBox activeTextBox = null;
            int originalSelectionStart = 0;
            int originalSelectionLength = 0;
            Control currentFocusedControl = this.ActiveControl;
            if (currentFocusedControl is TextBox)
            {
                activeTextBox = (TextBox)currentFocusedControl;
                if (activeTextBox == textBox2 || activeTextBox == textBox3 || activeTextBox == textBox4)
                {
                    originalSelectionStart = activeTextBox.SelectionStart;
                    originalSelectionLength = activeTextBox.SelectionLength;
                }
                else { activeTextBox = null; }
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = $"UPDATE Books SET `{fieldName}` = @value WHERE book_id = @bookId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@value", value);
                    cmd.Parameters.AddWithValue("@bookId", bookId);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Debug.WriteLine($"SUCCESS: Book ID {bookId} field '{fieldName}' updated to '{value}'.");
                        int currentRowIndex = -1;
                        if (bookstable.SelectedRows.Count > 0)
                            currentRowIndex = bookstable.SelectedRows[0].Index;
                        DataGridViewCell currentDgvCellBeforeLoad = bookstable.CurrentCell;

                        LoadBooksData();

                        if (currentRowIndex != -1 && bookstable.Rows.Count > currentRowIndex)
                        {
                            _isProgrammaticChange = true;
                            bookstable.ClearSelection();
                            bookstable.Rows[currentRowIndex].Selected = true;
                            if (activeTextBox == null && currentDgvCellBeforeLoad != null && currentDgvCellBeforeLoad.RowIndex == currentRowIndex &&
                                currentDgvCellBeforeLoad.ColumnIndex >= 0 && currentDgvCellBeforeLoad.ColumnIndex < bookstable.ColumnCount &&
                                bookstable.Columns[currentDgvCellBeforeLoad.ColumnIndex].Visible)
                            {
                                bookstable.CurrentCell = bookstable.Rows[currentRowIndex].Cells[currentDgvCellBeforeLoad.ColumnIndex];
                            }
                            else if (bookstable.Rows[currentRowIndex].Cells.Count > 0)
                            {
                                bookstable.CurrentCell = bookstable.Rows[currentRowIndex].Cells[GetFirstVisibleColumnIndex(bookstable)];
                            }
                            if (!bookstable.Rows[currentRowIndex].Displayed)
                                bookstable.FirstDisplayedScrollingRowIndex = currentRowIndex;
                            _isProgrammaticChange = false;
                        }

                        if (activeTextBox != null)
                        {
                            activeTextBox.Focus();
                            if (originalSelectionStart <= activeTextBox.Text.Length)
                                activeTextBox.SelectionStart = originalSelectionStart;
                            else
                                activeTextBox.SelectionStart = activeTextBox.Text.Length;
                            activeTextBox.SelectionLength = originalSelectionLength;
                        }
                    }
                    else
                    {
                        Debug.WriteLine($"INFO: Update for Book ID {bookId} (Field: {fieldName}, Value: {value}) did not affect any rows.");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Database error while updating '{fieldName}': " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadBooksData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred during update of '{fieldName}': " + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadBooksData();
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e) // Title
        {
            if (_isProgrammaticChange || bookstable.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(textBox1.Text))
                return;
            if (int.TryParse(textBox1.Text, out int bookId))
            {
                UpdateBookField("title", textBox2.Text, bookId);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e) // Category ID
        {
            if (_isProgrammaticChange || bookstable.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(textBox1.Text))
                return;
            if (int.TryParse(textBox1.Text, out int bookId))
            {
                if (int.TryParse(textBox3.Text, out int categoryIdValue))
                {
                    UpdateBookField("category_id", categoryIdValue, bookId);
                }
                else if (!string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    Debug.WriteLine("Category ID TextChanged: Content is not a valid integer for update.");
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e) // Author ID
        {
            if (_isProgrammaticChange || bookstable.SelectedRows.Count == 0 || string.IsNullOrWhiteSpace(textBox1.Text))
                return;
            if (int.TryParse(textBox1.Text, out int bookId))
            {
                if (int.TryParse(textBox4.Text, out int authorIdValue))
                {
                    UpdateBookField("author_id", authorIdValue, bookId);
                }
                else if (!string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    Debug.WriteLine("Author ID TextChanged: Content is not a valid integer for update.");
                }
            }
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            if (bookstable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(textBox1.Text, out int bookId))
            {
                MessageBox.Show("No valid book selected for deletion (Book ID is missing or invalid).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string bookTitle = string.IsNullOrWhiteSpace(textBox2.Text) ? "this book" : $"'{textBox2.Text}' (ID: {bookId})";
            DialogResult confirmResult = MessageBox.Show($"Are you sure you want to delete {bookTitle}?\nThis action may also affect related records based on database settings (ON DELETE CASCADE).", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Books WHERE book_id = @bookId";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@bookId", bookId);
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Book deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadBooksData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete book. It might have already been deleted or the Book ID was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Database error while deleting book: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An unexpected error occurred while deleting book: " + ex.Message, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void search_button_Click(object sender, EventArgs e)
        {
            string searchTerm = searchbox.Text.Trim();
            this.bookstable.SelectionChanged -= bookstable_SelectionChanged;
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
                        query = @"SELECT b.book_id AS 'Book ID', b.title AS 'Title',
                                   a.author_name AS 'Author', c.category_name AS 'Category',
                                   b.category_id AS 'CategoryID_Hidden', b.author_id AS 'AuthorID_Hidden'
                            FROM Books b LEFT JOIN Authors a ON b.author_id = a.author_id
                            LEFT JOIN Categories c ON b.category_id = c.category_id ORDER BY b.book_id ASC;";
                    }
                    else
                    {
                        query = @"SELECT b.book_id AS 'Book ID', b.title AS 'Title',
                                   a.author_name AS 'Author', c.category_name AS 'Category',
                                   b.category_id AS 'CategoryID_Hidden', b.author_id AS 'AuthorID_Hidden'
                            FROM Books b LEFT JOIN Authors a ON b.author_id = a.author_id
                            LEFT JOIN Categories c ON b.category_id = c.category_id
                            WHERE b.title LIKE @searchTerm OR a.author_name LIKE @searchTerm OR
                                  c.category_name LIKE @searchTerm OR b.book_id LIKE @searchTermNumericEquivalent
                            ORDER BY b.book_id ASC;";
                        cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                        cmd.Parameters.AddWithValue("@searchTermNumericEquivalent", searchTerm);
                    }
                    cmd.CommandText = query;
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                    bookstable.DataSource = dataTable;
                    if (bookstable.Columns.Contains("CategoryID_Hidden")) bookstable.Columns["CategoryID_Hidden"].Visible = false;
                    if (bookstable.Columns.Contains("AuthorID_Hidden")) bookstable.Columns["AuthorID_Hidden"].Visible = false;
                    if (dataTable.Rows.Count == 0 && !string.IsNullOrWhiteSpace(searchTerm))
                        MessageBox.Show("No books found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (MySqlException ex) { MessageBox.Show("DB error during search: " + ex.Message); }
                catch (Exception ex) { MessageBox.Show("Error during search: " + ex.Message); }
            }
            ClearInputFields(true);
            _isProgrammaticChange = false;
            this.bookstable.SelectionChanged += bookstable_SelectionChanged;
            if (bookstable.Rows.Count > 0)
            {
                bookstable.CurrentCell = bookstable.Rows[0].Cells[GetFirstVisibleColumnIndex(bookstable)];
                bookstable_SelectionChanged(bookstable, EventArgs.Empty);
            }
        }

        private void save2Excel_button_Click(object sender, EventArgs e)
        {
            if (bookstable.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true, FileName = "LibraryBooks.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            DataTable dtSource = (DataTable)bookstable.DataSource;
                            DataTable dtExport = new DataTable();
                            dtExport.Columns.Add("Book ID", typeof(int));
                            dtExport.Columns.Add("Title", typeof(string));
                            dtExport.Columns.Add("Author", typeof(string));
                            dtExport.Columns.Add("Category", typeof(string));
                            dtExport.Columns.Add("Category ID", typeof(int));
                            dtExport.Columns.Add("Author ID", typeof(int));
                            foreach (DataRow row in dtSource.Rows)
                            {
                                dtExport.Rows.Add(row["Book ID"], row["Title"], row["Author"], row["Category"],
                                    row["CategoryID_Hidden"], row["AuthorID_Hidden"]);
                            }
                            var worksheet = workbook.Worksheets.Add(dtExport, "Books");
                            worksheet.Columns().AdjustToContents();
                            workbook.SaveAs(sfd.FileName);
                        }
                        MessageBox.Show("Data exported to Excel!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error exporting: " + ex.Message); }
        }

        // --- Navigation Event Handlers ---
        private void authors_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Authors button clicked - Method Started!");
            try
            {
                this.Hide();
                using (authors authors_form = new authors())
                {
                    authors_form.ShowDialog();
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
            MessageBox.Show("Users button clicked - Method Started!");
            try
            {
                this.Hide();
                using (users users_form = new users())
                {
                    users_form.ShowDialog();
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
            MessageBox.Show("Borrowed Books button clicked - Method Started!");
            try
            {
                this.Hide();
                using (borrowedbooks borrowed_form = new borrowedbooks())
                {
                    borrowed_form.ShowDialog();
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
            MessageBox.Show("Borrowers button clicked - Method Started!");
            try
            {
                this.Hide();
                using (borrowers borrowers_form = new borrowers())
                {
                    borrowers_form.ShowDialog();
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

        private void books_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Books button clicked - Method Started!");
            LoadBooksData();
        }

        private void logout_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Logout button clicked - Method Started!");
            try
            {
                this.Hide(); // Hide the current home form first

                login login_form = new login();
                // When the newly shown login_form closes (for any reason, including successful login
                // which might open another home form, or user closes the login window),
                // we want to ensure this current (now hidden) home form instance also closes.
                login_form.FormClosed += (s, args) => this.Close();

                login_form.Show();
                // At this point, 'home' is hidden, 'login_form' is shown.
                // The application will continue running as long as login_form (or subsequent forms) are open.
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during logout: {ex.ToString()}", "Logout Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show(); // If something went wrong trying to show login, re-show home.
                this.Activate();
            }
        }
    }
}
