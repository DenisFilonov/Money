using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Temat_11pd
{
    class Money
    {
        private int _grivna;    //Ukrainian national money - "hryvna", denomination like for example dollar
        private int _kopeyka;   //Ukrainian national money - "kopiyok", as "cent/penny" for example
        public int Grivna
        {
            get
            {
                return _grivna;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("\tGrivna cannot take negative values!\n");
                }
                _grivna = value;
            }
        }
        public int Kopeyka
        {
            get
            {
                return _kopeyka;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("\tKopeyka cann't take negative values!\n");
                }
                _kopeyka = value;
            }
        }
        //=============================================================================1
        public Money()
        {
            _grivna = 1;
            _kopeyka = 10;
        }
        public Money(int grivna, int kopeyka)
        {
            if (grivna < 0 || kopeyka < 0)
            {
                throw new Exception("\tHryvnia and/or kopeyka cann't be negative!\n");
            }
            if (kopeyka > 100)
            {
                throw new Exception("\tEnter a kopeyka value less than 100!\n");
            }
            Grivna = grivna;
            Kopeyka = kopeyka;
        }
        public override string ToString()
        {
            return $"Grivna: {Grivna} Kopeyka: {Kopeyka}";
        }
        public int CountKopeyka()
        {
            return (Grivna * 100) + Kopeyka;
        }
        public static Money ToGrivna(int val)
        {
            Money mn = new Money();
            mn.Grivna = val / 100;
            mn.Kopeyka = val % 100;
            return mn;
        }
        private static bool IsDoubleConvertableToInt(double v)
        {
            string s = v.ToString();
            if (s.IndexOf('.') > 10)
            {
                return false;
            }
            else if (s.IndexOf('.') <= 10) // -2147483648 до 2147483647 
            {
                string str = s.Substring(0, s.IndexOf('.'));
                double b = double.Parse(str);
                if (b < 2147483647)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public static Money DoubleToMoney(double v)
        {
            if (v <= 0)
            {
                throw new Exception("\tThe value cann't be zero or less!\n");
            }
            if (IsDoubleConvertableToInt(v))
            {
                int a = (int)v; //Целая часть до 10 значений
                int b = (int)((v % 1) * 100); //Дробная - 2 числа
                int c = a * 100 + b;
                Money m = ToGrivna(c);
                return m;
            }
            throw new Exception("\tCann't convert this double value!\n");
        }
        //private static Money CheckKopeyka(Money m)
        //{
        //    if (m.Kopeyka >= 100)
        //    {
        //        m.Kopeyka -= 100;
        //        m.Grivna++;
        //    }
        //    return m;
        //}
        //=============================================================================2
        public static bool operator >(Money m1, Money m2)
        {
            return m1.CountKopeyka() > m2.CountKopeyka();
        }
        public static bool operator <(Money m1, Money m2)
        {
            return m1.CountKopeyka() < m2.CountKopeyka();
        }
        public override bool Equals(object obj)
        {
            return this.ToString() == obj.ToString();
        }
        public static bool operator ==(Money m1, Money m2)
        {
            if (m1 is null || m2 is null)
            {
                throw new NullReferenceException();
            }
            return m1.Equals(m2);
        }
        public static bool operator !=(Money m1, Money m2)
        {
            return !(m1 == m2);
        }
        //=============================================================================3
        public static Money operator +(Money m, int n) 
        {
            if (n < 0)
            {
                throw new Exception("\tIt's undesirable to add a negative value! Use subtraction.\n");
            }
            //Money mn = new Money { Grivna = m.Grivna, Kopeyka = m.Kopeyka + n };
            //CheckKopeyka(mn);
            int a = m.CountKopeyka();
            a += n;
            Money mn = ToGrivna(a);
            return mn;
        }
        public static Money operator -(Money m, int n)
        {
            if (n < 0)
            {
                throw new Exception("\tIt's undesirable to subtract a negative value! Use addition.\n");
            }
            //int tmp = m.Kopeyka - n;
            //if (tmp < 0)
            //{
            //    tmp += 100;
            //    m.Grivna--;
            //}
            //return new Money { Grivna = m.Grivna, Kopeyka = tmp };

            int a = m.CountKopeyka();
            if (a < n)
            {
                throw new Exception("\tInsufficient funds on the account!\n");
            }
            a -= n;
            Money mn = ToGrivna(a);
            return mn;
        }
        public static Money operator *(Money m, int n)
        {
            Money buf = new Money() { Grivna = m.Grivna, Kopeyka = m.Kopeyka }; // Избежать ссылки и перезаписи
            int tmp = buf.CountKopeyka();
            tmp *= n;
            buf = ToGrivna(tmp);
            return buf;
        }
        public static Money operator /(Money m, int n)
        {
            Money buf = new Money() { Grivna = m.Grivna, Kopeyka = m.Kopeyka }; // Избежать ссылки и перезаписи
            int tmp = buf.CountKopeyka();
            tmp /= n;
            buf = ToGrivna(tmp);
            return buf;
        }
        //==================================================
        public static Money operator +(Money m, double d)
        {
            if (d < 0)
            {
                throw new Exception("\tIt's undesirable to add a negative value! Use subtraction.\n");
            }
            Money mn = DoubleToMoney(d);
            return m + mn;
        }
        public static Money operator -(Money m, double d)
        {
            if (d < 0)
            {
                throw new Exception("\tIt's undesirable to add a negative value! Use subtraction.\n");
            }
            Money mn = DoubleToMoney(d);
            return m - mn;
        }
        public static Money operator *(Money m, double d)
        {
            int a = (int)d; //Целая часть до 10 значений
            int b = (int)((d % 1) * 100); //Дробная - 2 числа интересует

            int c = (m.CountKopeyka() * a) + ((m.CountKopeyka() * b)/100);
            Money mn = ToGrivna(c);
            return mn;
        }
        public static Money operator /(Money m, double d)
        {
            int a = (int)d; //Целая часть до 10 значений
            int b = (int)((d % 1) * 100); //Дробная - 2 числа интересует

            int c = m.CountKopeyka() - ((m.CountKopeyka() / a) + ((m.CountKopeyka() / (b / 10)) / 2));
            c /= 100;
            Money mn = ToGrivna(c);
            return mn;
        }
        //==================================================
        public static Money operator +(Money m1, Money m2)
        {
            int a = m1.CountKopeyka();
            int b = m2.CountKopeyka();
            Money m = ToGrivna(a + b);
            return m;
        }
        public static Money operator -(Money m1, Money m2)
        {
            int a = m1.CountKopeyka();
            int b = m2.CountKopeyka();

            if (a < b)
            {
                throw new Exception("\tFirst object is less than second!\n");
            }
            int c = a - b;
            Money m = ToGrivna(c);
            return m;
        }
        //==================================================
        public static Money operator ++(Money m)
        {
            int a = m.CountKopeyka() + 1;
            Money mn = ToGrivna(a);
            return mn;
        }
        public static Money operator --(Money m)
        {
            //int tmp = m.Kopeyka - 1;
            //if (tmp < 0)
            //{
            //    tmp += 100;
            //    m.Grivna--;
            //    m.Kopeyka = tmp;
            //}
            int a = m.CountKopeyka();
            if (a == 0)
            {
                throw new Exception("\tInsufficient funds on the account!\n");
            }
            a--;
            Money mn = ToGrivna(a); 
            return mn;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            //double a = 1234567890.75456756789890000000001; // * 100
            //string s = a.ToString();
            //s.Substring(0, s.IndexOf('.'));
            //if s.IndexOf('.') <= 10
            //else Exception
            //int b1 = (int)a;

            //Console.WriteLine(a % 1);
            //int b = (int)((a % 1) * 100);
            //Console.WriteLine(b);

            //int c = (int)a;
            //Console.WriteLine(c);

            //string s = a.ToString();
            //if (s.IndexOf('.') > 10)
            //{
            //    Console.WriteLine("False");
            //}
            //else if (s.IndexOf('.') <= 10) // -2147483648 до 2147483647 
            //{
            //    string str = s.Substring(0, s.IndexOf('.'));
            //    double b = double.Parse(str);
            //    if (b < 2147483647)
            //    {
            //        Console.WriteLine("True");
            //    }
            //    else
            //    {
            //        Console.WriteLine("False");
            //    }
            //}



            Money m1;
            Money m2;
            int v1, v2;
            double val;
            int menu;
            do
            {
                Console.WriteLine("\n\tMAIN MENU");
                Console.WriteLine("1. Check the > operator");
                Console.WriteLine("2. Check the < operator");
                Console.WriteLine("3. Check the == operator");
                Console.WriteLine("4. Check the != operator");
                Console.WriteLine("5. Check the + operator");
                Console.WriteLine("6. Check the - operator");
                Console.WriteLine("7. Check the * operator");
                Console.WriteLine("8. Check the / operator");
                Console.WriteLine("9. Check the ++ operator");
                Console.WriteLine("10. Check the -- operator");
                Console.WriteLine("11. Count Kopeykas from value | Extra");
                Console.WriteLine("12. Convert to Money object from value | Extra");
                Console.WriteLine("13. Convert double to Money object and add to Money object | Extra");
                Console.WriteLine("14. Convert double to Money object and deduct it from Money object | Extra");
                Console.WriteLine("15. Multiply a Money class object by a double value | Extra");
                Console.WriteLine("16. Divide a Money class object by a double value | Extra");
                Console.Write("0. Exit\nChoice: ");
                menu = int.Parse(Console.ReadLine());

                switch (menu)
                {
                    case 1:
                        try
                        {
                            Console.WriteLine("\n\tFirst Money object");
                            Console.Write("Input integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m1 = new Money(v1, v2);

                            Console.WriteLine("\n\tSecond Money object");
                            Console.Write("Input integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m2 = new Money(v1, v2);

                            Console.WriteLine("\n\tThe objects is created, now let's see the comparison result:");
                            Console.WriteLine((m1 > m2) ? "\n\tTrue, m1 > m2 is correct\n" : "\n\tFalse, m1 (== or != or < or <= or >=) to m2\n");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nException: {ex.Message}");
                        }
                        break;

                    case 2:
                        try
                        {
                            Console.WriteLine("\n\tFirst Money object");
                            Console.Write("Input integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m1 = new Money(v1, v2);

                            Console.WriteLine("\n\tSecond Money object");
                            Console.Write("Input integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m2 = new Money(v1, v2);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nException: {ex.Message}");
                        }
                        break;

                    case 3:
                        try
                        {
                            Console.WriteLine("\n\tFirst Money object");
                            Console.Write("Input integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m1 = new Money(v1, v2);

                            Console.WriteLine("\n\tSecond Money object");
                            Console.Write("Input integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m2 = new Money(v1, v2);

                            Console.WriteLine("\n\tThe objects is created, now let's see the comparison result:");
                            Console.WriteLine((m1 == m2) ? "\n\tTrue, m1 == m2 is correct\n" : "\n\tFalse, m1 (!= or > or < or <= or >=) to m2\n");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nException: {ex.Message}");
                        }
                        break;

                    case 4:
                        try
                        {
                            Console.WriteLine("\n\tFirst Money object");
                            Console.Write("Input integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m1 = new Money(v1, v2);

                            Console.WriteLine("\n\tSecond Money object");
                            Console.Write("Input integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m2 = new Money(v1, v2);

                            Console.WriteLine("\n\tThe objects is created, now let's see the comparison result:");
                            Console.WriteLine((m1 == m2) ? "\n\tTrue, m1 != m2 is correct\n" : "\n\tFalse, m1 (== or > or < or <= or >=) to m2\n");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nException: {ex.Message}");
                        }
                        break;

                    case 5:
                        try
                        {
                            Console.Write("\nInput integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m1 = new Money(v1, v2);

                            Console.WriteLine("\n\tThe objects is created, now let's add kopeykas.");
                            Console.Write("Input integer value for adding kopeykas: ");
                            v1 = int.Parse(Console.ReadLine());

                            try
                            {
                                Console.WriteLine($"\nResult: {m1 + v1}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Exception: {ex.Message}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nException: {ex.Message}");
                        }                        
                        break;

                    case 6:
                        try
                        {
                            Console.Write("\nInput integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m1 = new Money(v1, v2);

                            Console.WriteLine("\n\tThe objects is created, now let's deduct kopeykas.");
                            Console.Write("Input integer value for deducting kopeykas: ");
                            v1 = int.Parse(Console.ReadLine());

                            try
                            {
                                Console.WriteLine($"\nResult: {m1 - v1}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"\nException: {ex.Message}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nException: {ex.Message}");
                        }
                        break;

                    case 7:
                        try
                        {
                            Console.Write("\nInput integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m1 = new Money(v1, v2);

                            Console.WriteLine("\n\tThe objects is created, now let's multiply kopeykas.");
                            Console.Write("Input integer value for multiplying kopeykas: ");
                            v1 = int.Parse(Console.ReadLine());

                            try
                            {
                                Console.WriteLine($"\n\tResult: {m1 * v1}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"\nException: {ex.Message}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nException: {ex.Message}");
                        }
                        break;

                    case 8:
                        try
                        {
                            Console.Write("\nInput integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m1 = new Money(v1, v2);

                            Console.WriteLine("\n\tThe objects is created, now let's divide kopeykas.");
                            Console.Write("Input integer value for dividing kopeykas: ");
                            v1 = int.Parse(Console.ReadLine());

                            try
                            {
                                Console.WriteLine($"\n\tResult: {m1 / v1}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"\nException: {ex.Message}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nException: {ex.Message}");
                        }
                        break;

                    case 9:
                        try
                        {
                            Console.Write("\nInput integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m1 = new Money(v1, v2);

                            Console.WriteLine("\n\tThe objects is created, now let's try to add one kopeyka.");
                            try
                            {
                                m1++;
                                Console.WriteLine($"\n\tResult: {m1}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"\nException: {ex.Message}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nException: {ex.Message}");
                        }
                        break;

                    case 10:
                        try
                        {
                            Console.Write("\nInput integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m1 = new Money(v1, v2);

                            Console.WriteLine("\n\tThe objects is created, now let's try to deduct one kopeyka.");
                            try
                            {
                                m1--;
                                Console.WriteLine($"\n\tResult: {m1}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"\nException: {ex.Message}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nException: {ex.Message}");
                        }
                        break;

                    case 11:
                        try
                        {
                            Console.Write("\nInput integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m1 = new Money(v1, v2);

                            Console.WriteLine("\n\tThe objects is created, now let's see how many kopeykas you have.");
                            Console.WriteLine($"\n\tResult: {m1.CountKopeyka()}"); 
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nException: {ex.Message}");
                        }
                        break;

                    case 12:
                        try
                        {
                            Console.Write("\nInput integer value for convert: ");
                            v1 = int.Parse(Console.ReadLine());
                            m1 = Money.ToGrivna(v1);
                            Console.WriteLine($"\n\tResult: {m1}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nException: {ex.Message}");
                        }
                        break;

                    case 13:
                        try
                        {
                            Console.WriteLine("\n\tMoney object");
                            Console.Write("Input integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m1 = new Money(v1, v2);

                            Console.WriteLine("\n\tDouble value object");
                            Console.Write("Input double value to convert: ");
                            val = double.Parse(Console.ReadLine());
                            m2 = Money.DoubleToMoney(val);
                            Console.WriteLine($"\n\tResult m1 + m2: {m1 + m2}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nException: {ex.Message}");
                        }
                        break;

                    case 14:
                        try
                        {
                            Console.WriteLine("\n\tMoney object");
                            Console.Write("Input integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m1 = new Money(v1, v2);

                            Console.WriteLine("\n\tDouble value object must be LESS than Money object");
                            Console.Write("Input double value to convert: ");
                            val = double.Parse(Console.ReadLine());
                            m2 = Money.DoubleToMoney(val);
                            Money m = m1 - m2;
                            Console.WriteLine($"\n\tResult m1 - m2: {m}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nException: {ex.Message}");
                        }
                        break;

                    case 15:
                        try
                        {
                            Console.WriteLine("\n\tMoney object");
                            Console.Write("Input integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m1 = new Money(v1, v2);

                            Console.WriteLine("\n\tDouble value object");
                            Console.Write("Input double value: ");
                            val = double.Parse(Console.ReadLine());

                            Console.WriteLine($"\n\tResult m1 * double: {m1 * val}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nException: {ex.Message}");
                        }
                        break;

                    case 16:
                        try
                        {
                            Console.WriteLine("\n\tMoney object");
                            Console.Write("Input integer value for grivna: ");
                            v1 = int.Parse(Console.ReadLine());
                            Console.Write("Input integer value for kopeyka: ");
                            v2 = int.Parse(Console.ReadLine());
                            m1 = new Money(v1, v2);

                            Console.WriteLine("\n\tDouble value object");
                            Console.Write("Input double value: ");
                            val = double.Parse(Console.ReadLine());

                            Console.WriteLine($"\n\tResult m1 * double: {m1 / val}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nException: {ex.Message}");
                        }
                        break;

                    default:
                        if (menu > 16 || menu != 0 || menu < 0) Console.WriteLine("\tWrong menu item selected.\n");
                        break;
                }
            } while (menu != 0);
        }
    }
}
