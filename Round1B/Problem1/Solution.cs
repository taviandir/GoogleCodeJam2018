using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Solution /* Problem 1 */
{
    public static void Main(string[] args)
    {
        string testCasesInput = Console.ReadLine();
        int testCases = int.Parse(testCasesInput);

        for (int i = 0; i < testCases; i++)
        {
            string parametersInput = Console.ReadLine();
            string[] parametersSplit = parametersInput.Split(' ');

            int n = int.Parse(parametersSplit[0]);

            string surveyResults = Console.ReadLine();
            var resultSplit = surveyResults.Split(' ');
            var survey = resultSplit.Select(int.Parse).ToList();
            int result = Algorithm(n, survey);
            Console.WriteLine($"Case #{i+1}: {result}");
        }
    }

    private static int Algorithm(int n, List<int> survey)
    {
        // is the total number fully dividable by 100? 
        if (100 % n == 0) return 100;

        //var partPerSurvey = (100.0 / n) % 1.0;
        var count = survey.Sum(i => i);

        List<List<int>> possibilities = new List<List<int>>()
        {
            survey
        };
        for (int i = count; i < n; i++)
        {
            var iterationVariations = new List<List<int>>();

            foreach (var it in possibilities)
            {
                for (int t = 0; t < it.Count; t++)
                {
                    var copy = it.ToList();
                    copy[t] += 1;
                    // TODO : dont add duplicates
                    iterationVariations.Add(copy);
                }

                // add one variation that adds a new "language" as answer
                var copyForAdd = it.ToList();
                copyForAdd.Add(1);
                iterationVariations.Add(copyForAdd);
            }

            possibilities = null;
            GC.Collect();
            possibilities = iterationVariations;
        }

        return possibilities.Select(p => CalculateScore(p, n)).Max();
    }

    private static int GetPercentage(int total, int answerCount)
    {
        var per = 100.000 / total;
        //var p = (answers * 1.0) / total;
        var p = per * answerCount;
        return Convert.ToInt32(p % 1 >= 0.5 ? Math.Ceiling(p) : Math.Floor(p));
    }

    private static int CalculateScore(List<int> survey, int total)
    {
        return survey.Sum(i => GetPercentage(total, i));
    }
}

