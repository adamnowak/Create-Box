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


    public SketchWindow()
    {
      InitializeComponent();
    }


    public void AddItemToSelectedElementClick(object sender, RoutedEventArgs e)
    {
      AddElementWindow addElementWindow = new AddElementWindow();
      addElementWindow.ShowDialog();
      FrameworkElement elem = addElementWindow.Element;
      WallToEdit.AddElement(elem);
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
      cnsWallTable.CopyToWall(_wallToEdit);
     
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
        //_wallToEdit.EditableCanvas = cnsWallTable;
        //_wallToEdit.CopyToWall(cnsWallTable, true);
        //Title = Title + " - edited wall (" + WallType.WallTypeEnumToString(_wallToEdit.WallType) +")";
        
      }
    }

    private void OkClick(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void CnsWallTableLoaded(object sender, RoutedEventArgs e)
    {
      _wallToEdit.EditableCanvas = cnsWallTable;
      _wallToEdit.CanvasWidth = int.Parse(cnsWallTable.Parent.GetValue(ActualWidthProperty).ToString());
      _wallToEdit.CanvasHeight = int.Parse(cnsWallTable.Parent.GetValue(ActualHeightProperty).ToString());
      _wallToEdit.CopyToWall(cnsWallTable, true);
      Title = Title + " - edited wall (" + WallType.WallTypeEnumToString(_wallToEdit.WallType) + ")";

    }

    private void CancelClick(object sender, RoutedEventArgs e)
    {
      Close();
    }
  }
}
