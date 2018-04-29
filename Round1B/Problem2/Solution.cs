using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Solution /* Problem 2 */
{
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
