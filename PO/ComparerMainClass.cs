using System;
using System.Collections.Generic;
using System.Text;

using com.hujun64.po;

namespace com.hujun64.po
{
    public class ComparerMainClass : IComparer<MainClass>
    {
        private bool IS_ASC = true;
        public ComparerMainClass()
        {
        }
        public ComparerMainClass(bool isASC)
        {
            IS_ASC = isASC;
        }
        public int Compare(MainClass x, MainClass y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else if (y == null)
            {
                return -1;
            }
            else if (x.id == y.id)
            {
                return 0;
            }
            else
            {
                return x.class_parent.CompareTo(y.class_parent);
            }
        }





    }
}
