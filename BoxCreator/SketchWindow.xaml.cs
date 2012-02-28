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


    /// <summary>
    /// Initializes a new instance of the <see cref="SketchWindow"/> class.
    /// </summary>
    public SketchWindow()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Add item button click handler. Creates and adds the item (text or image) to selected element.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
    public void AddItemToSelectedElementClick(object sender, RoutedEventArgs e)
    {
      AddElementWindow addElementWindow = new AddElementWindow();
      addElementWindow.ShowDialog();
      FrameworkElement elem = addElementWindow.Element;
      WallToEdit.AddElement(elem);
    }

    /// <summary>
    /// Up button click handler. Move selected element at 1 px up.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
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
      }
    }

    private void OkClick(object sender, RoutedEventArgs e)
    {
      cnsWallTable.CopyToWall(_wallToEdit);
      Close();
    }

    private void CnsWallTableLoaded(object sender, RoutedEventArgs e)
    {
      _wallToEdit.EditableCanvas = cnsWallTable;
      _wallToEdit.CanvasWidth = int.Parse(cnsWallTable.Parent.GetValue(ActualWidthProperty).ToString());
      _wallToEdit.CanvasHeight = int.Parse(cnsWallTable.Parent.GetValue(ActualHeightProperty).ToString());
      _wallToEdit.CopyToWall(cnsWallTable, true);
      Title = Title + " - edited wall (" + WallType.WallTypeEnumToString(_wallToEdit.WallType) + ")";
      foreach (ComboBoxItem cbi in cbWallColorSelection.Items)
      {
        if (_wallToEdit.WallColor.ToString() == cbi.Tag.ToString())
        {
          cbWallColorSelection.SelectedItem = cbi;
          break;
        }
      }

    }

    private void CancelClick(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void CbWallColorSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ComboBox comboBox = sender as ComboBox;
      if (comboBox != null)
      {
        ComboBoxItem comboBoxItem = comboBox.SelectedItem as ComboBoxItem;
        string color = comboBoxItem.Tag.ToString();
        cnsWallTable.WallColor = (Color)ColorConverter.ConvertFromString(color);
      }
    }

    private void DeleteItemClick(object sender, RoutedEventArgs e)
    {
      WallToEdit.DeleteEditedElement();
    }
  }
}

