using System;

namespace CoreAPI
{
    public class Test1
    {
        public string Name { get; }
        public DateTime DateTime { get; }

        public Test1(string name, DateTime dt)
        {
            Name = name;
            DateTime = dt;
        }
    }
}
