using KiStudioCalculator;
using Xunit;
using Xunit.Abstractions;

namespace NRecoLinqLambdaParser.functionalTests
{
    public class CalculatorV1Tests : CalculatorEvaluator
    {
        private readonly ITestOutputHelper _testOutput;

        public CalculatorV1Tests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        protected override void WriteLine(string s = "")
        {
            _testOutput.WriteLine(s);
        }

        [Fact]
        public void FormulasWithSignals()
        {
            var calculator = new CalculatorV1();

            //-------------------------------------------
            var samplingRate = 10;

            WriteLine();
            WriteLine($"Formulas with signals: 3 signals (A, B, C), sampling rate = {samplingRate}");
            WriteLine();


            var signalsInputData = new[]
            {
                new DecodedScan(1, new[] {1f, 10f, 100f}),
                new DecodedScan(1, new[] {2f, 20f, 200f}),
                new DecodedScan(1, new[] {3f, 30f, 300f}),
                new DecodedScan(1, new[] {4f, 40f, 400f}),
                new DecodedScan(1, new[] {5f, 50f, 500f}),
                new DecodedScan(1, new[] {6f, 60f, 600f}),
                new DecodedScan(1, new[] {7f, 70f, 700f})
            };


            WriteLineTitle(
                "----------------------------- input signals",
                "----------------------------------------------------------");
            WriteLineResult("FORMULA", "RESULT VALUES");
            WriteLine();

            Evaluate(calculator, "A(i)", samplingRate, signalsInputData);
            Evaluate(calculator, "B(i)", samplingRate, signalsInputData);
            Evaluate(calculator, "C(i)", samplingRate, signalsInputData);
            WriteLine();
            WriteLineTitle(
                "------------------------- simple operations",
                "----------------------------------------------------------");
            WriteLineResult("FORMULA", "RESULT VALUES");
            WriteLine();

            Evaluate(calculator, "C(i) + 7", samplingRate, signalsInputData);
            Evaluate(calculator, "SquareRoot(C(i))", samplingRate, signalsInputData);
            Evaluate(calculator, "B(i) + A(i)", samplingRate, signalsInputData);
            Evaluate(calculator, "B(i) - A(i)", samplingRate, signalsInputData);
            Evaluate(calculator, "B(i) * A(i)", samplingRate, signalsInputData);
            Evaluate(calculator, "B(i) / A(i)", samplingRate, signalsInputData);
            Evaluate(calculator, "B(i) / A(i) +1", samplingRate, signalsInputData);
            Evaluate(calculator, "B(i) / (A(i) +1)", samplingRate, signalsInputData);
            WriteLine();
            WriteLineTitle(
                "--------------------- integration operation",
                "-------------------------------------------------------");
            WriteLineResult("FORMULA", "RESULT VALUES");
            WriteLine();

            Evaluate(calculator, "C(i)", samplingRate, signalsInputData);
            Evaluate(calculator, "Integrate(C(i))", samplingRate, signalsInputData);
            Evaluate(calculator, "Integrate(C(i)*10)", samplingRate, signalsInputData);
            Evaluate(calculator, "111 - Integrate(C(i))", samplingRate, signalsInputData);
            WriteLine();

            WriteLineTitle(
                "-------------------differentiate operations",
                "-------------------------------------------------------");
            WriteLineResult("FORMULA", "RESULT VALUES");
            WriteLine();

            Evaluate(calculator, "C(i)", samplingRate, signalsInputData);
            Evaluate(calculator, "Differentiate(indexC, i)", samplingRate, signalsInputData);
            Evaluate(calculator, "111 - Differentiate(indexC, i)", samplingRate, signalsInputData);
        }
    }
}