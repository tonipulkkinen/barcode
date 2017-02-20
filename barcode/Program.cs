using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;




namespace barcode
{
    class Program
    {

        static void Main(string[] args)
        {
            string StartSymbol = "[105]";
            string VersionNumber = "4";
            string Hash2 = "";
            string StopSymbol = "[stop]";

            string AccountNumber = "";
            string Sum = "";
            string IndexNumber = "";
            string ExpDate = "";

            AccountNumber = AccountNumberCheck(AccountNumber);

            Sum = SumCheck(Sum);

            IndexNumber = IndexNumberCheck(IndexNumber);

            ExpDate = ExpDateCheck(ExpDate);

            string VirtualBarCode = VersionNumber + AccountNumber + Sum + "000" + IndexNumber + ExpDate;

            int i;
            int Multiplier = 0;
            string HashSum;
            int Total = 105;

            for (i = 0; Multiplier < 27; i += 2)
            {
                Multiplier += 1;
                HashSum = Convert.ToString(VirtualBarCode[i]) + Convert.ToString(VirtualBarCode[i + 1]);
                Total += Convert.ToInt32(HashSum) * Multiplier;
            }

            Total = Total / 103;
            Hash2 = Convert.ToString(Total);

            Console.Clear();
            Console.WriteLine(StartSymbol + " " + Regex.Replace(VirtualBarCode, ".{2}", "$0 ") + "[" + Hash2 + "] " + StopSymbol);
            Console.ReadLine();
        }

        static string AccountNumberCheck(string GivenAccountNumber)
        {

            Console.WriteLine("Anna saajan tilinumero");
            GivenAccountNumber = Console.ReadLine();

            if (GivenAccountNumber.Contains("-") || GivenAccountNumber.Contains(" "))
            {
                GivenAccountNumber = GivenAccountNumber.Replace("-", "");
                GivenAccountNumber = GivenAccountNumber.Replace(" ", "");
            }

            if (GivenAccountNumber[0] == '4' || GivenAccountNumber[0] == '5')
            {
                GivenAccountNumber = AddZeros(GivenAccountNumber, 14, 6);
            }
            else
            {
                GivenAccountNumber = AddZeros(GivenAccountNumber, 14, 5);
            }

            GivenAccountNumber += "FI00";

            return GivenAccountNumber;
        }

        static string SumCheck(string GivenSum)
        {
            string Sum = "";

            Console.Clear();
            Console.WriteLine("Anna maksun summa (esim: 99,99)");
            GivenSum = Console.ReadLine();

            if (Regex.IsMatch(GivenSum, @"^[a-zA-Z]+$"))
            {
                SumCheck(Sum);
            }
            if (GivenSum.Contains(",") || GivenSum.Contains("."))
            {
                GivenSum = GivenSum.Replace(",", "");
                GivenSum = GivenSum.Replace(".", "");
            }
            else
            {
                GivenSum += "00";
            }
            return AddZeros(GivenSum, 8, 0);
        }

        static string IndexNumberCheck(string GivenIndexNumber)
        {
            string IndexNumber = "";
            bool HashNeed = false;
            bool HashNeedAnswer = false;

            Console.Clear();
            Console.WriteLine("Anna viitenumero (esim: 123 12345 12345)");
            GivenIndexNumber = Console.ReadLine();

            if (GivenIndexNumber.Contains(" "))
            {
                GivenIndexNumber = GivenIndexNumber.Replace(" ", "");
            }

            int Lenght = GivenIndexNumber.Length;

            if (Lenght > 19 || Regex.IsMatch(GivenIndexNumber, @"^[a-zA-Z]+$"))
            {
                Console.WriteLine("Annettu viitenumero on virheellinen\n");
                IndexNumberCheck(IndexNumber);
            }

            while (HashNeedAnswer == false)
            {
                Console.Clear();
                Console.WriteLine("Tarvitaanko viitenumerolle tarkistenumeroa? (y/n)");
                ConsoleKeyInfo info = default(ConsoleKeyInfo);
                info = Console.ReadKey();
                if (info.KeyChar == 'y')
                {
                    HashNeed = true;
                    HashNeedAnswer = true;
                }
                else if (info.KeyChar == 'n')
                {
                    HashNeed = false;
                    HashNeedAnswer = true;
                }
                else
                {
                    HashNeedAnswer = false;
                }
            }
            
            if (HashNeed == true)
            {
                int i;
                int Multiplier = 7;
                double Total = 0;

                for (i = Lenght - 1; i >= 0; i--)
                {
                    Total += int.Parse(GivenIndexNumber[i].ToString()) * Multiplier;

                    if (Multiplier == 7)
                    {
                        Multiplier = 3;
                    }
                    else if (Multiplier == 3)
                    {
                        Multiplier = 1;
                    }
                    else
                    {
                        Multiplier = 7;
                    }
                }

                double HashNumber = (Math.Ceiling(Total / 10) * 10) - Total;
                Convert.ToInt32(HashNumber);
                if (HashNumber == 10)
                {
                    HashNumber = 0;
                }
                Convert.ToString(HashNumber);

                GivenIndexNumber = GivenIndexNumber + HashNumber;

                return AddZeros(GivenIndexNumber, 20, 0);
            }
            else
            {
                return AddZeros(GivenIndexNumber, 20, 0);
            }
        }

        static string ExpDateCheck(string GivenExpDate)
        {
            string ExpDate = "";

            Console.Clear();
            Console.WriteLine("Anna eräpäivä (esim: 01.01.2017)");
            GivenExpDate = Console.ReadLine();

            if (Regex.IsMatch(GivenExpDate, @"^[a-zA-Z]+$"))
            {
                ExpDateCheck(ExpDate);
            }
            if (GivenExpDate.Contains("."))
            {
                GivenExpDate = GivenExpDate.Replace(".", "");
            }
            if (GivenExpDate.Length != 8)
            {
                ExpDateCheck(ExpDate);
            }

            GivenExpDate = Convert.ToString(GivenExpDate[6]) + Convert.ToString(GivenExpDate[7]) + Convert.ToString(GivenExpDate[2]) + Convert.ToString(GivenExpDate[3]) + Convert.ToString(GivenExpDate[0]) + Convert.ToString(GivenExpDate[1]);
            return GivenExpDate;
        }

        static string AddZeros(string GivenString, int MaxLenght, int StartPoint)
        {
            int Lenght = GivenString.Length;
            int i;
            StringBuilder sb = new StringBuilder();
            StringBuilder Start = new StringBuilder();
            StringBuilder End = new StringBuilder();

            if (StartPoint == 0)
            {
                for (i = Lenght; i < MaxLenght; i++)
                {
                    sb.Append("0");
                }
                sb.Append(GivenString);
                return sb.ToString();
            }
            else
            {
                for (i = 0; i < StartPoint; i++)
                {
                    Start.Append(Convert.ToString(GivenString[i]));
                }
                for (i = Lenght; i < MaxLenght; i++)
                {
                    sb.Append("0");
                }
                for (i = StartPoint + 1; i < Lenght; i++)
                {
                    End.Append(Convert.ToString(GivenString[i]));
                }
                Start.Append(sb);
                Start.Append(End);
                return Start.ToString();
            }
        }
    }
}
