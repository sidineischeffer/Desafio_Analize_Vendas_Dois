using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio_Analize_Vendas_Dois
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Entre o caminho do arquivo: ");
            string path = Console.ReadLine();

            List<Sale> sales = new List<Sale>();

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] fields = line.Split(',');
                        int month = int.Parse(fields[0]);
                        int year = int.Parse(fields[1]);
                        string seller = fields[2];
                        int items = int.Parse(fields[3]);
                        double total = double.Parse(fields[4], CultureInfo.InvariantCulture);

                        sales.Add(new Sale
                        {
                            Month = month,
                            Year = year,
                            Seller = seller,
                            Items = items,
                            Total = total
                        });
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Erro: {path} (O sistema não pode encontrar o arquivo especificado)");
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
                return;
            }

            // Encontrar nomes únicos de vendedores
            var uniqueSellers = new HashSet<string>(sales.Select(s => s.Seller));

            // Calcular total vendido por cada vendedor
            var totalSalesBySeller = uniqueSellers
                .Select(seller => new
                {
                    Seller = seller,
                    Total = sales.Where(s => s.Seller == seller).Sum(s => s.Total)
                })
                .ToList();

            Console.WriteLine("Total de vendas por vendedor:");
            foreach (var entry in totalSalesBySeller)
            {
                Console.WriteLine($"{entry.Seller} - R$ {entry.Total.ToString("F2", CultureInfo.InvariantCulture)}");
            }
        }
    }
}
