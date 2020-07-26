using System.Linq;
using KiStudioCalculator;

namespace NRecoLinqLambdaParser
{
    public class CalculatorV1PerformanceTest : CalculatorEvaluator
    {
        public void Execute(int numberOfRuns = 1)
        {
            NumberOfRuns = numberOfRuns > 0 ? numberOfRuns: 1;
            WriteLine($"   ({numberOfRuns} runs per each formula and input data)");
            WriteLine();

            var shortData = new[]
            {
                new DecodedScan(1, new[]{1f , 10f, 100f}),
                new DecodedScan(1, new[]{2f , 20f, 200f}),
                new DecodedScan(1, new[]{3f , 30f, 300f}),
                new DecodedScan(1, new[]{4f , 40f, 400f}),
                new DecodedScan(1, new[]{5f , 50f, 500f}),
                new DecodedScan(1, new[]{6f , 60f, 600f}),
                new DecodedScan(1, new[]{7f , 70f, 700f})
            };

            var data1kSPS = Enumerable.Range(0, 1000)
                .Select(i => new DecodedScan((ulong)i, new[] { i + 1f, (i + 1) * 10f, (i + 1) * 100f })).ToArray();

            var data200kSPS = Enumerable.Range(0, 200_000)
                .Select(i => new DecodedScan((ulong)i, new[] { i + 1f, (i + 1) * 10f, (i + 1) * 100f })).ToArray();

            var data1000kSPS = Enumerable.Range(0, 1_000_000)
                .Select(i => new DecodedScan((ulong)i, new[] { i + 1f, (i + 1) * 10f, (i + 1) * 100f })).ToArray();

            var calculator = new CalculatorV1();

            WriteLine($" {"SAMPLES",7} | {"DURATION",16} | {"FORMULA",FormulaWidth} == {"RESULT VALUES",-10}");
            WriteLine();
            var formulaDefinition = "1 + 1";
            Evaluate(calculator, formulaDefinition, 10, shortData);
            Evaluate(calculator, formulaDefinition, 1000, data1kSPS);
            Evaluate(calculator, formulaDefinition, 200_000, data200kSPS);
            Evaluate(calculator, formulaDefinition, 1_000_000, data1000kSPS);
            WriteLine();
            formulaDefinition = "B(i) + A(i)";
            Evaluate(calculator, formulaDefinition, 10, shortData);
            Evaluate(calculator, formulaDefinition, 1000, data1kSPS);
            Evaluate(calculator, formulaDefinition, 200_000, data200kSPS);
            Evaluate(calculator, formulaDefinition, 1_000_000, data1000kSPS);
            WriteLine();
            formulaDefinition = "B(i) + A(i) + C(i)";
            Evaluate(calculator, formulaDefinition, 10, shortData);
            Evaluate(calculator, formulaDefinition, 1000, data1kSPS);
            Evaluate(calculator, formulaDefinition, 200_000, data200kSPS);
            Evaluate(calculator, formulaDefinition, 1_000_000, data1000kSPS);
            WriteLine();
            formulaDefinition = "SquareRoot(A(i)*A(i) + B(i)*B(i))";
            Evaluate(calculator, formulaDefinition, 10, shortData);
            Evaluate(calculator, formulaDefinition, 1000, data1kSPS);
            Evaluate(calculator, formulaDefinition, 200_000, data200kSPS);
            Evaluate(calculator, formulaDefinition, 1_000_000, data1000kSPS);
            WriteLine();
            formulaDefinition = "Differentiate(indexC, i)";
            Evaluate(calculator, formulaDefinition, 10, shortData);
            Evaluate(calculator, formulaDefinition, 1000, data1kSPS);
            Evaluate(calculator, formulaDefinition, 200_000, data200kSPS);
            Evaluate(calculator, formulaDefinition, 1_000_000, data1000kSPS);
            WriteLine();
            formulaDefinition = "Integrate(C(i))";
            Evaluate(calculator, formulaDefinition, 10, shortData);
            Evaluate(calculator, formulaDefinition, 1000, data1kSPS);
            Evaluate(calculator, formulaDefinition, 200_000, data200kSPS);
            Evaluate(calculator, formulaDefinition, 1_000_000, data1000kSPS);
            WriteLine("-------------------------------");
            WriteLine($"{"SUM:",10} {TotalDuration} ({TotalDuration.TotalMilliseconds} ms)");

        }
    }
}