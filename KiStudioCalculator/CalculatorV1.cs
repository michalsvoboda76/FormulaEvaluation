using System;
using System.Collections.Generic;
using NReco.Linq;

namespace KiStudioCalculator
{
    public class CalculatorV1 : ICalculator
    {
        private readonly LambdaParser _lambdaParser = new LambdaParser();
        private string _formulaDefinition;
        private Dictionary<string, object> _formulaVariables = new Dictionary<string, object>();
        private int _samplingRate;
        private DecodedScan[] _signalsInputData;

        public CalculatorV1()
        {
            Init();
        }

        public void Init()
        {
            const int signalAIndex = 0;
            const int signalBIndex = 1;
            const int signalCIndex = 2;

            _formulaVariables["indexA"] = signalAIndex;
            _formulaVariables["indexB"] = signalBIndex;
            _formulaVariables["indexC"] = signalCIndex;

            _formulaVariables["A"] = (Func<int, float>)((i) => _signalsInputData[i].ChannelValues[signalAIndex]);
            _formulaVariables["B"] = (Func<int, float>)((i) => _signalsInputData[i].ChannelValues[signalBIndex]);
            _formulaVariables["C"] = (Func<int, float>)((i) => _signalsInputData[i].ChannelValues[signalCIndex]);


            _formulaVariables["SquareRoot"] = (Func<float, float>)((v) => (float)Math.Sqrt(v));
            _formulaVariables["Integrate"] = (Func<float, float>)((v) => v / _samplingRate);
            _formulaVariables["Differentiate"] = (Func<int, int, float>)((iSignal, iSample)
                => (
                       _signalsInputData[iSample].ChannelValues[iSignal]
                       - (iSample == 0 ? 0f : _signalsInputData[iSample - 1].ChannelValues[iSignal])
                   )
                   * _samplingRate);
        }

        public IEnumerable<float> Calculate(string formulaDefinition, int samplingRate, DecodedScan[] signalsInputData)
        {
            _formulaDefinition = formulaDefinition;
            _samplingRate = samplingRate;
            _signalsInputData = signalsInputData;

            for (var sampleIndex = 0; sampleIndex < _signalsInputData.Length; sampleIndex++)
            {
                _formulaVariables["i"] = sampleIndex;
                
                var resultObject = _lambdaParser.Eval(_formulaDefinition, _formulaVariables);
                var resultValue = Convert.ToSingle(resultObject);
                yield return resultValue;
            }
        }

    }
}