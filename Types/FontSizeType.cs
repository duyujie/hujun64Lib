namespace com.hujun64.type
{
    /// <summary>
    ///FontSize ��ժҪ˵��
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
                   return "��";
               case FontSizeEnum.MIDDLE:
                   return "��";
               case FontSizeEnum.SMALL:
                   return "С";
               default:
                   return FontSize.ToString();
           }
       }

    }

}