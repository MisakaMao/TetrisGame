using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TetrisGame_VS2008
{
    public struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X;
        public int Y;
    }
    /// <summary>
    /// 元素类，描绘一个俄罗斯方块的组成信息和相关方法
    /// </summary>
    public class Element
    {
        #region 属性
        /// <summary>
        /// 构成俄罗斯方块的基本元素
        /// </summary>
        public static char Square = '■';
        /// <summary>
        /// 一个4*4的数组，描述了一个俄罗斯方块的构成
        /// </summary>
        public bool[,] Tetris = new bool[4,4];
        /// <summary>
        /// 4*4数组的坐标，借此得到一个俄罗斯方块的坐标
        /// </summary>
        public Point Coordinate;
        /// <summary>
        /// 描述一个俄罗斯方块的类型，有7类，分别编号为1--7
        /// </summary>
        public int Type;
        //俄罗斯方块类型如下：
        //■■   ■                ■ ■       ■■   ■■
        //■■ ■■■ ■■■■ ■■■ ■■■ ■■       ■■
        // 1     2       3       4      5      6         7
        #endregion

        /// <summary>
        /// 形成一个俄罗斯方块
        /// </summary>
        /// <param name="x">方块的起始横坐标</param>
        /// <param name="y">方块的起始纵坐标</param>
        public void GenerateTetris(int x, int y)
        {
            int i,j;
            for (i = 0; i <= this.Tetris.GetUpperBound(0); i++)
                for (j = 0; j <= this.Tetris.GetUpperBound(1); j++)
                    this.Tetris[i, j] = false;
            Random random = new Random();
            Type = random.Next(1, 8);
            switch (Type)
            {
                case 1:
                    this.Tetris[0, 0] = true;
                    this.Tetris[0, 1] = true;
                    this.Tetris[1, 0] = true;
                    this.Tetris[1, 1] = true;
                    break;
                case 2:
                    this.Tetris[0, 1] = true;
                    this.Tetris[1, 0] = true;
                    this.Tetris[1, 1] = true;
                    this.Tetris[1, 2] = true;
                    break;
                case 3:
                    this.Tetris[0, 0] = true;
                    this.Tetris[0, 1] = true;
                    this.Tetris[0, 2] = true;
                    this.Tetris[0, 3] = true;
                    break;
                case 4:
                    this.Tetris[1, 0] = true;
                    this.Tetris[1, 1] = true;
                    this.Tetris[1, 2] = true;
                    this.Tetris[0, 2] = true;
                    break;
                case 5:
                    this.Tetris[0, 0] = true;
                    this.Tetris[1, 0] = true;
                    this.Tetris[1, 1] = true;
                    this.Tetris[1, 2] = true;
                    break;
                case 6:
                    this.Tetris[0, 1] = true;
                    this.Tetris[0, 2] = true;
                    this.Tetris[1, 0] = true;
                    this.Tetris[1, 1] = true;
                    break;
                case 7:
                    this.Tetris[0, 0] = true;
                    this.Tetris[0, 1] = true;
                    this.Tetris[1, 1] = true;
                    this.Tetris[1, 2] = true;
                    break;
            }
            this.Coordinate = new Point(x, y);
        }
        /// <summary>
        /// 翻转一个俄罗斯方块
        /// </summary>
        public void TransformTetris()
        {
            switch (Type)
            {
                    //从2开始，因为类型1 “田”不需要翻转
                case 2:
                    if (!this.Tetris[2, 1])
                    {
                        this.Tetris[2, 1] = true;
                        this.Tetris[1, 0] = false;
                    }
                    else if (!this.Tetris[1, 2])
                    {
                        this.Tetris[1, 2] = true;
                        this.Tetris[2, 1] = false;
                    }
                    else if (!this.Tetris[0, 1])
                    {
                        this.Tetris[0, 1] = true;
                        this.Tetris[1, 2] = false;
                    }
                    else
                    {
                        this.Tetris[1, 0] = true;
                        this.Tetris[0, 1] = false;
                    }
                    break;
                case 3:
                    if (!this.Tetris[0, 0])
                    {
                        for (int i = 0; i < 4; i++)
                            this.Tetris[i, 1] = false;
                        for (int j = 0; j < 4; j++)
                            this.Tetris[0, j] = true;
                    }
                    else
                    {
                        for (int j = 0; j < 4; j++)
                            this.Tetris[0, j] = false;
                        for (int i = 0; i < 4; i++)
                            this.Tetris[i, 1] = true;
                    }
                    break;
                case 4:
                    if (this.Tetris[0, 2])
                    {
                        this.Tetris[1, 0] = false;
                        this.Tetris[1, 2] = false;
                        this.Tetris[0, 2] = false;
                        this.Tetris[0, 0] = true;
                        this.Tetris[0, 1] = true;
                        this.Tetris[2, 1] = true;
                    }
                    else if (this.Tetris[0, 0])
                    {
                        this.Tetris[0, 0] = false;
                        this.Tetris[0, 1] = false;
                        this.Tetris[2, 1] = false;
                        this.Tetris[2, 0] = true;
                        this.Tetris[1, 0] = true;
                        this.Tetris[1, 2] = true;
                    }
                    else if (this.Tetris[2, 0])
                    {
                        this.Tetris[2, 0] = false;
                        this.Tetris[1, 0] = false;
                        this.Tetris[1, 2] = false;
                        this.Tetris[0, 1] = true;
                        this.Tetris[2, 1] = true;
                        this.Tetris[2, 2] = true;
                    }
                    else
                    {
                        this.Tetris[0, 1] = false;
                        this.Tetris[2, 1] = false;
                        this.Tetris[2, 2] = false;
                        this.Tetris[1, 0] = true;
                        this.Tetris[1, 2] = true;
                        this.Tetris[0, 2] = true;
                    }
                    break;
                case 5:
                    if (this.Tetris[0, 0])
                    {
                        this.Tetris[0, 0] = false;
                        this.Tetris[1, 0] = false;
                        this.Tetris[1, 2] = false;
                        this.Tetris[2, 0] = true;
                        this.Tetris[2, 1] = true;
                        this.Tetris[0, 1] = true;
                    }
                    else if (this.Tetris[2, 0])
                    {
                        this.Tetris[2, 0] = false;
                        this.Tetris[2, 1] = false;
                        this.Tetris[0, 1] = false;
                        this.Tetris[1, 0] = true;
                        this.Tetris[1, 2] = true;
                        this.Tetris[2, 2] = true;
                    }
                    else if (this.Tetris[2, 2])
                    {
                        this.Tetris[1, 0] = false;
                        this.Tetris[1, 2] = false;
                        this.Tetris[2, 2] = false;
                        this.Tetris[2, 1] = true;
                        this.Tetris[0, 1] = true;
                        this.Tetris[0, 2] = true;
                    }
                    else
                    {
                        this.Tetris[0, 2] = false;
                        this.Tetris[0, 1] = false;
                        this.Tetris[2, 1] = false;
                        this.Tetris[0, 0] = true;
                        this.Tetris[1, 0] = true;
                        this.Tetris[1, 2] = true;
                    }
                    break;
                case 6:
                    if (this.Tetris[0, 2])
                    {
                        this.Tetris[0, 2] = false;
                        this.Tetris[0, 1] = false;
                        this.Tetris[0, 0] = true;
                        this.Tetris[2, 1] = true;

                    }
                    else
                    {
                        this.Tetris[0, 2] = true;
                        this.Tetris[0, 1] = true;
                        this.Tetris[0, 0] = false;
                        this.Tetris[2, 1] = false;
                    }
                    break;
                case 7:
                    if(this.Tetris[0, 0])
                    {
                        this.Tetris[0, 0] = false;
                        this.Tetris[1, 2] = false;
                        this.Tetris[1, 0] = true;
                        this.Tetris[2, 0] = true;
                    }
                    else
                    {
                        this.Tetris[0, 0] = true;
                        this.Tetris[1, 2] = true;
                        this.Tetris[1, 0] = false;
                        this.Tetris[2, 0] = false;
                    }
                    break;
            }
        }
        /// <summary>
        /// 将当前Element类拷贝到目标Element类
        /// </summary>
        /// <param name="element">拷贝的目标Element类</param>
        public void CopyTo(Element element)
        {
            int i, j;
            for (i = 0; i <= this.Tetris.GetUpperBound(0); i++)
                for (j = 0; j <= this.Tetris.GetUpperBound(1); j++)
                    element.Tetris[i, j] = this.Tetris[i, j];
            element.Coordinate = this.Coordinate;
            element.Type = this.Type;
        }
    }
}
