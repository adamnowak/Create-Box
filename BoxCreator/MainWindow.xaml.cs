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
    private Box box;

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
        MessageBox.Show(dimension+" musi byc wieksza od zera");
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
      int tableSize = 550;

      if (!GetParsedDimension(txtBoxWidth.Text, txtBoxWidth.Name, out width))
        return;
      if (!GetParsedDimension(txtBoxLength.Text, txtBoxLength.Name, out length))
        return;
      if (!GetParsedDimension(txtBoxHeigth.Text, txtBoxHeigth.Name, out height))
        return;
      if (rbBoxWithCoverType.IsChecked.HasValue &&
        rbBoxWithCoverType.IsChecked.Value &&
        !GetParsedDimension(txtBoxCoverHeight.Text, txtBoxCoverHeight.Name, out coverHeight))
        return;
      if (coverHeight > height)
      {
        MessageBox.Show("Wysokosc pokrywki nie moze byc wieksza niz wysokosc pudelka.");
        return;
      }

      box.Rebuild(cnsBoxTable, width, length, height, coverHeight, 550, 550);
      box.WallMouseButtonEventHandler = boxWall_MouseLeftButtonDown;
      box.Show();      
    }

    void boxWall_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

    private void OpenClick(object sender, RoutedEventArgs e)
    {
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Multiselect = false;
      ofd.Filter = "Box files (*.box)|*.box|All files (*.*)|*.*";
      bool? b = ofd.ShowDialog();      
      if (b != null && b.Value == true)
      {
        box.WallMouseButtonEventHandler = boxWall_MouseLeftButtonDown;
        box.Load(cnsBoxTable, ofd.FileName);
        box.Show();
        txtBoxWidth.Text = box.RealWidth.ToString();
        txtBoxLength.Text = box.RealLength.ToString();
        txtBoxHeigth.Text = box.RealHeight.ToString();
        txtBoxCoverHeight.Text = box.RealCoverHeight.ToString();
      }
    }
  }
}
