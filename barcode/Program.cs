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
            Query();
        }

        static void Query()
        {
            string StartSymbol = "[105]";
            string VersionNumber = "4";
            string Hash2 = "";
            string StopSymbol = "[stop]";

            Console.WriteLine("Anna saajan tilinumero (esim. FI12 1234 1234 1234 12)");

            string AccountNumber = Console.ReadLine();
            AccountNumber = AccountNumberCheck(AccountNumber);

            Console.Clear();
            Console.WriteLine("Anna maksun summa (esim. 99,99");

            string Sum = Console.ReadLine();
            Sum = SumCheck(Sum);

            Console.Clear();
            Console.WriteLine("Anna viitenumero (esim. 123 12345 12345)");

            string IndexNumber = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Tarvitaanko viitenumerolle tarkistenumeroa? (y/n)");
            bool HashNeed = false;
            int KeyPressCheck = 0;
            while (KeyPressCheck == 0)
            {
                ConsoleKeyInfo info = Console.ReadKey();
                if (info.KeyChar == 'y')
                {
                    HashNeed = true;
                    KeyPressCheck = 1;
                }
                else if (info.KeyChar == 'n')
                {
                    HashNeed = false;
                    KeyPressCheck = 1;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Tarvitaanko viitenumerolle tarkistenumeroa? (y/n)");
                    KeyPressCheck = 0;
                }
            }
            
            IndexNumber = IndexNumberCheck(IndexNumber, HashNeed);

            Console.Clear();
            Console.WriteLine("Anna eräpäivä (esim. 01.01.2017)");

            string ExpDate = Console.ReadLine();
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
            Console.WriteLine(StartSymbol + " " + Regex.Replace(VirtualBarCode, ".{2}", "$0 ") + " [" + Hash2 + "] " + StopSymbol);
            Console.ReadLine();

        }

        static string AccountNumberCheck(string GivenAccountNumber)
        {
            if (GivenAccountNumber.Contains(" "))
            {
                GivenAccountNumber = GivenAccountNumber.Replace(" ", "");
            }

            if (GivenAccountNumber.Contains("FI"))
            {
                GivenAccountNumber = GivenAccountNumber.Replace("FI", "");
            }
            else
            {
                Console.WriteLine("Annettu tilinumero on virheellinen\n");
                Query();
            }

            if (GivenAccountNumber.Length != 16)
            {
                Console.WriteLine("Annettu tilinumero on virheellinen\n");
                Query();
            }            
            return GivenAccountNumber;
        }

        static string SumCheck(string GivenSum)
        {
            if (Regex.IsMatch(GivenSum, @"^[a-zA-Z]+$"))
            {
                Console.WriteLine("Annettu summa on virheellinen\n");
                Query();
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
            return AddZeros(GivenSum, 8);
        }

        static string IndexNumberCheck(string GivenIndexNumber, bool HashNeed)
        {
            if (GivenIndexNumber.Contains(" "))
            {
                GivenIndexNumber = GivenIndexNumber.Replace(" ", "");
            }

            int Lenght = GivenIndexNumber.Length;

            if (Lenght > 19 || Regex.IsMatch(GivenIndexNumber, @"^[a-zA-Z]+$"))
            {
                Console.WriteLine("Annettu viitenumero on virheellinen\n");
                Query();
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

                return AddZeros(GivenIndexNumber, 20);
            }
            else
            {
                return AddZeros(GivenIndexNumber, 20);
            }
        }

        static string ExpDateCheck(string GivenExpDate)
        {
            if (Regex.IsMatch(GivenExpDate, @"^[a-zA-Z]+$"))
            {
                Console.WriteLine("Annettu summa on virheellinen\n");
                Query();
            }
            if (GivenExpDate.Contains("."))
            {
                GivenExpDate = GivenExpDate.Replace(".", "");
            }
            if (GivenExpDate.Length != 8)
            {
                Console.WriteLine("Annettu eräpäivä on virheellinen\n");
                Query();
            }

            GivenExpDate = Convert.ToString(GivenExpDate[6]) + Convert.ToString(GivenExpDate[7]) + Convert.ToString(GivenExpDate[2]) + Convert.ToString(GivenExpDate[3]) + Convert.ToString(GivenExpDate[0]) + Convert.ToString(GivenExpDate[1]);
            return GivenExpDate;
        }

        static string AddZeros(string GivenString, int MaxLenght)
        {
            int Lenght = GivenString.Length;
            int i;
            StringBuilder sb = new StringBuilder();

            for (i = Lenght; i < MaxLenght; i++)
            {
                sb.Append("0");
            }
            sb.Append(GivenString);
            return sb.ToString();
        }
    }
}
