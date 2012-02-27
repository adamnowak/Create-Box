using System;
using System.Collections.Generic;
using System.IO;
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
  /// Class represents wall of the box.
  /// </summary>
  public class Wall : Canvas
  {
    public const int EdgeGap = 10;
    public const int CanvasWidth = 440;
    public const int CanvasHeight = 440;

    /// <summary>
    /// Initializes a new instance of the <see cref="Wall"/> class.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="color">The color.</param>
    /// <param name="wallTypeEnum">Type of the wall.</param>
    public Wall(int width, int height, Color color, WallType.WallTypeEnum wallTypeEnum)
    {
      Height = height;
      Width = width;
      WallColor = color;
      WallType = wallTypeEnum;
    }

    private Color _wallColor;
    /// <summary>
    /// Gets the color of the wall.
    /// </summary>
    /// <value>
    /// The color of the wall.
    /// </value>
    public Color WallColor
    {
      get { return _wallColor; }
      set
      {
        _wallColor = value;
        Background = new SolidColorBrush(_wallColor);
      }
    }

    /// <summary>
    /// Gets the type of the wall.
    /// </summary>
    /// <value>
    /// The type of the wall.
    /// </value>
    public WallType.WallTypeEnum WallType { get; private set; }

    /// <summary>
    /// Copy the children between canvases. The children from source canvas will be removed.
    /// </summary>
    /// <param name="sourceCanvas">Source canvase.</param>
    /// <param name="destinationCanvas">Destination cavase.</param>
    /// <param name="moveSize">if set to <c>true</c> the destination canvase size will be set to source canvase size.</param>
    public static void CopyCanvases(Canvas sourceCanvas, Canvas destinationCanvas, bool moveSize = false)
    {
      if (moveSize)
      {
        destinationCanvas.Height = sourceCanvas.Height;
        destinationCanvas.Width = sourceCanvas.Width;
      }
      destinationCanvas.Children.Clear();
      List<UIElement> col = new List<UIElement>();
      foreach (UIElement ele in sourceCanvas.Children)
      {
        col.Add(ele);
      }
      sourceCanvas.Children.Clear();
      foreach (UIElement ele in col)
      {
        destinationCanvas.Children.Add(ele);
      }

      destinationCanvas.Background = sourceCanvas.Background;

      if (moveSize)
      {
        double scale = GetScale((int)destinationCanvas.Width, (int)destinationCanvas.Height, CanvasWidth, CanvasHeight);
        destinationCanvas.RenderTransform = new ScaleTransform();
        {
          ((ScaleTransform)destinationCanvas.RenderTransform).ScaleX = scale;
          ((ScaleTransform)destinationCanvas.RenderTransform).ScaleY = scale;
        }

        Thickness m = destinationCanvas.Margin;
        m.Left = EdgeGap + (double)(CanvasWidth - EdgeGap * 2 - GetSizeToDisplay(scale, (int)destinationCanvas.Width)) / 2;
        m.Top = EdgeGap + (double)(CanvasHeight - EdgeGap * 2 - GetSizeToDisplay(scale, (int)destinationCanvas.Height)) / 2;
        destinationCanvas.Margin = m;
      }
    }

    public static double GetScale(int width, int height, int displayWidth = 440, int displayHeight = 440)
    {
      //scale which will be return
      double scale = 1;
      if (((double)(EdgeGap * 2 + height) / displayHeight) > ((double)(EdgeGap * 2 + width) / displayWidth))
      {
        if ((EdgeGap * 2 + height) > displayHeight)
        {
          scale = (double)(height) / (displayHeight - EdgeGap * 2);
        }
        else
        {
          scale = (double)(displayHeight - EdgeGap * 2) / (height);
        }
      }
      else
      {
        if ((EdgeGap * 2 + width) > displayWidth)
        {
          scale = (double)(width) / (displayWidth - EdgeGap * 2);
        }
        else
        {
          scale = (double)(displayWidth - EdgeGap * 2) / (width);
        }
      }
      return scale;
    }


    private static RotateTransform _rotate;
    private static TranslateTransform _translate;
    private static ScaleTransform _scale;
    private static FrameworkElement _editedElement = null;

    public void AddTransformationsToElement(FrameworkElement elem)
    {
      TransformGroup tg = new TransformGroup();
      ScaleTransform sc = new ScaleTransform();
      tg.Children.Add(sc);
      RotateTransform rt = new RotateTransform();
      tg.Children.Add(rt);
      TranslateTransform tt = new TranslateTransform();
      tg.Children.Add(tt);
      elem.RenderTransform = tg;
    }

    public void CleanTransforms()
    {
      if (_editedElement != null)
      {
        _editedElement.Opacity = 1.0;
        _editedElement = null;
      }
      _rotate = null;
      _translate = null;
      _scale = null;
    }

    public void RegisterTransformsElement(UIElement elem)
    {
      CleanTransforms();
      if (elem != null && elem.RenderTransform is TransformGroup)
      {
        if (elem is FrameworkElement)
        {
          _editedElement = elem as FrameworkElement;
          _editedElement.Opacity = 0.75;
        }
        TransformGroup tg = elem.RenderTransform as TransformGroup;
        foreach (Transform tran in tg.Children)
        {
          if (tran is TranslateTransform) _translate = tran as TranslateTransform;
          if (tran is RotateTransform) _rotate = tran as RotateTransform;
          if (tran is ScaleTransform) _scale = tran as ScaleTransform;
        }
      }
    }

    private void MoveEditedElementX(double x)
    {
      if (_translate != null)
        _translate.X = x;
    }
    private void MoveEditedElementY(double y)
    {
      if (_translate != null)
        _translate.Y = y;
    }

    public void MoveEditedElementLeft()
    {
      if (_translate != null)
        _translate.X -= 1;
    }
    public void MoveEditedElementRight()
    {
      if (_translate != null)
        _translate.X += 1;
    }
    public void MoveEditedElementDown()
    {
      if (_translate != null)
        _translate.Y += 1;
    }
    public void MoveEditedElementUp()
    {
      if (_translate != null)
        _translate.Y -= 1;
    }

    private void TurnEditedElement(double angle)
    {
      if (_rotate != null)
        _rotate.Angle = angle;
    }

    public void TurnEditedElementLeft()
    {
      if (_rotate != null)
        _rotate.Angle -= 1;
    }
    public void TurnEditedElementRight()
    {
      if (_rotate != null)
        _rotate.Angle += 1;
    }

    private void ScaleEditedElement(double scale)
    {
      if (_scale != null)
      {
        _scale.ScaleX = scale;
        _scale.ScaleY = scale;
      }
    }

    public void EnlargeEditedElement()
    {
      if (_scale != null)
      {
        _scale.ScaleX += 0.01;
        _scale.ScaleY += 0.01;
      }
    }
    public void ShrinkEditedElement()
    {
      if (_scale != null)
      {
        _scale.ScaleX -= 0.01;
        _scale.ScaleY -= 0.01;
      }
    }
    public void ChangeEditedElementPosition(Point p)
    {
      if (_translate != null)
      {
        _translate.X += p.X;
        _translate.Y += p.Y;
      }
    }

    /// <summary>
    /// Gets the size to display for given real dimention.
    /// </summary>
    /// <param name="scale">The scale.</param>
    /// <param name="realDimention">The real dimention.</param>
    /// <returns>
    /// Size to diplay.
    /// </returns>
    private static int GetSizeToDisplay(double scale, int realDimention)
    {
      return (int)(scale * realDimention);
    }

    public void Save(XmlElement boxXmlElement)
    {
      if (boxXmlElement == null) return;
      XmlElement xmlWallElement = boxXmlElement.OwnerDocument.CreateElement(WallType.ToString());
      boxXmlElement.AppendChild(xmlWallElement);
      foreach (FrameworkElement frameworkElement in Children)
      {
        SaveElement(xmlWallElement, frameworkElement);
      }
    }

    private void SaveElement(XmlElement xmlWallElement, FrameworkElement frameworkElement)
    {
      if (xmlWallElement == null) return;

      XmlElement xmlItemElement = null;
      if (frameworkElement is TextBlock)
      {
        TextBlock textBlock = (TextBlock)frameworkElement;
        xmlItemElement = xmlWallElement.OwnerDocument.CreateElement("Text");
        xmlWallElement.AppendChild(xmlItemElement);
        xmlItemElement.SetAttribute("Text", textBlock.Text);

      }
      if (frameworkElement is Image)
      {
        Image image = (Image)frameworkElement;
        xmlItemElement = xmlWallElement.OwnerDocument.CreateElement("Image");
        xmlWallElement.AppendChild(xmlItemElement);
        xmlItemElement.SetAttribute("ImagePath", image.Tag.ToString());
      }
      if (xmlItemElement != null && frameworkElement.RenderTransform != null && frameworkElement.RenderTransform is TransformGroup)
      {
        TransformGroup tranGroup = (TransformGroup)frameworkElement.RenderTransform;

        ScaleTransform scaleTransform = GetSccaleTransform(tranGroup);
        if (scaleTransform != null)
          xmlItemElement.SetAttribute("Scale", scaleTransform.ScaleX.ToString());

        RotateTransform rotateTransform = GetRotateTransform(tranGroup);
        if (rotateTransform != null)
          xmlItemElement.SetAttribute("Angle", rotateTransform.Angle.ToString());

        TranslateTransform translateTransform = GetTranslateTransform(tranGroup);
        if (translateTransform != null)
        {
          xmlItemElement.SetAttribute("X", translateTransform.X.ToString());
          xmlItemElement.SetAttribute("Y", translateTransform.Y.ToString());
        }
      }
    }

    private ScaleTransform GetSccaleTransform(TransformGroup transformGroup)
    {
      foreach (Transform transform in transformGroup.Children)
      {
        if (transform is ScaleTransform)
          return (ScaleTransform)transform;
      }
      return null;
    }

    private RotateTransform GetRotateTransform(TransformGroup transformGroup)
    {
      foreach (Transform transform in transformGroup.Children)
      {
        if (transform is RotateTransform)
          return (RotateTransform)transform;
      }
      return null;
    }

    private TranslateTransform GetTranslateTransform(TransformGroup transformGroup)
    {
      foreach (Transform transform in transformGroup.Children)
      {
        if (transform is TranslateTransform)
          return (TranslateTransform)transform;
      }
      return null;
    }

    public void Load(XmlElement xmlWallElement)
    {
      foreach (XmlElement xmlItemElement in xmlWallElement.ChildNodes)
      {
        FrameworkElement frameworkElement = null;

        switch (xmlItemElement.Name)
        {
          case "Text":
            TextBlock textBlock = new TextBlock();
            textBlock.Text = xmlItemElement.GetAttribute("Text");
            frameworkElement = textBlock;


            break;
          case "Image":
            string imagePath = xmlItemElement.GetAttribute("ImagePath");
            if (File.Exists(imagePath))
            {
              Image img = new Image();
              ImageSourceConverter converter = new ImageSourceConverter();
              img.Source = (ImageSource) converter.ConvertFromString(imagePath);
              img.Width = 60;
              img.Height = 30;
              img.Tag = imagePath;
              frameworkElement = img;
            }
            break;
        }

        if (frameworkElement != null)
        {
          AddElement(frameworkElement, this);
          RegisterTransformsElement(frameworkElement);
          MoveEditedElementX(double.Parse(xmlItemElement.GetAttribute("X")));
          MoveEditedElementY(double.Parse(xmlItemElement.GetAttribute("Y")));
          ScaleEditedElement(double.Parse(xmlItemElement.GetAttribute("Scale")));
          TurnEditedElement(double.Parse(xmlItemElement.GetAttribute("Angle")));
        }
      }
    }

    private void AddElement(FrameworkElement elem, Canvas destinationCanvas)
    {
      if (elem != null && destinationCanvas != null)
      {
        AddTransformationsToElement(elem);
        elem.MouseLeftButtonDown += SelectElementToEdit;
        elem.MouseMove += MoveMouse;
        elem.MouseLeftButtonUp += UnselectElementToEdit;
        destinationCanvas.Children.Add(elem);
      }
    }

    public void AddElement(FrameworkElement elem)
    {
      AddElement(elem, EditableCanvas);      
    }

    
    private Point _clickPosition;
    private bool _isMouseDown = false;

    private void SelectElementToEdit(object sender, MouseButtonEventArgs e)
    {
      RegisterTransformsElement(e.OriginalSource as UIElement);
      _clickPosition = e.GetPosition(EditableCanvas);
      _isMouseDown = true;
    }
    private void UnselectElementToEdit(object sender, MouseButtonEventArgs e)
    {
      _isMouseDown = false;
    }
    public void MoveMouse(object sender, MouseEventArgs e)
    {
      if (_isMouseDown)
      {
        Point mousePos = e.GetPosition(EditableCanvas);
        Point delta = new Point(mousePos.X - _clickPosition.X, mousePos.Y - _clickPosition.Y);
        ChangeEditedElementPosition(delta);
        _clickPosition = mousePos;
      }
    }


    private Canvas _editableCanvas;
    public Canvas EditableCanvas
    {
      get { return _editableCanvas; }
      set
      {
        if (_editableCanvas != null)
          _editableCanvas.MouseMove -= MoveMouse;
        _editableCanvas = value;
        _editableCanvas.MouseMove += MoveMouse;
      }
    }
  }
}