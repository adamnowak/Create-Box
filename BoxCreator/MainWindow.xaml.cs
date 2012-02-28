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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace BoxCreator
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    /// <summary>
    /// Box which is displayed on this window.
    /// </summary>
    private Box box;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
      InitializeComponent();
      box = new Box();
    }

    /// <summary>
    /// Handles the Click event of the Preview control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
    private void PreviewClick(object sender, RoutedEventArgs e)
    {
      ThreeDPreviewWindow preview = new ThreeDPreviewWindow();
      preview.BoxToDisplay3D = box;
      preview.ShowDialog();
    }

    /// <summary>
    /// Gets the parsed dimension.
    /// </summary>
    /// <param name="text">The text in control.</param>
    /// <param name="dimension">The dimension description.</param>
    /// <param name="result">The parsed result.</param>
    /// <returns>True if parsing was posible; otherwise false</returns>
    private bool GetParsedDimension(string text, string dimension, out int result)
    {
      if (!int.TryParse(text, out result))
      {
        MessageBox.Show(dimension + " nie jest liczbą.");
        return false;
      }
      if (result <= 0)
      {
        MessageBox.Show(dimension + " musi byc wieksza od zera");
        return false;
      }
      return true;
    }

    /// <summary>
    /// Handles the Click event of the Reset control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
    /// <remarks>This funciton will rebuild the box.</remarks>
    private void ResetClick(object sender, RoutedEventArgs e)
    {
      int width = 0;
      int length = 0;
      int height = 0;
      int coverHeight = 0;
      

      if (!GetParsedDimension(txtBoxWidth.Text, txtBoxWidth.Name, out width))
        return;
      if (!GetParsedDimension(txtBoxLength.Text, txtBoxLength.Name, out length))
        return;
      if (!GetParsedDimension(txtBoxHeigth.Text, txtBoxHeigth.Name, out height))
        return;
      if (rbBoxWithCoverType.IsChecked.HasValue &&
          rbBoxWithCoverType.IsChecked.Value)
      {
        if (!GetParsedDimension(txtBoxCoverHeight.Text, txtBoxCoverHeight.Name, out coverHeight))
        {
          return;
        }
        if (coverHeight > height)
        {
          MessageBox.Show("Wysokosc pokrywki nie moze byc wieksza niz wysokosc pudelka.");
          return;
        }
      }

      BoxType.BoxTypeEnum boxType = BoxType.BoxTypeEnum.Open;
      if (rbCloseBoxType.IsChecked.HasValue && rbCloseBoxType.IsChecked.Value)
        boxType = BoxType.BoxTypeEnum.Close;
      else
        if (rbBoxWithCoverType.IsChecked.HasValue && rbBoxWithCoverType.IsChecked.Value)
          boxType = BoxType.BoxTypeEnum.WithCover;

      box.Rebuild(cnsBoxTable, width, length, height, coverHeight, boxType);
      box.WallMouseButtonEventHandler = BoxWallMouseLeftButtonDown;
      box.Show();
    }

    /// <summary>
    /// Handler for click on wall. The window for edition wall will  be opened.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
    void BoxWallMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Wall wallToDisplay = sender as Wall;
      if (wallToDisplay != null)
      {
        SketchWindow sketch = new SketchWindow();
        //box.BoxCanvas.Children.Remove(wallToDisplay);
        sketch.WallToEdit = wallToDisplay;
        sketch.ShowDialog();
        //box.BoxCanvas.Children.Add(wallToDisplay);
      }
    }

    /// <summary>
    /// Handler for click on save button. Save box to selected file.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
    private void SaveClick(object sender, RoutedEventArgs e)
    {
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.Filter = "Box files (*.box)|*.box|All files (*.*)|*.*";
      bool? b = sfd.ShowDialog();
      if (b != null && b.Value == true)
      {
        box.Save(sfd.FileName);
      }
    }

    /// <summary>
    /// Handler for click on open button. Open box from selected file.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
    private void OpenClick(object sender, RoutedEventArgs e)
    {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Multiselect = false;
      ofd.Filter = "Box files (*.box)|*.box|All files (*.*)|*.*";
      bool? b = ofd.ShowDialog();
      if (b != null && b.Value == true)
      {
        box.WallMouseButtonEventHandler = BoxWallMouseLeftButtonDown;
        box.Load(cnsBoxTable, ofd.FileName);
        box.Show();
        txtBoxWidth.Text = box.RealWidth.ToString();
        txtBoxLength.Text = box.RealLength.ToString();
        txtBoxHeigth.Text = box.RealHeight.ToString();
        txtBoxCoverHeight.Text = box.RealCoverHeight.ToString();
      }
    }

    private void CnsBoxTableLoaded(object sender, RoutedEventArgs e)
    {
      box.MainCanvasHeight = (int)cnsBoxTable.ActualHeight;
      box.MainCanvasWidth = (int)cnsBoxTable.ActualWidth;
    }

    private void CloseApplicationClick(object sender, RoutedEventArgs e)
    {
      Close();
    }
  }
}
