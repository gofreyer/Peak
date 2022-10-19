// Helper functions

// Random
function getRandomInt(max) {
  return Math.floor(Math.random() * max);
}

// Sleep
function Sleep(milliseconds) {
  return new Promise((resolve) => setTimeout(resolve, milliseconds));
}
async function test() {
  /*
	call always with async ... await
	*/
  console.log("Before sleep function");
  await Sleep(3000); // Pauses function for 3 seconds
  console.log("After sleep function");
}

const Player = {
  White: 0,
  Black: 1,
  None: 2,
};

const Direction = {
  NONE: 0,
  INVALID: 1,
  N: 2,
  E: 3,
  W: 4,
  S: 5,
  NE: 6,
  NW: 7,
  SE: 8,
  SW: 9,
};

const TOTAL = 2;
const DIM = 6;

// Algorithmn
class Algorithmn {
  static RandomMove(board) {
    let move = null;
    let n = 0;
    board.MakeMoveList(nextMoves);
    n = nextMoves.length;
    if (n > 0) {
      let m = 0;
      if (n > 1) {
        m = getRandomInt(n); // creates a number between 0 and n
      }
      move = nextMoves[m];
    }
    return move;
  }

  static MinMaxMove(board, depth) {
    let bestmove = null;
    let value = 0;
    let MinValue = -999999;
    let MaxValue = +999999;

    let bestvalue = board.NextPlayer == Player.White ? MinValue : MaxValue;
    let otherPlayer =
      board.NextPlayer == Player.White ? Player.Black : Player.White;

    if (board.CheckWin(otherPlayer)) {
      var obj = {
        VALUE: bestvalue,
        MOVE: bestmove,
      };
      return obj;
    }
    if (depth == 0) {
      bestvalue = board.Evaluation();
      var obj = {
        VALUE: bestvalue,
        MOVE: bestmove,
      };
      return obj;
    }

    let n = 0;
    board.MakeMoveList(nextMoves);
    n = nextMoves.length;
    let bestMoves = [];
    if (n == 0) {
      bestvalue = board.Evaluation();
      var obj = {
        VALUE: bestvalue,
        MOVE: bestmove,
      };
      return obj;
    }
    for (let i = 0; i < n; i++) {
      let move = nextMoves[i];
      board.DoMove(move);
      let { value, nextbestmove } = MinMaxMove(board, depth - 1);
      board.UndoMove(move);

      if (board.NextPlayer == Player.White) {
        if (value > bestValue) {
          bestvalue = value;
          bestmove = move;
          bestMoves = [];
          bestMoves.push(move);
        } else if (value == bestvalue) {
          bestMoves.push(move);
        }
      } else {
        if (value < bestValue) {
          bestvalue = value;
          bestmove = move;
          bestMoves = [];
          bestMoves.push(move);
        } else if (value == bestvalue) {
          bestMoves.push(move);
        }
      }
    }
    let bm = bestMoves.length;
    if (bm > 0) {
      let m = 0;
      if (bm > 1) {
        m = getRandomInt(bm); // creates a number between 0 and bm
      }
      bestmove = bestMoves[m];
    } else {
      bestmove = null;
      bestValue = board.Evaluation();
    }
    var obj = {
      VALUE: bestvalue,
      MOVE: bestmove,
    };
    return obj;
  }
}

// Gameboard position
class Position {
  _myboard = null;
  _x = -1;
  _y = -1;

  constructor(y, x, g) {
    this._myboard = g;
    this._x = x;
    this._y = y;
  }
  static newPosition(pos) {
    var newPos = new Position(pos.Y, pos.X, pos.MyBoard);
    return newPos;
  }
  get MyBoard() {
    return this._myboard;
  }
  set MyBoard(value) {
    this._myboard = value;
  }
  get X() {
    return this._x;
  }
  set X(value) {
    if (value < this.MyBoard.GetDim()) {
      this._x = value;
    } else {
      this._x = -1;
    }
  }
  get Y() {
    return this._y;
  }
  set Y(value) {
    if (value < this.MyBoard.GetDim()) {
      this._y = value;
    } else {
      this._y = -1;
    }
  }
  IsValid() {
    return (
      this.X >= 0 &&
      this.X < this.MyBoard.GetDim() &&
      this.Y >= 0 &&
      this.Y < this.MyBoard.GetDim()
    );
  }
  IsEqual(pos) {
    return pos.X == this.X && pos.Y == this.Y;
  }
}

// Gameboard move
class Move {
  _myboard = null;
  _from = null;
  _to = null;
  _prevalfrom = -1;
  _postvalfrom = -1;
  _prevalto = -1;
  _postvalto = -1;
  _applied = false;

  constructor(from, to, g) {
    this._myboard = g;
    this._from = from;
    this._to = to;
    this._prevalfrom = 0;
    this._postvalfrom = 0;
    this._prevalto = 0;
    this._postvalto = 0;
    this._applied = false;
  }
  static newMove(move) {
    var newMove = new Move(move.From, move.To, move.MyBoard);
    newMove.PreValFrom = move.PreValFrom();
    newMove.PreValTo = move.PreValTo();
    newMove.PostValFrom = move.PostValFrom();
    newMove.PostValTo = move.PostValTo();
    newMove.Apllied = move.Applied();
    return newMove;
  }

  get MyBoard() {
    return this._myboard;
  }
  set MyBoard(value) {
    this._myboard = value;
  }
  get From() {
    return this._from;
  }
  set From(value) {
    this._from = value;
  }
  get To() {
    return this._to;
  }
  set To(value) {
    this._to = value;
  }
  get PreValFrom() {
    return this._prevalfrom;
  }
  set PreValFrom(value) {
    this._prevalfrom = value;
  }
  get PostValFrom() {
    return this._postvalfrom;
  }
  set PostValFrom(value) {
    this._postvalfrom = value;
  }
  get PreValTo() {
    return this._prevalto;
  }
  set PreValTo(value) {
    this._prevalto = value;
  }
  get PostValTo() {
    return this._postvalto;
  }
  set PostValTo(value) {
    this._postvalto = value;
  }
  get Applied() {
    return this._applied;
  }
  set Applied(value) {
    this._applied = value;
  }

  IsValid() {
    var dir = this.GetDirection();
    if (dir == Direction.INVALID || dir == Direction.NONE) {
      return false;
    }
    let intermediateDistance = this.MyBoard.GetIntermediateDistance(
      this.From,
      this.To,
      dir
    );
    return intermediateDistance == 2;
  }

  GetDirection() {
    if (
      !this.From.IsValid() ||
      !this.To.IsValid() ||
      (this.From.X != this.To.X &&
        this.From.Y != this.To.Y &&
        Math.abs(this.From.X - this.To.X) != Math.abs(this.From.Y - this.To.Y))
    )
      return Direction.INVALID;
    else if (this.From.X === this.To.X && this.From.Y === this.To.Y)
      return Direction.NONE;
    else if (this.From.Y === this.To.Y && this.From.X < this.To.X)
      return Direction.E;
    else if (this.From.Y === this.To.Y && this.From.X > this.To.X)
      return Direction.W;
    else if (this.From.X === this.To.X && this.From.Y < this.To.Y)
      return Direction.S;
    else if (this.From.X === this.To.X && this.From.Y > this.To.Y)
      return Direction.N;
    else if (this.From.X < this.To.X && this.From.Y > this.To.Y)
      return Direction.NE;
    else if (this.From.X > this.To.X && this.From.Y > this.To.Y)
      return Direction.NW;
    else if (this.From.X < this.To.X && this.From.Y < this.To.Y)
      return Direction.SE;
    else if (this.From.X > this.To.X && this.From.Y < this.To.Y)
      return Direction.SW;
    return Direction.INVALID;
  }

  GetMovePositions() {
    let movePositions = null;
    let dir = GetDirection();
    if (dir == Direction.INVALID || dir == Direction.NONE) {
      return movePositions;
    }
    movePositions = this.MyBoard.GetMovePositions(this.From, this.To, dir);

    return movePositions;
  }

  IsEqual(move) {
    return move.From.IsEqual(this.From) && move.To.IsEqual(this.To);
  }
  PreMove() {
    this.PreValueFrom = this.MyBoard.GetAt(this.From.Y, this.From.X);
    this.PreValueTo = this.MyBoard.GetAt(this.To.Y, this.To.X);
  }
  PostMove() {
    this.PostValueFrom = this.MyBoard.GetAt(this.From.Y, this.From.X);
    this.PostValueTo = this.MyBoard.GetAt(this.To.Y, this.To.X);
  }
  Undo() {
    this.MyBoard.SetAt(this.From.Y, this.From.X, this.PreValueFrom);
    this.MyBoard.SetAt(this.To.Y, this.To.X, this.PreValueTo);
  }
}

// Gameboard
class GameBoard {
  _board = [];
  _scoring = [0, 0, 0];
  _passing = [0, 0];
  _lastmoves = [];
  _nextplayer = Player.White;

  GetDim() {
    return DIM;
  }

  constructor(nextPlayer) {
    if (nextPlayer == null) {
      this._nextplayer = Player.White;
    } else {
      this._nextplayer = nextPlayer;
    }
    this.InitBoard();
    this.Evaluation();
  }

  static newGameBoard(g) {
    let newGameBoard = new GameBoard(Player.White);

    for (let r = 0; r < DIM; r++) {
      for (let c = 0; c < DIM; c++) {
        newGameBoard.SetAt(r, c, g.GetAt(r, c));
      }
    }
    newGameBoard.LastMoves = [];
    newGameBoard.Scoring = [0, 0, 0];
    newGameBoard.Passing = [g.Passing[0], g.Passing[1]];
    this.Evaluation();
    newGameBoard.NextPlayer = g.NextPlayer();
  }

  get Scoring() {
    return this._scoring;
  }
  set Scoring(value) {
    this._scoring = value;
  }
  get Passing() {
    return this._passing;
  }
  set Passing(value) {
    this._passing = value;
  }
  get LastMoves() {
    return this._lastmoves;
  }
  set LastMoves(value) {
    this._lastmoves = value;
  }

  get NextPlayer() {
    return this._nextplayer;
  }
  set NextPlayer(value) {
    this._nextplayer = value;
  }

  GetAt(row, col) {
    if (row >= 0 && row < DIM && col >= 0 && col < DIM) {
      return this._board[row * DIM + col];
    } else {
      return 0;
    }
  }
  SetAt(row, col, value) {
    if (row >= 0 && row < DIM && col >= 0 && col < DIM) {
      this._board[row * DIM + col] = value;
    }
  }

  Pass(player, setPass) {
    if (setPass == true) {
      this.Passing[player] = this.Passing[player] + 1;
    } else {
      this.Passing[player] = 0;
    }
  }
  CheckWin(player) {
    let otherPlayer = OtherPlayer(player);
    if (
      this.Passing[Player.White] > 1 ||
      this.Passing[Player.Black] > 1 ||
      (this.Passing[Player.White] == 1 && this.Passing[Player.Black] == 1)
    ) {
      let score = Evaluation();
      if (player == Player.White && score > 0) return true;
      if (player == Player.Black && score < 0) return true;
    }
    return false;
  }
  GameOver() {
    let winner = Player.None;
    let score = 0;
    let whitescore = 0;
    let blackscore = 0;
    let totalscore = 0;
    Evaluation();
    whitescore = this.Scoring()[Player.White];
    blackscore = this.Scoring()[Player.Black];
    totalscore = this.Scoring()[Player.None];
    if (
      this.Passing[Player.White] > 1 ||
      this.Passing[Player.Black] > 1 ||
      (this.Passing[Player.White] == 1 && this.Passing[Player.Black] == 1)
    ) {
      if (
        Math.abs(this.Scoring[Player.White]) >
        Math.abs(this.Scoring[Player.Black])
      ) {
        winner = Player.White;
        score = Math.abs(this.Scoring[Player.White]);
        var obj = {
          WINNER: winner,
          SCORE: score,
          WHITESCORE: whitescore,
          BLACKSCORE: blackscore,
          TOTALSCORE: totalscore,
          GAMEOVER: true,
        };

        return obj;
      }
      if (
        Math.abs(this.Scoring[Player.White]) <
        Math.abs(this.Scoring[Player.Black])
      ) {
        winner = Player.Black;
        score = Math.abs(this.Scoring[Player.Black]);
        var obj = {
          WINNER: winner,
          SCORE: score,
          WHITESCORE: whitescore,
          BLACKSCORE: blackscore,
          TOTALSCORE: totalscore,
          GAMEOVER: true,
        };

        return obj;
      } else {
        winner = Player.None;
        score = Math.abs(this.Scoring[Player.White]);
        var obj = {
          WINNER: winner,
          SCORE: score,
          WHITESCORE: whitescore,
          BLACKSCORE: blackscore,
          TOTALSCORE: totalscore,
          GAMEOVER: true,
        };

        return obj;
      }
    }
    var obj = {
      WINNER: winner,
      SCORE: score,
      WHITESCORE: whitescore,
      BLACKSCORE: blackscore,
      TOTALSCORE: totalscore,
      GAMEOVER: false,
    };

    return obj;
  }

  GetInitValue(player) {
    return player === Player.White ? 1 : -1;
  }
  ChangePlayer() {
    if (this.NextPlayer === Player.White) {
      this.NextPlayer = Player.Black;
    } else {
      this.NextPlayer = Player.White;
    }
  }
  OtherPlayer(player) {
    return player == Player.White ? Player.Black : Player.White;
  }
  MakeMoveList() {
    let moves = this.GetPossibleMovesPlayer(this.NextPlayer);
    return moves;
  }
  GetPossibleMovesPlayer(player) {
    let possibleMoves = [];

    let startChip = this.GetInitValue(player);

    for (let r = 0; r < DIM; r++) {
      for (let c = 0; c < DIM; c++) {
        if (this.GetAt(r, c) === startChip) {
          let position = new Position(r, c, this);
          let movesFromPosition = this.GetPossibleMovesPosition(
            player,
            position
          );
          let newMoves = possibleMoves.concat(movesFromPosition);
          possibleMoves = newMoves;
        }
      }
    }
    return possibleMoves;
  }
  GetPossibleMovesPosition(player, pos) {
    let possibleMoves = [];

    for (let r = 0; r < DIM; r++) {
      for (let c = 0; c < DIM; c++) {
        let targetposition = new Position(r, c, this);
        let move = new Move(pos, targetposition, this);
        if (move.IsValid()) {
          possibleMoves.push(move);
        }
      }
    }
    return possibleMoves;
  }

  InitBoard() {
    this.NextPlayer = Player.White;
    let chip = this.GetInitValue(this.NextPlayer);
    for (let r = 0; r < DIM; r++) {
      for (let c = 0; c < DIM; c++) {
        if (r % 2 === 0 && c === 0) {
          this.NextPlayer = Player.White;
        } else if (r % 2 === 1 && c === 0) {
          this.NextPlayer = Player.Black;
        }
        this.ChangePlayer();
        chip = this.GetInitValue(this.NextPlayer);
        this.SetAt(r, c, chip);
        //console.log(`Cell:${r}${c} Player:${this.NextPlayer} Chip:${chip} Board:${this.GetAt(r, c)}\n`);
        c++;
        this.SetAt(r, c, chip);
        //console.log(`Cell:${r}${c} Player:${this.NextPlayer} Chip:${chip} Board:${this.GetAt(r, c)}\n`);
      }
    }
    //console.log(this.toString());
  }
  ApplyMove(move) {
    let sign = this.GetAt(move.From.Y, move.From.X) >= 0 ? 1 : -1;
    this.SetAt(move.From.Y, move.From.X, 0);
    let currentWeight = this.GetAt(move.To.Y, move.To.X);
    this.SetAt(move.To.Y, move.To.X, (Math.abs(currentWeight) + 1) * sign);
  }
  DoMove(move) {
    if (move.IsValid() && move.Applied == false) {
      move.PreMove();
      this.ApplyMove(move);
      move.PostMove();
      move.Applied = true;
      this.LastMoves.push(move);
      this.ChangePlayer();
    }
  }
  UndoMove(move) {
    if (
      this.LastMoves.length > 0 &&
      move.Applied == true &&
      move.IsEqual(this.LastMoves[this.LastMoves.length - 1])
    ) {
      move.Undo();
      move.Applied = false;
      this.LastMoves.Pop();
      this.ChangePlayer();
    }
  }
  GetMovePositions(From, To, dir) {
    let row = From.Y;
    let col = From.X;
    let movePositions = null;
    if (!From.IsValid()) {
      return movePositions;
    }
    movePositions = [];

    movePositions.push(From);

    switch (dir) {
      case Direction.NONE:
      case Direction.INVALID:
        break;
      case Direction.N:
        col = From.X;
        for (row = From.Y - 1; row > To.Y; row--) {
          movePositions.push(new Position(row, col, this));
        }
        break;
      case Direction.E:
        row = From.Y;
        for (col = From.X + 1; col < To.X; col++) {
          movePositions.push(new Position(row, col, this));
        }
        break;
      case Direction.S:
        col = From.X;
        for (row = From.Y + 1; row < To.Y; row++) {
          movePositions.push(new Position(row, col, this));
        }
        break;
      case Direction.W:
        row = From.Y;
        for (col = From.X - 1; col > To.X; col--) {
          movePositions.push(new Position(row, col, this));
        }
        break;
      case Direction.NE:
        col = From.X + 1;
        row = From.Y - 1;
        while (col < To.X && row > To.Y) {
          movePositions.push(new Position(row, col, this));
          col++;
          row--;
        }
        break;
      case Direction.NW:
        col = From.X - 1;
        row = From.Y - 1;
        while (col > To.X && row > To.Y) {
          movePositions.push(new Position(row, col, this));
          col--;
          row--;
        }
        break;
      case Direction.SE:
        col = From.X + 1;
        row = From.Y + 1;
        while (col < To.X && row < To.Y) {
          movePositions.push(new Position(row, col, this));
          col++;
          row++;
        }
        break;
      case Direction.SW:
        col = From.X - 1;
        row = From.Y + 1;
        while (col > To.X && row < To.Y) {
          movePositions.push(new Position(row, col, this));
          col--;
          row++;
        }
        break;
      default:
        break;
    }
    movePositions.push(To);
    return movePositions;
  }
  GetIntermediateDistance(From, To, dir) {
    let row = From.Y;
    let col = From.X;
    let intermediateDistance = 0;
    if (!From.IsValid()) {
      return 0;
    }

    if (
      Math.abs(this.GetAt(row, col)) != 1 ||
      Math.abs(this.GetAt(To.Y, To.X)) == 0
    ) {
      return 0;
    }
    switch (dir) {
      case Direction.NONE:
      case Direction.INVALID:
        break;
      case Direction.N:
        col = From.X;
        for (row = From.Y - 1; row > To.Y; row--) {
          intermediateDistance += Math.abs(this.GetAt(row, col));
        }
        break;
      case Direction.E:
        row = From.Y;
        for (col = From.X + 1; col < To.X; col++) {
          intermediateDistance += Math.abs(this.GetAt(row, col));
        }
        break;
      case Direction.S:
        col = From.X;
        for (row = From.Y + 1; row < To.Y; row++) {
          intermediateDistance += Math.abs(this.GetAt(row, col));
        }
        break;
      case Direction.W:
        row = From.Y;
        for (col = From.X - 1; col > To.X; col--) {
          intermediateDistance += Math.abs(this.GetAt(row, col));
        }
        break;
      case Direction.NE:
        col = From.X + 1;
        row = From.Y - 1;
        while (col < To.X && row > To.Y) {
          intermediateDistance += Math.abs(this.GetAt(row, col));
          col++;
          row--;
        }
        break;
      case Direction.NW:
        col = From.X - 1;
        row = From.Y - 1;
        while (col > To.X && row > To.Y) {
          intermediateDistance += Math.abs(this.GetAt(row, col));
          col--;
          row--;
        }
        break;
      case Direction.SE:
        col = From.X + 1;
        row = From.Y + 1;
        while (col < To.X && row < To.Y) {
          intermediateDistance += Math.abs(this.GetAt(row, col));
          col++;
          row++;
        }
        break;
      case Direction.SW:
        col = From.X - 1;
        row = From.Y + 1;
        while (col > To.X && row < To.Y) {
          intermediateDistance += Math.abs(this.GetAt(row, col));
          col--;
          row++;
        }
        break;
      default:
        break;
    }
    return intermediateDistance;
  }

  Evaluation() {
    this.Scoring[Player.White] = 0;
    this.Scoring[Player.Black] = 0;
    this.Scoring[TOTAL] = 0;
    for (let r = 0; r < DIM; r++) {
      for (let c = 0; c < DIM; c++) {
        if (this.GetAt(r, c) > 1) {
          this.Scoring[Player.White] += this.GetAt(r, c);
          this.Scoring[TOTAL] += this.GetAt(r, c);
        } else if (this.GetAt(r, c) < -1) {
          this.Scoring[Player.Black] += Math.abs(this.GetAt(r, c));
          this.Scoring[TOTAL] += this.GetAt(r, c);
        }
      }
    }
    return this.Scoring[TOTAL];
  }

  toString() {
    let output = "";
    for (let r = 0; r < DIM; r++) {
      for (let c = 0; c < DIM; c++) {
        if (this.GetAt(r, c) < 0) {
          output += ` ${this.GetAt(r, c)} `;
        } else {
          output += `  ${this.GetAt(r, c)} `;
        }
      }
      output += "\n";
    }

    return output;
  }
}

// Main Window
let PeakBoard = null;
let RedPlayer = "human";
let BluePlayer = "human";
let GameIsRunning = false;
let From = null;
let To = null;
let NextMoves = [];
let NextMoveCount = 0;
let FrameButtons = [];
let FromButton = null;
let ToButton = null;
let HintMove = null;

let winLeft = document.getElementById("win_left_item");
let winRight = document.getElementById("win_right_item");
let scoreLeft = document.getElementById("score_left_item");
let scoreRight = document.getElementById("score_right_item");
let passLeft = document.getElementById("pass_left_item");
let passRight = document.getElementById("pass_right_item");
let playerSelectLeft = document.getElementById("player_left_select");
let playerSelectRight = document.getElementById("player_right_select");

let goButton = document.getElementById("go");
let newButton = document.getElementById("new");
let passButton = document.getElementById("pass");

function InitGame() {
  PeakBoard = new GameBoard(Player.White);
  //console.log(PeakBoard.toString());

  ActionButtonEnable(newButton, true);
  ActionButtonEnable(goButton, true);
  ActionButtonEnable(passButton, false);

  ItemVisible(winLeft, true);
  ItemVisible(winRight, true);
  ItemVisible(passLeft, true);
  ItemVisible(passRight, true);

  ScoreBoard(0, 0);

  playerSelectLeft.value = "human";
  playerSelectRight.value = "minmax3";

  DrawBoard();

  RedPlayer = "human";
  RedPlayer = playerSelectLeft.value;

  BluePlayer = "human";
  BluePlayer = playerSelectLeft.value;
}

function ActionButtonEnable(action, enable) {
  if (enable === false) {
    action.disabled = true;
    action.classList.add("action_disabled");
  } else {
    action.disabled = false;
    action.classList.remove("action_disabled");
  }
}

function ItemVisible(item, visible) {
  if (visible === true) {
    item.style.visibility = "visible";
  } else {
    item.style.visibility = "hidden";
  }
}

function ScoreBoard(leftscore, rightscore) {
  scoreLeft.innerHTML = leftscore.toString();
  scoreRight.innerHTML = rightscore.toString();

  if (leftscore > rightscore) {
    scoreLeft.classList.add("winner");
    scoreLeft.classList.remove("loser");
    scoreLeft.classList.remove("tied");

    scoreRight.classList.remove("winner");
    scoreRight.classList.add("loser");
    scoreRight.classList.remove("tied");
  } else if (leftscore < rightscore) {
    scoreLeft.classList.remove("winner");
    scoreLeft.classList.add("loser");
    scoreLeft.classList.remove("tied");

    scoreRight.classList.add("winner");
    scoreRight.classList.remove("loser");
    scoreRight.classList.remove("tied");
  } else {
    scoreLeft.classList.remove("winner");
    scoreLeft.classList.remove("loser");
    scoreLeft.classList.add("tied");

    scoreRight.classList.remove("winner");
    scoreRight.classList.remove("loser");
    scoreRight.classList.add("tied");
  }
}

function DrawBoard() {
  for (let row = 0; row < 6; row++) {
    for (let col = 0; col < 6; col++) {
      let button = GetGridElement(row, col);
      if (button != null) {
        let chip = PeakBoard.GetAt(row, col);
        let val = "";
        if (Math.abs(chip) >= 2) {
          val = Math.abs(chip).toString();
        }
        button.innerHTML = val;

        if (chip > 0) {
          button.classList.remove("itemblue");
          button.classList.remove("itemblank");
          button.classList.add("itemred");
        } else if (chip < 0) {
          button.classList.remove("itemred");
          button.classList.remove("itemblank");
          button.classList.add("itemblue");
        } else {
          button.classList.remove("itemred");
          button.classList.remove("itemblue");
          button.classList.add("itemblank");
        }
        FrameButton(button, false);
      }
    }
  }
}

function FrameButton(button, frame) {
  if (frame == true) {
    button.classList.add("frame");
  } else {
    button.classList.remove("frame");
  }
}

function GetGridElement(row, col) {
  let itemName = `item${row}${col}`;
  let buttons = document.getElementsByClassName("item");
  for (let i = 0; i < buttons.length; i++) {
    let button = buttons[i];
    if (button.id === itemName) {
      return button;
    }
  }
  return null;
}
function GetButtonGridPosition(sender) {
  let row = parseInt(sender.id.substring(4, 5));
  let col = parseInt(sender.id.substring(5));
  var obj = {
    ROW: row,
    COL: col,
  };

  return obj;
}

function MouseIsAllowed() {
  if (!GameIsRunning) return false;
  if (PeakBoard.NextPlayer === Player.White && RedPlayer != "human")
    return false;
  if (PeakBoard.NextPlayer == Player.Black && BluePlayer != "human")
    return false;
  return true;
}
function RemoveHint() {}

// User interactions
function mouse_click_example(obj) {
  if (obj.classList.contains("itemblue")) {
    obj.classList.remove("itemblue");
    obj.classList.add("itemred");
  } else if (obj.classList.contains("itemred")) {
    obj.classList.remove("itemred");
    obj.classList.add("itemblue");
  }

  obj.innerHTML = "X";
  document.getElementById("debug_output").innerHTML = "mouse_click " + obj.id;
}

// click
function button_mouseclick(obj) {
  if (!MouseIsAllowed()) return;
}

// mouseenter
function button_mouseenter(obj) {
  if (!MouseIsAllowed()) return;

  RemoveHint();

  let row, col;
  let result = GetButtonGridPosition(obj);
  row = result.ROW;
  col = result.COL;

  if (From == null) {
    FrameButtons = [];
    for (let m = 0; m < NextMoves.length; m++) {
      let move = NextMoves[m];
      if (move.From.X === col && move.From.Y === row) {
        let button = GetGridElement(move.To.Y, move.To.X);
        if (button != null) {
          FrameButtons.push(button);
          FrameButton(button, true);
        }
      }
    }
  }
}

// mousemove
function button_mousemove(obj) {
  if (!MouseIsAllowed()) return;

  RemoveHint();

  let row, col;
  let result = GetButtonGridPosition(obj);
  row = result.ROW;
  col = result.COL;
}

// mouseleave
function button_mouseleave(obj) {
  if (!MouseIsAllowed()) return;

  RemoveHint();

  let row, col;
  let result = GetButtonGridPosition(obj);
  row = result.ROW;
  col = result.COL;

  if (From == null) {
    for (let b = 0; b < FrameButtons.length; b++) {
      let button = FrameButtons[b];
      if (button != null) {
        FrameButton(button, false);
      }
    }
  }
}

// select_change
function player_select_change(select) {
  RedPlayer = playerSelectLeft.value;
  BluePlayer = playerSelectRight.value;

  console.log(`RedPlayer: ${RedPlayer} BluePlayer: ${BluePlayer}\n`);
}

// Start
InitGame();

NextMoves = PeakBoard.MakeMoveList();
NextMovesCount = NextMoves.length;

/*
let from = new Position(1, 1, PeakBoard);
let to = new Position(3, 3, PeakBoard);
let move = new Move(from, to, PeakBoard);
let dir = move.GetDirection();
console.log(dir);
*/
