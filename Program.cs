﻿//Por Kleber Vargas
using System;

namespace CalculoFinanciamento
{
    public class Program
    {
        private static double juros;
        private static double amortizacao;
        private static double prestacao;
        private static double saldoDevedor;
        private static double futureValue = 0;
        private static double totalParcela = 0;
        private static double totalAmortizacao = 0;
        private static double totalJuros = 0;
        private static DateTime Data = DateTime.Today;
        static void Main(string[] args)
        {
                Console.WriteLine("CÁLCULO FINANCIAMENTO");
                Console.WriteLine("");
                iniciarFinanciamento();
        }

        private static void iniciarFinanciamento()
        {
            string resposta = TipoCalculoFin();
            if (resposta == "1" || resposta == "2")
            {
                Console.Write("Digite o valor do Financiamento: R$ ");
                double valorFinanciamento = double.Parse(Console.ReadLine());

                Console.Write("Digite o valor da Entrada: R$ ");
                double valorEntrada = double.Parse(Console.ReadLine());

                Console.Write("Digite a quantidade de Parcelas: ");
                int numParcelas = int.Parse(Console.ReadLine());

                Console.Write("Digite 1 para taxa anual e 2 para taxa mensal: ");
                int taxaAM = int.Parse(Console.ReadLine());

                Console.Write("Digite a taxa de juros: ");
                double taxaJuros = double.Parse(Console.ReadLine());

                if (resposta == "1")
                {
                    Console.WriteLine();
                    Console.WriteLine(" -- Cálculo Método SAC --");
                    calcularSac(valorFinanciamento, valorEntrada, numParcelas, taxaAM, taxaJuros);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine(" -- Cálculo Método PRICE --");
                    calcularPrice(valorFinanciamento, valorEntrada, numParcelas, taxaAM, taxaJuros);
                }

                Console.WriteLine("");
                Console.Write("Deseja reafazer a simulação? (S)im ou (N)ão :");
                resposta = Console.ReadLine();
                if (resposta.ToUpper() == "S")
                {
                    Console.WriteLine();
                    iniciarFinanciamento();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Obrigado por usar o simulador de Financiamento!");
                }

            }
            
        }

        private static void calcularSac(double valorFinanciamento, double valorEntrada, int numParcelas, int taxaAM, double taxaJuros)
        {
            valorFinanciamento -= valorEntrada;
            futureValue = valorFinanciamento;
            taxaJuros /= 100;
            
            double x1 =  1 + taxaJuros;
            double y1 = 1.0 / 12;
            if (taxaAM == 1) { taxaJuros = (Math.Pow(x1, y1)-1)  ; }
            int ln = numParcelas + 2;

            string[,] table = new string[ln, 6];
            
            for (int x = 1; x <= numParcelas; x++)
            {
                juros = futureValue * (taxaJuros);
                amortizacao = valorFinanciamento / numParcelas;
                prestacao = juros + amortizacao;
                saldoDevedor = futureValue - amortizacao;

                totalParcela += prestacao;
                totalAmortizacao += amortizacao;
                totalJuros += juros;
                preencherTabela(table, Data, x, prestacao, amortizacao, juros, saldoDevedor);
                
                futureValue -= amortizacao;
            }
            preencherCabecalhoRodape(table, ln, totalParcela, totalAmortizacao, totalJuros);
            printFinanciamento(table);

        }

        private static void calcularPrice(double valorFinanciamento, double valorEntrada, int numParcelas, int taxaAM, double taxaJuros)
        {
            valorFinanciamento -= valorEntrada;
            futureValue = valorFinanciamento;
            taxaJuros /= 100;
            
            double x1 = 1 + taxaJuros;
            double y1 = 1.0 / 12;
            if (taxaAM == 1) { taxaJuros = (Math.Pow(x1, y1) - 1); }
            int ln = numParcelas + 2;

            string[,] table = new string[ln, 6];
                        
            double x2 = Math.Pow(1 + taxaJuros, numParcelas);
            prestacao = (futureValue * (x2 * taxaJuros) / (x2 - 1));

            for (int x = 1; x <= numParcelas; x++)
            {
                juros = futureValue * (taxaJuros);
                amortizacao = prestacao - juros;
                saldoDevedor = futureValue - amortizacao;
                totalParcela += prestacao;
                totalAmortizacao += amortizacao;
                totalJuros += juros;

                preencherTabela(table, Data, x, prestacao, amortizacao, juros, saldoDevedor);
                futureValue -= amortizacao;
            }
            preencherCabecalhoRodape(table, ln, totalParcela, totalAmortizacao, totalJuros);
            printFinanciamento(table);
        }
        private static void preencherTabela(string[,] table, DateTime Data, int x, double prestacao, double amortizacao, double juros, double saldoDevedor)
        {
            table[x, 0] = x.ToString("00") + "        ";
            table[x, 1] = "R$ " + prestacao.ToString("#,#00.00") + "        ";
            table[x, 2] = "R$ " + amortizacao.ToString("#,#00.00") + "      ";
            table[x, 3] = "R$ " + juros.ToString("#,#00.00") + "     ";
            table[x, 4] = "R$ " + saldoDevedor.ToString("#,#00.00") + "     ";
            table[x, 5] = Data.AddMonths(x).ToString("dd/MM/yyyy");
        }

        private static void preencherCabecalhoRodape(string[,] table, int ln, double totalParcela, double totalAmortizacao, double totalJuros)
        {
            ln -= 1;
            table[0, 0] = "Parcela   ";
            table[0, 1] = "Prestação          ";
            table[0, 2] = "Amortização    ";
            table[0, 3] = "Juros           ";
            table[0, 4] = "Saldo Devedor     ";
            table[0, 5] = "Data Vencimento";
            table[ln, 0] = "Total      ";
            table[ln, 1] = "R$ " + totalParcela.ToString("#,#00.00") + "     ";
            table[ln, 2] = "R$ " + totalAmortizacao.ToString("#.#00.00") + "   ";
            table[ln, 3] = "R$ " + totalJuros.ToString("#,#00.00") + "  ";
            table[ln, 4] = "R$ 0,00";
            table[ln, 5] = "";
        }

        private static void printFinanciamento(string[,] table)
        {
            Console.WriteLine("");
            for (int a = 0; a < table.GetLength(0); a++)
            {
                Console.WriteLine(table[a, 0] + table[a, 1] + table[a, 2] + table[a, 3] + table[a, 4] + table[a, 5]);
            }
        }

        private static string TipoCalculoFin()
        {
            Console.WriteLine("Digite: 1 - Para cálculo SAC");
            Console.WriteLine("Digite: 2 - Para cálculo Price");
            Console.WriteLine("Digite: X - Para Sair");
            string tipoCalculo = Console.ReadLine();
            if (tipoCalculo.ToUpper() == "X")
            {
                return tipoCalculo;
            }
            else if (int.Parse(tipoCalculo) == 1 || int.Parse(tipoCalculo) == 2)
            {
                return tipoCalculo;
            }
            else
            {
                return TipoCalculoFin();
            }
        }
    }
}

