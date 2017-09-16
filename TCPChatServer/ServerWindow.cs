using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;

namespace TCPChatServer
{
    public partial class ServerWindow : Form
    {
        ServerWindow TO;

        public ServerWindow()
        {
            InitializeComponent();
            TO = this;
        }

        
        static ServerObject server; // объект сервера
        static Thread listenThread; // поток для прослушивания подключений по tcp

        private void StartSRV_Click(object sender, EventArgs e) //Обработчик нажатия кнопки запуска сервера
        {
            statSRV.Text = "Запущен.";
            StartSRV.Enabled = false;
            StopSRV.Enabled = true;

                try
                {
                //Инициализация объекта сервера с дальнейшей инициализацией объекта потока прослушивания tcp подключений
                    server = new ServerObject();
                    server.form(TO);
                    listenThread = new Thread(new ThreadStart(server.Listen));
                    listenThread.Start(); 
                }
                catch (Exception ex)
                {
                //В случае исключения, вывод текста исключения в поле лога сервера
                server.Disconnect();
                toBody(ex.Message, null,0);
                }
            
        }

        public void toBody(string begin, string body,int f)//Вывод в поле лога сервера
        {
            logBody.Invoke(new Action(() =>
            {   
                logBody.SelectionFont = new Font(logBody.Font.FontFamily, this.Font.Size, FontStyle.Bold);
                logBody.AppendText(begin);
                if(f == 0){ 
                logBody.SelectionFont = new Font(logBody.Font.FontFamily, this.Font.Size, FontStyle.Regular);
            }
                logBody.AppendText(body + Environment.NewLine);
                logBody.ScrollToCaret(); }));
        }

        public void onlineList(List<ClientObject> clients) //Вывод списка подключенных пользователей в Listbox
        {
            onlineUser.Invoke(new Action(() => {
                onlineUser.Items.Clear();

            foreach (var item in clients)
            {
                   onlineUser.Items.Add(item.name);
                }
                }));
        }

        private void StopSRV_Click(object sender, EventArgs e)//Обработчик нажатия кнопки остановки сервера
        {
            //Остановка потока прослушивания подключений и разъединение всех подключений
            if (listenThread != null)
                listenThread.Suspend();
                server.Disconnect();
                statSRV.Text = "Остановлен.";
                StartSRV.Enabled = true;
                StopSRV.Enabled = false;
            
        }

        private void ServerWindow_FormClosed(object sender, FormClosedEventArgs e)//Обработчик закрытий формы
        {
            //При закрытии формы поток прослушивания останавливается и разъединяются все подключения, далее выход из приложения
            if(listenThread != null)
            listenThread.Suspend();
            if (server != null)
            server.Disconnect();
            Environment.Exit(0); //завершение процесса
        }

        private void ShowLogs_Click(object sender, EventArgs e)//Обработчик нажатия кнопки открытия формы просмотра логов
        {
            CheckLogs logs = new CheckLogs();
            logs.Visible = true;
        }
    }

}
