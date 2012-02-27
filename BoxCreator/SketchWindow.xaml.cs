using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BoxCreator
{
  /// <summary>
  /// Interaction logic for SketchWindow.xaml
  /// </summary>
  public partial class SketchWindow : Window
  {
    private Point clickPosition;
    private bool isMouseDown = false;

    public SketchWindow()
    {
      InitializeComponent();
    }


    public void AddItemToSelectedElementClick(object sender, RoutedEventArgs e)
    {
      AddElementWindow addElementWindow = new AddElementWindow();
      addElementWindow.ShowDialog();
      FrameworkElement elem = addElementWindow.Element;
      if (elem != null)
      {
         WallToEdit.AddTransformationsToElement(elem);
        elem.MouseLeftButtonDown += SelectElementToEdit;
        elem.MouseMove += MoveMouse;
        elem.MouseLeftButtonUp += UnselectElementToEdit;
        editableCanvas.Children.Add(elem);
      }

      //FrameworkElement elem = null;
      //Random rd = new Random();
      ////zawsze tekst tymczasowo
      //if (rd.Next() % 2 == 0)
      //{
      //  Image img = new Image();
      //  ImageSourceConverter converter = new ImageSourceConverter();
      //  img.Source = (ImageSource)converter.ConvertFromString("test.png");
      //  img.Width = 60;
      //  img.Height = 30;
      //  elem = img;
      //}
      //else
      //{
      //  TextBlock tb = new TextBlock();
      //  tb.Text = "test";
      //  elem = tb;
      //}
     
    }

    
    private void SelectElementToEdit(object sender, MouseButtonEventArgs e)
    {
      WallToEdit.RegisterTransformsElement(e.OriginalSource as UIElement);
      clickPosition = e.GetPosition(editableCanvas);
      isMouseDown = true;
    }
    private void UnselectElementToEdit(object sender, MouseButtonEventArgs e)
    {
      isMouseDown = false;
    }
    private void MoveMouse(object sender, MouseEventArgs e)
    {
      if (isMouseDown)
      {
        Point mousePos = e.GetPosition(editableCanvas);
        Point delta = new Point(mousePos.X - clickPosition.X, mousePos.Y - clickPosition.Y);
        WallToEdit.ChangeEditedElementPosition(delta);
        clickPosition = mousePos;
      }
    }

    private void MoveUp(object sender, RoutedEventArgs e)
    {
      WallToEdit.MoveEditedElementUp();
    }

    private void MoveDown(object sender, RoutedEventArgs e)
    {
      WallToEdit.MoveEditedElementDown();
    }

    private void MoveLeft(object sender, RoutedEventArgs e)
    {
      WallToEdit.MoveEditedElementLeft();
    }

    private void MoveRight(object sender, RoutedEventArgs e)
    {
      WallToEdit.MoveEditedElementRight();
    }

    private void TurnLeft(object sender, RoutedEventArgs e)
    {
      WallToEdit.TurnEditedElementLeft();
    }

    private void TurnRight(object sender, RoutedEventArgs e)
    {
      WallToEdit.TurnEditedElementRight();
    }

    private void Enlarge(object sender, RoutedEventArgs e)
    {
      WallToEdit.EnlargeEditedElement();
    }

    private void Shrink(object sender, RoutedEventArgs e)
    {
      WallToEdit.ShrinkEditedElement();
    }

    /// <summary>
    /// Handles the Closing event of the Window control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      Wall.CopyCanvases(editableCanvas, _wallToEdit);      
    }

    private Wall _wallToEdit;
    /// <summary>
    /// Gets or sets the wall which will be displayed on sketch window. This wall can be modified.
    /// </summary>
    /// <value>
    /// The wall to edit.
    /// </value>
    public Wall WallToEdit
    {
      get { return _wallToEdit; }

      set
      {
        _wallToEdit = value;
        Wall.CopyCanvases(_wallToEdit, editableCanvas, true);
        Title = Title + " - edited wall (" + WallType.WallTypeEnumToString(_wallToEdit.WallType) +")";
      }
    }

    private void OkClick(object sender, RoutedEventArgs e)
    {
      Close();
    }
  }
}
