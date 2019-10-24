using CoreAPI;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var mech = new Core1(string.Join("", args));
                mech.ProcessLoop();
            }
            catch (Exception e)
            {
            }
        }
    }
}
