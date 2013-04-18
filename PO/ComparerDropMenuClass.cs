using System;
using System.Collections.Generic;
using System.Text;

using com.hujun64.po;

namespace com.hujun64.logic
{
    public class ComparerDropMenuClass : IComparer<DropMenuClass>
    {

        public ComparerDropMenuClass()
        {
        }

        public int Compare(DropMenuClass x, DropMenuClass y)
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
            else if (x.mainClass.Equals(y.mainClass))
            {
                return 0;
            }
            else if (x.mainClass.id==y.mainClass.id)
            {
                return 0;
            }
            else if (x.boot == y.boot)
            {
                if (x.mainClass.class_parent == y.mainClass.class_parent)
                {
                    return x.sort_seq.CompareTo(y.sort_seq);
                }
                else
                {
                    return x.mainClass.class_parent.CompareTo(y.mainClass.class_parent);
                }


            }
            else
            {
                return x.boot.CompareTo(y.boot);
            }
        }
    }
}
