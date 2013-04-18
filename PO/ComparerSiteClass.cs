using System;
using System.Collections.Generic;
using System.Text;

using com.hujun64.po;

namespace com.hujun64.po
{
    public class ComparerSiteClass : IComparer<SiteClass>
    {
        private bool IS_ASC = true;
        public ComparerSiteClass()
        {
        }
        public ComparerSiteClass(bool isASC)
        {
            IS_ASC = isASC;
        }
        public int Compare(SiteClass x, SiteClass y)
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
            else if (x.site_id == y.site_id)
            {
                return x.sort_seq.CompareTo(y.sort_seq);
            }
            else
            {
                return x.site_id.CompareTo(y.site_id);
            }
        }





    }
}
