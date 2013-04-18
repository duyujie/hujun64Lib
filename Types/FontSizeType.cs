namespace com.hujun64.type
{
    /// <summary>
    ///FontSize 的摘要说明
    /// </summary>
    public class FontSizeType
    {

        public readonly FontSizeEnum FontSize;
        public FontSizeType(FontSizeEnum fontSize)
        {
            this.FontSize = fontSize;
        }

       public override string ToString()
       {
           switch (FontSize)
           {
               case FontSizeEnum.LARGE:
                   return "大";
               case FontSizeEnum.MIDDLE:
                   return "中";
               case FontSizeEnum.SMALL:
                   return "小";
               default:
                   return FontSize.ToString();
           }
       }

    }

}