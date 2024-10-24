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
using System.Windows.Threading;

namespace Lists_5___Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Point> _snakeBody = new List<Point>();
        DispatcherTimer _timer = new DispatcherTimer();
        int _celSize = 20;
        private int _maximumNumberOfFoodBlocks = 10;
        private int _maximumBonus = 2;
        readonly List<Point> _food = new List<Point>();
        readonly List<Point> _bonusses = new List<Point>();
        string[] _directions = new string[4] { "Up", "Left", "Down", "Right" };
        string _direction = "Left";
        string _newDirection = "Left";
        double _maxX;
        double _maxY;
        Random _rand = new Random();
        int _playerScore = 0;
        int _highScore = 0;
        //int countDown;

        public MainWindow()
        {
            InitializeComponent();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            _timer.Tick += Timer_Tick;
            _maxX = snakeWindow.Width;
            _maxY = snakeWindow.Height;
            StartGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Point newPositionHead = NewPositionHead();
            if (IsGameOver(newPositionHead))
            {
                snakeWindow.Background = Brushes.Red;
                _timer.Stop();
                if (_playerScore > _highScore)
                {
                    _highScore = _playerScore;
                    highScoreTextBlock.Text = $"Hoogste score: {_highScore}";
                }
            }
            else
            {
                if (_food.Contains(newPositionHead))
                {
                    _food.Remove(newPositionHead);
                    ExtendSnake();
                    _playerScore += 10;
                }
                if (_bonusses.Contains(newPositionHead))
                {
                    _bonusses.Remove(newPositionHead);
                    ExtendSnake(5);
                    _playerScore += 50;
                }
                MoveSnake(newPositionHead);
                DrawGame();
                scoreTextBlock.Text = _playerScore.ToString();
            }
        }

        private void StartGame()
        {
            _snakeBody.Clear();
            _food.Clear();
            _bonusses.Clear();
            _snakeBody.Add(new Point(520, 80));
            ExtendSnake(2);
            snakeWindow.Background = Brushes.Black;
            _direction = "Left";
            _newDirection = "Left";

            _timer.Start();
            countdownTextBlock.Visibility = Visibility.Hidden;
            //Countdown timer
            //countdownTextBlock.Visibility = Visibility.Visible;
            //countDown = 5;
            //DispatcherTimer countDownTimer = new DispatcherTimer();
            //countDownTimer.Interval = new TimeSpan(0, 0, 1);
            //countDownTimer.Tick += CountDown_Tick;
            //countDownTimer.Start();
        }

        //private void CountDown_Tick(object sender, EventArgs e)
        //{
        //    if (countDown == 0)
        //    {
        //        countdownTextBlock.Visibility = Visibility.Hidden;
        //        timer.Start();
        //    }
        //    else
        //        countdownTextBlock.Text = $"{countDown--}";
        //}

        private void ExtendSnake(int length = 1)
        {
            switch (_newDirection)
            {
                case "Up":
                    {
                        for (int i = 0; i < length; i++)
                        {
                            _snakeBody.Add(new Point(_snakeBody[_snakeBody.Count - 1].X, _snakeBody[_snakeBody.Count - 1].Y - _celSize));
                        }
                        break;
                    }
                case "Left":
                    {
                        for (int i = 0; i < length; i++)
                        {
                            _snakeBody.Add(new Point(_snakeBody[_snakeBody.Count - 1].X + _celSize, _snakeBody[_snakeBody.Count - 1].Y));
                        }
                        break;
                    }
                case "Right":
                    {
                        for (int i = 0; i < length; i++)
                        {
                            _snakeBody.Add(new Point(_snakeBody[_snakeBody.Count - 1].X - _celSize, _snakeBody[_snakeBody.Count - 1].Y));
                        }
                        break;
                    }
                case "Down":
                    {
                        for (int i = 0; i < length; i++)
                        {
                            _snakeBody.Add(new Point(_snakeBody[_snakeBody.Count - 1].X, _snakeBody[_snakeBody.Count - 1].Y + _celSize));
                        }
                        break;
                    }
            }
        }

        private void DrawGame()
        {
            SpawnFoodBlock();
            snakeWindow.Children.Clear();
            foreach (Point positionSnakePart in _snakeBody)
            {
                Ellipse snakePart = new Ellipse();
                snakePart.Fill = Brushes.LightGreen;
                snakePart.Width = _celSize;
                snakePart.Height = _celSize;
                Canvas.SetBottom(snakePart, positionSnakePart.Y);
                Canvas.SetLeft(snakePart, positionSnakePart.X);
                snakeWindow.Children.Add(snakePart);
            }
            foreach (Point foodBlock in _food)
            {
                Ellipse food = new Ellipse();
                food.Fill = Brushes.Orange;
                food.Width = _celSize;
                food.Height = _celSize;
                Canvas.SetBottom(food, foodBlock.Y);
                Canvas.SetLeft(food, foodBlock.X);
                snakeWindow.Children.Add(food);
            }
            foreach (Point bonusBlock in _bonusses)
            {
                Ellipse bonus = new Ellipse();
                bonus.Fill = Brushes.Green;
                bonus.Width = _celSize;
                bonus.Height = _celSize;
                Canvas.SetBottom(bonus, bonusBlock.Y);
                Canvas.SetLeft(bonus, bonusBlock.X);
                snakeWindow.Children.Add(bonus);
            }
        }

        private bool IsGameOver(Point newHead)
        {
            switch (_newDirection)
            {
                case "Up":
                    {
                        if (newHead.Y >= _maxY) return true;
                    }
                    break;
                case "Left":
                    {
                        if (newHead.X < 0) return true;
                    }
                    break;
                case "Right":
                    {
                        if (newHead.X >= _maxX) return true;
                    }
                    break;
                case "Down":
                    {
                        if (newHead.Y < 0) return true;
                    }
                    break;
            }
            if (_snakeBody.Contains(newHead)) return true;
            return false;
        }

        private void SpawnFoodBlock()
        {
            if (_food.Count < _maximumNumberOfFoodBlocks)
            {
                //Do while controle op einde pas inbouwen!
                Point foodPoint = new Point();
                do
                {
                    int posX = _rand.Next(0, (int)(_maxX / _celSize));
                    int posY = _rand.Next(0, (int)(_maxY / _celSize));
                    posX *= _celSize;
                    posY *= _celSize;
                    foodPoint.X = posX;
                    foodPoint.Y = posY;
                } while (_bonusses.Contains(foodPoint) && _food.Contains(foodPoint) && _snakeBody.Contains(foodPoint));
                _food.Add(foodPoint);
            }
            if (_bonusses.Count < _maximumBonus)
            {
                //Do while controle op einde pas inbouwen!
                Point bonusPoint = new Point();
                do
                {
                    int posX = _rand.Next(0, (int)(_maxX / _celSize));
                    int posY = _rand.Next(0, (int)(_maxY / _celSize));
                    posX *= _celSize;
                    posY *= _celSize;
                    bonusPoint.X = posX;
                    bonusPoint.Y = posY;
                } while (_bonusses.Contains(bonusPoint) && _food.Contains(bonusPoint) && _snakeBody.Contains(bonusPoint));
                _bonusses.Add(bonusPoint);
            }
        }

        private Point NewPositionHead()
        {
            switch (_newDirection)
            {
                case "Up":
                    {
                        return new Point(_snakeBody[0].X, _snakeBody[0].Y + _celSize);
                    }
                case "Left":
                    {
                        return new Point(_snakeBody[0].X - _celSize, _snakeBody[0].Y);
                    }
                case "Right":
                    {
                        return new Point(_snakeBody[0].X + _celSize, _snakeBody[0].Y);
                    }
                case "Down":
                    {
                        return new Point(_snakeBody[0].X, _snakeBody[0].Y - _celSize);
                    }
                default:
                    return new Point(_snakeBody[0].X, _snakeBody[0].Y);
            }
        }

        private void MoveSnake(Point newHead)
        {
            _snakeBody.RemoveAt(_snakeBody.Count - 1);
            _snakeBody.Insert(0, newHead);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (_direction != _directions[0])
                    {
                        //Indien vorige righting niet boven was, ga naar onder
                        _direction = _newDirection = _directions[2];
                    }
                    break;
                case Key.Up:
                    if (_direction != _directions[2])
                    {
                        //Indien vorige righting niet onder was, ga naar boven
                        _direction = _newDirection = _directions[0];
                    }
                    break;
                case Key.Right:
                    if (_direction != _directions[1])
                    {
                        //Indien vorige righting niet links was, ga naar rechts
                        _direction = _newDirection = _directions[3];
                    }
                    break;
                case Key.Left:
                    if (_direction != _directions[3])
                    {
                        //Indien vorige righting niet rechts was, ga naar links
                        _direction = _newDirection = _directions[1];
                    }
                    break;
                case Key.R:
                    StartGame();
                    break;
            }
        }
    }
}
