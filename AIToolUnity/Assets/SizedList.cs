using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    [Serializable]
    public class SizedList<T> 
    {
        private T[] array;
        private int _size;
        private int currentSize;

        public SizedList(int maxSize)
        {
            array = new T[maxSize];
            _size = maxSize;
            currentSize = 0;
        }
        /// <summary>
        /// Adds an item to the array. If the array is full, it will bump of the first
        /// item in the array, shuffle everything down, and place the new item at the end
        /// of the list
        /// </summary>
        /// <param name="item">place on back of array, push off first item</param>
        /// <returns></returns>
        public bool Add(T item)
        {
            bool toReturn = false;
            if (item != null)
            {
                toReturn = true;
                if (currentSize == _size)
                {
                    for (int i = 0; i < _size; i++)
                    {
                        if (i < _size - 1)
                        {
                            array[i] = array[i + 1];
                        }
                    }
                    array[_size - 1] = item;
                }
                else
                {
                    array[currentSize++] = item;
                }
            }
            return toReturn;
        }

        public T Get(int index)
        {
            if (index < 0 || index > _size - 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            else
            {
                return array[index];
            }
        }

        public int Count
        {
            get { return currentSize; }
        }
    }


