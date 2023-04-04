using IBM.Application.Constants;
using IBM.Application.Models.DTOs.Rates;
using IBM.Application.Models.DTOs.Transactions;
using System.Collections;

namespace IBM.Unit.Tests.TestData
{
    public class CurrencyConversionTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { Scenario1.Rates, Scenario1.Transactions, Scenario1.Currency, Scenario1.Expected };
            yield return new object[] { Scenario2.Rates, Scenario2.Transactions, Scenario2.Currency, Scenario2.Expected };
            yield return new object[] { Scenario3.Rates, Scenario3.Transactions, Scenario3.Currency, Scenario3.Expected };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        class Scenario1
        {
            public static IEnumerable<RateDTO> Rates =>
                new List<RateDTO>
                {
                    new RateDTO { From = Currencies.EUR, To = Currencies.USD, Rate = 1.359m },
                    new RateDTO { From = Currencies.CAD, To = Currencies.EUR, Rate = 0.732m },
                    new RateDTO { From = Currencies.USD, To = Currencies.EUR, Rate = 0.736m },
                    new RateDTO { From = Currencies.EUR, To = Currencies.CAD, Rate = 1.366m }
                };

            public static IEnumerable<TransactionDTO> Transactions =>
                new List<TransactionDTO>
                {
                    new TransactionDTO { Sku = "T2006", Amount = 10, Currency = Currencies.USD },
                    new TransactionDTO { Sku = "T2006", Amount = 34.57m, Currency = Currencies.CAD },
                    new TransactionDTO { Sku = "T2006", Amount = 17.95m, Currency = Currencies.USD },
                    new TransactionDTO { Sku = "T2006", Amount = 7.63m, Currency = Currencies.EUR },
                    new TransactionDTO { Sku = "T2006", Amount = 21.23m, Currency = Currencies.USD }
                };

            public static string Currency => Currencies.EUR;

            public static IEnumerable<TransactionDTO> Expected =>
                new List<TransactionDTO>
                {
                    new TransactionDTO { Sku = "T2006", Amount = 7.36m, Currency = Currencies.EUR },
                    new TransactionDTO { Sku = "T2006", Amount = 25.31m, Currency = Currencies.EUR },
                    new TransactionDTO { Sku = "T2006", Amount = 13.21m, Currency = Currencies.EUR },
                    new TransactionDTO { Sku = "T2006", Amount = 7.63m, Currency = Currencies.EUR },
                    new TransactionDTO { Sku = "T2006", Amount = 15.63m, Currency = Currencies.EUR }
                };
        }

        class Scenario2
        {
            public static IEnumerable<RateDTO> Rates =>
                new List<RateDTO>
                {
                    new RateDTO { From = Currencies.EUR, To = Currencies.USD, Rate = 0.988m },
                    new RateDTO { From = Currencies.CAD, To = Currencies.EUR, Rate = 1.732m },
                    new RateDTO { From = Currencies.USD, To = Currencies.EUR, Rate = 2.736m },
                    new RateDTO { From = Currencies.EUR, To = Currencies.CAD, Rate = 1.450m }
                };

            public static IEnumerable<TransactionDTO> Transactions =>
                new List<TransactionDTO>
                {
                    new TransactionDTO { Sku = "B2009", Amount = 110, Currency = Currencies.USD },
                    new TransactionDTO { Sku = "B2009", Amount = 341.57m, Currency = Currencies.CAD },
                    new TransactionDTO { Sku = "B2009", Amount = 171.95m, Currency = Currencies.USD },
                    new TransactionDTO { Sku = "B2009", Amount = 71.63m, Currency = Currencies.EUR },
                    new TransactionDTO { Sku = "B2009", Amount = 211.23m, Currency = Currencies.USD }
                };

            public static string Currency => Currencies.USD;

            public static IEnumerable<TransactionDTO> Expected =>
                new List<TransactionDTO>
                {
                    new TransactionDTO { Sku = "B2009", Amount = 110, Currency = Currencies.USD },
                    new TransactionDTO { Sku = "B2009", Amount = 584.5m, Currency = Currencies.USD },
                    new TransactionDTO { Sku = "B2009", Amount = 171.95m, Currency = Currencies.USD },
                    new TransactionDTO { Sku = "B2009", Amount = 70.77m, Currency = Currencies.USD },
                    new TransactionDTO { Sku = "B2009", Amount = 211.23m, Currency = Currencies.USD }
                };
        }

        class Scenario3
        {
            public static IEnumerable<RateDTO> Rates =>
                new List<RateDTO>
                {
                    new RateDTO { From = Currencies.EUR, To = Currencies.USD, Rate = 31.988m },
                    new RateDTO { From = Currencies.CAD, To = Currencies.EUR, Rate = 45.732m },
                    new RateDTO { From = Currencies.USD, To = Currencies.EUR, Rate = 7.736m },
                    new RateDTO { From = Currencies.EUR, To = Currencies.CAD, Rate = 21.450m },
                    new RateDTO { From = Currencies.ARS, To = Currencies.USD, Rate = 0.000450m },
                    new RateDTO { From = Currencies.USD, To = Currencies.ARS, Rate = 389.450m },
                    new RateDTO { From = Currencies.JPY, To = Currencies.EUR, Rate = 311.23m },
                    new RateDTO { From = Currencies.EUR, To = Currencies.JPY, Rate = 0.72m }
                };

            public static IEnumerable<TransactionDTO> Transactions =>
                new List<TransactionDTO>
                {
                    new TransactionDTO { Sku = "R2008", Amount = 1110, Currency = Currencies.USD },
                    new TransactionDTO { Sku = "R2008", Amount = 3041.57m, Currency = Currencies.CAD },
                    new TransactionDTO { Sku = "R2008", Amount = 1741.95m, Currency = Currencies.USD },
                    new TransactionDTO { Sku = "R2008", Amount = 718.63m, Currency = Currencies.EUR },
                    new TransactionDTO { Sku = "R2008", Amount = 2141.23m, Currency = Currencies.USD },

                    new TransactionDTO { Sku = "R2008", Amount = 320.51m, Currency = Currencies.JPY },
                    new TransactionDTO { Sku = "R2008", Amount = 5432.57m, Currency = Currencies.ARS },
                    new TransactionDTO { Sku = "R2008", Amount = 1741.95m, Currency = Currencies.USD },
                    new TransactionDTO { Sku = "R2008", Amount = 518.63m, Currency = Currencies.JPY },
                    new TransactionDTO { Sku = "R2008", Amount = 20141.23m, Currency = Currencies.ARS }
                };

            public static string Currency => Currencies.ARS;

            public static IEnumerable<TransactionDTO> Expected =>
                new List<TransactionDTO>
                {
                    new TransactionDTO { Sku = "R2008", Amount = 432289.50m, Currency = Currencies.ARS },
                    new TransactionDTO { Sku = "R2008", Amount = 1732833395.43m, Currency = Currencies.ARS },
                    new TransactionDTO { Sku = "R2008", Amount = 678402.43m, Currency = Currencies.ARS },
                    new TransactionDTO { Sku = "R2008", Amount = 8952497.45m, Currency = Currencies.ARS },
                    new TransactionDTO { Sku = "R2008", Amount = 833902.02m, Currency = Currencies.ARS },
                    new TransactionDTO { Sku = "R2008", Amount = 1242687254.06m, Currency = Currencies.ARS },
                    new TransactionDTO { Sku = "R2008", Amount = 5432.57m, Currency = Currencies.ARS },
                    new TransactionDTO { Sku = "R2008", Amount = 678402.43m, Currency = Currencies.ARS },
                    new TransactionDTO { Sku = "R2008", Amount = 2010841639.23m, Currency = Currencies.ARS },
                    new TransactionDTO { Sku = "R2008", Amount = 20141.23m, Currency = Currencies.ARS }
                };
        }
    }
}