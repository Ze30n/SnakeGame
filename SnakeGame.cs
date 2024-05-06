using System.Diagnostics;
using System.Timers;
using Timer = System.Timers.Timer;

public class SnakeGame{
    public static Direction direction;
    public static List<Vector2> walls;
    public static bool pressed = false;
    public static Timer timer = new Timer(800);
    public static int xMap;
    public static int yMap;
    public static bool isFalled = false;
    public static Vector2 fruit = new Vector2(0, 0);
    public static int score = 0;
    public static int time = 1;
    public static List<Vector2> snake;
    public class Vector2{
        public int x;
        public int y;
        public Vector2(int x, int y){
            this.x = x;
            this.y = y;
        }
    }

    public static void GetKey(){
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        if (keyInfo.Key == ConsoleKey.UpArrow)
        {
            // Di chuyển đối tượng lên trên
            if(direction != Direction.down){
                direction = Direction.up;
                pressed = true; 
            }            
        }
        else if (keyInfo.Key == ConsoleKey.DownArrow)
        {
            // Di chuyển đối tượng xuống dưới
            if(direction != Direction.up)
            {
                direction = Direction.down;
                pressed = true; 
            }           
        }
        else if (keyInfo.Key == ConsoleKey.LeftArrow)
        {
            // Di chuyển đối tượng sang trái
            if (direction != Direction.right){
                direction = Direction.left;
                pressed = true; 
            }
        }
        else if (keyInfo.Key == ConsoleKey.RightArrow)
        {
            // Di chuyển đối tượng sang phải
            if(direction != Direction.left){
                direction = Direction.right;
                pressed = true; 
            }
        }

        // pressed = false; 
    }

    public static void Update(Direction direction, Vector2 map, List<Vector2> snake){
        //Console.Clear();
        DrawMap(map);
      
        if(direction == Direction.up){
            snake = SnakeCalculation(snake, 0, -1);                   
        }                   
        else if (direction == Direction.down)
        {
            snake = SnakeCalculation(snake, 0, 1);     
        }
        else if (direction == Direction.left)
        {
            snake = SnakeCalculation(snake, -1, 0);
        }
        else if (direction == Direction.right)
        {
            snake = SnakeCalculation(snake, 1, 0);
        }
        Console.Clear();
        DrawMap(map);
        DrawFruit();
        DrawSnake(snake);
        Draw(xMap / 2, yMap, ConsoleColor.Black, ConsoleColor.Black, "Score : " + score);
        Reset();
    }
    
    public static void Reset(){
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Black;
    }
    
    public static void DrawMap(Vector2 map){
        for(int i = 0; i < map.y; i++){
            for(int j = 0; j < map.x; j+=1){
                string s = "  ";
                ConsoleColor backgroundColor = ConsoleColor.Black;
                if(i == 0 || j == 0 || i == map.y - 1 || j == map.x - 1){
                    s = "***";
                    backgroundColor = ConsoleColor.White;
                }
                foreach(Vector2 v in walls){
                    if(v.x == i && v.y == j){
                        s = "***";
                        backgroundColor = ConsoleColor.White;
                        break;
                    }                  
                }
                Draw(j, i, ConsoleColor.Black ,backgroundColor, s);
            }
        }

        Reset();
    }

    public static void DrawSnake(List<Vector2> snake){
        for(int i = 0; i < snake.Count; i++){
            string s = " X ";
            if(i == 0){
                s = " O ";
               
            }
             

            Draw(snake[i].x, snake[i].y, ConsoleColor.Green, ConsoleColor.Green, s);
            Reset();
        }
    }
    
    public static void DrawFruit(){
        Draw(fruit.x, fruit.y, ConsoleColor.Red, ConsoleColor.Red, " 0 ");
    }

    public static void Draw(int x, int y, ConsoleColor fontColor, ConsoleColor backgroundColor, string o){
        Console.SetCursorPosition(x * 3, y); 
        Console.ForegroundColor = fontColor; 
        Console.BackgroundColor = backgroundColor;
        Console.Write(o);
    }
    
    public static List<Vector2> SnakeCalculation(List<Vector2> snake, int x, int y){    
        for(int i = snake.Count - 1; i > 0; i--){
            snake[i].x = snake[i - 1].x;
            snake[i].y = snake[i - 1].y;
        }
        snake[0].y += y;
        snake[0].x += x;

        for(int i = 1; i < snake.Count; i++){
            if(snake[0].x == snake[i].x && snake[0].y == snake[i].y){
                timer.Stop();
                isFalled = true;
            }
        } 
        //SnakeOutPut(snake);
        if(snake[0].y >= yMap - 1 || snake[0].x >= xMap - 1 || snake[0].y <= 0 || snake[0].x <= 0){
            timer.Stop();
            isFalled = true;
        }

        foreach(Vector2 v in walls){
            if(snake[0].x == v.x && snake[0].y == v.y){
                timer.Stop();
                isFalled = true;
            }
        }

        if(snake[0].y == fruit.y && snake[0].x == fruit.x){
            score++;
            RandomFruit();
            Draw(snake[0].x, snake[0].y, ConsoleColor.Green, ConsoleColor.Green, " 0 ");
            Reset();
            int count = snake.Count - 1;
            if(direction == Direction.up){
                snake.Add(new Vector2(snake[count].x, snake[count].y - 1));             
            }                   
            else if (direction == Direction.down)
            {
                snake.Add(new Vector2(snake[count].x, snake[count].y + 1));
            }
            else if (direction == Direction.left)
            {
                snake.Add(new Vector2(snake[count].x - 1, snake[count].y));
            }
            else if (direction == Direction.right)
            {
                snake.Add(new Vector2(snake[count].x + 1, snake[count].y));
            }   
        }
        return snake;
    }

    static void SnakeOutPut(List<Vector2> snake){
        Console.WriteLine();
        for(int i = 0; i < snake.Count;i++){
            Console.WriteLine(" i: " + i + " : " + snake[i].x + ", " + snake[i].y);
        }
    }

    static void TimeToUpdate(object sender, ElapsedEventArgs e, Vector2 map, List<Vector2> snake){
        time++;
        
        //direction = GetKey(keyInfo);
        if(time >= 1){
            if(pressed){
                Console.WriteLine(1);
                pressed = false;
                time = 0;
            }
        }
        Update(direction, map, snake);
        if(isFalled){
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Game Over !! \n Score : " + score);
            Environment.Exit(0);
        } 
        //Console.WriteLine("time : " + time);
    }

    public static void RandomFruit(){
        Random rnd = new Random();
        int ramdomX = 0;
        int ramdomY = 0;

        while(true){
            bool check = true;
            ramdomX = rnd.Next(1, xMap - 1);
            ramdomY = rnd.Next(1, yMap - 1);
            foreach(Vector2 v in snake){
                while(ramdomX == v.x && ramdomY == v.y){
                    check = false;
                    break;
                }
            }

            if(check){
                break;
            }
        }
        

        fruit.x = ramdomX;
        fruit.y = ramdomY; 
    }
    public static void Run()
    {
        Console.Clear();
        //khoi tao map moi
        xMap = Console.BufferHeight;
        yMap = Console.BufferHeight - 1;

        Vector2 map = new Vector2(xMap, yMap); 
        
        //khoi tao list tuong
        walls = new List<Vector2>(){};
        DrawMap(map);

        //khoi tao snake moi
        snake = new List<Vector2>() {new Vector2(xMap / 2, yMap / 2), new Vector2(xMap / 2 + 1, yMap / 2)};
        direction = Direction.left;
        DrawSnake(snake);
        //SnakeOutPut(snake);
        RandomFruit();
        Reset();

        timer.Elapsed += (sender, e) => TimeToUpdate(sender, e, map, snake);
        timer.Start();
        while(true){            
            if(!pressed){
                //nhan key
                GetKey();
            }  
        }
    }
}