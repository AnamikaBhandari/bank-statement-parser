using AutoLedger.Models;
using System.Globalization;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using static UglyToad.PdfPig.Core.PdfSubpath;

namespace AutoLedger.Services
{
    public class DocumentProcessor
    {
        public List<Transaction> Process(string filePath)
        {
            var transactions = new List<Transaction>();

            decimal previousBalance = 0;
            bool isFirstTransaction = true;

            using (var document = PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {
                    string text = page.Text;

                    text = Regex.Replace(text, @"(\d{2}-[A-Za-z]{3}-\d{4})", "\n$1");

                    var lines = text.Split('\n');

                    foreach (var rawLine in lines)
                    {
                        var line = rawLine.Trim();

                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        line = line.Replace("–", "-");
                        line = Regex.Replace(line, @"\s+", " ");

                        var dateMatch = Regex.Match(line, @"\d{2}-[A-Za-z]{3}-\d{4}");
                        if (!dateMatch.Success)
                            continue;

                        if (line.Contains("DateDescription") ||
                            line.Contains("Statement") ||
                            line.Contains("Account") ||
                            line.Contains("Bank") ||
                            line.Contains("Period"))
                            continue;

                        string dateStr = dateMatch.Value;

                        var amountMatches = Regex.Matches(line, @"\d{1,3}(?:,\d{2})*(?:,\d{3})");
                        var numbers = amountMatches
                                        .Select(x => x.Value)
                                        .ToList();

                        if (numbers.Count == 0)
                            continue;

                        string lastAmount = numbers.Last();
                        int lastAmountIndex = line.LastIndexOf(lastAmount);

                        if (lastAmountIndex <= dateMatch.Index)
                            continue;

                        string beforeAmounts = line.Substring(0, lastAmountIndex);

                        string description = beforeAmounts
                            .Substring(dateMatch.Index + dateMatch.Length)
                            .Trim();

                        decimal debit = 0, credit = 0, balance = 0;

                        balance = ParseAmount(numbers.Last());

                        if (isFirstTransaction)
                        {
                            // Opening balance if any
                            isFirstTransaction = false;
                        }
                        else
                        {
                            decimal diff = balance - previousBalance;

                            if (diff > 0)
                            {
                                credit = diff;
                            }
                            else if (diff < 0)
                            {
                                debit = Math.Abs(diff);
                            }
                        }
                        var txn = new Transaction
                        {
                            Date = DateTime.ParseExact(dateStr, "dd-MMM-yyyy", CultureInfo.InvariantCulture),
                            Description = description,
                            Debit = debit,
                            Credit = credit,
                            Balance = balance,
                            UserId = "USER001",
                            ModifiedTime = DateTime.Now,
                            TranType = debit > 0 ? "Payment" :
                                       credit > 0 ? "Receipt" : ""
                        };

                        transactions.Add(txn);

                        previousBalance = balance;
                    }
                }
            }
            return transactions;
        }
        private decimal ParseAmount(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return 0;

                return decimal.Parse(value.Replace(",", ""));
            }
    }
}
