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

        private GameBoard PeakBoard { get; set; }
        private List<IMove> NextMoves { get; set; }
        private List<Button> FrameButtons { get; set; } 
        private Position From { get; set; }
        private Position To { get; set; }
        private Button FromButton { get; set; }
        private Button ToButton { get; set; }

        private void Init()
        {
            PeakBoard = new GameBoard();

            DrawBoard();
            int moveCount;
            NextMoves = PeakBoard.MakeMoveList(out moveCount);
            FrameButtons = new List<Button>();
            From = null;
            To = null;
            FromButton = null;
            ToButton = null;
        }
        private void DrawBoard()
        {
            Style styleRedButton = this.FindResource("RedButton") as Style;
            Style styleBlueButton = this.FindResource("BlueButton") as Style;
            Style styleBlankButton = this.FindResource("BlankButton") as Style;

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    Button button = (Button)GetGridElement(FieldGrid, row, col);
                    if (button != null)
                    {
                        int chip = PeakBoard.GetAt(row, col);
                        string val = "";
                        if (Math.Abs(chip) >= 2)
                        {
                            val = $"{Math.Abs(chip)}";
                        }
                        button.Content = val;

                        if (chip > 0)
                        {
                            button.Style = styleRedButton;
                        }
                        else if (chip < 0)
                        {
                            button.Style = styleBlueButton;
                        }
                        else
                        {
                            button.Style = styleBlankButton;
                        }
                    }
                }
            }
            Player winner, theOther;
            
            int score, whitescore, blackscore, totalscore;
            if (PeakBoard.GameOver(out winner, out score, out whitescore, out blackscore, out totalscore))
            {
                if (winner == Player.None)
                {
                    GameInformationLabel.Content = $"Tie game with the score of {score}!";
                }
                else
                {
                    theOther = PeakBoard.OtherPlayer(winner);
                    int theOtherScore = (score == Math.Abs(whitescore)) ? Math.Abs(blackscore) : Math.Abs(whitescore);
                    GameInformationLabel.Content = $"{GetPlayerColor(winner)} wins with the score of {score}\n{GetPlayerColor(theOther)} has a score of {theOtherScore}";
                }
            }
            else
            {
                GameInformationLabel.Content = $"{GetPlayerColor(Player.White)}: {whitescore} / {GetPlayerColor(Player.Black)}: {blackscore}";
            }
        }
        private string GetPlayerColor(Player player)
        {
            if (player == Player.White) return "RED";
            if (player == Player.Black) return "BLUE";
            return "WHITE";
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
            Button buttonClicked = (Button)sender;
            int row, col;
            GetButtonGridPosition(sender, out row, out col);
            if (buttonClicked != null && From == null)
            {
                foreach (IMove _move in NextMoves)
                {
                    Move move = (Move)_move;
                    if (move.From.X == col && move.From.Y == row)
                    {
                        From = new Position(row, col, PeakBoard);
                        FromButton = buttonClicked;
                        To = null;
                        FrameButtons.Add(buttonClicked);
                        FrameButton(buttonClicked, true);
                        break;
                    }
                }
            }
            else if (buttonClicked != null && From != null && To == null)
            {
                foreach (IMove _move in NextMoves)
                {
                    Move move = (Move)_move;
                    if (move.From.X == From.X && move.From.Y == From.Y && move.To.X == col && move.To.Y == row)
                    {
                        To = new Position(row, col, PeakBoard);
                        ToButton = buttonClicked;
                        Player currentPlayer = PeakBoard.NextPlayer;
                        PeakBoard.Pass(currentPlayer, false);
                        PeakBoard.DoMove(move);

                        foreach (Button button in FrameButtons)
                        {
                            FrameButton(button, false);
                        }
                        FrameButtons.Clear();

                        DrawBoard();
                        int moveCount;
                        NextMoves = PeakBoard.MakeMoveList(out moveCount);
                        FrameButtons = new List<Button>();
                        From = null;
                        To = null;
                        FromButton = null;
                        ToButton = null;

                        break;
                    }
                }
            }
        }
        private void Button_MouseEnter(object sender, EventArgs e)
        {
            int row, col;
            GetButtonGridPosition(sender, out row, out col);
            //GameInformationLabel.Content = $"{sender.GetType().ToString()}: MouseEnter [{row}/{col}]";
            
            if (From == null)
            {
                FrameButtons = new List<Button>();

                foreach (IMove _move in NextMoves)
                {
                    Move move = (Move)_move;
                    if (move.From.X == col && move.From.Y == row)
                    {
                        Button button = (Button)GetGridElement(FieldGrid, move.To.Y, move.To.X);
                        if (button != null)
                        {
                            FrameButtons.Add(button);
                            FrameButton(button, true);
                        }
                    }
                }
            }
         }

        private void Button_MouseMove(object sender, EventArgs e)
        {
            int row, col;
            GetButtonGridPosition(sender, out row, out col);
            //GameInformationLabel.Content = $"{sender.GetType().ToString()}: MouseMove [{row}/{col}]";
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            int row, col;
            GetButtonGridPosition(sender, out row, out col);
            //GameInformationLabel.Content = $"{sender.GetType().ToString()}: MouseLeave [{row}/{col}]";

            if (From == null)
            {
                foreach (Button button in FrameButtons)
                {
                    FrameButton(button, false);
                }
            }
        }
    }
}
