using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPChatServer
{
    public partial class CheckLogs : Form
    {
        public CheckLogs()
        {
            InitializeComponent();
        }

        private void ShowLogs_Click(object sender, EventArgs e)//Метод достаёт логи из базы данных с необходимыми параметрами
        {
            dataGridLog.Rows.Clear();

            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=tcpChat.db; Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = conn.CreateCommand();
                SQLiteCommand cmdInsert = conn.CreateCommand();

                string dateFilter = "1";
                string format = "yyyy-MM-dd HH:mm:ss";
                if (checkDate.Checked)
                {
                    dateFilter = "datetime(datetime) >= datetime('" + dateTimeFrom.Value.ToString(format) + "') AND  datetime(datetime) <= datetime('" + dateTimeTo.Value.ToString(format) + "')";
                }

                string user = "1";
                if(inputUser.Text.Length!=0)
                user = "username='"+inputUser.Text+"'";
                cmd.CommandText = "SELECT * FROM logs where "+ dateFilter + " AND 1";
                try
                {
                    SQLiteDataReader r = cmd.ExecuteReader();
                    string line = String.Empty;
                    while (r.Read())
                    {
                        string[] toGrid = { r["datetime"].ToString(), r["username"].ToString(), r["body"].ToString() };

                        dataGridLog.Rows.Add(toGrid);
                    }
                    r.Close();

                }
                catch (SQLiteException ex)
                {
                     MessageBox.Show(ex.Message);
                }

            }

        }
    }
}
