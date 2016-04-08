using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius
{
    /// <summary>
    /// Class allows for a pooled resource.
    /// For objects that get recycles at high rates, 
    /// this class will help prevent unnecessary "new" calls.
    /// </summary>
    /// <typeparam name="SoundBit"></typeparam>
    public class Pool<particle> where particle : new()
    {
        private Stack<particle> stack;

        public Pool()
        {
            stack = new Stack<particle>();
        }

        public Pool(int size)
        {
            stack = new Stack<particle>(size);
            for (int i = 0; i < size; i++)
            {
                stack.Push(new particle());
            }
        }

        public particle Fetch()
        {
            if (stack.Count > 0)
            {
                return stack.Pop();
            }
            return new particle();
        }

        public void Return(particle item)
        {
            stack.Push(item);
        }
    }
}
