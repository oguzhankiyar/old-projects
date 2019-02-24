using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Classes
{
    class ObjectList
    {
        public BaseObject Head { get; set; }
        public BaseObject Last { get; set; }
        public int Count { get; set; }

        public ObjectList()
        {
            Count = 0;
            Head = null;
            Last = null;
        }
        bool isEmpty()
        {
            return Head == null;
        }
        public BaseObject GetObject(int id)
        {
            BaseObject current = Head;
            while (current != null)
            {
                if (current.ID == id)
                    return current;
                current = current.Next;
            }
            return null;
        }
        private BaseObject GetObjectFromPosition(int position)
        {
            BaseObject current = Head;
            for (int i = 0; current != null && i < position; i++)
            {
                current = current.Next;
            }
            return current;
        }
        public int GetPosition(int id)
        {
            BaseObject current = Head;
            for (int i = 0; current != null; i++)
            {
                if (id == current.ID)
                    return i;
                current = current.Next;
            }
            return -1;
        }
        public bool Add(BaseObject newObject)
        {
            if (newObject == null)
                return false;
            if (GetPosition(newObject.ID) != -1)
                return false;
            if (Head == null)
                (Head = newObject).Next = null;
            else
                Last.Next = newObject;
            Last = newObject;
            Count++;
            return true;
        }
        public bool Delete(int id)
        {
            if (this.isEmpty())
                return false;
            int position = GetPosition(id);
            BaseObject prevObject = GetObjectFromPosition(position - 1);
            BaseObject nextObject = GetObjectFromPosition(position + 1);
            prevObject.Next = nextObject;
            Count--;
            return true;
        }
    }
}
