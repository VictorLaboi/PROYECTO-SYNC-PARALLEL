using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Stopwatch stopwatch = new Stopwatch();

        string source = @"C:\Users\Lione\Downloads\PRUEBA DE PROYECTO";

        Console.WriteLine("1: PARALELO \n 2: SINCRONO");
        string option = Console.ReadLine().ToString();
        string letrasBuscadas;       
        do
        {
            Console.WriteLine("Ingresa una o varias letras a buscar.");
            letrasBuscadas = Console.ReadLine();

            if (string.IsNullOrEmpty(letrasBuscadas))
            {
                Console.WriteLine("No se ingresaron letras para buscar: \n");
            }
        } while (string.IsNullOrEmpty(letrasBuscadas));

        char[] letras = letrasBuscadas.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(c => c.Trim().ToLower()[0])
        .ToArray();

        switch (option)
        {
            case "1":

                stopwatch.Start();
                Parallel.ForEach(letras, letrasBuscadas => { Busqueda(source, letrasBuscadas); });
                stopwatch.Stop();
                Console.WriteLine($"\nTiempo (PARALELO): {stopwatch.Elapsed}");
                break;

            case "2":

                stopwatch.Restart();
                foreach (char letraBuscada in letras) {
                    Busqueda(source,letraBuscada);
                }
                stopwatch.Stop();
                Console.WriteLine($"\nTiempo (SINCRONO): {stopwatch.Elapsed}");

                break;

            default:return;
        }
        static void Busqueda(string carpeta, char letraBuscada)
        {
            try
            {
                if (Directory.Exists(carpeta))
                {
                    string[] archivos = Directory.GetFiles(carpeta, "*.txt");

                    foreach (string archivo in archivos)
                    {
                        Procesar(archivo, letraBuscada);
                    }
                }
                else
                {
                    Console.WriteLine("Carpeta inexistente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        static void Procesar(string rutaArchivo, char letraBuscada)
        {
            try
            {
                string contenido = File.ReadAllText(rutaArchivo);
                List<string> palabrasEncontradas = EncontrarPalabras(contenido, letraBuscada);

                if (palabrasEncontradas.Any())
                {
                    int count=0;
                    
                    foreach (string palabra in palabrasEncontradas)
                        {
                        Console.WriteLine($"Palabra: {palabra}");
                        Console.WriteLine($" Archivo: {Path.GetFileName(rutaArchivo)}");
                        count++;
                        }
                    Console.WriteLine($"\nNumero de palabras:{count}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de procesamiento de: {rutaArchivo}: \n{ex.Message}");
            }
        }

        static List<string> EncontrarPalabras(string texto, char letra)
        {   
            string patron = $@"\b\w*{letra}\w*\b";
            return System.Text.RegularExpressions.Regex.Matches(texto, patron, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                .Cast<System.Text.RegularExpressions.Match>()
                .Select(match => match.Value)
                .ToList();
        }
    }
}