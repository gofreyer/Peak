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

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GameInformationLabel.Content = "X ist am Zug";
        }

        private List<List<Player>> fields = new List<List<Player>>()
        {
            new List<Player>() { Player.None, Player.None, Player.None},
            new List<Player>() { Player.None, Player.None, Player.None},
            new List<Player>() { Player.None, Player.None, Player.None},
        };
        private Player currentPlayer = Player.Player1;
        private bool gameActive = true;
        private int FIELD_SIZE = 3;
        private int fieldsMarked = 0;

        private void Button_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            int column = Grid.GetColumn(button);
            int row = Grid.GetRow(button);
            if (gameActive && fields[column][row] == Player.None)
            {
                var currentSymbol = "";
                if (currentPlayer == Player.Player1)
                {
                    currentSymbol = "X";
                }
                else
                {
                    currentSymbol = "O";
                }
                button.Content = currentSymbol;
                fields[column][row] = currentPlayer;
                fieldsMarked++;

                var playerWon = CheckForWinner();
                if (playerWon)
                {
                    GameInformationLabel.Content = $"{currentSymbol} hat gewonnen!";
                    gameActive = false;
                    return;
                }
                if (fieldsMarked >= 9)
                {
                    GameInformationLabel.Content = "Unentschieden!";
                    gameActive = false;
                }
                else if (currentPlayer == Player.Player1)
                {
                    currentPlayer = Player.Player2;
                    GameInformationLabel.Content = "O ist am Zug";
                }
                else
                {
                    currentPlayer = Player.Player1;
                    GameInformationLabel.Content = "X ist am Zug";
                }
            }

        }
        private bool CheckForWinner()
        {
            // Check rows
            foreach (List<Player> row in fields)
            {
                if (row[0] != Player.None && row[0] == row[1] && row[1] == row[2])
                {
                    return true;
                }
            }
            // Check columns
            for (var column_index = 0; column_index < FIELD_SIZE; column_index++)
            {
                if (fields[0][column_index] != Player.None
                    && fields[0][column_index] == fields[1][column_index]
                    && fields[1][column_index] == fields[2][column_index])
                {
                    return true;
                }
            }
            // Check left-right diagonal
            if (fields[0][0] != Player.None && fields[0][0] == fields[1][1]
                && fields[1][1] == fields[2][2])
            {
                return true;
            }
            // Check right-left diagonal
            if (fields[0][2] != Player.None && fields[1][1] == fields[2][0]
                && fields[1][1] == fields[0][2])
            {
                return true;
            }
            return false;
        }
        private void StartButton_Click(object sender, EventArgs e)
        {
            gameActive = true;
            for (var x = 0; x < 3; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    fields[x][y] = Player.None;
                }
            }
            Button0_0.Content = "";
            Button0_1.Content = "";
            Button0_2.Content = "";

            Button1_0.Content = "";
            Button1_1.Content = "";
            Button1_2.Content = "";

            Button2_0.Content = "";
            Button2_1.Content = "";
            Button2_2.Content = "";

            fieldsMarked = 0;
        }
    }
}
