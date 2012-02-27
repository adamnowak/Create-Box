using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BoxCreator
{
  /// <summary>
  /// Class represents wall of the box.
  /// </summary>
  public class Wall
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Wall"/> class.
    /// </summary>
    /// <param name="height">The height.</param>
    /// <param name="width">The width.</param>
    /// <param name="color">The color.</param>
    public Wall(int width, int height, Color color, WallType wallType)
    {
      Height = height;
      Width = width;
      WallColor = color;
      WallCanvas = new Canvas();
      WallCanvas.Height = Height;
      WallCanvas.Width = Width;
      WallCanvas.Background = new SolidColorBrush(WallColor);
      WallType = wallType;
    }

    /// <summary>
    /// Shows the specified canvas on display wall.
    /// </summary>
    /// <param name="canvasToDisplayWall">The canvas to display wall.</param>
    /// <param name="heightCanvasToDisplayWall">The height canvas to display wall.</param>
    /// <param name="widthCanvasToDisplayWall">The width canvas to display wall.</param>
    public void Show(Canvas canvasToDisplayWall, int heightCanvasToDisplayWall, int widthCanvasToDisplayWall)
    {

    }

    /// <summary>
    /// Gets the height.
    /// </summary>
    public int Height { get; private set; }

    /// <summary>
    /// Gets the width.
    /// </summary>
    public int Width { get; private set; }

    /// <summary>
    /// Gets the color of the wall.
    /// </summary>
    /// <value>
    /// The color of the wall.
    /// </value>
    public Color WallColor { get; private set; }

    /// <summary>
    /// Gets the wall canvas.
    /// </summary>
    public Canvas WallCanvas { get; private set; }

    /// <summary>
    /// Gets the type of the wall.
    /// </summary>
    /// <value>
    /// The type of the wall.
    /// </value>
    public WallType WallType { get; private set; }
  }
}