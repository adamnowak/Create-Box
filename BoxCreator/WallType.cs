using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BoxCreator
{
  public class WallType
  {
    /// <summary>
    /// Types of walls
    /// </summary> 
    public enum WallTypeEnum { Front, Left, Bottom, Right, Back, Up, Cover, FrontCover, BackCover, LeftCover, RightCover }

    /// <summary>
    /// Converts wall type enum to string.
    /// </summary>
    /// <param name="wallTypeEnum">The wall type enum.</param>
    /// <returns>String describing wallTypeEnum</returns>
    public static string WallTypeEnumToString(WallTypeEnum wallTypeEnum)
    {
      string result = "Unknown";
      switch (wallTypeEnum)
      {
        case WallTypeEnum.Front:
          result = "Front";
          break;
        case WallTypeEnum.Left:
          result = "Left";
          break;
        case WallTypeEnum.Bottom:
          result = "Bottom";
          break;
        case WallTypeEnum.Right:
          result = "Right";
          break;
        case WallTypeEnum.Back:
          result = "Back";
          break;
        case WallTypeEnum.Up:
          result = "Up";
          break;
        case WallTypeEnum.Cover:
          result = "Cover";
          break;
        case WallTypeEnum.FrontCover:
          result = "Front of cover";
          break;
        case WallTypeEnum.BackCover:
          result = "Back of cover";
          break;
        case WallTypeEnum.LeftCover:
          result = "Left of cover";
          break;
        case WallTypeEnum.RightCover:
          result = "Right of cover";
          break;

      }
      return result;
    }
  }
}