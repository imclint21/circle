using System;
using Circle.Interfaces;

namespace IntegrationTest
{
    public class Work : IWorkHandler
    {
        public void DoWork()
        {
            Console.WriteLine("Test!");
        }
    }
}