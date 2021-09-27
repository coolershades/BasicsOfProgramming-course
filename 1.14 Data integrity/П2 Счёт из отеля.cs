using System;

namespace HotelAccounting
{
    public class AccountingModel : ModelBase
    {
        private double price;
        private int nightsCount;
        private double discount;
        private double total;

        private void RecalculateTotal()
        {
            Total = Price * NightsCount * (1 - Discount / 100);
        }

        public double Price
        {
            get => price;
            set
            {
                if (value < 0)
                    throw new ArgumentException();
                price = value;
                RecalculateTotal();
                Notify(nameof(Price));
            }
        }

        public int NightsCount
        {
            get => nightsCount;
            set
            {
                if (value <= 0)
                    throw new ArgumentException();
                nightsCount = value;
                RecalculateTotal();
                Notify(nameof(NightsCount));
            }
        }

        public double Discount
        {
            get => discount;
            set
            {
                if (value > 100)
                    throw new ArgumentException();
                discount = value;
                RecalculateTotal();
                Notify(nameof(Discount));
            }
        }

        public double Total
        {
            get => total;
            set
            {
                if (value < 0)
                    throw new ArgumentException();
                total = value;
                Notify(nameof(Total));
                
                if (Math.Abs(Total - Price * NightsCount * (1 - Discount / 100)) > 1e-5)
                {
                    discount = (1 - Total / (Price * NightsCount)) * 100;
                    Notify(nameof(Discount));
                }
            }
        }
    }
}