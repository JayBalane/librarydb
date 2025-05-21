namespace librarydb
{
    partial class borrowedbooks
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
            this.borrowers_button = new System.Windows.Forms.Button();
            this.borrowedbooks_button = new System.Windows.Forms.Button();
            this.users_button = new System.Windows.Forms.Button();
            this.authors_button = new System.Windows.Forms.Button();
            this.books_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.return_date = new System.Windows.Forms.TextBox();
            this.borrow_date = new System.Windows.Forms.TextBox();
            this.borrower_id = new System.Windows.Forms.TextBox();
            this.book_id = new System.Windows.Forms.TextBox();
            this.borrow_id = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.b_bookstable = new System.Windows.Forms.DataGridView();
            this.save2Excel_button = new System.Windows.Forms.Button();
            this.delete_button = new System.Windows.Forms.Button();
            this.add_button = new System.Windows.Forms.Button();
            this.edit_button = new System.Windows.Forms.Button();
            this.searchbox = new System.Windows.Forms.TextBox();
            this.search_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.logout_button = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.b_bookstable)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.GrayText;
            this.panel1.Controls.Add(this.borrowers_button);
            this.panel1.Controls.Add(this.borrowedbooks_button);
            this.panel1.Controls.Add(this.users_button);
            this.panel1.Controls.Add(this.authors_button);
            this.panel1.Controls.Add(this.books_button);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(-1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(248, 451);
            this.panel1.TabIndex = 0;
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
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel2.Controls.Add(this.return_date);
            this.panel2.Controls.Add(this.borrow_date);
            this.panel2.Controls.Add(this.borrower_id);
            this.panel2.Controls.Add(this.book_id);
            this.panel2.Controls.Add(this.borrow_id);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.b_bookstable);
            this.panel2.Controls.Add(this.save2Excel_button);
            this.panel2.Controls.Add(this.delete_button);
            this.panel2.Controls.Add(this.add_button);
            this.panel2.Controls.Add(this.edit_button);
            this.panel2.Controls.Add(this.searchbox);
            this.panel2.Controls.Add(this.search_button);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(244, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(557, 451);
            this.panel2.TabIndex = 1;
            // 
            // return_date
            // 
            this.return_date.Location = new System.Drawing.Point(405, 338);
            this.return_date.Name = "return_date";
            this.return_date.Size = new System.Drawing.Size(139, 22);
            this.return_date.TabIndex = 19;
            // 
            // borrow_date
            // 
            this.borrow_date.Location = new System.Drawing.Point(405, 284);
            this.borrow_date.Name = "borrow_date";
            this.borrow_date.Size = new System.Drawing.Size(139, 22);
            this.borrow_date.TabIndex = 18;
            // 
            // borrower_id
            // 
            this.borrower_id.Location = new System.Drawing.Point(405, 227);
            this.borrower_id.Name = "borrower_id";
            this.borrower_id.Size = new System.Drawing.Size(139, 22);
            this.borrower_id.TabIndex = 17;
            // 
            // book_id
            // 
            this.book_id.Location = new System.Drawing.Point(405, 171);
            this.book_id.Name = "book_id";
            this.book_id.Size = new System.Drawing.Size(139, 22);
            this.book_id.TabIndex = 16;

            // 
            // borrow_id
            // 
            this.borrow_id.Location = new System.Drawing.Point(405, 117);
            this.borrow_id.Name = "borrow_id";
            this.borrow_id.Size = new System.Drawing.Size(139, 22);
            this.borrow_id.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(402, 265);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 16);
            this.label7.TabIndex = 14;
            this.label7.Text = "borrow_date";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(402, 319);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 16);
            this.label6.TabIndex = 13;
            this.label6.Text = "return_date";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(402, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 16);
            this.label5.TabIndex = 12;
            this.label5.Text = "borrower_id";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(402, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "book_id";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(402, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "borrow_id";
            // 
            // b_bookstable
            // 
            this.b_bookstable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.b_bookstable.Location = new System.Drawing.Point(9, 98);
            this.b_bookstable.Name = "b_bookstable";
            this.b_bookstable.RowHeadersWidth = 51;
            this.b_bookstable.RowTemplate.Height = 24;
            this.b_bookstable.Size = new System.Drawing.Size(387, 272);
            this.b_bookstable.TabIndex = 9;
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
            this.edit_button.Click += new System.EventHandler(this.edit_button_Click);
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
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(162, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(258, 38);
            this.label2.TabIndex = 0;
            this.label2.Text = "Borrowed Books";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // logout_button
            // 
            this.logout_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logout_button.Location = new System.Drawing.Point(75, 392);
            this.logout_button.Name = "logout_button";
            this.logout_button.Size = new System.Drawing.Size(94, 33);
            this.logout_button.TabIndex = 7;
            this.logout_button.Text = "Logout";
            this.logout_button.UseVisualStyleBackColor = true;
            this.logout_button.Click += new System.EventHandler(this.logout_button_Click);
            // 
            // borrowedbooks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.logout_button);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "borrowedbooks";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.b_bookstable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button borrowers_button;
        private System.Windows.Forms.Button borrowedbooks_button;
        private System.Windows.Forms.Button users_button;
        private System.Windows.Forms.Button authors_button;
        private System.Windows.Forms.Button books_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox searchbox;
        private System.Windows.Forms.Button search_button;
        private System.Windows.Forms.Button add_button;
        private System.Windows.Forms.Button delete_button;
        private System.Windows.Forms.Button save2Excel_button;
        private System.Windows.Forms.Button edit_button;
        private System.Windows.Forms.Button logout_button;
        private System.Windows.Forms.DataGridView b_bookstable;
        private System.Windows.Forms.TextBox return_date;
        private System.Windows.Forms.TextBox borrow_date;
        private System.Windows.Forms.TextBox borrower_id;
        private System.Windows.Forms.TextBox book_id;
        private System.Windows.Forms.TextBox borrow_id;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}