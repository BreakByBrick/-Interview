namespace InstLikeApp.Client
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxCommentsText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.buttonAddPost = new System.Windows.Forms.Button();
            this.buttonAddUser = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonAddComment = new System.Windows.Forms.Button();
            this.buttonNextPost = new System.Windows.Forms.Button();
            this.buttonPrevPost = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxNewComment = new System.Windows.Forms.TextBox();
            this.buttonShowUserPosts = new System.Windows.Forms.Button();
            this.buttonShowLastPosts = new System.Windows.Forms.Button();
            this.labelPost = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxCommentsText
            // 
            this.textBoxCommentsText.Location = new System.Drawing.Point(3, 415);
            this.textBoxCommentsText.Multiline = true;
            this.textBoxCommentsText.Name = "textBoxCommentsText";
            this.textBoxCommentsText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxCommentsText.Size = new System.Drawing.Size(366, 162);
            this.textBoxCommentsText.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(375, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enter User Name:";
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(375, 27);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(293, 22);
            this.textBoxUserName.TabIndex = 4;
            this.textBoxUserName.TextChanged += new System.EventHandler(this.textBoxUserName_textChanged);
            // 
            // buttonAddPost
            // 
            this.buttonAddPost.Location = new System.Drawing.Point(375, 109);
            this.buttonAddPost.Name = "buttonAddPost";
            this.buttonAddPost.Size = new System.Drawing.Size(293, 48);
            this.buttonAddPost.TabIndex = 5;
            this.buttonAddPost.Text = "Add Post";
            this.buttonAddPost.UseVisualStyleBackColor = true;
            this.buttonAddPost.Click += new System.EventHandler(this.buttonAddPost_Click);
            // 
            // buttonAddUser
            // 
            this.buttonAddUser.Location = new System.Drawing.Point(375, 55);
            this.buttonAddUser.Name = "buttonAddUser";
            this.buttonAddUser.Size = new System.Drawing.Size(293, 48);
            this.buttonAddUser.TabIndex = 6;
            this.buttonAddUser.Text = "Add User";
            this.buttonAddUser.UseVisualStyleBackColor = true;
            this.buttonAddUser.Click += new System.EventHandler(this.buttonAddUser_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(3, 55);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(366, 304);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 395);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Comments:";
            // 
            // buttonAddComment
            // 
            this.buttonAddComment.Location = new System.Drawing.Point(3, 674);
            this.buttonAddComment.Name = "buttonAddComment";
            this.buttonAddComment.Size = new System.Drawing.Size(366, 48);
            this.buttonAddComment.TabIndex = 9;
            this.buttonAddComment.Text = "Add Comment";
            this.buttonAddComment.UseVisualStyleBackColor = true;
            this.buttonAddComment.Click += new System.EventHandler(this.buttonAddComment_Click);
            // 
            // buttonNextPost
            // 
            this.buttonNextPost.Location = new System.Drawing.Point(194, 12);
            this.buttonNextPost.Name = "buttonNextPost";
            this.buttonNextPost.Size = new System.Drawing.Size(175, 37);
            this.buttonNextPost.TabIndex = 10;
            this.buttonNextPost.Text = "next ->";
            this.buttonNextPost.UseVisualStyleBackColor = true;
            this.buttonNextPost.Click += new System.EventHandler(this.buttonNextPost_Click);
            // 
            // buttonPrevPost
            // 
            this.buttonPrevPost.Location = new System.Drawing.Point(3, 12);
            this.buttonPrevPost.Name = "buttonPrevPost";
            this.buttonPrevPost.Size = new System.Drawing.Size(185, 37);
            this.buttonPrevPost.TabIndex = 11;
            this.buttonPrevPost.Text = "<- prev";
            this.buttonPrevPost.UseVisualStyleBackColor = true;
            this.buttonPrevPost.Click += new System.EventHandler(this.buttonPrevPost_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 580);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "New comment:";
            // 
            // textBoxNewComment
            // 
            this.textBoxNewComment.Location = new System.Drawing.Point(3, 597);
            this.textBoxNewComment.Multiline = true;
            this.textBoxNewComment.Name = "textBoxNewComment";
            this.textBoxNewComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxNewComment.Size = new System.Drawing.Size(366, 71);
            this.textBoxNewComment.TabIndex = 13;
            // 
            // buttonShowUserPosts
            // 
            this.buttonShowUserPosts.Location = new System.Drawing.Point(375, 163);
            this.buttonShowUserPosts.Name = "buttonShowUserPosts";
            this.buttonShowUserPosts.Size = new System.Drawing.Size(293, 49);
            this.buttonShowUserPosts.TabIndex = 14;
            this.buttonShowUserPosts.Text = "User\'s Posts";
            this.buttonShowUserPosts.UseVisualStyleBackColor = true;
            this.buttonShowUserPosts.Click += new System.EventHandler(this.buttonShowUserPosts_Click);
            // 
            // buttonShowLastPosts
            // 
            this.buttonShowLastPosts.Location = new System.Drawing.Point(375, 218);
            this.buttonShowLastPosts.Name = "buttonShowLastPosts";
            this.buttonShowLastPosts.Size = new System.Drawing.Size(293, 49);
            this.buttonShowLastPosts.TabIndex = 15;
            this.buttonShowLastPosts.Text = "Last Posts";
            this.buttonShowLastPosts.UseVisualStyleBackColor = true;
            this.buttonShowLastPosts.Click += new System.EventHandler(this.buttonShowLastPosts_Click);
            // 
            // labelPost
            // 
            this.labelPost.AutoSize = true;
            this.labelPost.Location = new System.Drawing.Point(0, 362);
            this.labelPost.Name = "labelPost";
            this.labelPost.Size = new System.Drawing.Size(75, 17);
            this.labelPost.TabIndex = 16;
            this.labelPost.Text = "Posted by:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 736);
            this.Controls.Add(this.labelPost);
            this.Controls.Add(this.buttonShowLastPosts);
            this.Controls.Add(this.buttonShowUserPosts);
            this.Controls.Add(this.textBoxNewComment);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonPrevPost);
            this.Controls.Add(this.buttonNextPost);
            this.Controls.Add(this.buttonAddComment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonAddUser);
            this.Controls.Add(this.buttonAddPost);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxCommentsText);
            this.Name = "Form1";
            this.Text = "InstLikeApp";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxCommentsText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.Button buttonAddPost;
        private System.Windows.Forms.Button buttonAddUser;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonAddComment;
        private System.Windows.Forms.Button buttonNextPost;
        private System.Windows.Forms.Button buttonPrevPost;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxNewComment;
        private System.Windows.Forms.Button buttonShowUserPosts;
        private System.Windows.Forms.Button buttonShowLastPosts;
        private System.Windows.Forms.Label labelPost;
    }
}

