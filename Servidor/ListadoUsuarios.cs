
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;


namespace Servidor
{
    
     public class ListadoUsuarios
    {
       
        public static List<Usuario> usuarios = new  List<Usuario>();


        public static string ponerEnCola(Usuario u1)
        {
            string rivalEncontrado = "";
            usuarios.Add(u1);
            Console.WriteLine(usuarios.Count + " usuarios en cola");
            rivalEncontrado= BuscarRival(u1);
            return rivalEncontrado;
        }


       public static void sacarDeLaCola(Usuario usuario)
        {
            foreach (var user in usuarios)
            {
               if(user.usuario.Equals(usuario.usuario))
                {
                    usuarios.Remove(user);          
            break;
                } 
            }  
                 Console.WriteLine(usuarios.Count + " usuarios en cola");
        }

        public static string BuscarRival(Usuario u1)
        {
            string jugador = "";
            string nombre = u1.usuario;
            int min = u1.puntaje - 200;
            int max = u1.puntaje + 200;
            string rival = "";
            Byte[] sendBytes = null;

            while (rival == "")
            {
                foreach (var usuario in usuarios)
                {
                    if (usuario.puntaje >= min && usuario.puntaje <= max && usuario.usuario != nombre)
                    {
                        TcpClient conexion = usuario.conexion;

                        usuario.conexion = null;
                        rival = JsonConvert.SerializeObject(usuario);
                        Console.WriteLine(rival + "imprimir rival");

                        u1.conexion = null;
                        jugador = JsonConvert.SerializeObject(u1);
                        Console.WriteLine(jugador + "imprimir jugador");

                        string serverResponse = jugador;
                        sendBytes = System.Text.Encoding.ASCII.GetBytes(serverResponse);
                        byte[] intBytes = BitConverter.GetBytes(sendBytes.Length);
                        NetworkStream networkStream = conexion.GetStream();
                        networkStream.Write(intBytes, 0, intBytes.Length);
                        networkStream.Write(sendBytes, 0, sendBytes.Length);
                        networkStream.Flush();
                    }
                }
            }
            return rival;
        }
    }
    
}
