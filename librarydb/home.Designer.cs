namespace librarydb
{
    partial class home
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.author_id = new System.Windows.Forms.Label();
            this.category_id = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this.book_id = new System.Windows.Forms.Label();
            this.bookstable = new System.Windows.Forms.DataGridView();
            this.save2Excel_button = new System.Windows.Forms.Button();
            this.delete_button = new System.Windows.Forms.Button();
            this.add_button = new System.Windows.Forms.Button();
            this.edit_button = new System.Windows.Forms.Button();
            this.searchbox = new System.Windows.Forms.TextBox();
            this.search_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.borrowers_button = new System.Windows.Forms.Button();
            this.borrowedbooks_button = new System.Windows.Forms.Button();
            this.users_button = new System.Windows.Forms.Button();
            this.authors_button = new System.Windows.Forms.Button();
            this.books_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.logout_button = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bookstable)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel2.Controls.Add(this.textBox4);
            this.panel2.Controls.Add(this.textBox3);
            this.panel2.Controls.Add(this.textBox2);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.author_id);
            this.panel2.Controls.Add(this.category_id);
            this.panel2.Controls.Add(this.title);
            this.panel2.Controls.Add(this.book_id);
            this.panel2.Controls.Add(this.bookstable);
            this.panel2.Controls.Add(this.save2Excel_button);
            this.panel2.Controls.Add(this.delete_button);
            this.panel2.Controls.Add(this.add_button);
            this.panel2.Controls.Add(this.edit_button);
            this.panel2.Controls.Add(this.searchbox);
            this.panel2.Controls.Add(this.search_button);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(244, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(557, 451);
            this.panel2.TabIndex = 3;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(405, 284);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(139, 22);
            this.textBox4.TabIndex = 17;
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(405, 227);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(139, 22);
            this.textBox3.TabIndex = 16;
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(405, 171);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(139, 22);
            this.textBox2.TabIndex = 15;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(405, 117);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(139, 22);
            this.textBox1.TabIndex = 14;
            // 
            // author_id
            // 
            this.author_id.AutoSize = true;
            this.author_id.Location = new System.Drawing.Point(402, 265);
            this.author_id.Name = "author_id";
            this.author_id.Size = new System.Drawing.Size(62, 16);
            this.author_id.TabIndex = 13;
            this.author_id.Text = "author_id";
            // 
            // category_id
            // 
            this.category_id.AutoSize = true;
            this.category_id.Location = new System.Drawing.Point(402, 208);
            this.category_id.Name = "category_id";
            this.category_id.Size = new System.Drawing.Size(78, 16);
            this.category_id.TabIndex = 12;
            this.category_id.Text = "category_id";
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Location = new System.Drawing.Point(402, 152);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(27, 16);
            this.title.TabIndex = 11;
            this.title.Text = "title";
            // 
            // book_id
            // 
            this.book_id.AutoSize = true;
            this.book_id.Location = new System.Drawing.Point(402, 98);
            this.book_id.Name = "book_id";
            this.book_id.Size = new System.Drawing.Size(56, 16);
            this.book_id.TabIndex = 10;
            this.book_id.Text = "book_id";
            // 
            // bookstable
            // 
            this.bookstable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.bookstable.Location = new System.Drawing.Point(9, 98);
            this.bookstable.Name = "bookstable";
            this.bookstable.RowHeadersWidth = 51;
            this.bookstable.RowTemplate.Height = 24;
            this.bookstable.Size = new System.Drawing.Size(387, 272);
            this.bookstable.TabIndex = 9;
            this.bookstable.SelectionChanged += new System.EventHandler(this.bookstable_SelectionChanged);
            // 
            // save2Excel_button
            // 
            this.save2Excel_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.save2Excel_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save2Excel_button.Location = new System.Drawing.Point(125, 413);
            this.save2Excel_button.Name = "save2Excel_button";
            this.save2Excel_button.Size = new System.Drawing.Size(110, 33);
            this.save2Excel_button.TabIndex = 7;
            this.save2Excel_button.Text = "Save as Excel";
            this.save2Excel_button.UseVisualStyleBackColor = true;
            this.save2Excel_button.Click += new System.EventHandler(this.save2Excel_button_Click);
            // 
            // delete_button
            // 
            this.delete_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.delete_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delete_button.Location = new System.Drawing.Point(241, 374);
            this.delete_button.Name = "delete_button";
            this.delete_button.Size = new System.Drawing.Size(110, 33);
            this.delete_button.TabIndex = 6;
            this.delete_button.Text = "Delete";
            this.delete_button.UseVisualStyleBackColor = true;
            this.delete_button.Click += new System.EventHandler(this.delete_button_Click);
            // 
            // add_button
            // 
            this.add_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.add_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.add_button.Location = new System.Drawing.Point(9, 374);
            this.add_button.Name = "add_button";
            this.add_button.Size = new System.Drawing.Size(110, 33);
            this.add_button.TabIndex = 4;
            this.add_button.Text = "Add";
            this.add_button.UseVisualStyleBackColor = true;
            this.add_button.Click += new System.EventHandler(this.add_button_Click);
            // 
            // edit_button
            // 
            this.edit_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edit_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edit_button.Location = new System.Drawing.Point(125, 374);
            this.edit_button.Name = "edit_button";
            this.edit_button.Size = new System.Drawing.Size(110, 33);
            this.edit_button.TabIndex = 5;
            this.edit_button.Text = "Edit";
            this.edit_button.UseVisualStyleBackColor = true;
            // 
            // searchbox
            // 
            this.searchbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchbox.Location = new System.Drawing.Point(363, 68);
            this.searchbox.Name = "searchbox";
            this.searchbox.Size = new System.Drawing.Size(100, 22);
            this.searchbox.TabIndex = 2;
            // 
            // search_button
            // 
            this.search_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.search_button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.search_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.search_button.Location = new System.Drawing.Point(469, 65);
            this.search_button.Name = "search_button";
            this.search_button.Size = new System.Drawing.Size(85, 27);
            this.search_button.TabIndex = 1;
            this.search_button.Text = "search";
            this.search_button.UseVisualStyleBackColor = true;
            this.search_button.Click += new System.EventHandler(this.search_button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(243, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 38);
            this.label2.TabIndex = 0;
            this.label2.Text = "Books";
            // 
            // borrowers_button
            // 
            this.borrowers_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.borrowers_button.Location = new System.Drawing.Point(48, 287);
            this.borrowers_button.Name = "borrowers_button";
            this.borrowers_button.Size = new System.Drawing.Size(155, 39);
            this.borrowers_button.TabIndex = 5;
            this.borrowers_button.Text = "Borrowers";
            this.borrowers_button.UseVisualStyleBackColor = true;
            this.borrowers_button.Click += new System.EventHandler(this.borrowers_button_Click);
            // 
            // borrowedbooks_button
            // 
            this.borrowedbooks_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.borrowedbooks_button.Location = new System.Drawing.Point(48, 242);
            this.borrowedbooks_button.Name = "borrowedbooks_button";
            this.borrowedbooks_button.Size = new System.Drawing.Size(155, 39);
            this.borrowedbooks_button.TabIndex = 4;
            this.borrowedbooks_button.Text = "Borrowed Books";
            this.borrowedbooks_button.UseVisualStyleBackColor = true;
            this.borrowedbooks_button.Click += new System.EventHandler(this.borrowedbooks_button_Click);
            // 
            // users_button
            // 
            this.users_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.users_button.Location = new System.Drawing.Point(48, 197);
            this.users_button.Name = "users_button";
            this.users_button.Size = new System.Drawing.Size(155, 39);
            this.users_button.TabIndex = 3;
            this.users_button.Text = "Users";
            this.users_button.UseVisualStyleBackColor = true;
            this.users_button.Click += new System.EventHandler(this.users_button_Click);
            // 
            // authors_button
            // 
            this.authors_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.authors_button.Location = new System.Drawing.Point(48, 152);
            this.authors_button.Name = "authors_button";
            this.authors_button.Size = new System.Drawing.Size(155, 39);
            this.authors_button.TabIndex = 2;
            this.authors_button.Text = "Authors";
            this.authors_button.UseVisualStyleBackColor = true;
            this.authors_button.Click += new System.EventHandler(this.authors_button_Click);
            // 
            // books_button
            // 
            this.books_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.books_button.Location = new System.Drawing.Point(48, 107);
            this.books_button.Name = "books_button";
            this.books_button.Size = new System.Drawing.Size(155, 39);
            this.books_button.TabIndex = 1;
            this.books_button.Text = "Books";
            this.books_button.UseVisualStyleBackColor = true;
            this.books_button.Click += new System.EventHandler(this.books_button_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(32, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 64);
            this.label1.TabIndex = 0;
            this.label1.Text = "Library \r\nManagement";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.GrayText;
            this.panel1.Controls.Add(this.logout_button);
            this.panel1.Controls.Add(this.borrowers_button);
            this.panel1.Controls.Add(this.borrowedbooks_button);
            this.panel1.Controls.Add(this.users_button);
            this.panel1.Controls.Add(this.authors_button);
            this.panel1.Controls.Add(this.books_button);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(-1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(248, 451);
            this.panel1.TabIndex = 2;
            // 
            // logout_button
            // 
            this.logout_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logout_button.Location = new System.Drawing.Point(75, 392);
            this.logout_button.Name = "logout_button";
            this.logout_button.Size = new System.Drawing.Size(94, 33);
            this.logout_button.TabIndex = 6;
            this.logout_button.Text = "Logout";
            this.logout_button.UseVisualStyleBackColor = true;
            this.logout_button.Click += new System.EventHandler(this.logout_button_Click);
            // 
            // home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "home";
            this.Text = "Form1";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bookstable)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button save2Excel_button;
        private System.Windows.Forms.Button delete_button;
        private System.Windows.Forms.Button add_button;
        private System.Windows.Forms.Button edit_button;
        private System.Windows.Forms.TextBox searchbox;
        private System.Windows.Forms.Button search_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button borrowers_button;
        private System.Windows.Forms.Button borrowedbooks_button;
        private System.Windows.Forms.Button users_button;
        private System.Windows.Forms.Button authors_button;
        private System.Windows.Forms.Button books_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button logout_button;
        private System.Windows.Forms.DataGridView bookstable;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label author_id;
        private System.Windows.Forms.Label category_id;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label book_id;
    }
}