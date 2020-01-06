using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace TetrisGame_VS2008
{
    public delegate void KeyDownEventHander(ConsoleKey key);
    public delegate void PaintEventHander(Type type, Element element);
    public delegate void RestructWallEventHander();
    /// <summary>
    /// 枚举，描述如何进行UI绘图 
    /// </summary>
    public enum Type
    {
        /// <summary>
        /// 将全部方块组成的墙画出
        /// </summary>
        AllWallTetris,
        /// <summary>
        /// 画出一个方块
        /// </summary>
        OneTetris,
        /// <summary>
        /// 清除由全部方块组成的墙
        /// </summary>
        ClearAllWallTetris
    }
    /// <summary>
    /// 游戏过程类，负责运行整个游戏的类
    /// </summary>
    public class GameProcess
    {
        /// <summary>
        /// 按键事件
        /// </summary>
        public event KeyDownEventHander KeyDown;
        /// <summary>
        /// 绘制事件
        /// </summary>
        public event PaintEventHander Paint;
        /// <summary>
        /// 重构整个俄罗斯方块所构成的墙的事件
        /// </summary>
        public event RestructWallEventHander RestructWall;
        /// <summary>
        /// 一个游戏数据类，里面包含了游戏运行的信息
        /// </summary>
        GameData gameData;
        Thread keyDownThread;
        System.Timers.Timer timer;
        
        /// <summary>
        /// 开始运行游戏
        /// </summary>
        public void StartGame()
        {
            InitGameData();
            InitGameUI();
            keyDownThread = new Thread(new ThreadStart(KeyDownThread));
            Paint += new PaintEventHander(UIPaint);
            RestructWall += new RestructWallEventHander(RestructWallEvent);
            KeyDown += new KeyDownEventHander(KeyDownEvent);
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            keyDownThread.Start();
            timer.Start();

        }
        /// <summary>
        /// 重新开始游戏
        /// </summary>
        private void ReStartGame()
        {
            int i, j;
            //清空游戏运行区屏幕
            for (i = 0; i <= gameData.Screen.GetUpperBound(0); i++)
                for (j = 0; j <= gameData.Screen.GetUpperBound(1); j++)
                  //  if (gameData.Screen[i, j])
                    {
                        Console.SetCursorPosition(gameData.RunningRect.X + 2 * j, gameData.RunningRect.Y + i);
                        Console.Write("  ");
                    }
            //清空游戏显示下一个方块处的屏幕
            for(i = 0; i <= gameData.NextTetris.Tetris.GetUpperBound(0); i++)
                for (j = 0; j <= gameData.NextTetris.Tetris.GetUpperBound(1); j++)
                {
                    if (gameData.NextTetris.Tetris[i, j])
                    {
                        Console.SetCursorPosition(gameData.NextTetris.Coordinate.X + 2 * j, gameData.NextTetris.Coordinate.Y + i);
                        Console.Write("  ");
                    }
                }
            Console.SetCursorPosition(gameData.ShowRect.X + 8, gameData.ShowRect.Y + gameData.ShowRect.Height - 3);
            Console.Write(0);
            //清空提示文字
            Console.SetCursorPosition(gameData.RunningRect.X - 6, gameData.RunningRect.Y + gameData.RunningRect.Height + 1);
     //     Console.WriteLine("游戏结束，你的分数是：{}，输入Q退出，R重玩:");
            Console.WriteLine("                                           ");
            //重新初始化数据
            gameData.CurrentTetris.GenerateTetris(gameData.RunningRect.X + gameData.RunningRect.Width / 2 - 2, gameData.RunningRect.Y);
            gameData.NextTetris.GenerateTetris(gameData.ShowRect.X + 1, gameData.ShowRect.Y + gameData.ShowRect.Height / 2 - 2);
            gameData.InitData();
            ShowOneTetris(gameData.CurrentTetris);
            ShowOneTetris(gameData.NextTetris);
            timer.Start();

        }
        #region 初始化游戏
        /// <summary>
        /// 初始化游戏数据
        /// </summary>
        private void InitGameData()
        {
            gameData = new GameData();
            gameData.CurrentTetris = new Element();
            gameData.CurrentTetris.GenerateTetris(gameData.RunningRect.X + gameData.RunningRect.Width / 2 - 2, gameData.RunningRect.Y);
            gameData.NextTetris = new Element();
            gameData.NextTetris.GenerateTetris(gameData.ShowRect.X + 1, gameData.ShowRect.Y + gameData.ShowRect.Height / 2 - 2);
        }
        /// <summary>
        /// 初始化游戏界面
        /// </summary>
        private void InitGameUI()
        {
            #region  画出游戏界面 
            Console.SetCursorPosition(gameData.RunningRect.X - 1, gameData.RunningRect.Y - 2);
            Console.WriteLine("提示：输入Q退出，R重玩           ");
            Console.SetCursorPosition(gameData.RunningRect.X - 1, gameData.RunningRect.Y - 1);
                                     // Console.Write("*********************************");//33个*
            string Str = null;
            for (int i = 0; i < gameData.RunningRect.Width + gameData.ShowRect.Width + 3; i++)
                Str += "*";
            Console.Write(Str);
            for (int y = gameData.RunningRect.Y; y < gameData.RunningRect.Y + gameData.RunningRect.Height; y++)
            {
                Console.SetCursorPosition(gameData.RunningRect.X - 1, y);
                                       // Console.Write("*                   *          *");//第一个为20空格，第二个为10空格
                string str = "*";
                for (int i = 0; i < gameData.RunningRect.Width; i++)
                    str += " ";
                str += "*";
                for (int i = 0; i < gameData.ShowRect.Width; i++)
                    str += " ";
                str += "*";
                Console.Write(str);
            }
            Str = null;
            Console.SetCursorPosition(gameData.RunningRect.X - 1,gameData.RunningRect.Y + gameData.RunningRect.Height);
            for (int i = 0; i < gameData.RunningRect.Width + gameData.ShowRect.Width + 3; i++)
                Str += "*";
            Console.Write(Str);
                                     //Console.SetCursorPosition(20,24);
                                     //Console.Write("*********************************");//33个*
            Console.SetCursorPosition(gameData.ShowRect.X + 1, gameData.ShowRect.Y + gameData.ShowRect.Height - 3);
            Console.Write("Score：{0}", gameData.Score);
            #endregion
            ShowOneTetris(gameData.CurrentTetris);
            ShowOneTetris(gameData.NextTetris);
        }
        /// <summary>
        /// 根据4*4的数组将一个俄罗斯方块显示出来
        /// </summary>
        /// <param name="element"></param>
        private void ShowOneTetris(Element element)
        {
            for (int i = 0; i <= element.Tetris.GetUpperBound(0); i++)
                for (int j = 0; j <= element.Tetris.GetUpperBound(1); j++)
                {                                                //因为'■'占用两个字符，所以横坐标每次加2*i
                    if (element.Tetris[i, j])
                    {
                        Console.SetCursorPosition(element.Coordinate.X + 2 * j, element.Coordinate.Y + i);
                        Console.Write(Element.Square);
                    }
                }
        }
        #endregion
        /// <summary>
        /// 一个用于接受键盘输入的线程
        /// </summary>
        private void KeyDownThread()
        {
            while (true)
            {
                KeyDown(Console.ReadKey(true).Key);//触发KeyDown事件
            }
        }
        /// <summary>
        /// 用于固定一段时间就刷新游戏画面的timer事件
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object Source,ElapsedEventArgs e)
        {
            KeyDown(ConsoleKey.DownArrow);
        }
        /// <summary>
        /// 根据不同的按键，响应不同的操作
        /// </summary>
        /// <param name="key"></param>
        private void KeyDownEvent(ConsoleKey key)
        {
            int i, j;
            gameData.CurrentTetris.CopyTo(gameData.FormerElement);
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    gameData.CurrentTetris.TransformTetris();//下面代码为判断能不能翻转，不能翻转则将上次的状态拷贝回来
                    for (i = 0; i <= gameData.CurrentTetris.Tetris.GetUpperBound(0); i++)
                        for (j = 0; j <= gameData.CurrentTetris.Tetris.GetUpperBound(1); j++)
                            if (gameData.CurrentTetris.Tetris[i, j])
                            {      //                         *               * 
                                                     //     ■*             ■*
                                //有可能数组越界，情况如下■■*  转换后为 ■■■ （*表示墙壁）
                                //                          ■*               *
                                //                            *               *
                                if (   (gameData.CurrentTetris.Coordinate.Y + i - gameData.RunningRect.Y) >= GameData.Row
                                    || (gameData.CurrentTetris.Coordinate.X + j * 2 - gameData.RunningRect.X) / 2 >= GameData.Col
                                    || (gameData.CurrentTetris.Coordinate.Y + i - gameData.RunningRect.Y) < 0
                                    || (gameData.CurrentTetris.Coordinate.X + j * 2 - gameData.RunningRect.X) / 2 < 0
                                    )
                                {
                                    gameData.FormerElement.CopyTo(gameData.CurrentTetris);
                                    return;
                                }
                                else if (gameData.Screen[gameData.CurrentTetris.Coordinate.Y + i - gameData.RunningRect.Y, (gameData.CurrentTetris.Coordinate.X + j * 2 - gameData.RunningRect.X) / 2])
                                {
                                    gameData.FormerElement.CopyTo(gameData.CurrentTetris);
                                    return;
                                }
                            }
                    break;
                case ConsoleKey.DownArrow:
                    for (j = 0; j <= gameData.CurrentTetris.Tetris.GetUpperBound(1); j++)
                        for (i = gameData.CurrentTetris.Tetris.GetUpperBound(0); i >= 0; i--)
                            if (gameData.CurrentTetris.Tetris[i, j])
                            {      //如果方块已经到最底层或方块下面已有方块，则此方块停留在此处，继续下一方块的出现，同时判断一下游戏是否结束
                                if ((gameData.CurrentTetris.Coordinate.Y + i) == (gameData.RunningRect.Y + gameData.RunningRect.Height - 1)
                                    || gameData.Screen[(gameData.CurrentTetris.Coordinate.Y + i - gameData.RunningRect.Y + 1), (gameData.CurrentTetris.Coordinate.X + j * 2 - gameData.RunningRect.X) / 2]
                                   )
                                {
                                    for (j = 0; j <= gameData.CurrentTetris.Tetris.GetUpperBound(1); j++)
                                        for (i = gameData.CurrentTetris.Tetris.GetUpperBound(0); i >= 0; i--)
                                            if (gameData.CurrentTetris.Tetris[i, j])
                                                gameData.Screen[(gameData.CurrentTetris.Coordinate.Y + i - gameData.RunningRect.Y), (gameData.CurrentTetris.Coordinate.X + j * 2 - gameData.RunningRect.X) / 2] = true;
                                    //判断游戏是否结束
                                    if (IsGameOver())
                                    {
                                        timer.Stop();
                                        ShowOneTetris(gameData.CurrentTetris);//将这个方块显示出来让玩家看到，才知道游戏结束
                                        Console.SetCursorPosition(gameData.RunningRect.X - 6, gameData.RunningRect.Y + gameData.RunningRect.Height + 1);
                                        Console.WriteLine("游戏结束，你的分数是：{0}，输入Q退出，R重玩:",gameData.Score);
                                        while(true)
                                        {
                                            switch(Console.ReadKey(true).Key)
                                            {
                                                case ConsoleKey.Q:
                                                    keyDownThread.Abort();
                                                    return;
                                                case ConsoleKey.R:
                                                    ReStartGame();
                                                    return;
                                            }
                                        }
                                    }
                                    gameData.NextTetris.CopyTo(gameData.CurrentTetris);
                                    gameData.CurrentTetris.Coordinate.X = gameData.RunningRect.X + gameData.RunningRect.Width / 2 - 2;
                                    gameData.CurrentTetris.Coordinate.Y = gameData.RunningRect.Y;
                                    gameData.NextTetris.CopyTo(gameData.FormerElement);
                                    gameData.NextTetris.GenerateTetris(gameData.ShowRect.X + 1, gameData.ShowRect.Y + gameData.ShowRect.Height / 2 - 2);
                                    Paint(Type.OneTetris, gameData.NextTetris);
                                    RestructWall();
                                    return;
                                }
                                else
                                    break;

                            }
                    gameData.CurrentTetris.Coordinate.Y++;
                    break;

                case ConsoleKey.LeftArrow:
                    for (i = 0; i <= gameData.CurrentTetris.Tetris.GetUpperBound(0); i++)
                        for (j = 0; j <= gameData.CurrentTetris.Tetris.GetUpperBound(1); j++)
                            if (gameData.CurrentTetris.Tetris[i, j])
                            {
                                if ((gameData.CurrentTetris.Coordinate.X + j * 2) == gameData.RunningRect.X)
                                    return;  //这里的else if是为了防止数组越界而设的 
                                else if (gameData.Screen[(gameData.CurrentTetris.Coordinate.Y + i - gameData.RunningRect.Y), (gameData.CurrentTetris.Coordinate.X + j * 2 - gameData.RunningRect.X) / 2 - 1])
                                    return;
                                else
                                    break;
                            }
                    gameData.CurrentTetris.Coordinate.X -= 2;
                    break;
                case ConsoleKey.RightArrow:
                    for (i = 0; i <= gameData.CurrentTetris.Tetris.GetUpperBound(0); i++)
                        for (j = gameData.CurrentTetris.Tetris.GetUpperBound(1); j >= 0; j--)
                            if (gameData.CurrentTetris.Tetris[i, j])
                            {                                               //在逻辑上坐标应为X + width - 1，但由于■占两个字符宽度，所以为了匹配比较坐标，坐标变成了X + width - 2，
                                                                            //也可以把比较前面的坐标加1，而后面的坐标依然为X + width - 1
                                if ((gameData.CurrentTetris.Coordinate.X + j * 2) == (gameData.RunningRect.X + gameData.RunningRect.Width - 2))
                                    return;   //这里的else if是为了防止数组越界而设的
                                else if (gameData.Screen[(gameData.CurrentTetris.Coordinate.Y + i - gameData.RunningRect.Y), (gameData.CurrentTetris.Coordinate.X + j * 2 - gameData.RunningRect.X) / 2 + 1])
                                    return;
                                else
                                    break;
                            }
                    gameData.CurrentTetris.Coordinate.X += 2;
                    break;
                case ConsoleKey.R: timer.Stop();
                                   ReStartGame();
                                   return;
                case ConsoleKey.Q: keyDownThread.Abort();
                                   return;
                default:
                    return;
            }
            Paint(Type.OneTetris, gameData.CurrentTetris);
        }
        /// <summary>
        /// 绘制游戏画面
        /// </summary>
        /// <param name="type">绘制类型，一个方块或者整个屏幕</param>
        /// <param name="element">一个元素类，描绘一个俄罗斯方块的组成信息和相关方法</param>
        private void UIPaint(Type type, Element element)
        {
            int i,j;
            switch (type)
            {
                case Type.OneTetris:
                    for(i = 0; i <= gameData.FormerElement.Tetris.GetUpperBound(0); i++)
                        for (j = 0; j <= gameData.FormerElement.Tetris.GetUpperBound(1); j++)
                        {
                            if (gameData.FormerElement.Tetris[i, j])
                            {
                                Console.SetCursorPosition(gameData.FormerElement.Coordinate.X + 2 * j, gameData.FormerElement.Coordinate.Y + i);
                                Console.Write("  ");
                            }
                        }
                    for (i = 0; i <= element.Tetris.GetUpperBound(0); i++)
                        for (j = 0; j <= element.Tetris.GetUpperBound(1); j++)
                        {
                            if (element.Tetris[i, j])
                            {
                                Console.SetCursorPosition(element.Coordinate.X + 2 * j, element.Coordinate.Y + i);
                                Console.Write(Element.Square);
                            }
                        }
                    break;
                case Type.AllWallTetris:
                    for(i = 0; i <= gameData.Screen.GetUpperBound(0); i++)
                        for (j = 0; j <= gameData.Screen.GetUpperBound(1); j++)
                            if (gameData.Screen[i, j])
                            {
                                Console.SetCursorPosition(gameData.RunningRect.X + 2*j, gameData.RunningRect.Y + i);
                                Console.Write(Element.Square);
                            }
                    break;
                case Type.ClearAllWallTetris:
                    for(i = 0; i <= gameData.FormerScreen.GetUpperBound(0); i++)
                        for (j = 0; j <= gameData.FormerScreen.GetUpperBound(1); j++)
                            if (gameData.FormerScreen[i, j])
                            {
                                Console.SetCursorPosition(gameData.RunningRect.X + 2 * j, gameData.RunningRect.Y + i);
                                Console.Write("  ");
                            }
                    break;
            }
            Console.SetCursorPosition(gameData.ShowRect.X + 8, gameData.ShowRect.Y + gameData.ShowRect.Height - 3);
            Console.Write(gameData.Score);
        }
        /// <summary>
        /// 重构整个俄罗斯方块所组成的墙面
        /// </summary>
        private void RestructWallEvent()
        {
            int i,j,k,count;
            bool restruct = false;
            //将现在的Screen数组拷贝到FormerScreen数组
            for (i = 0; i <= gameData.Screen.GetUpperBound(0); i++)
                for (j = 0; j <= gameData.Screen.GetUpperBound(1); j++)
                    gameData.FormerScreen[i, j] = gameData.Screen[i, j];
            for (i = gameData.Screen.GetUpperBound(0); i >= 0; i--)
            {
                count = 0;
                for (j = 0; j <= gameData.Screen.GetUpperBound(1); j++)
                    if (gameData.Screen[i, j])
                        count++;
                if (count == GameData.Col)
                {
                    restruct = true;
                    gameData.Score++;//游戏分数为消去一行增加一分
                    for (j = 0; j <= gameData.Screen.GetUpperBound(1); j++)
                        gameData.Screen[i, j] = false;
                }
                else if (count == 0)
                    break;
            }
            if (!restruct)
                return;
            //将要重构，先把墙面清除
            Paint(Type.ClearAllWallTetris, null);
            //下面的方法为将所有的方块向下移动（如果能移动的话）
            for(i = GameData.Row - 1; i > 0; i--)
                if (IsEmpty(i))
                {
                    for (k = i - 1; k >= 0; k--)
                        if (!IsEmpty(k))
                            break;
                    if (k < 0)
                        break;
                    for (j = 0; j <= gameData.Screen.GetUpperBound(1); j++)
                    {
                        gameData.Screen[i, j] = gameData.Screen[k, j];
                        gameData.Screen[k, j] = false;
                    }
                }
            Paint(Type.AllWallTetris, null);
        }
        /// <summary>
        /// 判断屏幕数组的某一行是否是空
        /// </summary>
        /// <param name="row">要判断的行的索引</param>
        /// <returns>返回true表示空，false表示非空</returns>
        private bool IsEmpty(int row)
        {
            for (int j = 0; j <= gameData.Screen.GetUpperBound(1); j++)
                if (gameData.Screen[row, j])
                    return false;
            return true;
        }
        /// <summary>
        /// 判断游戏是否结束
        /// </summary>
        /// <returns></returns>
        private bool IsGameOver()
        {
            for (int j = 0; j <= gameData.Screen.GetUpperBound(1); j++)
                if (gameData.Screen[0, j])
                    return true;
            return false;
        }
        
    }
}
