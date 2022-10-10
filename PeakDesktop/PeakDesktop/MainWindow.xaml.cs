using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PeakDesktop
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void GetButtonGridPosition(object sender, out int row, out int column)
        {
            var button = (Button)sender;
            column = Grid.GetColumn(button);
            row = Grid.GetRow(button);
        }
        private UIElement GetGridElement(Grid g, int row, int column)
        {
            UIElement e = null;
            for (int i = 0; i < g.Children.Count; i++)
            {
                e = g.Children[i];
                if (Grid.GetRow(e) == row && Grid.GetColumn(e) == column)
                {
                    return e;
                }
            }
            return null;
        }
        private void FrameButton(Button button, bool bFrame)
        {
            if (bFrame)
            {
                button.BorderThickness = new Thickness(6);
                button.BorderBrush = Brushes.Yellow;
            }
            else
            {
                button.BorderThickness = new Thickness(1);
                button.BorderBrush = Brushes.DarkGray;
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
        }
        private void Button_Click(object sender, EventArgs e)
        {
        }
        private void Button_MouseEnter(object sender, EventArgs e)
        {
            int row, col;
            GetButtonGridPosition(sender, out row, out col);
            GameInformationLabel.Content = $"{sender.GetType().ToString()}: MouseEnter [{row}/{col}]";
        }

        private void Button_MouseMove(object sender, EventArgs e)
        {
            int row, col;
            GetButtonGridPosition(sender, out row, out col);
            GameInformationLabel.Content = $"{sender.GetType().ToString()}: MouseMove [{row}/{col}]";

            Button button = (Button)GetGridElement(FieldGrid, row - 1, col);
            if (button != null) FrameButton(button, true);
            button = (Button)GetGridElement(FieldGrid, row +1, col);
            if (button != null) FrameButton(button, true);
            button = (Button)GetGridElement(FieldGrid, row, col-1);
            if (button != null) FrameButton(button, true);
            button = (Button)GetGridElement(FieldGrid, row, col + 1);
            if (button != null) FrameButton(button, true);
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            int row, col;
            GetButtonGridPosition(sender, out row, out col);
            GameInformationLabel.Content = $"{sender.GetType().ToString()}: MouseLeave [{row}/{col}]";

            Button button = (Button)GetGridElement(FieldGrid, row - 1, col);
            if (button != null) FrameButton(button, false);
            button = (Button)GetGridElement(FieldGrid, row + 1, col);
            if (button != null) FrameButton(button, false);
            button = (Button)GetGridElement(FieldGrid, row, col - 1);
            if (button != null) FrameButton(button, false);
            button = (Button)GetGridElement(FieldGrid, row, col + 1);
            if (button != null) FrameButton(button, false);
        }
    }
}
