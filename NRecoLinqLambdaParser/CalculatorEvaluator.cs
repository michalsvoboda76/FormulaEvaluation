using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using KiStudioCalculator;

namespace NRecoLinqLambdaParser
{
    public class CalculatorEvaluator
    {
        public int NumberOfRuns { get; set; } = 1;

        public TimeSpan TotalDuration { get; private set; } = TimeSpan.Zero;

        protected void Evaluate(ICalculator calculator, string formulaDefinition, int samplingRate, DecodedScan[] signalsInputData)
        {
            if (NumberOfRuns < 1) throw new ArgumentOutOfRangeException(nameof(NumberOfRuns), NumberOfRuns, "Must be 1 or higher");

            var evalOutputs = new (TimeSpan duration, float[] evalResults)[NumberOfRuns];
            for (var runCounter = 0; runCounter < NumberOfRuns; runCounter++)
            {
                evalOutputs[runCounter] = EvaluateOneRun(calculator, formulaDefinition, samplingRate, signalsInputData);
            }

            var firstEvalResults = evalOutputs[0].evalResults;
            var firstDuration = evalOutputs[0].duration;

            var worstDuration = evalOutputs.Max(_ => _.duration);
            var bestDuration = evalOutputs.Min(_ => _.duration);
            var avgDurationTicks = evalOutputs.Average(_ => _.duration.Ticks);
            var avgDuration = TimeSpan.FromTicks((long)avgDurationTicks);


            const int maxNumberOfItemToOutput = 7;
            var countOfNotDisplayedItems = firstEvalResults.Length - maxNumberOfItemToOutput;
            var suffix = countOfNotDisplayedItems > 0 ? $" ... (other {countOfNotDisplayedItems} not displayed)" : "";

            var evalResultString = string.Join(" ", firstEvalResults.Take(maxNumberOfItemToOutput).Select(o => string.Format(CultureInfo.InvariantCulture, "{0, 7:0.##}", o)));
            if (NumberOfRuns > 1)
            {
                WriteLineResult(signalsInputData.Length, formulaDefinition, evalResultString + suffix);
                WriteLineStats(NumberOfRuns, worstDuration, bestDuration, avgDuration);
            }
            else
            {
                WriteLineResult(firstDuration, signalsInputData.Length, formulaDefinition, evalResultString + suffix);
            }
        }

        protected (TimeSpan duration, float[] evalResults) EvaluateOneRun(ICalculator calculator, string formulaDefinition, int samplingRate, DecodedScan[] signalsInputData)
        {
            var stopwatch = new Stopwatch();
            
            stopwatch.Start();
            var evalResults = calculator
                .Calculate(formulaDefinition, samplingRate, signalsInputData)
                .ToArray();
            stopwatch.Stop();
            
            var duration = stopwatch.Elapsed;
            TotalDuration += duration;

            
            return (duration, evalResults);
        }

        protected virtual void WriteLine(string s = "")
        {
            Console.WriteLine(s);
        }

        protected void WriteLineStats(int numberOfRuns, TimeSpan worstDuration, TimeSpan bestDuration, TimeSpan avgDuration)
        {
            WriteLine($" {"runs",7}   {numberOfRuns}x");
            WriteLine($" {"worst",7}   {worstDuration}");
            WriteLine($" {"best",7}   {bestDuration}");
            WriteLine($" {"avg",7}   {avgDuration}");
            WriteLine();
        }
        public const int FormulaWidth = 40;

        protected void WriteLineResult(int samplesCount, string formulaDefinition, string evalResultString)
        {
            
            WriteLine($" {samplesCount, 7} | -- statistics -- | {formulaDefinition,FormulaWidth} == {evalResultString,-10}");
        }
        protected void WriteLineResult(TimeSpan duration, int samplesCount, string formulaDefinition, string evalResultString)
        {
            WriteLine($" {samplesCount,7} | {duration} | {formulaDefinition,FormulaWidth} == {evalResultString,-10}");
        }

        protected void WriteLineResult(string formulaDefinition, string evalResultString)
        {
            WriteLine($"{formulaDefinition,1+7+22+FormulaWidth} == {evalResultString,-10}");
        }

        protected void WriteLineTitle(string prefix, string suffix)
        {
            WriteLine($"{prefix,FormulaWidth} {suffix,-10}");
        }
    }
}