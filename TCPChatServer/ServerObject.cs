using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPChatServer
{
    public class ServerObject
    {
        static TcpListener tcpListener; //сервер для прослушивания tcp подключений
        static public List<ClientObject> clients = new List<ClientObject>(); //Список подключенных пользователей

        ServerWindow FROM;//Объект формы ServerWindow

        public  static List<string>  GetClients()//Полученает и возвращает список подключенных пользователей
        {
            List<string> tempOnline = new List<string>();
            foreach (var item in clients)
            {
                if(item.name != null)
                tempOnline.Add(item.name);
                
            }
            
            return tempOnline;
        }

     
        public void form(ServerWindow ff)//Метод для инициализации объекта формы
        {
            this.FROM = ff;
        }

        protected internal void AddConnection(ClientObject clientObject)//Добавление подключенных пользователей в List
        {
            clients.Add(clientObject);
            FROM.onlineList(clients);
        }

        protected internal void RemoveConnection(string id)//Удаление пользователей из списка по id
        {
            ClientObject client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
                clients.Remove(client);
            FROM.onlineList(clients);
        }

       
        protected internal void Listen()//Прослушивание всех входящих соединений
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 8888);//Инициализция объекта прослушивания tcp подключений с последующим стартом
                tcpListener.Start();
                FROM.toBody("Сервер запущен. Ожидание подключений...",null,1);

                while (true)//Цикл принятия подключений
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();//Объект подключенного tcp клиента

                    CoreServer.Message ForNameUser;//Объект сообщений

                    NetworkStream Stream = tcpClient.GetStream();//Объект сетевого потока
                    IFormatter formatter = new BinaryFormatter();//Интерфейс/объект для сериализации и десериализации объектов

                    ForNameUser = (CoreServer.Message)formatter.Deserialize(Stream);//Десериализация объекта Message
                    if (ForNameUser.type == "connection")//Обработка сообщения с запросом на подключение к серверу
                    {
                        //Создание объекта клиента и запуск потока прослушивания сообщений от клиента
                        ClientObject clientObject = new ClientObject(tcpClient, Stream, this, FROM, ForNameUser.UserName);

                        Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                        clientThread.Start();

                    }
                    
                }
            }
            catch (Exception ex)
            {
                FROM.toBody(ex.Message,null,1);//Вывод текста исключения
                Disconnect();
            }
        }

        //Трансляция подключенным пользователям
        protected internal void BroadcastMessage(CoreServer.Message message, string id)
        {
            FROM.onlineList(clients);
            message.OnlineUser = GetClients();
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id) // если id клиента не равно id отправляющего сообщение
                {

                    IFormatter formatter = new BinaryFormatter();//Сериализация объекта Mtssage в сетевой поток
                    formatter.Serialize(clients[i].Stream, message);
                }
            }
        }

        //Отключение сервера и отключение всех присоединённых пользователей
        protected internal void Disconnect()
        {
            tcpListener.Stop(); //Остановка сервера

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //Отключение пользователя
            }
        }
    }

}
