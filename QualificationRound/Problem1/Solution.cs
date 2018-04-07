using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Solution
{
    // ref : https://codejam.withgoogle.com/2018/challenges/00000000000000cb/dashboard
    /* Saving The Universe Again
        Problem

        An alien robot is threatening the universe, using a beam that will destroy all algorithms knowledge. We have to stop it!

        Fortunately, we understand how the robot works. It starts off with a beam with a strength of 1, and it will run a program that is a series of instructions, which will be executed one at a time, in left to right order. Each instruction is of one of the following two types:

            C (for "charge"): Double the beam's strength.
            S (for "shoot"): Shoot the beam, doing damage equal to the beam's current strength.

        For example, if the robot's program is SCCSSC, the robot will do the following when the program runs:

            Shoot the beam, doing 1 damage.
            Charge the beam, doubling the beam's strength to 2.
            Charge the beam, doubling the beam's strength to 4.
            Shoot the beam, doing 4 damage.
            Shoot the beam, doing 4 damage.
            Charge the beam, increasing the beam's strength to 8.

        In that case, the program would do a total of 9 damage.

        The universe's top algorithmists have developed a shield that can withstand a maximum total of D damage. But the robot's current program might do more damage than that when it runs.

        The President of the Universe has volunteered to fly into space to hack the robot's program before the robot runs it. The only way the President can hack (without the robot noticing) is by swapping two adjacent instructions. For example, the President could hack the above program once by swapping the third and fourth instructions to make it SCSCSC. This would reduce the total damage to 7. Then, for example, the president could hack the program again to make it SCSSCC, reducing the damage to 5, and so on.

        To prevent the robot from getting too suspicious, the President does not want to hack too many times. What is this smallest possible number of hacks which will ensure that the program does no more than D total damage, if it is possible to do so? 
        */
    public static void Main(string[] args)
    {
        var t = int.Parse(Console.ReadLine());
        for (int i = 1; i <= t; ++i)
        {
            var row = Console.ReadLine();
            var split = row.Split(' ');

            var maxDamage = int.Parse(split[0]);
            var sequence = split[1].ToCharArray();
            int? result = Algorithm(maxDamage, sequence);

            string resultText = result == null ? "IMPOSSIBLE" : $"{result.Value}";
            Console.WriteLine($"Case #{i}: {resultText}");
        }
    }

    /// <summary>
    /// Calculates how many times we have to hack the robot's attack sequence for our shield to be able to block the full attack sequence.
    /// Returns NULL if it's impossible to defend.
    /// </summary>
    /// <param name="maxDamage">The max damage that the shield can sustain.</param>
    /// <param name="sequence">The attack sequence of the robot.</param>
    private static int? Algorithm(int maxDamage, char[] sequence)
    {
        // calculate initial damage
        int initialDamage = CalculateDamage(sequence);

        if (initialDamage <= maxDamage)
        {
            return 0;
        }

        // is it impossible? (more shots than max damage allowed)
        if (sequence.Count(s => s == 'S') > maxDamage)
        {
            return null;
        }

        int hacks = 1, maxAttempts = sequence.Length * 2, lastDamage = initialDamage;
        while (hacks < maxAttempts)
        {
            // We must detect which swap will have the biggest impact on the total damage of the sequence
            // This will always be the longest chain of C's
            // First, find all occurrences of S
            List<int> shotsIndex = new List<int>(sequence.Length);
            for (int i = 0; i < sequence.Length; ++i)
            {
                if (sequence[i] == 'S') shotsIndex.Add(i);
            }

            int maxDistance = 0;
            int maxDistanceIndex = 0;
            int lastShotIndex = 0;
            for (int i = 0; i < shotsIndex.Count; ++i)
            {
                int shotIndex = shotsIndex[i];
                int distance = shotIndex - lastShotIndex;
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    maxDistanceIndex = shotIndex;
                }

                lastShotIndex = shotIndex;
            }

            // reduce total damage by moving an S further to the left (i.e. earlier) in the sequence
            Hack(sequence, maxDistanceIndex);

            // calculate new damage
            int newDamage = CalculateDamage(sequence);

            // is the shield going to survive by this hack, or do we need to hack more?
            if (newDamage <= maxDamage)
            {
                return hacks;
            }

            lastDamage = newDamage;

            hacks += 1;
        }

        // couldnt manage to find a solution
        return null;
    }

    /// <summary>
    /// Hacks the robot by swapping the two adjacent indices.
    /// </summary>
    private static void Hack(char[] sequence, int index)
    {
        int index2 = index - 1;
        char x = sequence[index];
        sequence[index] = sequence[index2];
        sequence[index2] = x;
    }

    private static int CalculateDamage(char[] sequence)
    {
        const int BaseDamage = 1;
        int totalDamage = 0;
        int damageNextShot = BaseDamage;

        foreach (var t in sequence)
        {
            // Charge
            if (t == 'C')
            {
                damageNextShot *= 2;
            }
            // Shoot
            else if (t == 'S')
            {
                // apply damage
                totalDamage += damageNextShot;
            }
        }

        return totalDamage;
    }
}
