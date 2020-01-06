using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TetrisGame_VS2008
{
    public struct Rect
    {
        public Rect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        public int X;
        public int Y;
        public int Width;
        public int Height;
    }
    /// <summary>
    /// 游戏数据类，保存相关的游戏数据和状态
    /// </summary>
    public class GameData
    {
        public GameData()
        {
            //GameState = false;
            Score = 0;
          //  Count = 0;
            for (int i = 0; i < Row; i++)
                for (int j = 0; j < Col; j++)
                    Screen[i, j] = false;
        }
        #region 属性
        /// <summary>
        /// 获取或设置当前的分数
        /// </summary>
        public int Score { get; set; }
        private Rect runningRect = new Rect(20, 5, 20, 20);
        /// <summary>
        /// 当前运行游戏的画面矩形大小及坐标
        /// </summary>
        public Rect RunningRect 
        {
            get { return runningRect; } 
        }
        private Rect showRect = new Rect(41, 5, 10, 20);
        /// <summary>
        /// 显示下一个方块的画面矩形大小及坐标
        /// </summary>
        public Rect ShowRect
        {
            get { return showRect; }
        }
        /// <summary>
        /// 获取游戏中容纳方块元素（■）的行数，即游戏画面大小的行数
        /// </summary>
        public static int Row = 20;
        /// <summary>
        /// 获取游戏中容纳方块元素（■）的列数，即游戏画面大小的列数
        /// </summary>
        public static int Col = 10;
        /// <summary>
        /// 二维数组，记录整个游戏屏幕的方块元素（■）的位置以及有无，false为无，true为有
        /// </summary>
        public bool[,] Screen = new bool[Row,Col];
        /// <summary>
        /// 记录上一次屏幕的方块元素信息
        /// </summary>
        public bool[,] FormerScreen = new bool[Row, Col];
        /// <summary>
        /// 当前在屏幕上运动的一个俄罗斯方块
        /// </summary>
        public Element CurrentTetris;
        /// <summary>
        /// 下一个将要出现的俄罗斯方块
        /// </summary>
        public Element NextTetris;
        /// <summary>
        /// 记录一个Element类，里面包含一个俄罗斯方块上次的类型，坐标和4*4Tetris数组等信息
        /// </summary>
        public Element FormerElement = new Element();
        public void InitData()
        {
            Score = 0;
            for (int i = 0; i < Row; i++)
                for (int j = 0; j < Col; j++)
                    Screen[i, j] = false;
        }
        #endregion
    }
}
