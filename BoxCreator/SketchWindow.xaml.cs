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



    public class Kontroler
    {

      private static RotateTransform rotacja;
      private static TranslateTransform translacja;
      private static ScaleTransform skala;
      private static FrameworkElement lastselected = null;

      static public void Dolacz_Uklad(FrameworkElement elem)
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

      public static void CleanTransforms()
      {
        if (lastselected != null)
        {
          lastselected.Opacity = 1.0;
          lastselected = null;
        }
        rotacja = null;
        translacja = null;
        skala = null;
      }
      public static void RegisterElementTransforms(UIElement elem)
      {
        CleanTransforms();
        if (elem != null && elem.RenderTransform is TransformGroup)
        {
          if (elem is FrameworkElement)
          {
            lastselected = elem as FrameworkElement;
            lastselected.Opacity = 0.75;
          }
          TransformGroup tg = elem.RenderTransform as TransformGroup;
          foreach (Transform tran in tg.Children)
          {
            if (tran is TranslateTransform) translacja = tran as TranslateTransform;
            if (tran is RotateTransform) rotacja = tran as RotateTransform;
            if (tran is ScaleTransform) skala = tran as ScaleTransform;
          }
        }
      }
      public static void PrzesunLewo()
      {
        if (translacja != null)
          translacja.X -= 1;
      }
      public static void PrzesunPrawo()
      {
        if (translacja != null)
          translacja.X += 1;
      }
      public static void PrzesunDol()
      {
        if (translacja != null)
          translacja.Y += 1;
      }
      public static void PrzesunGora()
      {
        if (translacja != null)
          translacja.Y -= 1;
      }

      public static void ObrocLewo()
      {
        if (rotacja != null)
          rotacja.Angle -= 1;
      }
      public static void ObrocPrawo()
      {
        if (rotacja != null)
          rotacja.Angle += 1;
      }
      public static void Powieksz()
      {
        if (skala != null)
        {
          skala.ScaleX += 0.01;
          skala.ScaleY += 0.01;
        }
      }
      public static void Pomniejsz()
      {
        if (skala != null)
        {
          skala.ScaleX -= 0.01;
          skala.ScaleY -= 0.01;
        }
      }
      public static void AddElementPosition(Point p)
      {
        if (translacja != null)
        {
          translacja.X += p.X;
          translacja.Y += p.Y;
        }
      }
    }

    public void DodajElementClick(object sender, RoutedEventArgs e)
    {
      FrameworkElement elem = null;
      Random rd = new Random();
      //zawsze tekst tymczasowo
      if (false)//rd.Next() % 2 == 0)
      {
        Image img = new Image();
        ImageSourceConverter converter = new ImageSourceConverter();
        img.Source = (ImageSource)converter.ConvertFromString("test.jpg");
        img.Width = 60;
        img.Height = 30;
        elem = img;
      }
      else
      {
        TextBlock tb = new TextBlock();
        tb.Text = "test";
        elem = tb;
      }
      Kontroler.Dolacz_Uklad(elem);
      elem.MouseLeftButtonDown += WybranoElement;
      elem.MouseMove += RuchMyszy;
      elem.MouseLeftButtonUp += KoniecEdycji;
      kartka.Children.Add(elem);
    }

    private Point pozycja;
    private bool isMouseDown = false;
    private void WybranoElement(object sender, MouseButtonEventArgs e)
    {
      Kontroler.RegisterElementTransforms(e.OriginalSource as UIElement);
      pozycja = e.GetPosition(kartka);
      isMouseDown = true;
    }
    private void KoniecEdycji(object sender, MouseButtonEventArgs e)
    {
      isMouseDown = false;
    }
    private void RuchMyszy(object sender, MouseEventArgs e)
    {
      if (isMouseDown)
      {
        Point mousePos = e.GetPosition(kartka);
        Point delta = new Point(mousePos.X - pozycja.X, mousePos.Y - pozycja.Y);
        Kontroler.AddElementPosition(delta);
        pozycja = mousePos;
      }
    }

    private void PrzesunGora(object sender, RoutedEventArgs e)
    {
      Kontroler.PrzesunGora();
    }

    private void PrzesunDol(object sender, RoutedEventArgs e)
    {
      Kontroler.PrzesunDol();
    }

    private void PrzesunLewo(object sender, RoutedEventArgs e)
    {
      Kontroler.PrzesunLewo();
    }

    private void PrzesunPrawo(object sender, RoutedEventArgs e)
    {
      Kontroler.PrzesunPrawo();
    }

    private void ObtotLewo(object sender, RoutedEventArgs e)
    {
      Kontroler.ObrocLewo();
    }

    private void ObrotPrawo(object sender, RoutedEventArgs e)
    {
      Kontroler.ObrocPrawo();
    }

    private void Powieksz(object sender, RoutedEventArgs e)
    {
      Kontroler.Powieksz();
    }

    private void Pomniejsz(object sender, RoutedEventArgs e)
    {
      Kontroler.Pomniejsz();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      MoveChildren(kartka, boxWall);
      boxWall.Background = kartka.Background;
    }

    private void MoveChildren(Canvas from, Canvas to, bool withSize = false)
    {
      if (withSize)
      {
        to.Height = from.Height;
        to.Width = from.Width;
      }
      to.Children.Clear();
      List<UIElement> col = new List<UIElement>();
      foreach (UIElement ele in from.Children)
      {
        col.Add(ele);
      }
      from.Children.Clear();
      foreach (UIElement ele in col)
      {
        //                kartka.Children.Remove(ele);
        to.Children.Add(ele);
      }
    }
    private Canvas boxWall;
    public Canvas BoxWall
    {
      get { return boxWall; }

      set
      {
        boxWall = value;
        MoveChildren(boxWall, kartka, true);
        
        kartka.Background = boxWall.Background;

        kartka.RenderTransform = new ScaleTransform();
        {
          ((ScaleTransform)kartka.RenderTransform).ScaleX = 2;
          ((ScaleTransform)kartka.RenderTransform).ScaleY = 2;
        }

        kartka.SetValue(Canvas.LeftProperty, 10.0);
        kartka.SetValue(Canvas.TopProperty, 10.0);
      }
    }
  }
}
