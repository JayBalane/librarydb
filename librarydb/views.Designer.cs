namespace librarydb
{
    partial class views
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.logout_button = new System.Windows.Forms.Button();
            this.overdue_books = new System.Windows.Forms.Button();
            this.borrowers_list = new System.Windows.Forms.Button();
            this.available_books = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.viewslabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bookscount = new System.Windows.Forms.Button();
            this.booksavailability = new System.Windows.Forms.Button();
            this.booksbyauthor = new System.Windows.Forms.Button();
            this.overduebooks = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.GrayText;
            this.panel1.Controls.Add(this.overduebooks);
            this.panel1.Controls.Add(this.booksbyauthor);
            this.panel1.Controls.Add(this.booksavailability);
            this.panel1.Controls.Add(this.bookscount);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.viewslabel);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.logout_button);
            this.panel1.Controls.Add(this.overdue_books);
            this.panel1.Controls.Add(this.borrowers_list);
            this.panel1.Controls.Add(this.available_books);
            this.panel1.Location = new System.Drawing.Point(-2, -4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(248, 451);
            this.panel1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(41, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 58);
            this.label1.TabIndex = 0;
            this.label1.Text = "Library \r\nManagement";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // logout_button
            // 
            this.logout_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logout_button.Location = new System.Drawing.Point(73, 415);
            this.logout_button.Name = "logout_button";
            this.logout_button.Size = new System.Drawing.Size(94, 33);
            this.logout_button.TabIndex = 6;
            this.logout_button.Text = "Logout";
            this.logout_button.UseVisualStyleBackColor = true;
            // 
            // overdue_books
            // 
            this.overdue_books.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.overdue_books.Location = new System.Drawing.Point(58, 177);
            this.overdue_books.Name = "overdue_books";
            this.overdue_books.Size = new System.Drawing.Size(123, 32);
            this.overdue_books.TabIndex = 3;
            this.overdue_books.Text = "Overdue Books";
            this.overdue_books.UseVisualStyleBackColor = true;
            this.overdue_books.Click += new System.EventHandler(this.overdue_books_Click);
            // 
            // borrowers_list
            // 
            this.borrowers_list.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.borrowers_list.Location = new System.Drawing.Point(58, 139);
            this.borrowers_list.Name = "borrowers_list";
            this.borrowers_list.Size = new System.Drawing.Size(123, 32);
            this.borrowers_list.TabIndex = 2;
            this.borrowers_list.Text = "Borrowers List";
            this.borrowers_list.UseVisualStyleBackColor = true;
            // 
            // available_books
            // 
            this.available_books.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.available_books.Location = new System.Drawing.Point(58, 101);
            this.available_books.Name = "available_books";
            this.available_books.Size = new System.Drawing.Size(123, 32);
            this.available_books.TabIndex = 1;
            this.available_books.Text = "Available Books";
            this.available_books.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(252, 9);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(536, 424);
            this.dataGridView1.TabIndex = 4;
            // 
            // viewslabel
            // 
            this.viewslabel.AutoSize = true;
            this.viewslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewslabel.Location = new System.Drawing.Point(54, 76);
            this.viewslabel.Name = "viewslabel";
            this.viewslabel.Size = new System.Drawing.Size(58, 22);
            this.viewslabel.TabIndex = 7;
            this.viewslabel.Text = "Views";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(34, 227);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 22);
            this.label2.TabIndex = 8;
            this.label2.Text = "Stored Procedures";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // bookscount
            // 
            this.bookscount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bookscount.Location = new System.Drawing.Point(58, 252);
            this.bookscount.Name = "bookscount";
            this.bookscount.Size = new System.Drawing.Size(123, 32);
            this.bookscount.TabIndex = 9;
            this.bookscount.Text = "Books Count";
            this.bookscount.UseVisualStyleBackColor = true;
            this.bookscount.Click += new System.EventHandler(this.bookscount_Click);
            // 
            // booksavailability
            // 
            this.booksavailability.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.booksavailability.Location = new System.Drawing.Point(58, 290);
            this.booksavailability.Name = "booksavailability";
            this.booksavailability.Size = new System.Drawing.Size(123, 32);
            this.booksavailability.TabIndex = 10;
            this.booksavailability.Text = "Books Availability";
            this.booksavailability.UseVisualStyleBackColor = true;
            // 
            // booksbyauthor
            // 
            this.booksbyauthor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.booksbyauthor.Location = new System.Drawing.Point(58, 328);
            this.booksbyauthor.Name = "booksbyauthor";
            this.booksbyauthor.Size = new System.Drawing.Size(123, 32);
            this.booksbyauthor.TabIndex = 11;
            this.booksbyauthor.Text = "Books by Author";
            this.booksbyauthor.UseVisualStyleBackColor = true;
            this.booksbyauthor.Click += new System.EventHandler(this.button1_Click);
            // 
            // overduebooks
            // 
            this.overduebooks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.overduebooks.Location = new System.Drawing.Point(58, 366);
            this.overduebooks.Name = "overduebooks";
            this.overduebooks.Size = new System.Drawing.Size(123, 32);
            this.overduebooks.TabIndex = 12;
            this.overduebooks.Text = "Overdue Books";
            this.overduebooks.UseVisualStyleBackColor = true;
            // 
            // views
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 445);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Name = "views";
            this.Text = "views";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button logout_button;
        private System.Windows.Forms.Button overdue_books;
        private System.Windows.Forms.Button borrowers_list;
        private System.Windows.Forms.Button available_books;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label viewslabel;
        private System.Windows.Forms.Button bookscount;
        private System.Windows.Forms.Button booksavailability;
        private System.Windows.Forms.Button booksbyauthor;
        private System.Windows.Forms.Button overduebooks;
    }
}