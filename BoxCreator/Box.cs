using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;


namespace BoxCreator
{
  /// <summary>
  /// Class represents box. Holds functionality for creating box with or without cover.
  /// </summary>
  public class Box
  {
    /// <summary>
    /// Gap between wall and border of canvas
    /// </summary>
    public const int EdgeGap = 3;
    /// <summary>
    /// Gap between box walls 
    /// </summary>
    public const int WallGap = 5;
    /// <summary>
    /// Gap between box and cover (if box is created with cover)
    /// </summary>
    public const int CoverGap = 15;

    /// <summary>
    /// Initializes a new instance of the <see cref="Box"/> class.
    /// </summary>
    public Box()
    {
      IsRebuild = false;
    }

    /// <summary>
    /// Rebuilds the specified main canvas.
    /// </summary>
    /// <param name="canvasToDisplayBox">The canvas on which walls of box will be displayed.</param>
    /// <param name="realWidth">Width of the real box.</param>
    /// <param name="realLength">Length of the real.</param>
    /// <param name="realHeight">Height of the real.</param>
    /// <param name="realCoverHeight">Height of the real cover.</param>
    /// <param name="boxType">Type of the box.</param>
    public void Rebuild(Canvas canvasToDisplayBox, int realWidth, int realLength, int realHeight, int realCoverHeight, BoxType.BoxTypeEnum boxType)
    {
      BoxCanvas = canvasToDisplayBox;
      BoxCanvas.Children.Clear();

      RealWidth = realWidth;
      RealLength = realLength;
      RealHeight = realHeight;
      RealCoverHeight = realCoverHeight;
      BoxType = boxType;
      

      Scale = GetScale(realWidth, realLength, realHeight, realCoverHeight, BoxType, MainCanvasHeight, MainCanvasWidth); 

      UpWall = new Wall(GetSizeToDisplay(RealLength), GetSizeToDisplay(RealWidth), Colors.Yellow, WallType.WallTypeEnum.Up);
      BackWall = new Wall(GetSizeToDisplay(RealLength), GetSizeToDisplay(RealHeight), Colors.Orange, WallType.WallTypeEnum.Back);
      BottomWall = new Wall(GetSizeToDisplay(RealLength), GetSizeToDisplay(RealWidth), Colors.Black, WallType.WallTypeEnum.Bottom);
      FrontWall = new Wall(GetSizeToDisplay(RealLength), GetSizeToDisplay(RealHeight), Colors.Beige, WallType.WallTypeEnum.Front);
      LeftWall = new Wall(GetSizeToDisplay(RealHeight), GetSizeToDisplay(RealWidth), Colors.Orchid, WallType.WallTypeEnum.Left);
      RightWall = new Wall(GetSizeToDisplay(RealHeight), GetSizeToDisplay(RealWidth), Colors.Olive, WallType.WallTypeEnum.Right);

      CoverWall = new Wall(GetSizeToDisplay(RealLength), GetSizeToDisplay(RealWidth), Colors.Yellow, WallType.WallTypeEnum.Cover);
      BackCoverWall = new Wall(GetSizeToDisplay(RealLength), GetSizeToDisplay(RealCoverHeight), Colors.Orange, WallType.WallTypeEnum.BackCover);
      FrontCoverWall = new Wall(GetSizeToDisplay(RealLength), GetSizeToDisplay(RealCoverHeight), Colors.Orange, WallType.WallTypeEnum.FrontCover);
      LeftCoverWall = new Wall(GetSizeToDisplay(RealCoverHeight), GetSizeToDisplay(RealWidth), Colors.Orchid, WallType.WallTypeEnum.LeftCover);
      RightCoverWall = new Wall(GetSizeToDisplay(RealCoverHeight), GetSizeToDisplay(RealWidth), Colors.Orchid, WallType.WallTypeEnum.RightCover);

      //calculation for displaying on the center the canvas
      int lengthToDisplay = (int)(2 * EdgeGap + 2 * WallGap + GetSizeToDisplay(2 * RealHeight + RealLength));
      int widthToDisplay = (int)(2 * EdgeGap + 2 * WallGap + GetSizeToDisplay(2 * RealHeight + RealWidth));
      if (BoxType == BoxCreator.BoxType.BoxTypeEnum.Close)
      {
        lengthToDisplay = (int)(2 * EdgeGap + 2 * WallGap + GetSizeToDisplay(2 * RealHeight + RealLength));
        widthToDisplay = (int)(2 * EdgeGap + 3 * WallGap + GetSizeToDisplay(2 * RealHeight + 2 * RealWidth));
      }
      if (BoxType == BoxCreator.BoxType.BoxTypeEnum.WithCover)
      {
        lengthToDisplay = (int)(2 * EdgeGap + 2 * WallGap + GetSizeToDisplay(2 * RealHeight + RealLength));
        widthToDisplay = (int)(4 * WallGap + 2 * EdgeGap + CoverGap + GetSizeToDisplay(2 * RealHeight + 2 * RealWidth + 2 * RealCoverHeight));
      }

      LeftShift = (MainCanvasWidth - lengthToDisplay) / 2;
      TopShift = (MainCanvasHeight - widthToDisplay) / 2;

      IsRebuild = true;
    }

    /// <summary>
    /// Gets a value indicating whether this box is rebuild.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this box was rebuild; otherwise, <c>false</c>.
    /// </value>
    public bool IsRebuild { get; private set; }

    /// <summary>
    /// Gets or sets the left shift to display box on the center of the canvas.
    /// </summary>
    /// <value>
    /// The left shift.
    /// </value>
    private int LeftShift { get; set; }

    /// <summary>
    /// Gets or sets the top shift to display box on the center of the canvas.
    /// </summary>
    /// <value>
    /// The top shift.
    /// </value>
    private int TopShift { get; set; }

    /// <summary>
    /// Gets or sets the wall mouse button event handler - is used for mouse operation.
    /// </summary>
    /// <value>
    /// The wall mouse button event handler.
    /// </value>
    public MouseButtonEventHandler WallMouseButtonEventHandler { get; set; }

    /// <summary>
    /// Shows the box on the BoxCanvas.
    /// </summary>
    public void Show()
    {
      if (BoxType == BoxCreator.BoxType.BoxTypeEnum.WithCover)
      {
        //TODO:
        AddWall(BackCoverWall, TopShift + EdgeGap, LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
        AddWall(CoverWall, TopShift + EdgeGap + WallGap + GetSizeToDisplay(RealCoverHeight), LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
        AddWall(FrontCoverWall, TopShift + EdgeGap + 2 * WallGap + GetSizeToDisplay(RealCoverHeight + RealWidth), LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
        AddWall(LeftCoverWall, TopShift + EdgeGap + WallGap + GetSizeToDisplay(RealCoverHeight), LeftShift + EdgeGap + GetSizeToDisplay(RealHeight - RealCoverHeight));
        AddWall(RightCoverWall, TopShift + EdgeGap + WallGap + GetSizeToDisplay(RealCoverHeight), LeftShift + EdgeGap + 2*WallGap + GetSizeToDisplay(RealHeight + RealLength));

        AddWall(BackWall, TopShift + EdgeGap + 2 * WallGap + CoverGap + GetSizeToDisplay(2*RealCoverHeight + RealWidth), LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
        AddWall(BottomWall, TopShift + EdgeGap + 3 * WallGap + CoverGap + GetSizeToDisplay(2 * RealCoverHeight + RealWidth + RealHeight), LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
        AddWall(FrontWall, TopShift + EdgeGap + 4 * WallGap + CoverGap + GetSizeToDisplay(2 * RealCoverHeight + 2 * RealWidth + RealHeight), LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
        AddWall(LeftWall, TopShift + EdgeGap + 3 * WallGap + CoverGap + GetSizeToDisplay(2 * RealCoverHeight + RealWidth + RealHeight), LeftShift + EdgeGap);
        AddWall(RightWall, TopShift + EdgeGap + 3 * WallGap + CoverGap + GetSizeToDisplay(2 * RealCoverHeight + RealWidth + RealHeight), LeftShift + EdgeGap + 2 * WallGap + GetSizeToDisplay(RealHeight + RealLength));
      }
      else
        if (BoxType == BoxCreator.BoxType.BoxTypeEnum.Close)
        {
          AddWall(UpWall, TopShift + EdgeGap, LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
          AddWall(BackWall, TopShift + EdgeGap + WallGap + GetSizeToDisplay(RealWidth), LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
          AddWall(BottomWall, TopShift + EdgeGap + 2 * WallGap + GetSizeToDisplay(RealWidth + RealHeight), LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
          AddWall(FrontWall, TopShift + EdgeGap + 3 * WallGap + GetSizeToDisplay(2 * RealWidth + RealHeight), LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
          AddWall(LeftWall, TopShift + EdgeGap + 2 * WallGap + GetSizeToDisplay(RealWidth + RealHeight), LeftShift + EdgeGap);
          AddWall(RightWall, TopShift + EdgeGap + 2 * WallGap + GetSizeToDisplay(RealWidth + RealHeight), LeftShift + EdgeGap + 2 * WallGap + GetSizeToDisplay(RealHeight + RealLength));
        }
        else
        {          
          AddWall(BackWall, TopShift + EdgeGap, LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
          AddWall(BottomWall, TopShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight), LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
          AddWall(FrontWall, TopShift + EdgeGap + 2 * WallGap + GetSizeToDisplay(RealWidth + RealHeight), LeftShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight));
          AddWall(LeftWall, TopShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight), LeftShift + EdgeGap);
          AddWall(RightWall, TopShift + EdgeGap + WallGap + GetSizeToDisplay(RealHeight), LeftShift + EdgeGap + 2 * WallGap + GetSizeToDisplay(RealHeight + RealLength));
        }
    }

    /// <summary>
    /// Adds the wall display to the box display.
    /// </summary>
    /// <param name="wall">Wall which will be added.</param>
    /// <param name="top">The top position.</param>
    /// <param name="left">The left position.</param>
    private void AddWall(Wall wall, int top, int left)
    {
      BoxCanvas.Children.Add(wall);
      Canvas.SetLeft(wall, left);
      Canvas.SetTop(wall, top);
      wall.MouseLeftButtonDown += WallMouseButtonEventHandler;
    }

    /// <summary>
    /// Gets the size to display for given real dimention.
    /// </summary>
    /// <param name="realDimention">The real dimention.</param>
    /// <returns>Size to diplay.</returns>
    private int GetSizeToDisplay(int realDimention)
    {
      return (int)(Scale * realDimention);
    }

    /// <summary>
    /// Gets the main canvas, where box will be display.
    /// </summary>
    public Canvas BoxCanvas { get; private set; }

    /// <summary>
    /// Gets the width of the real box.
    /// </summary>
    /// <value>
    /// The width of the real box.
    /// </value>
    public int RealWidth { get; private set; }

    /// <summary>
    /// Gets the length of the real.
    /// </summary>
    /// <value>
    /// The length of the real.
    /// </value>
    public int RealLength { get; private set; }

    /// <summary>
    /// Gets the height of the real.
    /// </summary>
    /// <value>
    /// The height of the real.
    /// </value>
    public int RealHeight { get; private set; }

    /// <summary>
    /// Gets the height of the real cover.
    /// </summary>
    /// <value>
    /// The height of the real cover.
    /// </value>
    public int RealCoverHeight { get; private set; }

    /// <summary>
    /// Gets or sets the height of the main canvas.
    /// </summary>
    /// <value>
    /// The height of the main canvas.
    /// </value>
    public int MainCanvasHeight { get; set; }

    /// <summary>
    /// Gets or sets the width of the main canvas.
    /// </summary>
    /// <value>
    /// The width of the main canvas.
    /// </value>
    public int MainCanvasWidth { get; set; }

    /// <summary>
    /// Gets the scale used to display the box on main canvas.
    /// </summary>
    public double Scale { get; private set; }

    /// <summary>
    /// Gets up wall.
    /// </summary>
    public Wall UpWall { get; private set; }
    /// <summary>
    /// Gets the back wall.
    /// </summary>
    public Wall BackWall { get; private set; }
    /// <summary>
    /// Gets the bottom wall.
    /// </summary>
    public Wall BottomWall { get; private set; }
    /// <summary>
    /// Gets the front wall.
    /// </summary>
    public Wall FrontWall { get; private set; }
    /// <summary>
    /// Gets the left wall.
    /// </summary>
    public Wall LeftWall { get; private set; }
    /// <summary>
    /// Gets the right wall.
    /// </summary>
    public Wall RightWall { get; private set; }

    /// <summary>
    /// Gets the cover wall.
    /// </summary>
    public Wall CoverWall { get; private set; }
    /// <summary>
    /// Gets the back cover wall.
    /// </summary>
    public Wall BackCoverWall { get; private set; }
    /// <summary>
    /// Gets the front cover wall.
    /// </summary>
    public Wall FrontCoverWall { get; private set; }
    /// <summary>
    /// Gets the left cover wall.
    /// </summary>
    public Wall LeftCoverWall { get; private set; }
    /// <summary>
    /// Gets the right cover wall.
    /// </summary>
    public Wall RightCoverWall { get; private set; }

    private BoxType.BoxTypeEnum _boxType;
    /// <summary>
    /// Gets or sets the type of the box.
    /// </summary>
    /// <value>
    /// The type of the box.
    /// </value>
    public BoxType.BoxTypeEnum BoxType
    {
      get { return _boxType; }
      private set
      {
        _boxType = value;
        if (_boxType == BoxCreator.BoxType.BoxTypeEnum.WithCover && RealCoverHeight <= 0)
          _boxType = BoxCreator.BoxType.BoxTypeEnum.Open;
      }
    }

    /// <summary>
    /// Gets the scale which should be used to display the walls.
    /// </summary>
    /// <param name="width">The width of box.</param>
    /// <param name="length">The lenght of box.</param>
    /// <param name="height">The height of box.</param>
    /// <param name="coverHeight">Height of the cover (if cover used &gt; 0; ohterwise 0).</param>
    /// <param name="boxType">Type of the box.</param>
    /// <param name="tableWidth">Size of the table where box will be shown.</param>
    /// <param name="tableHeight">Height of the table.</param>
    /// <returns>
    /// Scale calculated.
    /// </returns>
    /// <remarks>
    /// To calculte is used <see cref="EdgeGap"/>, <see cref="WallGap"/> and <see cref="CoverGap"/>.
    /// </remarks>
    private double GetScale(int width, int length, int height, int coverHeight, BoxType.BoxTypeEnum boxType, int tableWidth = 550, int tableHeight = 550)
    {
      //scale which will be return
      double scale = 1;

      //gaps which apear on length
      int gapsLength = 2 * EdgeGap + 2 * WallGap;
      //gaps which apear on width
      int gapsWidth = 2 * WallGap + 2 * EdgeGap;
      //length of expanded box (only box) 
      int boxExpandedLength = 2 * height + length;
      //width of expanded box (only box) 
      int boxExpandedWidth = 2 * height + width;

      if (boxType == BoxCreator.BoxType.BoxTypeEnum.Close)
      {
        gapsWidth = 3 * WallGap + 2 * EdgeGap;
        boxExpandedWidth = 2 * height + 2 * width;        
      }
      if (boxType == BoxCreator.BoxType.BoxTypeEnum.WithCover)
      {
        boxExpandedWidth = 2 * height + 2 * width + 2 * coverHeight;
        gapsWidth = 4 * WallGap + 2 * EdgeGap + CoverGap;
      }
      //scale should be used only for box not for gap!!!
      if (gapsLength + boxExpandedLength > gapsWidth + boxExpandedWidth)
      {
        if (gapsLength + boxExpandedLength > tableWidth)
        {
          scale = (double)(boxExpandedLength) / (tableWidth - gapsLength);
        }
        else
        {
          scale = (double)(tableWidth - gapsLength) / (boxExpandedLength);
        }
      }
      else
      {
        if (gapsWidth + boxExpandedWidth > tableWidth - gapsWidth)
        {
          scale = (double)(boxExpandedWidth) / (tableWidth - gapsWidth);
        }
        else
        {
          scale = (double)(tableWidth - gapsWidth) / (boxExpandedWidth);
        }
      }
      return scale;
    }

    /// <summary>
    /// Saves box to file specified in path parameter.
    /// </summary>
    /// <param name="path">The path to file.</param>
    public void Save(string path)
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        XmlElement boxXmlElement = doc.CreateElement("Box");
        boxXmlElement.SetAttribute("RealWidth", RealWidth.ToString());
        boxXmlElement.SetAttribute("RealLength", RealLength.ToString());
        boxXmlElement.SetAttribute("RealHeight", RealHeight.ToString());
        boxXmlElement.SetAttribute("RealCoverHeight", RealCoverHeight.ToString());
        boxXmlElement.SetAttribute("BoxType", BoxCreator.BoxType.BoxTypeEnumToInt(BoxType).ToString());

        doc.AppendChild(boxXmlElement);

        CoverWall.Save(boxXmlElement);
        LeftCoverWall.Save(boxXmlElement);
        BackCoverWall.Save(boxXmlElement);
        FrontCoverWall.Save(boxXmlElement);
        RightCoverWall.Save(boxXmlElement);

        UpWall.Save(boxXmlElement);
        BottomWall.Save(boxXmlElement);
        FrontWall.Save(boxXmlElement);
        BackWall.Save(boxXmlElement);
        LeftWall.Save(boxXmlElement);
        RightWall.Save(boxXmlElement);

        doc.Save(path);
      }
      catch (Exception)
      {

        MessageBox.Show("Zapisanie pliku nie powiodlo sie.");
        //TODO: powinno wywalac wyjatek
      }
    }

    /// <summary>
    /// Loads box from the specified file to canvas.
    /// </summary>
    /// <param name="canvasToDisplay">The canvas to display loaded box.</param>
    /// <param name="filePath">The path of file which will be loaded.</param>
    public void Load(Canvas canvasToDisplay, string filePath)
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(filePath);
        if (doc.DocumentElement != null && doc.DocumentElement.Name == "Box")
        {
          XmlElement boxXmlElement = doc.DocumentElement;
          int realWidth = int.Parse(boxXmlElement.GetAttribute("RealWidth"));
          int realLength = int.Parse(boxXmlElement.GetAttribute("RealLength"));
          int realHeight = int.Parse(boxXmlElement.GetAttribute("RealHeight"));
          int realCoverHeight = int.Parse(boxXmlElement.GetAttribute("RealCoverHeight"));
          int boxType = int.Parse(boxXmlElement.GetAttribute("BoxType"));
          Rebuild(canvasToDisplay, realWidth, realLength, realHeight, realCoverHeight, BoxCreator.BoxType.IntToBoxTypeEnum(boxType));


          foreach (XmlElement xmlWallElement in boxXmlElement.ChildNodes)
          {
            switch (xmlWallElement.Name)
            {
              case "Up":
                UpWall.Load(xmlWallElement);
                break;
              case "Bottom":
                BottomWall.Load(xmlWallElement);
                break;
              case "Front":
                FrontWall.Load(xmlWallElement);
                break;
              case "Back":
                BackWall.Load(xmlWallElement);
                break;
              case "Left":
                LeftWall.Load(xmlWallElement);
                break;
              case "Right":
                RightWall.Load(xmlWallElement);
                break;
              //TODO: with cover
            }
          }
        }
      }
      catch (Exception)
      {
        MessageBox.Show("Wczytanie pliku nie powiodlo się.");
        //TODO: powinno wywalac wyjatek
      }
    }
  }
}