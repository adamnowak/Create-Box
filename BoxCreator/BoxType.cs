using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BoxCreator
{
  public class BoxType
  {
    /// <summary>
    /// Types of box
    /// </summary> 
    public enum BoxTypeEnum { Open, Close, WithCover }

    /// <summary>
    /// Converts int value to box type enum.
    /// </summary>
    /// <param name="intBoxType">Type of the int box.</param>
    /// <returns>Open if intBoxType == 1; WithCover if intBoxType == 3; otherwise  </returns>
    public static BoxTypeEnum IntToBoxTypeEnum(int intBoxType)
    {
      BoxTypeEnum result = BoxTypeEnum.Open;
      if (intBoxType == 2)
        result = BoxTypeEnum.Close;
      if (intBoxType == 3)
        result = BoxTypeEnum.WithCover;
      return result;
    }

    /// <summary>
    /// Convert box type enum to int.
    /// </summary>
    /// <param name="intBoxType">Type of the int box.</param>
    /// <returns></returns>
    public static int BoxTypeEnumToInt(BoxTypeEnum intBoxType)
    {
      int result = 1;
      if (intBoxType == BoxTypeEnum.Close)
        result = 2;
      if (intBoxType == BoxTypeEnum.WithCover)
        result = 3;
      return result;
    }

    /// <summary>
    /// Converts box type enum to string.
    /// </summary>
    /// <param name="boxTypeEnum">The wall type enum.</param>
    /// <returns>String describing wallTypeEnum</returns>
    public static string BoxTypeEnumToString(BoxTypeEnum boxTypeEnum)
    {
      string result = "Unknown";
      switch (boxTypeEnum)
      {
        case BoxTypeEnum.Open:
          result = "Open";
          break;
        case BoxTypeEnum.Close:
          result = "Close";
          break;
        case BoxTypeEnum.WithCover:
          result = "WithCover";
          break;
      }
      return result;
    }
  }
}