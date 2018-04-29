using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

            var result = Algorithm(signs);
            Console.WriteLine($"Case #{t + 1}: {result.Item1} {result.Item2}");
        }
    }

    private static Tuple<int, int> Algorithm(List<Sign> signs)
    {
        List<Combination> combinations = new List<Combination>();
        var allMs = signs.Select(s => s.M).Distinct().ToList();
        var allNs = signs.Select(s => s.N).Distinct().ToList();
        foreach (var oneM in allMs)
        {
            foreach (var oneN in allNs)
            {
                combinations.Add(new Combination
                {
                    M = oneM,
                    N = oneN,
                    Sequences = new List<List<int>>()
                });
            }
        }

        foreach (var combo in combinations)
        {
            List<int> currentSequence = new List<int>();
            foreach (var sign in signs)
            {
                if (combo.M == sign.M || combo.N == sign.N)
                {
                    currentSequence.Add(sign.AtKm); // AtKm or i ?
                }
                else
                {
                    combo.Sequences.Add(currentSequence);
                    currentSequence = new List<int>();
                }
            }

            // if it ended "in sequence"
            if (currentSequence.Count > 0)
            {
                combo.Sequences.Add(currentSequence);
            }
        }

        int longestSequenceCount = combinations.SelectMany(c => c.Sequences.Select(ss => ss.Count)).Max();

        var longestSequences = combinations.Where(c => c.Sequences.Any(ss => ss.Count == longestSequenceCount)).ToList();
        // TODO : substract duplicates!
        var longestSequencesSequence =
            longestSequences.SelectMany(c => c.Sequences.Where(s => s.Count == longestSequenceCount)).ToList();

        int validSequencesCount = CountDistinct(longestSequencesSequence); 
        return new Tuple<int, int>(longestSequenceCount, validSequencesCount);
    }

    private static int CountDistinct(List<List<int>> lists)
    {
        int count = 0;
        var checkedSequences = new List<List<int>>();
        foreach (var list in lists)
        {
            if (checkedSequences.Count == 0)
            {
                checkedSequences.Add(list);
                count += 1;
                continue;
            }

            bool isDuplicate = checkedSequences.Any(chkSeq =>
            {
                for (int i = 0; i < chkSeq.Count; i++)
                {
                    if (list[i] != chkSeq[i]) return false;
                }

                return true;
            });

            if (!isDuplicate)
            {
                checkedSequences.Add(list);
                count += 1;
            }
        }

        return count;
    }

    private class Combination
    {
        public int M { get; set; }
        public int N { get; set; }
        public List<List<int>> Sequences { get; set; }
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
