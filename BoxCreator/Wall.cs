using System.IO;
using System.Windows;
using System.Windows.Controls;
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
    public Wall()
    {
      Height = 440;
      Width = 440;
      WallColor = Colors.White;
      WallType = BoxCreator.WallType.WallTypeEnum.Unknown;
    }


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
    /// <param name="destinationWall">Destination cavase.</param>
    /// <param name="moveSize">if set to <c>true</c> the destination canvase size will be set to source canvase size.</param>
    public void CopyToWall(Wall destinationWall, bool moveSize = false)
    {
      if (moveSize)
      {
        destinationWall.Height = Height;
        destinationWall.Width = Width;
      }
      destinationWall.Children.Clear();

      foreach (FrameworkElement sourceFrameworkElement in Children)
      {
        FrameworkElement frameworkElement = null;

        if (sourceFrameworkElement is TextBlock)
        {
          frameworkElement = CreateTextBlock(((TextBlock)sourceFrameworkElement).Text);
        }

        if (sourceFrameworkElement is Image)
        {
          frameworkElement = CreateImage(sourceFrameworkElement.Tag.ToString());
        }

        if (frameworkElement != null)
        {
          AddElement(frameworkElement, destinationWall);
          RegisterTransformsElement(frameworkElement);

          if (frameworkElement.RenderTransform != null && frameworkElement.RenderTransform is TransformGroup)
          {
            TransformGroup tranGroup = (TransformGroup)sourceFrameworkElement.RenderTransform;

            ScaleTransform scaleTransform = GetScaleTransform(tranGroup);
            if (scaleTransform != null)
              ScaleEditedElement(scaleTransform.ScaleX);

            RotateTransform rotateTransform = GetRotateTransform(tranGroup);
            if (rotateTransform != null)
              TurnEditedElement(rotateTransform.Angle);

            TranslateTransform translateTransform = GetTranslateTransform(tranGroup);
            if (translateTransform != null)
            {
              MoveEditedElementX(translateTransform.X);
              MoveEditedElementY(translateTransform.Y);
            }
          }

        }
      }
      destinationWall.Background = Background;

      if (moveSize)
      {
        double scale = GetScale((int)destinationWall.Width, (int)destinationWall.Height, CanvasWidth, CanvasHeight);
        destinationWall.RenderTransform = new ScaleTransform();
        {
          ((ScaleTransform)destinationWall.RenderTransform).ScaleX = scale;
          ((ScaleTransform)destinationWall.RenderTransform).ScaleY = scale;
        }

        Thickness m = destinationWall.Margin;
        m.Left = EdgeGap + (double)(CanvasWidth - EdgeGap * 2 - GetSizeToDisplay(scale, (int)destinationWall.Width)) / 2;
        m.Top = EdgeGap + (double)(CanvasHeight - EdgeGap * 2 - GetSizeToDisplay(scale, (int)destinationWall.Height)) / 2;
        destinationWall.Margin = m;
      }
    }

    /// <summary>
    /// Creates the text block with given text.
    /// </summary>
    /// <param name="text">The text which will be set as Text property in creating TextBlock.</param>
    /// <returns>Created TextBlock.</returns>
    public static TextBlock CreateTextBlock(string text)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.Text = text;
      return textBlock;
    }

    /// <summary>
    /// Creates the image from given image path.
    /// </summary>
    /// <param name="imagePath">The image path.</param>
    /// <returns>Created Image.</returns>
    public static Image CreateImage(string imagePath)
    {
      if (File.Exists(imagePath))
      {
        Image image = new Image();
        ImageSourceConverter converter = new ImageSourceConverter();
        image.Source = (ImageSource)converter.ConvertFromString(imagePath);
        image.Width = 60;
        image.Height = 30;
        image.Tag = imagePath;
        return image;
      }
      return null;
    }

    /// <summary>
    /// Gets the scale which will be used to display wall on the EditableCanvas.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="displayWidth">The display width.</param>
    /// <param name="displayHeight">The display height.</param>
    /// <returns>Calculated scale.</returns>
    public static double GetScale(int width, int height, int displayWidth = 800, int displayHeight = 640)
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


    /// <summary>
    /// Rotate transform of edited element.
    /// </summary>
    private static RotateTransform _rotate;
    /// <summary>
    /// Translate transform of edited element.
    /// </summary>
    private static TranslateTransform _translate;
    /// <summary>
    /// Scale transform of edited element.
    /// </summary>
    private static ScaleTransform _scale;
    /// <summary>
    /// Edited element.
    /// </summary>
    private static FrameworkElement _editedElement = null;

    /// <summary>
    /// Adds the transformations to element.
    /// </summary>
    /// <param name="elem">The elem.</param>
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

    /// <summary>
    /// Cleans the transforms.
    /// </summary>
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

    /// <summary>
    /// Registers the transforms element.
    /// </summary>
    /// <param name="elem">The elem.</param>
    /// <remarks>The _editedElement is set to elem. The _translate, _rotate and _scale are set to tranforms of _editedElement.</remarks>
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

    /// <summary>
    /// Moves the edited element vertically to x.
    /// </summary>
    /// <param name="x">The x.</param>
    private void MoveEditedElementX(double x)
    {
      if (_translate != null)
        _translate.X = x;
    }
    /// <summary>
    /// Moves the edited element horizontally to y.
    /// </summary>
    /// <param name="y">The y.</param>
    private void MoveEditedElementY(double y)
    {
      if (_translate != null)
        _translate.Y = y;
    }

    /// <summary>
    /// Moves the edited element left.
    /// </summary>
    public void MoveEditedElementLeft()
    {
      if (_translate != null)
        _translate.X -= 1;
    }
    /// <summary>
    /// Moves the edited element right.
    /// </summary>
    public void MoveEditedElementRight()
    {
      if (_translate != null)
        _translate.X += 1;
    }
    /// <summary>
    /// Moves the edited element down.
    /// </summary>
    public void MoveEditedElementDown()
    {
      if (_translate != null)
        _translate.Y += 1;
    }
    /// <summary>
    /// Moves the edited element up.
    /// </summary>
    public void MoveEditedElementUp()
    {
      if (_translate != null)
        _translate.Y -= 1;
    }

    /// <summary>
    /// Turns the edited element to angle.
    /// </summary>
    /// <param name="angle">The angle.</param>
    private void TurnEditedElement(double angle)
    {
      if (_rotate != null)
        _rotate.Angle = angle;
    }

    /// <summary>
    /// Turns the edited element left.
    /// </summary>
    public void TurnEditedElementLeft()
    {
      if (_rotate != null)
        _rotate.Angle -= 1;
    }
    /// <summary>
    /// Turns the edited element right.
    /// </summary>
    public void TurnEditedElementRight()
    {
      if (_rotate != null)
        _rotate.Angle += 1;
    }

    /// <summary>
    /// Scales the edited element using scale.
    /// </summary>
    /// <param name="scale">The scale.</param>
    private void ScaleEditedElement(double scale)
    {
      if (_scale != null)
      {
        _scale.ScaleX = scale;
        _scale.ScaleY = scale;
      }
    }

    /// <summary>
    /// Enlarges the edited element.
    /// </summary>
    public void EnlargeEditedElement()
    {
      if (_scale != null)
      {
        _scale.ScaleX += 0.01;
        _scale.ScaleY += 0.01;
      }
    }
    /// <summary>
    /// Shrinks the edited element.
    /// </summary>
    public void ShrinkEditedElement()
    {
      if (_scale != null)
      {
        _scale.ScaleX -= 0.01;
        _scale.ScaleY -= 0.01;
      }
    }
    /// <summary>
    /// Changes the edited element position.
    /// </summary>
    /// <param name="p">The p - new position.</param>
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

    /// <summary>
    /// Saves the wall to box XML element.
    /// </summary>
    /// <param name="boxXmlElement">The box XML element - to this element will be added wall XML element.</param>
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

    /// <summary>
    /// Saves the wall element.
    /// </summary>
    /// <param name="xmlWallElement">The XML wall element.</param>
    /// <param name="frameworkElement">The framework element.</param>
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

        ScaleTransform scaleTransform = GetScaleTransform(tranGroup);
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

    /// <summary>
    /// Gets the scale transform from transform group.
    /// </summary>
    /// <param name="transformGroup">The transform group.</param>
    /// <returns>Scale transform if exists in transformGroup; otherwise null.</returns>
    private static ScaleTransform GetScaleTransform(TransformGroup transformGroup)
    {
      if (transformGroup == null) return null;

      foreach (Transform transform in transformGroup.Children)
      {
        if (transform is ScaleTransform)
          return (ScaleTransform)transform;
      }
      return null;
    }

    /// <summary>
    /// Gets the rotate transform from transform group.
    /// </summary>
    /// <param name="transformGroup">The transform group.</param>
    /// <returns>Rotate transform if exists in transformGroup; otherwise null.</returns>
    private static RotateTransform GetRotateTransform(TransformGroup transformGroup)
    {
      if (transformGroup == null) return null;

      foreach (Transform transform in transformGroup.Children)
      {
        if (transform is RotateTransform)
          return (RotateTransform)transform;
      }
      return null;
    }

    /// <summary>
    /// Gets the translate transform from transform group.
    /// </summary>
    /// <param name="transformGroup">The transform group.</param>
    /// <returns>Translate transform if exists in transformGroup; otherwise null.</returns>
    private static TranslateTransform GetTranslateTransform(TransformGroup transformGroup)
    {
      if (transformGroup == null) return null;

      foreach (Transform transform in transformGroup.Children)
      {
        if (transform is TranslateTransform)
          return (TranslateTransform)transform;
      }
      return null;
    }

    /// <summary>
    /// Loads the specified XML wall element.
    /// </summary>
    /// <param name="xmlWallElement">The XML wall element.</param>
    public void Load(XmlElement xmlWallElement)
    {
      foreach (XmlElement xmlItemElement in xmlWallElement.ChildNodes)
      {
        FrameworkElement frameworkElement = null;

        switch (xmlItemElement.Name)
        {
          case "Text":
            frameworkElement = CreateTextBlock(xmlItemElement.GetAttribute("Text"));
            break;
          case "Image":
            frameworkElement = CreateImage(xmlItemElement.GetAttribute("ImagePath"));
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

    /// <summary>
    /// Adds the element to destinationCanvas.
    /// </summary>
    /// <param name="elem">The elem - which will be added.</param>
    /// <param name="destinationCanvas">The destination canvas.</param>
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

    /// <summary>
    /// Adds the element to EditableCanvas.
    /// </summary>
    /// <param name="elem">The elem - which will be added.</param>
    public void AddElement(FrameworkElement elem)
    {
      AddElement(elem, EditableCanvas);
    }

    /// <summary>
    /// Position where user press left button of mouse (later will be changed to position when element was updated).
    /// </summary>
    private Point _clickPosition;
    /// <summary>
    /// Flag indicated if mouse is down.
    /// </summary>
    private bool _isMouseDown = false;

    /// <summary>
    /// Selects the element to edit.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
    private void SelectElementToEdit(object sender, MouseButtonEventArgs e)
    {
      RegisterTransformsElement(e.OriginalSource as UIElement);
      _clickPosition = e.GetPosition(EditableCanvas);
      _isMouseDown = true;
    }
    /// <summary>
    /// Unselects the element to edit.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
    private void UnselectElementToEdit(object sender, MouseButtonEventArgs e)
    {
      _isMouseDown = false;
    }
    /// <summary>
    /// Moves the mouse event handler.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
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
    /// <summary>
    /// Gets or sets the editable canvas. The canvas where wall be edited.
    /// </summary>
    /// <value>
    /// The editable canvas.
    /// </value>
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