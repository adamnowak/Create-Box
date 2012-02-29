using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace BoxCreator
{
  /// <summary>
  /// Interaction logic for AddElementWindow.xaml
  /// </summary>
  public partial class AddElementWindow : Window
  {
    public AddElementWindow()
    {
      InitializeComponent();
    }

    public FrameworkElement Element { get; private set; }

    private void BrowseClick(object sender, RoutedEventArgs e)
    {
       OpenFileDialog ofd = new OpenFileDialog();
       ofd.Multiselect = false;
       bool? b = ofd.ShowDialog();
       if (b != null && b.Value == true)
       {
         txtBoxImagePath.Text = ofd.FileName;
       }
    }

    private void OkClick(object sender, RoutedEventArgs e)
    {
      if (tabContItemSelector.SelectedItem == tabItemText)
      {
        string color = ((ComboBoxItem)cbFontColorSelection.SelectedItem).Tag.ToString();
        Element = Wall.CreateTextBlock(txtBoxTextToInsert.Text, new SolidColorBrush((Color)ColorConverter.ConvertFromString(color)));
      }
      if (tabContItemSelector.SelectedItem == tabItemImage)
      {
        Element = Wall.CreateImage(txtBoxImagePath.Text);
      }
      Close();
    }

    private void CancelClick(object sender, RoutedEventArgs e)
    {
      Element = null;
      Close();
    }
  }
}
