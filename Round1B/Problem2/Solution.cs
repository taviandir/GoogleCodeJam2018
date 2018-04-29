using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Solution /* Problem 2 */
{
    /*
     Mysterious Road Signs
    Problem

    The town of Signfield is on a perfectly straight and infinitely long road running from west to east. Along that road, there is a sequence of S mysterious road signs with numbers on both sides. The i-th sign (numbered in order from west to east) is at a point Di kilometers east of Signfield, and has a number Ai on the west-facing side and a number Bi on the east-facing side.

    Nobody in Signfield knows what these signs are trying to say. You think that the numbers on the west sides of the signs are intended for drivers traveling east, and that they represent distances to some particular destination. Similarly, you think that the numbers on the east sides of the signs are for drivers traveling west, and that they represent distances to some particular destination. You suspect that not all of the signs may be consistent with this theory, though.

    To start testing your theory, you are interested in finding valid sets of signs that obey the following rules:

        The set is a contiguous subsequence of the sequence of all road signs. (The entire sequence also counts as a contiguous subsequence.)
        There exist locations M and N kilometers east of Signfield, where M and N are (not necessarily positive and not necessarily distinct) numbers, such that for every sign in that set, at least one of the following is true:
            Di + Ai = M.
            Di - Bi = N.

    What is the largest possible number of signs in a valid set as described above, and how many different valid sets of that size are there? 
     */
    public static void Main(string[] args)
    {
        var totalTests = int.Parse(Console.ReadLine());

        for (int t = 0; t < totalTests; t++)
        {
            var totalSigns = int.Parse(Console.ReadLine());
            List<Sign> signs = new List<Sign>();
            for (int s = 0; s < totalSigns; s++)
            {
                var signRow = Console.ReadLine();
                var signSplit = signRow.Split(' ');
                var sign = new Sign
                {
                    AtKm = int.Parse(signSplit[0]),
                    West = int.Parse(signSplit[1]),
                    East = int.Parse(signSplit[2]),
                };
                signs.Add(sign);
            }
        }
    }

    private static Tuple<int, int> Algorithm(List<Sign> signs)
    {
        List<List<int>> sequences = new List<List<int>>();

        for (int i = 0; i < signs.Count; i++)
        {
            var s = signs[i];
            // TODO
        }

        int longestSequence = sequences.Select(s => s.Count).Max();
        int validSequencesCount = 0; // TODO : substract duplicates!
        return new Tuple<int, int>(longestSequence, validSequencesCount);
    }

    private class Sequence
    {
        public int? M { get; set; }
        public int? N { get; set; }
        public List<int> Positions { get; set; }
    }

    private class Sign
    {
        public int AtKm { get; set; }
        public int West { get; set; }
        public int East { get; set; }

        /*
         D + A = M.
         D - B = N. 
        */
        public int M
        {
            get { return AtKm + West; }
        }

        public int N
        {
            get { return AtKm - East; }
        }
    }
}
