using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;
using System.Collections;

namespace Servidor
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Iniciado servidor");

            TcpListener serverSocket = new TcpListener(8000);

            serverSocket.Start();
            while (true)
            {
                TcpClient clientSocket = default(TcpClient);
                clientSocket = serverSocket.AcceptTcpClient();
                HandlerClient client = new HandlerClient();
                client.startClient(clientSocket);
            }

        }
    }

    public class HandlerClient
    {
        ArrayList  usuarios = new ArrayList();
        Usuario usuario;


        TcpClient clientSocket;
        
        public void startClient(TcpClient clientSocket)
        {
           
            this.clientSocket = clientSocket;
            Thread threadClient = new Thread(doChat);
            threadClient.Start();

        }

        private void doChat()
        {
            
            
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;
            Usuario usuario = null;
            

            while (true)
            {
                byte[] bytesFrom = new byte[4];
               
                // Recibi mensaje
                NetworkStream networkStream = clientSocket.GetStream();
                networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                int buffersize = BitConverter.ToInt32(bytesFrom, 0);
                bytesFrom = new byte[buffersize];
                networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                
                if (dataFromClient.Contains("usuario"))
                {
                     usuario = JsonConvert.DeserializeObject<Usuario>(dataFromClient);
                     usuario.setConexion(clientSocket);
                }
                else
                {
                    mensajeAlCliente(recibirMensaje(dataFromClient,usuario));
                }


                //enviar mensajes

                void mensajeAlCliente(string mensaje)
                {
                    serverResponse = mensaje;
                    sendBytes = System.Text.Encoding.ASCII.GetBytes(serverResponse);
                    byte[] intBytes = BitConverter.GetBytes(sendBytes.Length);
                    networkStream.Write(intBytes, 0, intBytes.Length);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                }

            }

            
        }
        public string recibirMensaje(string msj, Usuario usuario)
        {
            
            string mensaje = "";
            bool abrimos = false;
            if (msj.Equals("c"))
            {
                string rival = ListadoUsuarios.ponerEnCola(usuario);
                if(rival == "")
                {
                mensaje = "estas en cola buscando rival";
                    Console.WriteLine(mensaje);
                }
                else if (rival != "")
                {
                    mensaje = rival;
                    usuario.setConexion(clientSocket);
                    abrimos = true;
               // if(abrimos == true)
                   // {
                 //       ListadoUsuarios.sacarDeLaCola(usuario);
                //   }    
                }
            }else if (msj.Equals("d"))
            {
                ListadoUsuarios.sacarDeLaCola(usuario);
                mensaje = "saliste de la cola";
                Console.WriteLine(mensaje);
            }
            
            return mensaje;
           
        }
        
        
    }
}
