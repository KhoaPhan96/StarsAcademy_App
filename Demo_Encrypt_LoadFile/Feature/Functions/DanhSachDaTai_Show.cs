using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Encrypt_LoadFile.Feature.Functions
{
    class DanhSachDaTai_Show
    {
        public static DataTable DanhSachDaTai(string path_XML_Course)
        {
            try
            {
                if (!File.Exists(path_XML_Course))
                    return null;

                DataTable dt_DanhSachKhoaHoc_DaTai = new DataTable("Courses"); //Danh sách Khóa học đã tải
                DataTable dt_DanhSacBaiGiang_DaTai = new DataTable("Lessions"); //Danh sách Khóa học đã tải

                // Đọc tệp XML vào DataSet
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(path_XML_Course);

                DataTable loadedDataTable_KhoaHoc = new DataTable();
                DataTable loadedDataTable_BaiGiang = new DataTable();

                if (dataSet.Tables.Count != 0)
                {
                    #region Danh sách khóa học
                    loadedDataTable_KhoaHoc = dataSet.Tables["Courses"];

                    if (loadedDataTable_KhoaHoc != null)
                    {
                        dt_DanhSachKhoaHoc_DaTai = loadedDataTable_KhoaHoc.Clone();

                        foreach(DataRow dr in loadedDataTable_KhoaHoc.Rows)
                        {
                            dt_DanhSachKhoaHoc_DaTai.ImportRow(dr);
                        }    
                    }
                    else
                    {
                        return null;
                    }
                    #endregion

                    #region Danh sách bài giảng
                    loadedDataTable_BaiGiang = dataSet.Tables["Lessions"];

                    if (loadedDataTable_BaiGiang != null)
                    {
                        dt_DanhSacBaiGiang_DaTai = loadedDataTable_BaiGiang.Clone();

                        foreach (DataRow dr in loadedDataTable_BaiGiang.Rows)
                        {
                            dt_DanhSacBaiGiang_DaTai.ImportRow(dr);
                        }
                    }
                    else
                    {
                        return null;
                    }
                    #endregion

                    DataTable dt_DanhSachDaTai = new DataTable();
                    dt_DanhSachDaTai.Columns.Add("uuid_KhoaHoc", typeof(string));
                    dt_DanhSachDaTai.Columns.Add("title_KhoaHoc", typeof(string));
                    dt_DanhSachDaTai.Columns.Add("description_KhoaHoc", typeof(string));
                    dt_DanhSachDaTai.Columns.Add("uuid_BaiGiang", typeof(string));
                    dt_DanhSachDaTai.Columns.Add("title_BaiGiang", typeof(string));
                    dt_DanhSachDaTai.Columns.Add("description_BaiGiang", typeof(string));
                    dt_DanhSachDaTai.Columns.Add("file_Path", typeof(string));
                    dt_DanhSachDaTai.Columns.Add("image_Path", typeof(string));
                    dt_DanhSachDaTai.Columns.Add("meta_image_Path", typeof(string));

                    foreach(DataRow dr_KhoaHoc in dt_DanhSachKhoaHoc_DaTai.Rows)
                    {
                        foreach (DataRow drBaiGiang in dt_DanhSacBaiGiang_DaTai.Rows)
                        {
                            if(dr_KhoaHoc["uuid"].ToString() == drBaiGiang["uuid_Course"].ToString())
                            {
                                dt_DanhSachDaTai.Rows.Add(dr_KhoaHoc["uuid"].ToString()
                                                          , dr_KhoaHoc["title"].ToString()
                                                          , dr_KhoaHoc["description"].ToString()
                                                          , drBaiGiang["uuid"].ToString()
                                                          , drBaiGiang["title"].ToString()
                                                          , drBaiGiang["description"].ToString()
                                                          , drBaiGiang["file_Path"].ToString()
                                                          , drBaiGiang["image_Path"].ToString()
                                                          , drBaiGiang["meta_image_Path"].ToString()
                                                        );
                            }    
                        }    
                    }

                    return dt_DanhSachDaTai;
                }
                else
                {
                    return null;
                }    
                    
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
