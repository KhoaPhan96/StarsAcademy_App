
namespace Demo_Encrypt_LoadFile.Feature
{
    partial class frm_TestXML
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
            this.btn_SaveXML = new System.Windows.Forms.Button();
            this.btn_LoadXML = new System.Windows.Forms.Button();
            this.btn_DeleteXML = new System.Windows.Forms.Button();
            this.btn_ThemXml = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.cardView1 = new DevExpress.XtraGrid.Views.Card.CardView();
            this.repositoryItemPictureEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.btn_ThemFileXML = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_ThemFileXML);
            this.panel1.Controls.Add(this.btn_SaveXML);
            this.panel1.Controls.Add(this.btn_LoadXML);
            this.panel1.Controls.Add(this.btn_DeleteXML);
            this.panel1.Controls.Add(this.btn_ThemXml);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 78);
            this.panel1.TabIndex = 0;
            // 
            // btn_SaveXML
            // 
            this.btn_SaveXML.Location = new System.Drawing.Point(103, 12);
            this.btn_SaveXML.Name = "btn_SaveXML";
            this.btn_SaveXML.Size = new System.Drawing.Size(75, 23);
            this.btn_SaveXML.TabIndex = 3;
            this.btn_SaveXML.Text = "Save XML";
            this.btn_SaveXML.UseVisualStyleBackColor = true;
            this.btn_SaveXML.Click += new System.EventHandler(this.btn_SaveXML_Click);
            // 
            // btn_LoadXML
            // 
            this.btn_LoadXML.Location = new System.Drawing.Point(294, 12);
            this.btn_LoadXML.Name = "btn_LoadXML";
            this.btn_LoadXML.Size = new System.Drawing.Size(75, 23);
            this.btn_LoadXML.TabIndex = 2;
            this.btn_LoadXML.Text = "Load XML";
            this.btn_LoadXML.UseVisualStyleBackColor = true;
            this.btn_LoadXML.Click += new System.EventHandler(this.btn_LoadXML_Click);
            // 
            // btn_DeleteXML
            // 
            this.btn_DeleteXML.Location = new System.Drawing.Point(198, 12);
            this.btn_DeleteXML.Name = "btn_DeleteXML";
            this.btn_DeleteXML.Size = new System.Drawing.Size(90, 23);
            this.btn_DeleteXML.TabIndex = 1;
            this.btn_DeleteXML.Text = "Delete XML";
            this.btn_DeleteXML.UseVisualStyleBackColor = true;
            this.btn_DeleteXML.Click += new System.EventHandler(this.btn_DeleteXML_Click);
            // 
            // btn_ThemXml
            // 
            this.btn_ThemXml.Location = new System.Drawing.Point(12, 12);
            this.btn_ThemXml.Name = "btn_ThemXml";
            this.btn_ThemXml.Size = new System.Drawing.Size(75, 23);
            this.btn_ThemXml.TabIndex = 0;
            this.btn_ThemXml.Text = "Insert XML";
            this.btn_ThemXml.UseVisualStyleBackColor = true;
            this.btn_ThemXml.Click += new System.EventHandler(this.btn_ThemXml_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 78);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 372);
            this.panel2.TabIndex = 1;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.cardView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemPictureEdit1});
            this.gridControl1.Size = new System.Drawing.Size(800, 372);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.cardView1});
            // 
            // cardView1
            // 
            this.cardView1.CardWidth = 250;
            this.cardView1.DetailHeight = 550;
            this.cardView1.GridControl = this.gridControl1;
            this.cardView1.Name = "cardView1";
            // 
            // repositoryItemPictureEdit1
            // 
            this.repositoryItemPictureEdit1.CustomHeight = 110;
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            this.repositoryItemPictureEdit1.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            // 
            // btn_ThemFileXML
            // 
            this.btn_ThemFileXML.Location = new System.Drawing.Point(397, 12);
            this.btn_ThemFileXML.Name = "btn_ThemFileXML";
            this.btn_ThemFileXML.Size = new System.Drawing.Size(97, 23);
            this.btn_ThemFileXML.TabIndex = 4;
            this.btn_ThemFileXML.Text = "Thêm file XML";
            this.btn_ThemFileXML.UseVisualStyleBackColor = true;
            this.btn_ThemFileXML.Click += new System.EventHandler(this.btn_ThemFileXML_Click);
            // 
            // frm_TestXML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frm_TestXML";
            this.Text = "frm_TestXML";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_ThemXml;
        private System.Windows.Forms.Button btn_DeleteXML;
        private System.Windows.Forms.Button btn_LoadXML;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Card.CardView cardView1;
        private System.Windows.Forms.Button btn_SaveXML;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private System.Windows.Forms.Button btn_ThemFileXML;
    }
}