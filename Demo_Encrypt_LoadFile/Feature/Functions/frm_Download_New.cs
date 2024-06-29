using Demo_Encrypt_LoadFile.Feature.API;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Encrypt_LoadFile.Feature.Functions
{
    class frm_Download_New
    {

        /// <summary>
        /// Tải file về máy (Chia làm 2 thư mục: khóa học - bài giảng)
        /// </summary>
        /// <param name="path_Course">Thư mục khóa học</param>
        /// <param name="path_XML_Course">File lưu thông tin khóa học</param>
        /// <param name="path_Lession">Thư mục bài giảng</param>
        /// <param name="path_XML_Lession">File lưu thông tin bài giảng</param>
        /// <param name="dt_DanhSachKhoaHoc">Danh sách khóa học</param>
        /// <param name="dt_DanhSachBaiGiang">Danh sách bài giảng</param>
        /// <returns></returns>
        public static async Task Download_New(string path_Course, string path_XML_Course, string path_Lession, string path_XML_Lession, DataTable dt_DanhSachKhoaHoc, DataTable dt_DanhSachBaiGiang)
        {
            try
            {
                #region Thư mục chứa khóa học
                if (!Directory.Exists(path_Course))
                {
                    Directory.CreateDirectory(path_Course);
                }
                #endregion

                #region Thư mục chứa bài giảng
                if (!Directory.Exists(path_Lession))
                {
                    Directory.CreateDirectory(path_Lession);
                }
                #endregion

                DataTable dt_DanhSachKhoaHoc_Download = new DataTable("Courses");
                DataTable dt_DanhSachKhoaHoc_XML = new DataTable("Courses");
                dt_DanhSachKhoaHoc_Download = dt_DanhSachKhoaHoc.Clone();
                dt_DanhSachKhoaHoc_XML = dt_DanhSachKhoaHoc.Clone();
                dt_DanhSachKhoaHoc_XML.PrimaryKey = new DataColumn[] { dt_DanhSachKhoaHoc_XML.Columns["uuid"] };

                DataTable dt_DanhSachBaiGiang_Download = new DataTable("Lessions");
                DataTable dt_DanhSachBaiGiang_XML = new DataTable("Lessions");
                dt_DanhSachBaiGiang_Download = dt_DanhSachBaiGiang.Clone();
                dt_DanhSachBaiGiang_XML = dt_DanhSachBaiGiang.Clone();
                dt_DanhSachBaiGiang_XML.PrimaryKey = new DataColumn[] { dt_DanhSachBaiGiang_XML.Columns["uuid"] };

                #region File XML

                DataSet dataSet = new DataSet("DataSet");

                if (File.Exists(path_XML_Course)) //Lấy dữ liệu từ dataset vào datatable
                {
                    // Đọc tệp XML vào DataSet
                    //dt_DanhSachKhoaHoc_XML.ReadXml(path_XML_Course);
                    dataSet.ReadXml(path_XML_Course);

                    #region Khóa học
                    dt_DanhSachKhoaHoc_XML = dataSet.Tables["Courses"];

                    if (dt_DanhSachKhoaHoc_XML != null)
                    {
                        dt_DanhSachKhoaHoc_XML.PrimaryKey = new DataColumn[] { dt_DanhSachKhoaHoc_XML.Columns["uuid"] };
                    }
                    else
                    {
                        dt_DanhSachKhoaHoc_XML = dt_DanhSachKhoaHoc.Clone();
                        dt_DanhSachKhoaHoc_XML.PrimaryKey = new DataColumn[] { dt_DanhSachKhoaHoc_XML.Columns["uuid"] };
                    }
                    #endregion



                    #region Bài giảng
                    dt_DanhSachBaiGiang_XML = dataSet.Tables["Lessions"];

                    if (dt_DanhSachBaiGiang_XML != null)
                    {
                        dt_DanhSachBaiGiang_XML.PrimaryKey = new DataColumn[] { dt_DanhSachBaiGiang_XML.Columns["uuid"] };
                    }
                    else
                    {
                        dt_DanhSachBaiGiang_XML = dt_DanhSachBaiGiang.Clone();
                        dt_DanhSachBaiGiang_XML.PrimaryKey = new DataColumn[] { dt_DanhSachBaiGiang_XML.Columns["uuid"] };
                    }

                    #endregion
                }
                else
                {
                    DataTable temp_Course = new DataTable("Course");
                    temp_Course.PrimaryKey = new DataColumn[] { temp_Course.Columns["uuid"] };

                    DataTable temp_Lession = new DataTable("Lession");
                    temp_Lession.PrimaryKey = new DataColumn[] { temp_Lession.Columns["uuid"] };

                    dataSet.Tables.Add(temp_Course);
                    dataSet.Tables.Add(temp_Lession);

                    dataSet.WriteXml(path_XML_Course);
                }

                #endregion

                #region |Course|
                DataRow[] filteredRows = dt_DanhSachKhoaHoc.Select("IsDownloaded = '" + false + "'");
                foreach (DataRow row in filteredRows)
                {
                    dt_DanhSachKhoaHoc_Download.ImportRow(row);
                }

                if(dt_DanhSachKhoaHoc_Download.Rows.Count == 0)
                {
                    //
                }    
                else
                {
                    
                    //Distinct
                    DataTable distinctTable_Course = dt_DanhSachKhoaHoc_Download.Clone();
                    HashSet<string> seenValues = new HashSet<string>();
                    foreach (DataRow row in dt_DanhSachKhoaHoc_Download.Rows)
                    {
                        string value = row["uuid"].ToString();
                        if (seenValues.Add(value))
                        {
                            distinctTable_Course.ImportRow(row);
                        }
                    }

                    foreach (DataRow dr in distinctTable_Course.Rows)
                    {
                        string image_Name_Path_Encode = null;
                        string meta_image_Name_Path_Encode = null;

                        #region image
                        image_Name_Path_Encode = await frm_Extention.Download_Image(dr["image_url"].ToString(), dr["uuid"].ToString(), path_Course);
                        #endregion

                        #region meta_image
                        meta_image_Name_Path_Encode = await frm_Extention.Download_Image(dr["meta_image_url"].ToString(), dr["uuid"].ToString(), path_Course);
                        #endregion

                        #region Bỏ - Nhưng chạy vẫn đúng nha

                        //#region image
                        //string image_Name_temp = null;
                        //string image_Name_KhongDau = null;
                        //string image_Name_Path = null;
                        //string image_Name_Path_Encode = null;

                        //if (dr["image_url"].ToString() != null)
                        //{
                        //    image_Name_temp = dr["uuid"].ToString() + "_" + Path.GetFileName(dr["image_url"].ToString());
                        //    image_Name_KhongDau = frm_Extention.RemoveDiacritics(image_Name_temp); //Chuyển sang tên file không dấu
                        //    image_Name_Path = path_Course + @"\" + image_Name_KhongDau; //Nơi lưu file ảnh
                        //    image_Name_Path_Encode = Encrypt_FileName.EncodeFileNameInPath(image_Name_Path + ".enc"); //Nơi lưu file ảnh đã mã hóa

                        //    //Lưu về máy
                        //    byte[] imageBytes_img_Course = await CallAPI.DownloadImageAsync(dr["image_url"].ToString());
                        //    using (MemoryStream stream = new MemoryStream(imageBytes_img_Course))
                        //    using (var fileStream = new System.IO.FileStream(image_Name_Path_Encode, System.IO.FileMode.Create))
                        //    {
                        //        await stream.CopyToAsync(fileStream);
                        //    }
                        //}
                        //#endregion

                        //#region meta_image
                        //string meta_image_Name_Path_Encode = null;
                        //string meta_image_Name_temp = null;
                        //string meta_image_Name_KhongDau = null;
                        //string meta_image_Name_Path = null;

                        //if (dr["meta_image_url"].ToString() != null)
                        //{
                        //    meta_image_Name_temp = dr["uuid"].ToString() + "_" + Path.GetFileName(dr["meta_image_url"].ToString());
                        //    meta_image_Name_KhongDau = frm_Extention.RemoveDiacritics(meta_image_Name_temp); //Chuyển sang tên file không dấu
                        //    meta_image_Name_Path = path_Course + @"\" + meta_image_Name_KhongDau; //Nơi lưu file ảnh
                        //    meta_image_Name_Path_Encode = Encrypt_FileName.EncodeFileNameInPath(meta_image_Name_Path + ".enc"); //Nơi lưu file ảnh đã mã hóa

                        //    //Lưu về máy
                        //    byte[] imageBytes_img_Course = await CallAPI.DownloadImageAsync(dr["meta_image_url"].ToString());
                        //    using (MemoryStream stream = new MemoryStream(imageBytes_img_Course))
                        //    using (var fileStream = new System.IO.FileStream(meta_image_Name_Path_Encode, System.IO.FileMode.Create))
                        //    {
                        //        await stream.CopyToAsync(fileStream);
                        //    }
                        //}
                        //#endregion

                        #endregion

                        #region Thêm dữ liệu vào file XML

                        DataRow existingRow = dt_DanhSachKhoaHoc_XML.Rows.Find(dr["uuid"].ToString());

                        if (existingRow == null)
                        {
                            // Thêm mới
                            DataRow row = dt_DanhSachKhoaHoc_XML.NewRow();
                            row["id"] = dr["id"];
                            row["title"] = dr["title"] == null ? "" : dr["title"];
                            row["description"] = dr["description"] == null ? "" : dr["description"];
                            row["uuid"] = dr["uuid"];
                            row["image_url"] = dr["image_url"] == null ? "" : dr["image_url"];
                            row["image_Path"] = image_Name_Path_Encode == null ? "" : image_Name_Path_Encode;
                            //row["image_Show"] = null;
                            row["meta_image_url"] = dr["meta_image_url"] == null ? "" : dr["meta_image_url"];
                            row["meta_image_Path"] = meta_image_Name_Path_Encode == null ? "" : meta_image_Name_Path_Encode;
                            //row["meta_image_Show"] = null;
                            row["IsDownloaded"] = true;
                            //row["IsSelected"] = dr["IsSelected"];

                            dt_DanhSachKhoaHoc_XML.Rows.Add(row);
                        }
                        else
                        {
                            // Cập nhật
                            existingRow["image_url"] = dr["image_url"];
                            existingRow["image_Path"] = image_Name_Path_Encode;
                           // existingRow["image_Show"] = null;
                            existingRow["meta_image_url"] = dr["meta_image_url"];
                            existingRow["meta_image_Path"] = meta_image_Name_Path_Encode;
                            //existingRow["meta_image_Show"] = null;
                            existingRow["IsDownloaded"] = true;
                        }

                        #endregion
                    }

                    foreach(DataRow dr_Save in dt_DanhSachKhoaHoc_XML.Rows)
                    {
                        dr_Save["image_url"] = "";
                        dr_Save["meta_image_url"] = "";

                    }    

                    //Nếu dataset chưa có bảng này thì add thêm
                    if (!dataSet.Tables.Contains("Courses"))
                    {
                        dataSet.Tables.Add(dt_DanhSachKhoaHoc_XML);
                    }
                    dataSet.WriteXml(path_XML_Course);



                }

                #endregion

                #region |Lession|
                DataRow[] filteredRows_Lession = dt_DanhSachBaiGiang.Select("IsDownloaded = '" + false + "'");
                foreach (DataRow row in filteredRows_Lession)
                {
                    dt_DanhSachBaiGiang_Download.ImportRow(row);
                }

                if (dt_DanhSachKhoaHoc_Download.Rows.Count == 0)
                {
                    //
                }
                else
                {
                    //Distinct
                    DataTable distinctTable_Lession = dt_DanhSachBaiGiang_Download.Clone();
                    HashSet<string> seenValues = new HashSet<string>();
                    foreach (DataRow row in dt_DanhSachBaiGiang_Download.Rows)
                    {
                        string value = row["uuid"].ToString();
                        if (seenValues.Add(value))
                        {
                            distinctTable_Lession.ImportRow(row);
                        }
                    }

                    foreach (DataRow dr in distinctTable_Lession.Rows)
                    {
                        string image_Name_Path_Encode = null;
                        string meta_image_Name_Path_Encode = null;
                        string file_Path_Encode = null;
                     
                        #region image
                        if (dr["image_url"].ToString() != "")
                        {
                            image_Name_Path_Encode = await frm_Extention.Download_Image(dr["image_url"].ToString(), dr["uuid"].ToString(), path_Lession);
                        }
                        #endregion

                        #region meta_image
                        if (dr["meta_image_url"].ToString() != "")
                        {
                            meta_image_Name_Path_Encode = await frm_Extention.Download_Image(dr["meta_image_url"].ToString(), dr["uuid"].ToString(), path_Lession);
                        }
                        #endregion

                        #region file
                        if (dr["file"].ToString() != "")
                        {
                            file_Path_Encode = await frm_Extention.Download_File(dr["file"].ToString(), dr["uuid"].ToString(), path_Lession);
                        }
                        #endregion

                        #region Thêm dữ liệu vào file XML
                        if (dt_DanhSachBaiGiang_XML == null)
                        {
                            // Thêm mới
                            DataRow row = dt_DanhSachBaiGiang_XML.NewRow();
                            row["uuid_Course"] = dr["uuid_Course"];
                            row["id"] = dr["id"];
                            row["uuid"] = dr["uuid"];
                            row["title"] = dr["title"] == null ? "" : dr["title"];
                            row["description"] = dr["description"] == null ? "" : dr["description"];
                            row["image_url"] = dr["image_url"] == null ? "" : dr["image_url"];
                            row["image_Path"] = image_Name_Path_Encode == null ? "" : image_Name_Path_Encode;
                            //row["image_Show"] = null;
                            row["meta_image_url"] = dr["meta_image_url"] == null ? "" : dr["meta_image_url"];
                            row["meta_image_Path"] = meta_image_Name_Path_Encode == null ? "" : meta_image_Name_Path_Encode;
                            // row["meta_image_Show"] = null;
                            row["file"] = "";
                            row["file_Path"] = file_Path_Encode == null ? "" : file_Path_Encode;
                            row["IsDownloaded"] = true;
                            //row["IsSelected"] = dr["IsSelected"];

                            dt_DanhSachBaiGiang_XML.Rows.Add(row);
                        }
                        else
                        {

                            DataRow existingRow = dt_DanhSachBaiGiang_XML.Rows.Find(dr["uuid"].ToString());

                            if (existingRow == null)
                            {
                                // Thêm mới
                                DataRow row = dt_DanhSachBaiGiang_XML.NewRow();
                                row["uuid_Course"] = dr["uuid_Course"];
                                row["id"] = dr["id"];
                                row["uuid"] = dr["uuid"];
                                row["title"] = dr["title"] == null ? "" : dr["title"];
                                row["description"] = dr["description"] == null ? "" : dr["description"];
                                row["image_url"] = dr["image_url"] == null ? "" : dr["image_url"];
                                row["image_Path"] = image_Name_Path_Encode == null ? "" : image_Name_Path_Encode;
                                //row["image_Show"] = null;
                                row["meta_image_url"] = dr["meta_image_url"] == null ? "" : dr["meta_image_url"];
                                row["meta_image_Path"] = meta_image_Name_Path_Encode == null ? "" : meta_image_Name_Path_Encode;
                                // row["meta_image_Show"] = null;
                                row["file"] = "";
                                row["file_Path"] = file_Path_Encode == null ? "" : file_Path_Encode;
                                row["IsDownloaded"] = true;
                                //row["IsSelected"] = dr["IsSelected"];

                                dt_DanhSachBaiGiang_XML.Rows.Add(row);
                            }
                            else
                            {
                                // Cập nhật
                                existingRow["image_url"] = dr["image_url"] == null ? "" : dr["image_url"];
                                existingRow["image_Path"] = image_Name_Path_Encode == null ? "" : image_Name_Path_Encode;
                                //existingRow["image_Show"] = null;
                                existingRow["meta_image_url"] = dr["meta_image_url"] == null ? "" : dr["meta_image_url"];
                                existingRow["meta_image_Path"] = meta_image_Name_Path_Encode == null ? "" : meta_image_Name_Path_Encode;
                                //existingRow["meta_image_Show"] = null;
                                existingRow["file_Path"] = file_Path_Encode == null ? "" : file_Path_Encode; ;
                                existingRow["IsDownloaded"] = true;
                            }
                        }
                        #endregion
                    }

                    foreach(DataRow dr in dt_DanhSachBaiGiang_XML.Rows)
                    {
                        dr["image_url"] = "";
                        dr["meta_image_url"] = "";
                    }    

                    //Nếu dataset chưa có bảng này thì add thêm
                    if (!dataSet.Tables.Contains("Lessions"))
                    {
                        dataSet.Tables.Add(dt_DanhSachBaiGiang_XML);
                    }
                    dataSet.WriteXml(path_XML_Course);

                }

                #endregion
                }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
