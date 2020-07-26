using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace KiStudioCalculator
{
    public class CalculatorApi
    {
        private static readonly CalculatorV3 Calculator = new CalculatorV3();

        private static readonly DecodedScan[] SampleSignalData = {
            new DecodedScan(1, new[]{1f, 10f, 100f, 1000f, 0.1f, 0.01f, 0.001f}),
            new DecodedScan(1, new[]{2f, 20f, 200f, 2000f, 0.2f, 0.02f, 0.002f}),
            new DecodedScan(1, new[]{3f, 30f, 300f, 3000f, 0.3f, 0.03f, 0.003f}),
            new DecodedScan(1, new[]{4f, 40f, 400f, 4000f, 0.4f, 0.04f, 0.004f}),
            new DecodedScan(1, new[]{5f, 50f, 500f, 5000f, 0.5f, 0.05f, 0.005f}),
            new DecodedScan(1, new[]{6f, 60f, 600f, 6000f, 0.6f, 0.06f, 0.006f}),
            new DecodedScan(1, new[]{7f, 70f, 700f, 7000f, 0.7f, 0.07f, 0.007f})
        };

        static CalculatorApi()
        {

            /*
            Calculator.SetSignalFormulaVariables(new []
            {
                new SignalFormulaVariable {Index = 0, Name = "A"},
                new SignalFormulaVariable {Index = 1, Name = "B"},
                new SignalFormulaVariable {Index = 2, Name = "C"},
                new SignalFormulaVariable {Index = 3, Name = "D"},
                new SignalFormulaVariable {Index = 4, Name = "E"},
                new SignalFormulaVariable {Index = 5, Name = "F"},
                new SignalFormulaVariable {Index = 6, Name = "G"}

            });*/
        }

        public static void SetSignals(string jsonString)
        {
            Console.WriteLine($"SetSignals({jsonString})");
            try
            {
                var signals = JArray.Parse(jsonString);
                var signalVariables = signals.Select(item =>
                {
                    dynamic signal = item;
                    return new SignalFormulaVariable { Index = signal.Index, Name = signal.Name };
                }).ToArray();
                Console.WriteLine($"SetSignals: signalVariables.Length={signalVariables.Length}");

                Calculator.SetSignalFormulaVariables(signalVariables);
            }
            catch (Exception e)
            {
                Console.WriteLine($"SetSignals failed: {e}");
                throw;
            }
            
        }

        public static string Calculate(string formula)
        {
            try
            {
                var result = Calculator.Calculate(formula, 10, SampleSignalData).ToArray();
                var resultString = string.Join(", ",
                    result.Select(r => string.Format("{0, 7:0.#####}", r.ChannelValues[0])));
                return resultString;
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                return $"ERR: {e.Message}";
            }
        }
    }
}