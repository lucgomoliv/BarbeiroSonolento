using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BarbeiroSonolento
{
    class Program
    {
        static string cabecalho()
        {
            StringBuilder aux = new StringBuilder();
            aux.AppendLine("Integrantes:")
                .AppendLine("Patryck Kenny Pereira de Paiva - 645684")
                .AppendLine("Ana Luiza Gonçalves Lourenço Barros - 650193")
                .AppendLine("Vinicius de Castro - 643297")
                .AppendLine("Victor Henrique de Souza Oliveira - 643287")
                .AppendLine("Douglas Barbosa da Silva - 539301")
                .AppendLine("Lucas Alves Costa de Souza Araujo - 641119")
                .AppendLine("Daniel de Pinho Matos - 287404")
                .AppendLine("Wernen Rodrigues - 597704")
                .AppendLine("Matheus Vinícius Nascimento - 685670")
                .AppendLine("Lucas Gomes Oliveira - 667357")
                .AppendLine("Alex Barros Vasconcelos - 663877");
            return aux.ToString();
        }

        //Variaveis de controle
        static int frequenciaClientes, frequenciaAtendimento;

        static Queue<Thread> fila;
        static Thread barbeiro;
        static AutoResetEvent esperarChegar;
        static AutoResetEvent esperarAtender;
        static System.Timers.Timer timer;
        static int numClientesChegado, numClientesAtendidos;
        static void Main(string[] args)
        {
            Console.WriteLine(cabecalho());
            frequenciaAtendimento = 10000;
            frequenciaClientes = 10000;

            numClientesChegado = 1;
            numClientesAtendidos = 1;
            fila = new Queue<Thread>();
            barbeiro = new Thread(dormir);
            Random rnd = new Random();
            timer = new System.Timers.Timer() { Interval = rnd.Next(100, frequenciaClientes) };
            timer.Elapsed += Timer_Elapsed;
            esperarChegar = new AutoResetEvent(false);
            esperarAtender = new AutoResetEvent(true);
            barbeiro.Start();
            timer.Start();
        }

        static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Random rnd = new Random();
            timer.Interval = rnd.Next(100, frequenciaClientes);
            entrarCliente();
        }

        static void entrarCliente()
        {
            Thread cliente;
            if (fila.Count < 5)
            {
                cliente = new Thread(esperar);
                fila.Enqueue(cliente);
                cliente.Start();
                Console.WriteLine("O cliente número " + numClientesChegado + " chegou!");
                numClientesChegado++;
            }
            else Console.WriteLine("Salão cheio!");
        }

        static void esperar()
        {
            esperarChegar.Set();
            esperarAtender.WaitOne();
        }

        static void atender()
        {
            Console.WriteLine("Barbeiro atendendo o cliente " + numClientesAtendidos + "!");
            fila.Dequeue();
            Random rnd = new Random();
            Thread.Sleep(rnd.Next(100, frequenciaAtendimento));
            esperarAtender.Set();
            Console.WriteLine("Atendimento terminado!");
            numClientesAtendidos++;
            dormir();
        }

        static void dormir()
        {
            if(fila.Count == 0) Console.WriteLine("Barbeiro dormindo...");
            esperarChegar.WaitOne();
            atender();
        }
    }
}
