using GasStationPOS.Core.Data.Models.TransactionModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GasStationPOS
{
    internal class ReceiptPrinter
    {
        private string Name = "Shake-Stack Petrol";
        private Dictionary<string, string> Location = new Dictionary<string, string>
        {
            { "address", "2808 W.Broadway" },
            { "city", "Vancouver" },
            { "province", "British Columbia" },
            { "postal code", "V6K2G7" },
        };
        private string PhoneNumber = "(604)-731-5911";
        private int ReceiptNumber = 0;

        /// <summary>
        /// Prints the reciept to console.
        /// </summary>
        /// <returns></returns>
        public void printReceipt()
        {
            StringBuilder sb = new StringBuilder();
            // Title Part
            sb.AppendLine("Gas Prices\nSelf Serve");
            sb.Append("\n");

            // Address and Phone Number Part
            sb.AppendLine($"{Name}");
            sb.AppendLine($"{string.Join("\n", Location.Values)}");
            sb.AppendLine($"{PhoneNumber}");
            sb.Append("\n");

            // GST, Date, etc Part.
            sb.AppendLine($"GST:\t{generateNumberOfType("GST")}");
            sb.AppendLine($"DATE:\t{getFormattedDateAndTime()["date"]}\t\t" +
                $"TIME:\t\t\t{getFormattedDateAndTime()["time"]}");
            sb.AppendLine($"S/S:\t{generateNumberOfType("S/S")}\t\t\t" +
                $"TERMINAL:\t{generateNumberOfType("TERMINAL")}");
            sb.Append("\n");

            // Authorization Number
            sb.AppendLine($"AUTHORIZATION #:\t{generateAuthNumber()}\n");

            // Product Part
            sb.AppendLine("PRODUCT\t\t\t\t\tOLD\t\t\t\t\tNEW\n");

            // Final Return
            string contentToPrint = sb.ToString();
            Console.WriteLine(contentToPrint);

            // Print the Reciept to file.
            outputReceiptToFile(contentToPrint);
        }

        /// <summary>
        /// Helper function that outputs a string to file.
        /// </summary>
        /// <param name="content"></param>
        private void outputReceiptToFile(string content)
        {
            using (StreamWriter sw = new StreamWriter($"output{ReceiptNumber}.txt"))
            {
                sw.WriteLine(content);
            }
            ReceiptNumber++;
        }

        /// <summary>
        /// Helper function that helps generate the gst number.
        /// For: GST, and Terminal Number
        /// </summary>
        /// <returns></returns>
        private string generateNumberOfType(string typeToGenerate)
        {
            var random = new Random();

            if (typeToGenerate == "GST")
            {
                return random.Next(100_000_000, 999_999_999).ToString();
            }
            if (typeToGenerate == "TERMINAL")
            {
                return "0" + random.Next(10_000_000, 99_999_999).ToString();
            }
            if (typeToGenerate == "S/S")
            {
                return random.Next(1_000_000, 9_999_999).ToString();
            }

            // Returns Nothing, if error.
            Console.WriteLine("Error: Please input a proper type.");
            return "";
        }

        /// <summary>
        /// Helper function that gets the current date in
        /// YYYY-MM-DD format.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> getFormattedDateAndTime()
        {
            return new Dictionary<string, string>
            {
                { "date", DateTime.Now.ToString("yyyy-MM-dd")},
                { "time", DateTime.Now.ToString("HH:mm")}
            };
        }

        /// <summary>
        /// Helper function that helps return an auth number.
        /// </summary>
        /// <returns></returns>
        private string generateAuthNumber()
        {
            var random = new Random();
            int mainID = random.Next(1_000_000, 9_999_999);
            int checkDigit = random.Next(0, 9);

            return $"{mainID}-{checkDigit}";
        }
    }
}
