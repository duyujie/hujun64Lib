using System;
using System.Collections.Generic;
using System.Text;

using com.hujun64.po;

namespace com.hujun64.logic
{
    public class ComparerLink : IComparer<Link>
    {
        private bool IS_ASC = true;
        public ComparerLink()
        {
        }
        public ComparerLink(bool isASC)
        {
            IS_ASC = isASC;
        }
        public int Compare(Link x, Link y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    if (IS_ASC)
                        return 1;
                    else
                        return -1;
                }
            }
            else if (y == null)
            {
                if (IS_ASC)
                    return -1;
                else
                    return 1;

            }
            else if (x.link_id == y.link_id)
            {
                return 0;
            }
            else
            {
                if (IS_ASC)
                    return x.sort_seq.CompareTo(y.sort_seq);
                else
                    return y.sort_seq.CompareTo(x.sort_seq);

            }
        }
    }
}
