using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e1_3
{ 
    //Разработать событийную модель взаимодействия водителя и инспектора полиции
    //Событие-превышения водителем максимально допустимой скорости на Х км/час
    //Реакция инспектора - штра, размер которого зависит от значения Х
    //0<X<=10 -штраф 100 грн.
    //10<X<=20 -штраф 200 грн.
    //20<X -штраф 500 грн.{
    
    class MakeViolationEventArgs : EventArgs
    {
        public int Speed { get; set; }
        public MakeViolationEventArgs(int s) { Speed = s; }
    }
    delegate void MakeViolation(object sender, MakeViolationEventArgs e);

    class WriteFineEventArgs : EventArgs
    {
        public int Fine { get; set; }
        public WriteFineEventArgs(int f) { Fine = f; }
    }
    delegate void WriteFine(object sender, WriteFineEventArgs e);

    class Driver
    {
        public int Money { get; set; }
        string name;
        public int Speed { get; set; }
        
        public Driver(string n, int m)
        {
            Money = m;
            name = n;
            Speed = 0;
        }
        public event MakeViolation StartMakeViolation;
        public void Accelerate(int delta)
        {
            Speed += delta;
            if (StartMakeViolation != null)
                StartMakeViolation(this, new MakeViolationEventArgs(Speed));
        }
        public override string ToString()
        {
            return name;
        }

        public void PayFine(object sender, WriteFineEventArgs e)
        {
            Money = Money - e.Fine;
        }
    }

class Cop
    {
        int MaxSpeed = 50;
        
        public void AvoidMakeViolation(object sender, MakeViolationEventArgs e)
        {
            int delta = e.Speed - MaxSpeed;
            int tax = 0;
            if (e.Speed<MaxSpeed)
            {
                return;
            }
            else if(delta<=10)
            {
                tax = 100;
            }
            else if (delta <= 20)
            {
                tax = 200;
            }
            else 
            {
                tax = 500;
            }
            Console.WriteLine("You have increased speed at{0} and your fine is {1}$", delta, tax);
            StartWriteFine(this, new WriteFineEventArgs(tax));
        }

        public event WriteFine StartWriteFine;
    }
    class Program
    {
        static void Main(string[] args)
        {
            Driver d1 = new Driver("Александр", 500);
            Driver d2 = new Driver("Евгений", 1000);
            Cop c1 = new Cop();
            d2.StartMakeViolation += c1.AvoidMakeViolation;
            d1.StartMakeViolation += c1.AvoidMakeViolation;
            c1.StartWriteFine += d1.PayFine;
            c1.StartWriteFine += d2.PayFine;



            d1.Accelerate(60);
            d2.Accelerate(100);

            Console.ReadKey();
        }
    }
}
