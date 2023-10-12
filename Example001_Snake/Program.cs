using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class SnakeGame
{
    private static int gameSpeed = 200; // Скорость игры (в миллисекундах)
    private static int gameWidth = 20; // Ширина игрового поля
    private static int gameHeight = 10; // Высота игрового поля

    private static int score = 0; // Текущий счет
    private static bool gameOver = false; // Признак завершения игры

    private static Direction direction = Direction.Right; // Начальное направление движения змейки
    private static Snake snake; // Змейка
    private static Point food; // Еда

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class Snake
    {
        private List<Point> body; // Список сегментов тела змейки

        public Snake()
        {
            body = new List<Point>();
            body.Add(new Point(0, 0)); // Начальная позиция головы змейки
        }

        public void Move(Direction direction)
        {
            // Определяем новую позицию головы в зависимости от текущего направления
            int dx = 0, dy = 0;
            switch (direction)
            {
                case Direction.Up:
                    dy = -1;
                    break;
                case Direction.Down:
                    dy = 1;
                    break;
                case Direction.Left:
                    dx = -1;
                    break;
                case Direction.Right:
                    dx = 1;
                    break;
            }

            // Создаем новую голову на основе старой головы, с учетом направления движения
            Point head = body[0];
            Point newHead = new Point(head.X + dx, head.Y + dy);

            // Проверяем, не вышла ли новая голова за границы игрового поля
            if (newHead.X < 0 || newHead.X >= gameWidth || newHead.Y < 0 || newHead.Y >= gameHeight)
            {
                gameOver = true; // Если да, игра завершается
                return;
            }

            // Проверяем, не пересекает ли новая голова существующее тело змейки
            if (body.Contains(newHead))
            {
                gameOver = true; // Если да, игра завершается
                return;
            }

            // Добавляем новую голову в начало списка сегментов тела змейки
            body.Insert(0, newHead);

            // Проверяем, не съела ли змейка еду
            if (newHead.X == food.X && newHead.Y == food.Y)
            {
                score++; // Увеличиваем счет
                GenerateFood(); // Генерируем новую еду
            }
            else
            {
                // Если змейка не съела еду, убираем последний сегмент тела
                body.RemoveAt(body.Count - 1);
            }
        }

        public void Draw()
        {
            Console.Clear();
            foreach (Point segment in body)
            {
                Console.SetCursorPosition(segment.X, segment.Y);
                Console.Write("*");
            }

            Console.SetCursorPosition(food.X, food.Y);
            Console.Write("#");

            Console.SetCursorPosition(0, gameHeight + 1);
            Console.WriteLine("Score: " + score);
        }

        public void GenerateFood()
        {
            Random random = new Random();
            do
            {
                // Генерируем случайные координаты для еды
                food = new Point(random.Next(0, gameWidth), random.Next(0, gameHeight));
            } while (body.Contains(food));
        }
    }

    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        Console.WindowHeight = gameHeight + 3;

        snake = new Snake();
        snake.GenerateFood();

        while (!gameOver)
        {
            if (Console.KeyAvailable)
            {
                // Обрабатываем нажатия клавиш для изменения направления змейки
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        if (direction != Direction.Down)
                            direction = Direction.Up;
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        if (direction != Direction.Up)
                            direction = Direction.Down;
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        if (direction != Direction.Right)
                            direction = Direction.Left;
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        if (direction != Direction.Left)
                            direction = Direction.Right;
                        break;
                }
            }

            snake.Move(direction);
            snake.Draw();

            Thread.Sleep(gameSpeed);
        }

        Console.SetCursorPosition(0, gameHeight + 2);
        Console.WriteLine("Game Over. Press any key to exit...");
        Console.ReadKey();
    }
}
