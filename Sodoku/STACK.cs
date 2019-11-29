using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sodoku
{
    class STACK<T>
    {
        Item header = null;
        public void push(T item)
        {
            if(header == null)
            {
                header = new Item();
                header.data = item;
                header.next = null;
            }
            else
            {
                Item temp = new Item();
                temp.next = header;
                temp.data = item;
                header = temp;
            }
        }

        public bool isEmpty()
        {
            if(header == null)
            {
                return true;
            }
            return false;
        }

        public T pop()
        {
            if(header != null)
            {
                T result = header.data;
                header = header.next;
                return result;
            }
            throw new Exception("Stack null");
        }

        private class Item
        {
            public T data;
            public Item next;
        }
    }
}
