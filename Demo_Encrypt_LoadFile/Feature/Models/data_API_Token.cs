using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Encrypt_LoadFile.Feature.Models
{
    class data_API_Token
    {
        public async static Task<DataTable> GetData_API_Token(string folderPath_XML)
        {            
            DataSet dataSet = new DataSet();
            // Đọc tệp XML vào DataSet
            dataSet.ReadXml(folderPath_XML);

            DataTable loadedDataTable = new DataTable();
            try
            {
                loadedDataTable = dataSet.Tables[0];
            }
            catch (Exception ex)
            {

            }

            return loadedDataTable;
        }
    }
}
