using Fleck;

namespace ServerWebSockets
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            int port = 9001;
            string ipServidor = $"ws://{ip}:{port}";

            WebSocketServer server = new WebSocketServer(ipServidor);
            List<IWebSocketConnection> clienteConexiones = new List<IWebSocketConnection>();
            Console.WriteLine("Inicio el servidor Web Socket");
            server.Start(cliente =>
            {
                cliente.OnOpen = () =>
                {
                    clienteConexiones.Add(cliente);

                    string info = "Se conecto un cliente con Ip " + cliente.ConnectionInfo.ClientIpAddress;
                    Console.WriteLine(info);
                };
                cliente.OnMessage = (mensaje) =>
                {
                    try
                    {
                        clienteConexiones.ForEach(p => p.Send(mensaje));
                        //Console.WriteLine(" Se notifico al socket el mensaje " + mensaje);
                        // Get the JSON object from the server.

                        foreach (var clientSocket in clienteConexiones)
                        {
                            clientSocket.Send(mensaje);
                        }

                    }
                    catch 
                    {
                        Console.WriteLine("error  al enviar mensaje");

                    }
                };
                cliente.OnClose = () =>
                {
                    clienteConexiones.Remove(cliente);
                    Console.WriteLine("Se desconecto el cliente con Ip " + cliente.ConnectionInfo.ClientIpAddress);
                };

            });
            Console.ReadLine();
        }
    }
}