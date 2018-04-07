#define DEBUG

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Solution
{
    // ref : https://codejam.withgoogle.com/2018/challenges/00000000000000cb/dashboard/0000000000007a30
    /* Go, Gopher!
        Problem
        The Code Jam team has just purchased an orchard that is a two-dimensional matrix of cells of unprepared soil, with 1000 rows and 1000 columns. We plan to use this orchard to grow a variety of trees — AVL, binary, red-black, splay, and so on — so we need to prepare some of the cells by digging holes:

        In order to have enough trees to use for each year's tree problems, we need there to be at least A prepared cells.
        In order to care for our trees properly, the set of all prepared cells must form a single grid-aligned rectangle in which every cell within the rectangle is prepared.
        Note that the above also implies that none of the cells outside of that rectangle can be prepared. We want the orchard to look tidy!

        For example, when A=11, although the eleven prepared cells in the left figure below form a 3x4 rectangle (that is, with 3 rows and 4 columns), the cell in the center of the rectangle is not prepared. As a result, we have not yet completed preparing our orchard, since not every cell of the 3x4 rectangle is prepared. However, after just preparing the center cell, the rectangle of size at least 11 is fully filled, and the orchard is ready.

        See below for another example. In this case, A=6. Note that the middle figure prepares a cell outside the 3x2 rectangle, so although the rightmost figure prepares a rectangle of size 6, the entire set of the prepared cells does not form a rectangle (due to the extra cell on the left). As a result, the orchard is not ready.

        Digging is hard work for humans, so we have borrowed the Go gopher from the Google Go team and trained it to help us out by preparing cells. We can deploy the gopher by giving it the coordinates of a target cell in the matrix that is not along any of the borders of the matrix. However, we have not yet perfected the gopher's training, so it will choose a cell uniformly at (pseudo-)random from the 3x3 block of nine cells centered on the target cell, and then prepare the cell it has chosen. (If it chooses a cell that was already prepared, it will uselessly prepare it again.)

        We can only deploy the gopher up to 1000 times before it gets too tired to keep digging, so we need your help in figuring out how to deploy it strategically. When you deploy the gopher, you will be told which cell the gopher actually prepared, and you can take this information into account before deploying it again, if needed. Note that you do not have to declare the dimensions or location of a rectangle in advance.
     */
    public static void Main(string[] args)
    {
        // NOTE : interactive problem

        #if DEBUG
        // init file path
        string dir = Directory.GetCurrentDirectory();
        FilePath = dir + $"\\app_input_{DateTime.UtcNow.Ticks}.log";
        #endif

        var t = int.Parse(JudgeReadLine());
        bool failed = false;
        for (int i = 0; i < t; ++i)
        {
            // NOTE : defined in the problem text as A
            var minPreparedCells = int.Parse(JudgeReadLine());

            // based on minPreparedCells, calculate smallest possible rectangle that meets the threshold
            int MinLength = 3;
            int maxLength = Math.Max(MinLength, Convert.ToInt32(Math.Ceiling(Math.Sqrt(minPreparedCells))) * 2);
            int bestWidth = 1000;
            int bestHeight = 1000;
            int lowestSum = 1000 * 1000;
            for (int n = MinLength; n <= maxLength; n++)
            {
                for (int m = MinLength; m <= maxLength; m++)
                {
                    var sum = n * m;
                    if (sum >= minPreparedCells && sum < lowestSum)
                    {
                        bestWidth = n;
                        bestHeight = m;
                        lowestSum = sum;
                    }
                }
            }

            Log($"Best shape for {minPreparedCells}: {bestWidth} x {bestHeight} ({lowestSum})", LogSource.Log);

            var field = new Field(bestWidth, bestHeight);
            while (true)
            {
                // 1. write desired dig spot
                var coords = field.DetermineBestDigSite();
                Console.WriteLine($"{coords.X} {coords.Y}");

                // 2. receive feedback on actual dig spot
                string row = JudgeReadLine();
                var split = row.Split(' ');
                int x = int.Parse(split[0]);
                int y = int.Parse(split[1]);

                // 3. interpret feedback ("0 0" = Completed, "-1 -1" means fail)
                if (x == -1 && y == -1)
                {
                    failed = true;
                    break;
                }
                else if (x == 0 && y == 0)
                {
                    break;
                }

                field.MarkAsPrepared(x, y);
            }

            if (failed) break;
        }
    }

    private class Field
    {
        private List<Cell> Cells { get; }

        private int MinWidth { get; } = 1;
        private int MaxWidth { get; }

        private int MinHeight { get; } = 1;
        private int MaxHeight { get; }

        public Field(int width, int height)
        {
            MaxWidth = width;
            MaxHeight = height;
            Cells = new List<Cell>(width * height);

            // 1-based 
            for (int w = 1; w <= width; ++w)
            {
                for (int h = 1; h <= height; ++h)
                {
                    Cells.Add(new Cell(w, h));
                }
            }
        }

        public void MarkAsPrepared(int x, int y)
        {
            var match = Cells.FirstOrDefault(c => c.X == x && c.Y == y);
            if (match != null)
            {
                match.IsPrepared = true;
            }
        }

        public Coords DetermineBestDigSite()
        {
            int bestWidth = 0, bestHeight = 0, highestUnpreparedScore = 0;

            for (int x = 2; x < MaxWidth; ++x)
            {
                for (int y = 2; y < MaxHeight ; ++y)
                {
                    int minX = x - 1;
                    int maxX = x + 1;
                    int minY = y - 1;
                    int maxY = y + 1;

                    // get all from w -1 and w + 1 to h - 1 to h + 1 (a 3x3 grid)
                    // calculate which one is best
                    var score = Cells.Count(c =>
                        c.X >= minX && c.X <= maxX && 
                        c.Y >= minY && c.Y <= maxY && 
                        !c.IsPrepared);
                    if (score > highestUnpreparedScore)
                    {
                        highestUnpreparedScore = score;
                        bestWidth = x;
                        bestHeight = y;
                    }
                }
            }

            return new Coords(bestWidth, bestHeight);
        }
    }

    private class Coords
    {
        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
    }

    private class Cell
    {
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
        public bool IsPrepared { get; set; }
    }

    private static string FilePath = null;
    private static void Log(string value, LogSource source)
    {
        #if DEBUG
        string prefix = source == LogSource.Input ? "[Input] " : source == LogSource.Output ? "[Output] " : "[Log] ";
        File.AppendAllLines(FilePath, new [] { $"{prefix}{value}" });
        #endif
    }

    private enum LogSource
    {
        Input = 0,
        Output = 1,
        Log = 2
    }

    /// <summary>
    /// Reads a line from the judge.
    /// </summary>
    private static string JudgeReadLine()
    {
        string row = Console.ReadLine();
        Log(row, LogSource.Input);
        return row;
    }

    /// <summary>
    /// Writes a line to the judge.
    /// </summary>
    private static void JudgeWriteLine(string row)
    {
        Log(row, LogSource.Output);
        Console.WriteLine(row);
    }
}
