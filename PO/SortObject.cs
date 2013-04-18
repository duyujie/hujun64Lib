namespace com.hujun64.util
{
    /// <summary>
    ///SortObject 的摘要说明
    /// </summary>
    public class SortObject
    {

        public string id;
        public int sortSeq;

        //默认是sortSeq小的排在前面
        public SortObject(string id,int sortSeq)
        {
            this.id=id;
            this.sortSeq=sortSeq;
        }

        //return the exchanged otherSortObject
        public SortObject ExchangeSortSeq(SortObject otherSortObject)
        {
            int tempSortSeq =sortSeq;
            
            sortSeq = otherSortObject.sortSeq;
            otherSortObject.sortSeq = tempSortSeq;

            return otherSortObject;
        }
        //return the exchanged otherSortObject
        public SortObject ExchangeUpSortSeq(SortObject otherSortObject)
        {          

            if (sortSeq == otherSortObject.sortSeq){
                sortSeq = otherSortObject.sortSeq - 1;

            }
            else if (sortSeq > otherSortObject.sortSeq)
            {
                int tempSortSeq = sortSeq;
                sortSeq = otherSortObject.sortSeq;
                otherSortObject.sortSeq = tempSortSeq;
            }
                       

            return otherSortObject;
        }
        //return the exchanged otherSortObject
        public SortObject ExchangeDownSortSeq(SortObject otherSortObject)
        {


            if (sortSeq == otherSortObject.sortSeq)
            {
                sortSeq = otherSortObject.sortSeq + 1;

            }
            else if (sortSeq < otherSortObject.sortSeq)
            {
                int tempSortSeq = sortSeq;
                sortSeq = otherSortObject.sortSeq;
                otherSortObject.sortSeq = tempSortSeq;
            }


            return otherSortObject;
        }
    }
}
