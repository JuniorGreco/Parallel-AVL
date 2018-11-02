using System;

namespace Tree_Parallel.Models
{
    public class Leaf
    {
        private int height;
        private Int64 value;
        private Leaf left;
        private Leaf right;

        public Int64 Value
        {
            get { return value; }
            set { this.value = value; }
        }
        
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        
        public Leaf Left
        {
            get { return left; }
            set { left = value; }
        }
        
        public Leaf Right
        {
            get { return right; }
            set { right = value; }
        }

    }
}