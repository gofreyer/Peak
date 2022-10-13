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
        private string RedPlayer { get; set; }
        private string BluePlayer { get; set; }
        private bool GameIsRunning { get; set; }
        private int NextMovesCount { get; set; }

        private void Init()
        {
            PeakBoard = new GameBoard();

            DrawBoard();
            
            RedPlayer = "Human";
            ComboBoxItem cbi = (ComboBoxRedPlayer.SelectedItem as ComboBoxItem);
            if (cbi != null && cbi.Content != null)
            {
                RedPlayer = cbi.Content.ToString();
            }
            BluePlayer = "Human";
            cbi = (ComboBoxBluePlayer.SelectedItem as ComboBoxItem);
            if (cbi != null && cbi.Content != null)
            {
                BluePlayer = cbi.Content.ToString();
            }
            GameIsRunning = false;
            GoButton.IsEnabled = true;
            PassButton.IsEnabled = false;

            GameInformationLabel.Content = $"RED is {RedPlayer} and BLUE is {BluePlayer}";
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
                        FrameButton(button, false);
                    }
                }
            }
            Player winner, theOther;
            
            int score, whitescore, blackscore, totalscore;
            bool isGameOver = PeakBoard.GameOver(out winner, out score, out whitescore, out blackscore, out totalscore);
            if (isGameOver)
            {
                if (winner == Player.None)
                {
                    GameInformationLabel.Content = "Tie game!";
                }
                else
                {
                    theOther = PeakBoard.OtherPlayer(winner);
                    int theOtherScore = (score == Math.Abs(whitescore)) ? Math.Abs(blackscore) : Math.Abs(whitescore);
                    GameInformationLabel.Content = $"{GetPlayerColor(winner)} wins!";
                }
                GameIsRunning = false;
                GoButton.IsEnabled = false;
                PassButton.IsEnabled = false;
                StartButton.IsEnabled = true;
            }
            else
            {
                //GameInformationLabel.Content = $"{GetPlayerColor(Player.White)}: {whitescore} / {GetPlayerColor(Player.Black)}: {blackscore}";
                GameInformationLabel.Content = "";
            }
            
            ButtonScoreRed.Content = (whitescore > 0) ? $"{whitescore}":"";
            ButtonScoreBlue.Content = (blackscore > 0) ? $"{blackscore}" : "";

            if (whitescore > blackscore)
            {
                ButtonScoreRed.Height = 160;
                ButtonScoreRed.Width = 160;
                ButtonScoreBlue.Height = 80;
                ButtonScoreBlue.Width = 80;
            }
            else if (blackscore > whitescore)
            {
                ButtonScoreRed.Height = 80;
                ButtonScoreRed.Width = 80;
                ButtonScoreBlue.Height = 160;
                ButtonScoreBlue.Width = 160;
            }
            else
            {
                ButtonScoreRed.Height = 120;
                ButtonScoreRed.Width = 120;
                ButtonScoreBlue.Height = 120;
                ButtonScoreBlue.Width = 120;
            }

            if (PeakBoard.Passing[(int)Player.White] > 0)
            {
                ButtonPassRed.Visibility = Visibility.Visible;
            }
            else
            {
                ButtonPassRed.Visibility = Visibility.Hidden;
            }
            if (PeakBoard.Passing[(int)Player.Black] > 0)
            {
                ButtonPassBlue.Visibility = Visibility.Visible;
            }
            else
            {
                ButtonPassBlue.Visibility = Visibility.Hidden;
            }

            if (!isGameOver)
            {
                if (PeakBoard.NextPlayer == Player.White)
                {
                    MoveHelper.SetIsTurn(ButtonScoreRed, true);
                    MoveHelper.SetIsTurn(ButtonScoreBlue, false);
                }
                else
                {
                    MoveHelper.SetIsTurn(ButtonScoreRed, false);
                    MoveHelper.SetIsTurn(ButtonScoreBlue, true);
                }
            }
            else
            {
                MoveHelper.SetIsTurn(ButtonScoreRed, false);
                MoveHelper.SetIsTurn(ButtonScoreBlue, false);
            }

            int movescount = 0;
            NextMoves = PeakBoard.MakeMoveList(out movescount);
            NextMovesCount = movescount;
            FrameButtons = new List<Button>();
            From = null;
            To = null;
            FromButton = null;
            ToButton = null;

            DoEvents();

            NextTurn();
        }
        private bool MouseIsAllowed()
        {
            if (!GameIsRunning) return false;
            if (PeakBoard.NextPlayer == Player.White && RedPlayer != "Human Player") return false;
            if (PeakBoard.NextPlayer == Player.Black && BluePlayer != "Human Player") return false;
            return true;
        }
        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,new Action(delegate { }));
        }
        private void WaitNMilliSeconds(int milli_seconds)
        {
            if (milli_seconds < 1) return;
            DateTime _desired = DateTime.Now.AddMilliseconds(milli_seconds);
            while (DateTime.Now < _desired)
            {
                DoEvents();
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

        private void Button_Click(object sender, EventArgs e)
        {
            if (!MouseIsAllowed()) return;

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

                        ShowAnimateMove(move,500);
                        
                        PeakBoard.Pass(currentPlayer, false);
                        PeakBoard.DoMove(move);

                        foreach (Button button in FrameButtons)
                        {
                            FrameButton(button, false);
                        }
                        FrameButtons.Clear();

                        DrawBoard();

                        break;
                    }
                }
            }
        }
        void ShowAnimateMove(IMove bestMove, int totaltime=1000)
        {
            foreach (Button frButton in FrameButtons)
            {
                FrameButton(frButton, false);
            }
            FrameButtons.Clear();

            Move move = (Move)bestMove;

            Style styleBlankButton = this.FindResource("BlankButton") as Style;
            
            List<Position> movePositions = move.GetMovePositions();

            if (movePositions == null || movePositions.Count == 0)
            {
                return;
            }

            int count = 2*movePositions.Count - 1;
            int intervall = (int)((float)totaltime / (float)count);
            Button button = null;
            Position pos = null;
            for (int i = 0; i < movePositions.Count; i++)
            {
                pos = movePositions[i];
                button = (Button)GetGridElement(FieldGrid, pos.Y, pos.X);
                if (button != null)
                {
                    FrameButton(button,true);
                    WaitNMilliSeconds(intervall);
                }
            }
            for (int i = 0; i < movePositions.Count-1; i++)
            {
                pos = movePositions[i];
                button = (Button)GetGridElement(FieldGrid, pos.Y, pos.X);
                if (button != null)
                {
                    if (i==0) button.Style = styleBlankButton;
                    FrameButton(button, false);
                    WaitNMilliSeconds(intervall);
                }
            }
            pos = movePositions[movePositions.Count - 1];
            button = (Button)GetGridElement(FieldGrid, pos.Y, pos.X);
            if (button != null)
            {
                FrameButton(button, false);
            }
        }
        void ShowMove(IMove bestMove, int show)
        {
            Move move = (Move)bestMove;

            Button buttonFrom = (Button)GetGridElement(FieldGrid, move.From.Y, move.From.X);
            Button buttonTo = (Button)GetGridElement(FieldGrid, move.To.Y, move.To.X);

            if (buttonFrom != null)
            {
                FrameButton(buttonFrom, (show <= 2) ? true : false);
            }
            if (buttonTo != null)
            {
                FrameButton(buttonTo, (show == 1 || show == 3) ? true : false);
            }
        }
        private void Button_MouseEnter(object sender, EventArgs e)
        {
            if (!MouseIsAllowed()) return;
            
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
            if (!MouseIsAllowed()) return;

            int row, col;
            GetButtonGridPosition(sender, out row, out col);
            //GameInformationLabel.Content = $"{sender.GetType().ToString()}: MouseMove [{row}/{col}]";
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            if (!MouseIsAllowed()) return;

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
        private void NextTurn()
        {
            if (GameIsRunning == false) return;

            Player currentPlayer = PeakBoard.NextPlayer;

            GameInformationLabel.Content = $"RED is {RedPlayer} and BLUE is {BluePlayer}";

            if (currentPlayer == Player.White && RedPlayer == "Human Player") return;
            if (currentPlayer == Player.Black && BluePlayer == "Human Player") return;

            IMove bestMove;
            bestMove = null;
            if (currentPlayer == Player.White)
            {
                if (RedPlayer == "Random")
                {
                    Algorithmn.RandomMove(PeakBoard, out bestMove);
                }
                else
                {
                    int maxDepth = 1;
                    if (RedPlayer == "MiniMax 1") maxDepth = 1;
                    else if (RedPlayer == "MiniMax 2") maxDepth = 2;
                    else if (RedPlayer == "MiniMax 3") maxDepth = 3;
                    else if (RedPlayer == "MiniMax 4") maxDepth = 4;
                    else if (RedPlayer == "MiniMax 5") maxDepth = 5;
                    int depth = 1;
                    if (NextMovesCount > 0)
                    {
                        depth = (int)Math.Round((1.0 / (float)NextMovesCount) * 80.0);
                        if (depth < 1) depth = 1;
                        if (depth > maxDepth) depth = maxDepth;
                    }
                    Algorithmn.MinMaxMove(PeakBoard, depth, out bestMove);
                }
            }
            else if(currentPlayer == Player.Black)
            {
                if (BluePlayer == "Random")
                {
                    Algorithmn.RandomMove(PeakBoard, out bestMove);
                }
                else
                {
                    int maxDepth = 1;
                    if (BluePlayer == "MiniMax 1") maxDepth = 1;
                    else if (BluePlayer == "MiniMax 2") maxDepth = 2;
                    else if (BluePlayer == "MiniMax 3") maxDepth = 3;
                    else if (BluePlayer == "MiniMax 4") maxDepth = 4;
                    else if (BluePlayer == "MiniMax 5") maxDepth = 5;
                    int depth = 1;
                    if (NextMovesCount > 0)
                    {
                        depth = (int)Math.Round((1.0 / (float)NextMovesCount) * 80.0);
                        if (depth < 1) depth = 1;
                        if (depth > maxDepth) depth = maxDepth;
                    }
                    Algorithmn.MinMaxMove(PeakBoard, depth, out bestMove);
                }
            }
            
            if (bestMove != null)
            {
                /*
                ShowMove(bestMove,1);
                WaitNMilliSeconds(400);
                ShowMove(bestMove,2);
                WaitNMilliSeconds(500);
                ShowMove(bestMove,3);
                WaitNMilliSeconds(600);
                ShowMove(bestMove, 4);
                */
                ShowAnimateMove(bestMove);

                PeakBoard.Pass(currentPlayer, false);
                PeakBoard.DoMove(bestMove);
                DrawBoard();
            }
            else
            {
                PeakBoard.Pass(currentPlayer, true);
                PeakBoard.ChangePlayer();
                DrawBoard();
                WaitNMilliSeconds(500);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            GameIsRunning = false;
            GoButton.IsEnabled = true;
            PassButton.IsEnabled = false;
            Init();
        }
        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            GoButton.IsEnabled = false;
            PassButton.IsEnabled = true;
            GameIsRunning = true;

            NextTurn();
        }
        private void PassButton_Click(object sender, RoutedEventArgs e)
        {
            Player currentPlayer = PeakBoard.NextPlayer;
            PeakBoard.Pass(currentPlayer, true);
            PeakBoard.ChangePlayer();

            DrawBoard();
            WaitNMilliSeconds(500);

            NextTurn();
        }
        void RedPlayer_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            RedPlayer = "";
            ComboBoxItem cbi = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            if (cbi != null && cbi.Content != null)
            {
                RedPlayer = cbi.Content.ToString();
            }
            GameInformationLabel.Content = $"RED is {RedPlayer} and BLUE is {BluePlayer}";
        }
        private void BluePlayer_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            BluePlayer = "";
            ComboBoxItem cbi = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            if (cbi != null && cbi.Content != null)
            {
                BluePlayer = cbi.Content.ToString();
            }
            GameInformationLabel.Content = $"RED is {RedPlayer} and BLUE is {BluePlayer}";
        }
    }
}
