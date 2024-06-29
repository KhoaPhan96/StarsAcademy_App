
namespace Demo_Encrypt_LoadFile.Feature
{
    partial class frm_InsertKeyAPI
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btn_OK = new DevExpress.XtraEditors.SimpleButton();
            this.pictureEdit_Avatar = new DevExpress.XtraEditors.PictureEdit();
            this.lbl_Active = new System.Windows.Forms.Label();
            this.chk_Active = new DevExpress.XtraEditors.CheckEdit();
            this.btn_Check = new DevExpress.XtraEditors.SimpleButton();
            this.txt_FullName = new DevExpress.XtraEditors.TextEdit();
            this.txt_ToKenAPI = new System.Windows.Forms.TextBox();
            this.txt_Expired = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layout_API = new DevExpress.XtraLayout.LayoutControlItem();
            this.layout_FullName = new DevExpress.XtraLayout.LayoutControlItem();
            this.layout_Expired = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.txt_ThongTin = new System.Windows.Forms.TextBox();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit_Avatar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_Active.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_FullName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Expired.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layout_API)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layout_FullName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layout_Expired)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.txt_ThongTin);
            this.layoutControl1.Controls.Add(this.btn_OK);
            this.layoutControl1.Controls.Add(this.pictureEdit_Avatar);
            this.layoutControl1.Controls.Add(this.lbl_Active);
            this.layoutControl1.Controls.Add(this.chk_Active);
            this.layoutControl1.Controls.Add(this.btn_Check);
            this.layoutControl1.Controls.Add(this.txt_FullName);
            this.layoutControl1.Controls.Add(this.txt_ToKenAPI);
            this.layoutControl1.Controls.Add(this.txt_Expired);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(104, 358, 650, 400);
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(581, 154);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(434, 120);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(135, 22);
            this.btn_OK.StyleController = this.layoutControl1;
            this.btn_OK.TabIndex = 11;
            this.btn_OK.Text = "Lưu dữ liệu";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // pictureEdit_Avatar
            // 
            this.pictureEdit_Avatar.Location = new System.Drawing.Point(12, 12);
            this.pictureEdit_Avatar.Name = "pictureEdit_Avatar";
            this.pictureEdit_Avatar.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit_Avatar.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.pictureEdit_Avatar.Size = new System.Drawing.Size(93, 94);
            this.pictureEdit_Avatar.StyleController = this.layoutControl1;
            this.pictureEdit_Avatar.TabIndex = 10;
            // 
            // lbl_Active
            // 
            this.lbl_Active.Enabled = false;
            this.lbl_Active.Location = new System.Drawing.Point(109, 62);
            this.lbl_Active.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_Active.Name = "lbl_Active";
            this.lbl_Active.Size = new System.Drawing.Size(260, 20);
            this.lbl_Active.TabIndex = 9;
            this.lbl_Active.Text = " Hoạt động";
            this.lbl_Active.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chk_Active
            // 
            this.chk_Active.Enabled = false;
            this.chk_Active.Location = new System.Drawing.Point(373, 62);
            this.chk_Active.Name = "chk_Active";
            this.chk_Active.Properties.Caption = "";
            this.chk_Active.Size = new System.Drawing.Size(196, 20);
            this.chk_Active.StyleController = this.layoutControl1;
            this.chk_Active.TabIndex = 8;
            // 
            // btn_Check
            // 
            this.btn_Check.Location = new System.Drawing.Point(523, 12);
            this.btn_Check.Name = "btn_Check";
            this.btn_Check.Size = new System.Drawing.Size(46, 22);
            this.btn_Check.StyleController = this.layoutControl1;
            this.btn_Check.TabIndex = 7;
            this.btn_Check.Text = "Kiểm tra";
            this.btn_Check.Click += new System.EventHandler(this.btn_Check_Click);
            // 
            // txt_FullName
            // 
            this.txt_FullName.Enabled = false;
            this.txt_FullName.Location = new System.Drawing.Point(192, 38);
            this.txt_FullName.Name = "txt_FullName";
            this.txt_FullName.Size = new System.Drawing.Size(377, 20);
            this.txt_FullName.StyleController = this.layoutControl1;
            this.txt_FullName.TabIndex = 5;
            // 
            // txt_ToKenAPI
            // 
            this.txt_ToKenAPI.Location = new System.Drawing.Point(192, 12);
            this.txt_ToKenAPI.Name = "txt_ToKenAPI";
            this.txt_ToKenAPI.Size = new System.Drawing.Size(327, 22);
            this.txt_ToKenAPI.TabIndex = 4;
            // 
            // txt_Expired
            // 
            this.txt_Expired.Enabled = false;
            this.txt_Expired.Location = new System.Drawing.Point(192, 86);
            this.txt_Expired.Name = "txt_Expired";
            this.txt_Expired.Size = new System.Drawing.Size(238, 20);
            this.txt_Expired.StyleController = this.layoutControl1;
            this.txt_Expired.TabIndex = 5;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layout_API,
            this.layout_FullName,
            this.layout_Expired,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.emptySpaceItem3,
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem5,
            this.layoutControlItem6});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(581, 154);
            this.Root.TextVisible = false;
            // 
            // layout_API
            // 
            this.layout_API.Control = this.txt_ToKenAPI;
            this.layout_API.Location = new System.Drawing.Point(97, 0);
            this.layout_API.MaxSize = new System.Drawing.Size(414, 26);
            this.layout_API.MinSize = new System.Drawing.Size(414, 26);
            this.layout_API.Name = "layout_API";
            this.layout_API.Size = new System.Drawing.Size(414, 26);
            this.layout_API.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layout_API.Text = "  Nhập key";
            this.layout_API.TextSize = new System.Drawing.Size(71, 13);
            // 
            // layout_FullName
            // 
            this.layout_FullName.Control = this.txt_FullName;
            this.layout_FullName.Location = new System.Drawing.Point(97, 26);
            this.layout_FullName.MaxSize = new System.Drawing.Size(464, 24);
            this.layout_FullName.MinSize = new System.Drawing.Size(464, 24);
            this.layout_FullName.Name = "layout_FullName";
            this.layout_FullName.Size = new System.Drawing.Size(464, 24);
            this.layout_FullName.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layout_FullName.Text = "  Họ tên";
            this.layout_FullName.TextSize = new System.Drawing.Size(71, 13);
            // 
            // layout_Expired
            // 
            this.layout_Expired.Control = this.txt_Expired;
            this.layout_Expired.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layout_Expired.CustomizationFormText = "layoutControlItem1";
            this.layout_Expired.Location = new System.Drawing.Point(97, 74);
            this.layout_Expired.MaxSize = new System.Drawing.Size(325, 24);
            this.layout_Expired.MinSize = new System.Drawing.Size(325, 24);
            this.layout_Expired.Name = "layout_Expired";
            this.layout_Expired.Size = new System.Drawing.Size(325, 24);
            this.layout_Expired.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layout_Expired.Text = "  Ngày hết hạn";
            this.layout_Expired.TextSize = new System.Drawing.Size(71, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btn_Check;
            this.layoutControlItem2.Location = new System.Drawing.Point(511, 0);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(50, 26);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(50, 26);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(50, 26);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.chk_Active;
            this.layoutControlItem3.Location = new System.Drawing.Point(361, 50);
            this.layoutControlItem3.MaxSize = new System.Drawing.Size(200, 24);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(200, 24);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(200, 24);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.lbl_Active;
            this.layoutControlItem4.Location = new System.Drawing.Point(97, 50);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(264, 24);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(264, 24);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(264, 24);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(0, 108);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(422, 26);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.pictureEdit_Avatar;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(97, 98);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(97, 98);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(97, 98);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 98);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(561, 10);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btn_OK;
            this.layoutControlItem5.Location = new System.Drawing.Point(422, 108);
            this.layoutControlItem5.MaxSize = new System.Drawing.Size(139, 26);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(139, 26);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(139, 26);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // txt_ThongTin
            // 
            this.txt_ThongTin.Enabled = false;
            this.txt_ThongTin.Location = new System.Drawing.Point(434, 86);
            this.txt_ThongTin.Name = "txt_ThongTin";
            this.txt_ThongTin.Size = new System.Drawing.Size(135, 20);
            this.txt_ThongTin.TabIndex = 12;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.txt_ThongTin;
            this.layoutControlItem6.Location = new System.Drawing.Point(422, 74);
            this.layoutControlItem6.MaxSize = new System.Drawing.Size(139, 24);
            this.layoutControlItem6.MinSize = new System.Drawing.Size(139, 24);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(139, 24);
            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // frm_InsertKeyAPI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 154);
            this.Controls.Add(this.layoutControl1);
            this.Name = "frm_InsertKeyAPI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thông tin key";
            this.Load += new System.EventHandler(this.frm_InsertKeyAPI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit_Avatar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk_Active.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_FullName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Expired.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layout_API)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layout_FullName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layout_Expired)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private System.Windows.Forms.TextBox txt_ToKenAPI;
        private DevExpress.XtraLayout.LayoutControlItem layout_API;
        private DevExpress.XtraEditors.TextEdit txt_FullName;
        private DevExpress.XtraLayout.LayoutControlItem layout_FullName;
        private DevExpress.XtraLayout.LayoutControlItem layout_Expired;
        private DevExpress.XtraEditors.SimpleButton btn_Check;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.TextEdit txt_Expired;
        private System.Windows.Forms.Label lbl_Active;
        private DevExpress.XtraEditors.CheckEdit chk_Active;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.SimpleButton btn_OK;
        private DevExpress.XtraEditors.PictureEdit pictureEdit_Avatar;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private System.Windows.Forms.TextBox txt_ThongTin;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
    }
}