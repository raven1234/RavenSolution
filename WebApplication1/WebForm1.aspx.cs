//#define FlagA
//#define FlagB
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        
        private SqlConnection sqlConn;
        private SqlCommand sqlCmd;
        private SqlDataReader sqlReader;
        private SqlDataAdapter sqlAdapter;
        private DataSet dataSet;
        private long startTime;
        private long endTime;
        private long costTime;
        private mylib lib;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlConn = new SqlConnection("Server=localhost;Database=Customers;Trusted_Connection=True;");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            #region Debug Mode Congiguration
#if (FlagA && FlagB)
            Label1.Text = "Testing";
#elif (FlagA && !FlagB)
            Label1.Text = "Testing2";
#elif (!FlagA && FlagB)
            Label1.Text = "Testing3";
#else
            Label1.Text = "Testing4";
#endif
            #endregion
            //Summary test
            lib = new mylib();
            lib.GetCount();
            startTime = DateTime.Now.Ticks/TimeSpan.TicksPerMillisecond;
            sqlConn.Open();

            #region Sql connection mode checking
            if (CheckBox1.Checked)
            {
                sqlAdapter = new SqlDataAdapter("select * from customers",sqlConn);
                DataSet dataSet = new DataSet();
                sqlAdapter.Fill(dataSet);
                DataTable table = dataSet.Tables[0];
                foreach (DataRow row in table.Rows)
                {
                    int field1 = row.Field<int>(0);
                    string field2 = row.Field<string>(1);
                    InsertNewTableRow(field1.ToString(),field2);
                }
                

            }
            else
            {
                sqlCmd = new SqlCommand("select * from customers", sqlConn);
                sqlReader = sqlCmd.ExecuteReader();
                while (sqlReader.Read())
                {
                    string c1 = sqlReader["CustomerID"].ToString();
                    string c2 = sqlReader["CustomerName"].ToString();
                    InsertNewTableRow(c1, c2);
                }
                sqlReader.Close();
            }
            #endregion

            CalculateMemory();
            endTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            costTime = endTime - startTime;
            Label2.Text = costTime.ToString() + " ms";
            
            sqlConn.Close();
        }

        private void InsertNewTableRow(string cellVal1,string cellVal2)
        {
            TableRow row = new TableRow();

            TableCell cell1 = new TableCell();
            cell1.Text = cellVal1;
            row.Cells.Add(cell1);

            TableCell cell2 = new TableCell();
            cell2.Text = cellVal2;
            row.Cells.Add(cell2);

            Table1.Rows.Add(row);
        }

        private void CalculateMemory()
        {
            long memorySize = Process.GetCurrentProcess().WorkingSet64 / 1024;
            Label3.Text = memorySize.ToString() + " KB";
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            CalculateMemory();
        }
    }
}