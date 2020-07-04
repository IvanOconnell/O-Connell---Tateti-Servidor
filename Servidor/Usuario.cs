using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Servidor
{
   public class Usuario
    {
        public int id { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public int puntaje { get; set; }
        public int partidasJugadas { get; set; }
        public int partidasGanadas { get; set; }
        public TcpClient conexion { get; set; }


        public Usuario(int id, string usuario, string contrasena, int puntaje,int partidasJugadas, int partidasGanadas)
        {
            this.id = id;
            this.usuario = usuario;
            this.contrasena = contrasena;
            this.puntaje = puntaje;
            this.partidasJugadas = partidasJugadas;
            this.partidasGanadas = partidasGanadas;
            this.conexion = null;

        }
        public void setConexion(TcpClient conexion)
        {
            this.conexion = conexion;
        }
    }
}
