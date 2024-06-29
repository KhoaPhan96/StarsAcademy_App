
namespace Demo_Encrypt_LoadFile.Feature
{
    partial class frm_TestGallery
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
            DevExpress.Utils.CheckContextButton checkContextButton1 = new DevExpress.Utils.CheckContextButton();
            DevExpress.Utils.ContextButton contextButton1 = new DevExpress.Utils.ContextButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.galleryControl1 = new DevExpress.XtraBars.Ribbon.GalleryControl();
            this.galleryControlClient1 = new DevExpress.XtraBars.Ribbon.GalleryControlClient();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.galleryControl1)).BeginInit();
            this.galleryControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.galleryControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 0;
            // 
            // galleryControl1
            // 
            this.galleryControl1.Controls.Add(this.galleryControlClient1);
            this.galleryControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            this.galleryControl1.Gallery.ContextButtonOptions.BottomPanelColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.galleryControl1.Gallery.ContextButtonOptions.TopPanelColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            checkContextButton1.AlignmentOptions.Panel = DevExpress.Utils.ContextItemPanel.Bottom;
            checkContextButton1.Id = new System.Guid("c42e1feb-0071-428f-8d5f-7399adad697e");
            checkContextButton1.Name = "itemCheck";
            checkContextButton1.Visibility = DevExpress.Utils.ContextItemVisibility.Visible;
            contextButton1.AlignmentOptions.Position = DevExpress.Utils.ContextItemPosition.Center;
            contextButton1.Id = new System.Guid("2d5d65ca-f1ab-498d-8e91-903ba7360fa4");
            contextButton1.Name = "itemInfo";
            contextButton1.Visibility = DevExpress.Utils.ContextItemVisibility.Visible;
            this.galleryControl1.Gallery.ContextButtons.Add(checkContextButton1);
            this.galleryControl1.Gallery.ContextButtons.Add(contextButton1);
            this.galleryControl1.Gallery.ContextButtonCustomize += new DevExpress.XtraBars.Ribbon.Gallery.GalleryContextButtonCustomizeEventHandler(this.galleryControl1_Gallery_ContextButtonCustomize);
            this.galleryControl1.Gallery.ContextButtonClick += new DevExpress.Utils.ContextItemClickEventHandler(this.galleryControl1_Gallery_ContextButtonClick);
            this.galleryControl1.Location = new System.Drawing.Point(0, 0);
            this.galleryControl1.Name = "galleryControl1";
            this.galleryControl1.Size = new System.Drawing.Size(800, 450);
            this.galleryControl1.TabIndex = 0;
            this.galleryControl1.Text = "galleryControl1";
            // 
            // galleryControlClient1
            // 
            this.galleryControlClient1.GalleryControl = this.galleryControl1;
            this.galleryControlClient1.Location = new System.Drawing.Point(2, 2);
            this.galleryControlClient1.Size = new System.Drawing.Size(779, 446);
            // 
            // frm_TestGallery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Name = "frm_TestGallery";
            this.Text = "frm_TestGallery";
            this.Load += new System.EventHandler(this.frm_TestGallery_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.galleryControl1)).EndInit();
            this.galleryControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraBars.Ribbon.GalleryControl galleryControl1;
        private DevExpress.XtraBars.Ribbon.GalleryControlClient galleryControlClient1;
    }
}