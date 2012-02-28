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