using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoreServer;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.SQLite;
using System.Runtime.Serialization;

namespace TCPChatServer
{
    public class ClientObject
        {
            protected internal string Id { get; private set; } //id пользователя
            protected internal NetworkStream Stream { get; private set; }//Сетевой поток для передачи
            protected internal string name { get; set; }//Имя пользователя
            string userName;//Имя пользователяX2
            TcpClient client;//Объект tcp клиента
            ServerObject server; //Объект сервера
            public ServerWindow LOG;//Объект формы ServerWindow

            public ClientObject(TcpClient tcpClient, NetworkStream inStream, ServerObject serverObject, ServerWindow log,string inName)//Конструктор объекта класса
            {
                Id = Guid.NewGuid().ToString();
                client = tcpClient;
                Stream = inStream;
                server = serverObject;
                LOG = log;
                name = inName; 
                serverObject.AddConnection(this);
        }

             public void Process()//Метод принятия сообщений и их обработка
            {
            //Отправлка сообщение со списком подключенных пользователей
            CoreServer.Message OnMes = new CoreServer.Message("online", "", "", "", ServerObject.GetClients());
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(this.Stream, OnMes);

                int flagForSign = 0;//Флаг обозначения что пользователь вошёл
            try
                {
                
               string messageString;//Часть строки сообщения для вывода в чат
               string bodyMess ="";//Часть строки сообщения для вывода в чат

                while (true)
                {

                    CoreServer.Message mess = new CoreServer.Message();//Создание объекта сообщения

                    try
                    {
                        mess = GetMessage();//Получение сообщения
                        userName = mess.UserName;
                        this.name = userName;

                        if (mess.type == "mess")//Если тип сообщения "Сообщение"
                        {
                            //Разбор сообщения и вывод его в чат и трансляция всем участникам
                            messageString = String.Format("{0}:{1}:", mess.DateTime, userName);
                            bodyMess = mess.body;
                            LOG.toBody(messageString, bodyMess, 0);
                            bodyMess = "";
                            logToBase(mess);//Логирование сообщения
                            server.BroadcastMessage(mess, this.Id);

                        }

                        else if (mess.type == "signIn")//Если тип сообщения "Вход"
                        {
                            //Обработка входа пользователя в чат. Поиск его в базе и отправкой соответствующего сообщения
                            logToBase(mess);//Логирование сообщения
                            //Подключение к базе и выполнение нужных действий
                            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\tcpChat.db; Version=3;"))
                            {
                                conn.Open();
                                SQLiteCommand cmd = conn.CreateCommand();
                                SQLiteCommand cmdInsert = conn.CreateCommand();


                                cmd.CommandText = "SELECT * FROM users WHERE login='" + mess.UserName + "' AND pass='" + mess.body + "'";
                                try
                                {
                                    SQLiteDataReader r = cmd.ExecuteReader();
                                    if (r.HasRows)
                                    {
                                        CoreServer.Message regAns = new CoreServer.Message("signAnYes", mess.UserName, "Добро пожаловать в чат :)!", "", ServerObject.GetClients());

                                        formatter.Serialize(this.Stream, regAns);
                                        r.Close();
                                        flagForSign = 1;


                                    }
                                    else
                                    {
                                        CoreServer.Message regAns = new CoreServer.Message("signAnNo", mess.UserName, "Такой пользователь не найден или неверен пароль!", "", ServerObject.GetClients());

                                        formatter.Serialize(this.Stream, regAns);
                                        Close();
                                    }

                                }
                                catch (SQLiteException ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }





                            }
                            //ТРанслирования сообщения если пользователь успешно вошёл в чат
                            if (flagForSign == 1)
                                server.BroadcastMessage(mess, this.Id);

                        }
                        else if (mess.type == "regg")//Если тип сообщения "Регистрация"
                        {
                            //Обработка регистрации пользователя в чате. Поиск его в базе и отправкой соответствующего сообщения

                            logToBase(mess);//Логирование сообщения
                            //Подключение к базе и выполнение нужных действий
                            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\tcpChat.db; Version=3;"))
                            {
                                conn.Open();
                                SQLiteCommand cmd = conn.CreateCommand();
                                SQLiteCommand cmdInsert = conn.CreateCommand();


                                cmd.CommandText = "SELECT * FROM users WHERE login='" + mess.UserName + "'";
                                try
                                {
                                    SQLiteDataReader r = cmd.ExecuteReader();
                                    if (r.HasRows)
                                    {
                                        CoreServer.Message regAns = new CoreServer.Message("regansvNo", mess.UserName, "Такой пользователь уже существует!", "", ServerObject.GetClients());
                                        formatter.Serialize(this.Stream, regAns);
                                        r.Close();
                                        Close();
                                    }
                                    else
                                    {
                                        cmdInsert.CommandText = String.Format("INSERT INTO users (login,pass) VALUES('{0}', '{1}');", mess.UserName, mess.body);
                                        cmdInsert.ExecuteNonQuery();
                                        LOG.toBody("Пользователь " + mess.UserName, " успешно зарегестрировался", 1);

                                        CoreServer.Message regAns = new CoreServer.Message("regansvYes", mess.UserName, "Вы успешно зарегистрированы!", "", ServerObject.GetClients());
                                        formatter.Serialize(this.Stream, regAns);

                                    }

                                }
                                catch (SQLiteException ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }

                            }

                        }
                        else if (mess.type == "reqonline") //Если тип сообщения "Запрос онлайна"
                        {
                            //Обработка сообщения запроса онлайн пользователей с последующим ответом
                            CoreServer.Message tempMess;
                            BinaryFormatter tempFprm = new BinaryFormatter();
                            do
                            {
                                tempMess = (CoreServer.Message)tempFprm.Deserialize(Stream);
                            }
                            while (Stream.DataAvailable);
                            tempMess.OnlineUser = ServerObject.GetClients();
                            OnMes = new CoreServer.Message("online", "", "", "", ServerObject.GetClients());
                            formatter.Serialize(this.Stream, OnMes);
                        }

                    }
                    catch
                    {
                        //Отправка сообщения о том, что пользователь вышел из чата
                        messageString = String.Format("{0}:{1} покинул чат!", DateTime.Now.ToString(), userName);
                        bodyMess = "";
                        LOG.toBody(messageString, bodyMess, 1);
                        break;
                    }
                }
            }
                catch (Exception e)
                {
                LOG.toBody(e.Message, null,1);//Вывод исключения в чат
                }
                finally
                {
                    //В случае выхода из цикла удаляется пользователь из списк онлайн и трансляция сообщения выхода пользователя
                server.RemoveConnection(this.Id);
                CoreServer.Message exMes = new CoreServer.Message("exit", userName, "Пользователь вышел из чата", DateTime.Now.ToString(), ServerObject.GetClients());
                logToBase(exMes);//Логирование сообщения
                if (flagForSign == 1)
                    server.BroadcastMessage(exMes, this.Id);
                Close();
                }


            
                
            


        }

        public static void logToBase(CoreServer.Message toLogMes)//Добавление логов в базу
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + Environment.CurrentDirectory + @"\tcpChat.db; Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmdInsert = conn.CreateCommand();
                DateTime tempDate = Convert.ToDateTime(toLogMes.DateTime);
                string format = "yyyy-MM-dd HH:mm:ss";
                cmdInsert.CommandText = String.Format("INSERT INTO logs (datetime,username,body) VALUES('{0}', '{1}','{2}');", tempDate.ToString(format), toLogMes.UserName,toLogMes.body);
                cmdInsert.ExecuteNonQuery();
            }
        }
            //Чтение входящего сообщения и преобразование в объект Message
            private CoreServer.Message GetMessage()
            {
            CoreServer.Message tempMess;
            BinaryFormatter formatter = new BinaryFormatter();
            do
             {
              tempMess = (CoreServer.Message)formatter.Deserialize(Stream);
             }
             while (Stream.DataAvailable);
            tempMess.OnlineUser = ServerObject.GetClients();
                return tempMess;
            }

            //Закрытие подключения
            protected internal void Close()
            {
                if (Stream != null)
                    Stream.Close();
                if (client != null)
                    client.Close();
            }
        }

    }

