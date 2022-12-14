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

async function WaitNMilliSeconds(milliseconds) {
  await Sleep(milliseconds);
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
    let nextMoves = board.MakeMoveList();
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

    let bestvalue = board.NextPlayer === Player.White ? MinValue : MaxValue;
    let otherPlayer =
      board.NextPlayer === Player.White ? Player.Black : Player.White;

    if (board.CheckWin(otherPlayer)) {
      bestvalue = board.Evaluation();
      var obj = {
        VALUE: bestvalue,
        MOVE: bestmove,
      };
      return obj;
    }
    if (depth === 0) {
      bestvalue = board.Evaluation();
      var obj = {
        VALUE: bestvalue,
        MOVE: bestmove,
      };
      return obj;
    }

    let n = 0;
    let nextMoves = board.MakeMoveList();
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
      let result = this.MinMaxMove(board, depth - 1);
      let value = result.VALUE;
      let nextbestmove = result.MOVE;
      board.UndoMove(move);

      if (board.NextPlayer === Player.White) {
        if (value > bestvalue) {
          bestvalue = value;
          bestmove = move;
          bestMoves = [];
          bestMoves.push(move);
        } else if (value === bestvalue) {
          bestMoves.push(move);
        }
      } else {
        if (value < bestvalue) {
          bestvalue = value;
          bestmove = move;
          bestMoves = [];
          bestMoves.push(move);
        } else if (value === bestvalue) {
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
      bestvalue = board.Evaluation();
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

  toString() {
    return `Row[${this.Y}]Col[${this.X}]`;
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
    let dir = this.GetDirection();
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
  toString() {
    return `${this.From.toString()} -> ${this.To.toString()} ${this.MyBoard.GetAt(
      this.To.Y,
      this.To.X
    )}+1`;
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
      console.log("Pass: ", player, " -> ", this.Passing[player]);
    } else {
      this.Passing[player] = 0;
    }
  }
  CheckWin(player) {
    let otherPlayer = this.OtherPlayer(player);
    if (
      this.Passing[Player.White] > 1 ||
      this.Passing[Player.Black] > 1 ||
      (this.Passing[Player.White] == 1 && this.Passing[Player.Black] == 1)
    ) {
      let score = this.Evaluation();
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
    this.Evaluation();
    whitescore = this.Scoring[Player.White];
    blackscore = this.Scoring[Player.Black];
    totalscore = this.Scoring[Player.None];
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
    return player === Player.White ? Player.Black : Player.White;
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
    this.Passing[0] = 0;
    this.Passing[1] = 0;
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
      this.LastMoves.pop();
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
function InitGame() {
  DrawTitle("Peak!");

  PeakBoard = new GameBoard(Player.White);
  //console.log(PeakBoard.toString());

  GameIsRunning = false;

  ActionButtonEnable(newButton, true);
  ActionButtonEnable(goButton, true);
  ActionButtonEnable(passButton, false);

  ScoreBoard(0, 0);
  PassingDisplay(
    PeakBoard.Passing[Player.White],
    PeakBoard.Passing[Player.Black]
  );

  DrawBoard();

  RedPlayer = playerSelectLeft.value;
  BluePlayer = playerSelectRight.value;
  console.log(`RedPlayer: ${RedPlayer} BluePlayer: ${BluePlayer}\n`);
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

function PassingDisplay(leftpass, rightpass) {
  passLeft.innerHTML = "";
  passRight.innerHTML = "";
  ItemVisible(passLeft, leftpass > 0);
  if (leftpass > 1) passLeft.innerHTML = "||";
  ItemVisible(passRight, rightpass > 0);
  if (rightpass > 1) passRight.innerHTML = "||";

  console.log("LeftPass: ", leftpass, "  RightPass: ", rightpass);
}

function WinDisplay(isGameOver, winner) {
  if (!isGameOver) {
    ItemVisible(winLeft, false);
    ItemVisible(winRight, false);
  } else {
    if (winner === Player.None) {
      ItemVisible(winLeft, true);
      ItemVisible(winRight, true);
    } else if (winner === Player.White) {
      ItemVisible(winLeft, true);
      ItemVisible(winRight, false);
    } else if (winner === Player.Black) {
      ItemVisible(winLeft, false);
      ItemVisible(winRight, true);
    }
  }
}

function TurnDisplay(isGameOver, nextPlayer) {
  if (isGameOver || !GameIsRunning) {
    TurnButton(scoreLeft, false);
    TurnButton(scoreRight, false);
  } else {
    TurnButton(scoreLeft, nextPlayer === Player.White);
    TurnButton(scoreRight, nextPlayer === Player.Black);
  }
}

async function DrawBoard() {
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
  let winner, score, whitescore, blackscore, totalscore, isGameOver, theOther;

  let result = PeakBoard.GameOver();
  winner = result.WINNER;
  score = result.SCORE;
  whitescore = result.WHITESCORE;
  blackscore = result.BLACKSCORE;
  totalscore = result.TOTALSCORE;
  isGameOver = result.GAMEOVER;

  if (GameIsRunning && isGameOver) {
    if (winner == Player.None) {
      DrawTitle("Tie Game!");
    } else {
      theOther = PeakBoard.OtherPlayer(winner);
      let theOtherScore =
        score == Math.abs(whitescore)
          ? Math.abs(blackscore)
          : Math.abs(whitescore);
      DrawTitle(`${GetPlayerColor(winner)} wins!`);
    }
    GameIsRunning = false;
    ActionButtonEnable(newButton, true);
    ActionButtonEnable(goButton, false);
    ActionButtonEnable(passButton, false);
  } else if (GameIsRunning) {
  }

  ScoreBoard(whitescore, blackscore);
  PassingDisplay(
    PeakBoard.Passing[Player.White],
    PeakBoard.Passing[Player.Black]
  );
  WinDisplay(isGameOver, winner);
  TurnDisplay(isGameOver, PeakBoard.NextPlayer);

  From = null;
  To = null;
  NextMoves = [];
  NextMovesCount = 0;
  FrameButtons = [];
  FromButton = null;
  ToButton = null;
  HintMove = null;

  NextMoves = PeakBoard.MakeMoveList();
  NextMovesCount = NextMoves.length;

  await NextTurn();
}

function GetPlayerColor(player) {
  if (player === Player.White) return "RED";
  if (player === Player.Black) return "BLUE";
  return "WHITE";
}

function GetPlayerName(player) {
  if (player === "human") return "Human Player";
  if (player === "random") return "Random";
  if (player === "minmax1") return "MiniMax One";
  if (player === "minmax2") return "MiniMax Two";
  if (player === "minmax3") return "MiniMax Three";
  if (player === "minmax4") return "MiniMax Four";
  if (player === "minmax5") return "MiniMax Five";
  return "WHITE";
}

function FrameButton(button, frame) {
  if (frame === true) {
    button.classList.add("frame");
  } else {
    button.classList.remove("frame");
  }
}

function TurnButton(button, turn) {
  if (turn == true) {
    button.classList.add("turn");
  } else {
    button.classList.remove("turn");
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
async function button_mouseclick(obj) {
  if (!MouseIsAllowed()) return;

  let buttonClicked = obj;
  let row, col;
  let result = GetButtonGridPosition(buttonClicked);
  row = result.ROW;
  col = result.COL;

  if (buttonClicked != null && From == null) {
    for (let i = 0; i < NextMoves.length; i++) {
      let move = NextMoves[i];
      if (move != null && move.From.X === col && move.From.Y === row) {
        buttonClicked.innerHTML = "1";
        From = new Position(row, col, PeakBoard);
        FromButton = buttonClicked;
        To = null;
        FrameButtons.push(buttonClicked);
        FrameButton(buttonClicked, true);
        return;
      }
    }
  } else if (buttonClicked != null && From != null && To == null) {
    if (From.X === col && From.Y === row) {
      // Reset From-Button
      From = null;
      for (let b = 0; b < FrameButtons.length; b++) {
        let button = FrameButtons[b];
        FrameButton(button, false);
      }
      FrameButtons = [];
      DrawBoard();
      return;
    }

    for (let i = 0; i < NextMoves.length; i++) {
      let move = NextMoves[i];
      if (
        move != null &&
        move.From.X === From.X &&
        move.From.Y === From.Y &&
        move.To.X === col &&
        move.To.Y === row
      ) {
        To = new Position(row, col, PeakBoard);
        ToButton = buttonClicked;
        let currentPlayer = PeakBoard.NextPlayer;

        await ShowAnimateMove(move, 500);

        PeakBoard.Pass(currentPlayer, false);
        PeakBoard.DoMove(move);

        for (let b = 0; b < FrameButtons.length; b++) {
          let button = FrameButtons[b];
          FrameButton(button, false);
        }
        FrameButtons = [];

        DrawBoard();
        break;
      }
    }
  }
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
  if (select.id === "player_left_select") {
    RedPlayer = select.value;
  } else if (select.id === "player_right_select") {
    BluePlayer = select.value;
  }
  console.log(`RedPlayer: ${RedPlayer} BluePlayer: ${BluePlayer}\n`);
}

async function NextTurn() {
  if (GameIsRunning === false) return;

  let currentPlayer = PeakBoard.NextPlayer;

  DrawTitle(
    `${GetPlayerColor(Player.White)} is ${GetPlayerName(
      RedPlayer
    )}&nbsp;&nbsp;&nbsp;&nbsp;${GetPlayerColor(
      Player.Black
    )} is ${GetPlayerName(BluePlayer)}`
  );

  if (currentPlayer === Player.White && RedPlayer === "human") return;
  if (currentPlayer === Player.Black && BluePlayer === "human") return;

  // Waitcursor
  root.style.cursor = "wait";

  let bestMove = null;
  if (currentPlayer === Player.White) {
    if (RedPlayer === "random") {
      bestMove = Algorithmn.RandomMove(PeakBoard);
      if (bestMove != null)
        console.log(bestMove.toString(), currentPlayer, RedPlayer);
    } else {
      let maxDepth = 1;
      if (RedPlayer === "minmax1") maxDepth = 1;
      else if (RedPlayer === "minmax2") maxDepth = 2;
      else if (RedPlayer === "minmax3") maxDepth = 3;
      else if (RedPlayer === "minmax4") maxDepth = 4;
      else if (RedPlayer === "minmax5") maxDepth = 5;
      let depth = 1;
      if (NextMovesCount > 0) {
        depth = Math.round((1.0 / parseFloat(NextMovesCount)) * 80.0);
        depth++;
        if (depth < 1) depth = 1;
        if (depth > maxDepth) depth = maxDepth;
      }
      let result = Algorithmn.MinMaxMove(PeakBoard, depth);
      bestMove = result.MOVE;
      if (bestMove != null)
        console.log(bestMove.toString(), currentPlayer, RedPlayer, depth);
    }
  } else if (currentPlayer === Player.Black) {
    if (BluePlayer === "random") {
      bestMove = Algorithmn.RandomMove(PeakBoard);
      if (bestMove != null)
        console.log(bestMove.toString(), currentPlayer, BluePlayer);
    } else {
      let maxDepth = 1;
      if (BluePlayer === "minmax1") maxDepth = 1;
      else if (BluePlayer === "minmax2") maxDepth = 2;
      else if (BluePlayer === "minmax3") maxDepth = 3;
      else if (BluePlayer === "minmax4") maxDepth = 4;
      else if (BluePlayer === "minmax5") maxDepth = 5;
      let depth = 1;
      if (NextMovesCount > 0) {
        depth = Math.round((1.0 / parseFloat(NextMovesCount)) * 80.0);
        depth++;
        if (depth < 1) depth = 1;
        if (depth > maxDepth) depth = maxDepth;
      }
      let result = Algorithmn.MinMaxMove(PeakBoard, depth);
      bestMove = result.MOVE;
      if (bestMove != null)
        console.log(bestMove.toString(), currentPlayer, BluePlayer, depth);
    }
  }

  // NoWaitcursor
  root.style.cursor = "auto";

  if (bestMove != null) {
    await ShowAnimateMove(bestMove, 850);

    PeakBoard.Pass(currentPlayer, false);
    PeakBoard.DoMove(bestMove);
    DrawBoard();
  } else {
    PeakBoard.Pass(currentPlayer, true);
    PeakBoard.ChangePlayer();
    DrawBoard();
    await WaitNMilliSeconds(500);
  }
}

async function go_mouseclick(button) {
  GameIsRunning = true;
  ActionButtonEnable(goButton, false);
  ActionButtonEnable(passButton, true);
  TurnDisplay(false, PeakBoard.NextPlayer);
  await NextTurn();
}

function new_mouseclick(button) {
  GameIsRunning = false;
  ActionButtonEnable(goButton, true);
  ActionButtonEnable(passButton, false);
  InitGame();
}

async function pass_mouseclick(button) {
  let currentPlayer = PeakBoard.NextPlayer;
  PeakBoard.Pass(currentPlayer, true);
  PassingDisplay(
    PeakBoard.Passing[Player.White],
    PeakBoard.Passing[Player.Black]
  );
  PeakBoard.ChangePlayer();
  await WaitNMilliSeconds(200);
  DrawBoard();
  /*await WaitNMilliSeconds(500);*/
  /*await NextTurn();*/
}

async function ShowAnimateMove(bestMove, totaltime) {
  if (bestMove === null) return;

  for (let i = 0; i < FrameButtons.length; i++) {
    let frButton = FrameButtons[i];
    FrameButton(frButton, false);
  }
  FrameButtons = [];

  let movePositions = bestMove.GetMovePositions();

  if (movePositions === null || movePositions.length === 0) {
    return;
  }

  let count = 2 * movePositions.length - 1;
  let intervall = Math.round(parseFloat(totaltime) / parseFloat(count));
  let button = null;
  let pos = null;

  pos = movePositions[0];
  button = GetGridElement(pos.Y, pos.X);
  if (button != null) {
    button.innerHTML = "0";
  }
  pos = movePositions[movePositions.length - 1];
  button = GetGridElement(pos.Y, pos.X);
  if (button != null) {
    button.innerHTML = `${Math.abs(PeakBoard.GetAt(pos.Y, pos.X))}+1`;
  }
  for (let i = 0; i < movePositions.length; i++) {
    pos = movePositions[i];
    button = GetGridElement(pos.Y, pos.X);
    if (button != null) {
      FrameButton(button, true);
      await WaitNMilliSeconds(intervall);
    }
  }
  for (let i = 0; i < movePositions.length - 1; i++) {
    pos = movePositions[i];
    button = GetGridElement(pos.Y, pos.X);
    if (button != null) {
      FrameButton(button, false);
      await WaitNMilliSeconds(intervall);
    }
  }
  pos = movePositions[0];
  button = GetGridElement(pos.Y, pos.X);
  if (button != null) {
    await WaitNMilliSeconds(200);
  }
  pos = movePositions[movePositions.length - 1];
  button = GetGridElement(pos.Y, pos.X);
  if (button != null) {
    button.innerHTML = `${Math.abs(PeakBoard.GetAt(pos.Y, pos.X)) + 1}`;
    FrameButton(button, false);
    await WaitNMilliSeconds(200);
  }
}

function DrawTitle(title) {
  if (title === "" || title === "Peak!") {
    header.style.fontSize = `2.5em`;
    header.innerHTML = "Peak!";
  } else {
    header.style.fontSize = `1em`;
    header.innerHTML = title;
  }
}

function getDimensions() {
  let width = window.innerWidth;
  let height = 0; /*window.innerHeight*/

  console.log(
    `WindowWidth: ${window.innerWidth} WindowHeight: ${window.innerHeight}\n`
  );

  let unit =
    width >= height
      ? parseFloat(width) / (1920.0 * 1.33)
      : parseFloat(height) / (1080.0 * 1.33);

  root.style.setProperty("--f", `${unit}`);

  return unit;
}

// Initial
let root = document.documentElement;
let myUnit = getDimensions();

let PeakBoard = null;
let RedPlayer = "minmax2";
let BluePlayer = "minmax4";
let GameIsRunning = false;
let From = null;
let To = null;
let NextMoves = [];
let NextMovesCount = 0;
let FrameButtons = [];
let FromButton = null;
let ToButton = null;
let HintMove = null;

let header = document.getElementById("peak");

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

if (playerSelectLeft != null && playerSelectRight != null) {
  playerSelectLeft.value = RedPlayer;
  playerSelectRight.value = BluePlayer;
  // Start
  InitGame();
}
