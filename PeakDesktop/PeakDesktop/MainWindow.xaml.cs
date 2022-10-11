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
            Init();
        }

        private void Init()
        {
            Style styleRedButton = this.FindResource("RedButton") as Style;
            Style styleBlueButton = this.FindResource("BlueButton") as Style;

            List<Style> styles = new List<Style>{ styleRedButton, styleBlueButton };

            int counter = 0;
            for (int row = 0; row < 6; row++)
            {
                if (row%2 == 0)
                {
                    counter = 0;
                }
                else
                {
                    counter = 1;
                }
                for (int col = 0; col < 6; col++)
                {
                    Button button = (Button)GetGridElement(FieldGrid, row, col);
                    if (button != null)
                    {
                        button.Style = styles[counter % 2];
                        counter++;
                        button.Content = $"[{row},{col}]";
                    }
                }
            }
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
                /*
                button.BorderThickness = new Thickness(6);
                button.BorderBrush = Brushes.Yellow;
                */
                MoveHelper.SetIsFrame(button, true);
            }
            else
            {
                /*
                button.BorderThickness = new Thickness(1);
                button.BorderBrush = Brushes.DarkGray;
                */
                MoveHelper.SetIsFrame(button, false);
            }
        }

        private void CandidateButton(Button button, bool bCandidate)
        {
            if (bCandidate)
            {
                button.BorderThickness = new Thickness(6);
                button.BorderBrush = Brushes.LimeGreen;
            }
            else
            {
                button.BorderThickness = new Thickness(1);
                button.BorderBrush = Brushes.DarkGray;
            }
        }
        private void PossibleButton(Button button, bool bPossible)
        {
            if (bPossible)
            {
                button.BorderThickness = new Thickness(6);
                button.BorderBrush = Brushes.LightGreen;
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

            Button button = (Button)GetGridElement(FieldGrid, row , col);
            if (button != null) CandidateButton(button, true);
            
            button = (Button)GetGridElement(FieldGrid, row - 1, col);
            if (button != null) FrameButton(button, true);
            button = (Button)GetGridElement(FieldGrid, row + 2, col);
            if (button != null) FrameButton(button, true);
            button = (Button)GetGridElement(FieldGrid, row, col - 2);
            if (button != null) FrameButton(button, true);
            button = (Button)GetGridElement(FieldGrid, row, col + 1);
            if (button != null) FrameButton(button, true);
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            int row, col;
            GetButtonGridPosition(sender, out row, out col);
            GameInformationLabel.Content = $"{sender.GetType().ToString()}: MouseLeave [{row}/{col}]";

            Button button = (Button)GetGridElement(FieldGrid, row, col);
            if (button != null) CandidateButton(button, false);

            button = (Button)GetGridElement(FieldGrid, row - 1, col);
            if (button != null) FrameButton(button, false);
            button = (Button)GetGridElement(FieldGrid, row + 2, col);
            if (button != null) FrameButton(button, false);
            button = (Button)GetGridElement(FieldGrid, row, col - 2);
            if (button != null) FrameButton(button, false);
            button = (Button)GetGridElement(FieldGrid, row, col + 1);
            if (button != null) FrameButton(button, false);
        }
    }
}
