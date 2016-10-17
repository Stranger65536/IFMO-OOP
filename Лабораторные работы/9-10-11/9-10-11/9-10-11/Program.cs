// Course: Programming paradigms (C#)
// Lab 9-10-11. Inheritance. Interfaces.
// Student: Trofimov V.A. Group: 2511
// Teacher: Povyshev V.V.
// Created 24.09.2013 Modified: 24.09.2013
// Description: Program entry point.

using System;

namespace _9_10_11
{
    public class Utils
    {
        private Utils() { }
        static uint id = 1;
        public static uint GetNextID() { return id++; }
        public static double Max(double a, double b)
            { return (a > b) ? a : b; }
        public static double Min(double a, double b)
            { return (a < b) ? a : b; }
        public static string Error(int code)
        {
            switch (code)
            {
                case -1:
                    return "overflow while supplement";
                case 0:
                    return "ok";
                case 1:
                    return "account blocked";
                case 2:
                    return "wrong attempt";
                case 3:
                    return "not enough money";
                case 4:
                    return "month money limit";
                case 5:
                    return "once money limit";
                default:
                    return "unknown error";
            }
        }
    }
    interface RoubleAccount
    {
        string Currency();
        double exchangeRateRoubleToEur();
        double exchangeRateRoubleToDollar();
    }
    interface DollarAccount
    {
        string Currency();
        double exchangeRateDollarToEur();
        double exchangeRateDollarToRouble();
    }
    public class RoubleDollarAccount : RoubleAccount, DollarAccount
    {
        public string Currency() { return "ru-usd"; }
        public double exchangeRateRoubleToEur() { return 0.025; }
        public double exchangeRateRoubleToDollar() { return 0.033; }
        public double exchangeRateDollarToEur() { return 0.75; }
        public double exchangeRateDollarToRouble() { return 30; }
    }
    public abstract class Account : RoubleDollarAccount
    {
        protected double Cash;
        protected bool IsOverDraft;
        protected double OverDraft;
        protected double MaxMonthRelease;
        protected double MaxOnceRelease;
        public abstract int Release(double sum, uint signature);
        public abstract int Supplement(double sum);
    }
    public class BankAccount : Account
    {
        protected uint id;
        public uint ID { get { return id; } }
        public class Holder
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public uint signature { get; set; }
        }
        protected Holder accountHolder;
        public Holder AccountHolder { get { return accountHolder; } }
        protected bool blocked;
        public bool Blocked { get { return blocked; } }
        protected double monthLeft;
        public double MonthSpend { get { return monthLeft; } }
        new public bool IsOverDraft
        {
            get { return base.IsOverDraft; }
            protected set { base.IsOverDraft = value; }
        }
        new public double OverDraft
        {
            get { return base.OverDraft; }
            protected set { base.OverDraft = value; }
        }
        public double Balance { get { return Cash; } }
        public BankAccount() { }
        public BankAccount(string firstName, string lastName, uint signature,
            double maxMonthRelease, double maxOnceRelease)
        {
            id = Utils.GetNextID();
            accountHolder = new Holder();
            accountHolder.firstName = firstName;
            accountHolder.lastName = lastName;
            accountHolder.signature = signature;
            base.IsOverDraft = false; base.OverDraft = 0.0d;
            MaxMonthRelease = Utils.Max(Math.Abs(maxMonthRelease), Math.Abs(maxOnceRelease));
            MaxOnceRelease = Utils.Min(Math.Abs(maxMonthRelease), Math.Abs(maxOnceRelease));
            blocked = false; monthLeft = 0;
        }
        public BankAccount(string firstName, string lastName, uint signature,
            double maxMonthRelease, double maxOnceRelease, double overDraft)
        {
            id = Utils.GetNextID();
            accountHolder = new Holder();
            accountHolder.firstName = firstName;
            accountHolder.lastName = lastName;
            accountHolder.signature = signature;
            base.IsOverDraft = true; base.OverDraft = Math.Abs(overDraft);
            MaxMonthRelease = Utils.Max(Math.Abs(maxMonthRelease), Math.Abs(maxOnceRelease));
            MaxOnceRelease = Utils.Min(Math.Abs(maxMonthRelease), Math.Abs(maxOnceRelease));
            blocked = false; monthLeft = 0;
        }
        public override int Release(double sum, uint signature)
        {
            if (blocked) return 1;
            if (signature != accountHolder.signature) return 2;
            else
            {
                sum = Math.Abs(sum);
                if (Cash >= sum || Cash + OverDraft >= sum)
                {
                    if (monthLeft + sum > MaxMonthRelease) { return 4; }
                    else if (sum > MaxOnceRelease) { return 5; }
                    else { Cash -= sum; monthLeft += sum; return 0; }
                }
                else return 3;
            }
        }
        public override int Supplement(double sum)
        {
            if (blocked) return 1;
            try { checked { sum = Math.Abs(sum); double cash = Cash; cash += sum; } }
            catch (OverflowException e) { return -1; }
            Cash += sum; return 0;
        }
    }
    public class CardAccount : BankAccount
    {
        private byte wrongAttempts;
        private uint pin;
        public CardAccount(BankAccount ba, uint pin,
            double maxMonthRelease, double maxOnceRelease)
        {
            id = ba.ID; Cash = ba.Balance;
            accountHolder = ba.AccountHolder;
            MaxMonthRelease = Utils.Max(Math.Abs(maxMonthRelease), Math.Abs(maxOnceRelease));
            MaxOnceRelease = Utils.Min(Math.Abs(maxMonthRelease), Math.Abs(maxOnceRelease));
            IsOverDraft = ba.IsOverDraft; OverDraft = ba.OverDraft;
            blocked = ba.Blocked; monthLeft = ba.MonthSpend; wrongAttempts = 0; this.pin = pin;

        }
        public override int Release(double sum, uint pin)
        {
            if (blocked) return 1;
            if (pin != this.pin)
            {
                if (++wrongAttempts == 3) { this.blocked = true; return 1; }
                else return 2;
            }
            else
            {
                wrongAttempts = 0;
                sum = Math.Abs(sum);
                if (Cash >= sum || Cash + OverDraft >= sum)
                {
                    if (monthLeft + sum > MaxMonthRelease) { return 4; }
                    else if (sum > MaxOnceRelease) { return 5; }
                    else { Cash -= sum; monthLeft += sum; return 0; }
                }
                else return 3;
            }
        }
    }
    class Program
    {
        // 0 - ok
        // 1 - blocked
        //-1 - overflow while supplement
        // 2 - wrong attempt
        // 3 - not enough money
        // 4 - money month limit
        // 5 - money once limit
        static void Main(string[] args)
        {
            BankAccount b = new BankAccount("Vladislav", "Trofimov", 1234, 40000, 15000, 1000);
            Console.Write("We just opened an account with 0$, signature 1234, max month ");
            Console.WriteLine("limit of 40000$,  max once limit of 15000$ and overdraft of 1000$.\n");
            Console.Write("Try to release 50$ with signature 1234: ");
            Console.WriteLine(Utils.Error(b.Release(50, 1234)));
            Console.WriteLine("Cash now: " + b.Balance);
            Console.WriteLine("Spend in a month: " + b.MonthSpend + "\n");
            Console.Write("Try to release 50$ with signature 1243: ");
            Console.WriteLine(Utils.Error(b.Release(50, 1243)));
            Console.WriteLine("Cash now: " + b.Balance);
            Console.WriteLine("Spend in a month: " + b.MonthSpend + "\n");
            Console.Write("Try to supplement 40050$: ");
            Console.WriteLine(Utils.Error(b.Supplement(40050)));
            Console.WriteLine("Cash now: " + b.Balance + "\n");
            Console.Write("Try to release 17000$ with signature 1234: ");
            Console.WriteLine(Utils.Error(b.Release(17000, 1234)));
            Console.WriteLine("Cash now: " + b.Balance);
            Console.WriteLine("Spend in a month: " + b.MonthSpend + "\n");
            Console.Write("Try to release 15000$ with signature 1234: ");
            Console.WriteLine(Utils.Error(b.Release(15000, 1234)));
            Console.WriteLine("Cash now: " + b.Balance);
            Console.WriteLine("Spend in a month: " + b.MonthSpend + "\n");
            Console.Write("Try to release 15000$ with signature 1234: ");
            Console.WriteLine(Utils.Error(b.Release(15000, 1234)));
            Console.WriteLine("Cash now: " + b.Balance);
            Console.WriteLine("Spend in a month: " + b.MonthSpend + "\n");
            Console.Write("Try to release 15000$ with signature 1234: ");
            Console.WriteLine(Utils.Error(b.Release(15000, 1234)));
            Console.WriteLine("Cash now: " + b.Balance);
            Console.WriteLine("Spend in a month: " + b.MonthSpend + "\n");
            Console.Write("Try to release 5050$ with signature 1234: ");
            Console.WriteLine(Utils.Error(b.Release(5050, 1234)));
            Console.WriteLine("Cash now: " + b.Balance);
            Console.WriteLine("Spend in a month: " + b.MonthSpend + "\n");
            Console.Write("Try to supplement 5000$: ");
            Console.WriteLine(Utils.Error(b.Supplement(5000)));
            Console.WriteLine("Cash now: " + b.Balance + "\n");
            Console.Write("Try to release 5000$ with signature 1234: ");
            Console.WriteLine(Utils.Error(b.Release(5000, 1234)));
            Console.WriteLine("Cash now: " + b.Balance);
            Console.WriteLine("Spend in a month: " + b.MonthSpend + "\n");
            Console.WriteLine();
            CardAccount c = new CardAccount(b, 0000, 40000, 15000);
            Console.WriteLine("We just opened a card account for bank account with pin 0000.\n");
            Console.Write("Try to release 50$ with pin 1234: ");
            Console.WriteLine(Utils.Error(c.Release(50, 1234)));
            Console.WriteLine("Cash now: " + c.Balance);
            Console.WriteLine("Spend in a month: " + c.MonthSpend + "\n");
            Console.Write("Try to release 50$ with pin 0000: ");
            Console.WriteLine(Utils.Error(c.Release(50, 0000)));
            Console.WriteLine("Cash now: " + c.Balance);
            Console.WriteLine("Spend in a month: " + c.MonthSpend + "\n");
            Console.Write("Try to release 50$ with pin 6666: ");
            Console.WriteLine(Utils.Error(c.Release(50, 6666)) + "\n");
            Console.Write("Try to supplement 5000$: ");
            Console.WriteLine(Utils.Error(c.Supplement(5000)));
            Console.WriteLine("Cash now: " + c.Balance + "\n");
            Console.Write("Try to release 50$ with pin 6666: ");
            Console.WriteLine(Utils.Error(c.Release(50, 6666)) + "\n");
            Console.Write("Try to release 50$ with pin 6666: ");
            Console.WriteLine(Utils.Error(c.Release(50, 6666)) + "\n");
            Console.Write("Try to release 50$ with pin 0000: ");
            Console.WriteLine(Utils.Error(c.Release(50, 0000)) + "\n");
            Console.Write("Cash left in $: ");
            Console.WriteLine(c.Balance);
            Console.Write("Cash left in Eur: ");
            Console.WriteLine(c.Balance * c.exchangeRateDollarToEur());
            Console.Write("Cash left in Rub: ");
            Console.WriteLine(c.Balance * c.exchangeRateDollarToRouble());
        }
    }
}