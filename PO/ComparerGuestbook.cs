using System;
using System.Collections.Generic;
using System.Text;

using com.hujun64.po;

namespace com.hujun64.logic
{
    public class ComparerGuestbook : IComparer<Guestbook>
    {
        private bool IS_ASC = false;
        public ComparerGuestbook()
        {
        }
        public ComparerGuestbook(bool isASC)
        {
            IS_ASC = isASC;
        }
        public int Compare(Guestbook x, Guestbook y)
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
            else if (x.id == y.id)
            {
                return 0;
            }else
            {
                if (IS_ASC)
                   return x.addtime.CompareTo(y.addtime);
                else
                    return y.addtime.CompareTo(x.addtime);
                
            }
        }
    } 
}
