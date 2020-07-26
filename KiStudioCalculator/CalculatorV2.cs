using System;
using System.Collections.Generic;
using NReco.Linq;

namespace KiStudioCalculator
{
    public class CalculatorV2 : ICalculator
    {
        private readonly LambdaParser _lambdaParser = new LambdaParser();
        private string _formulaDefinition;
        private Dictionary<string, object> _formulaVariables = new Dictionary<string, object>();
        private int _samplingRate;
        private DecodedScan[] _signalsInputData;

        private int _currentSampleIndex = -1;

        public CalculatorV2()
        {
            Init();
        }


        public void Init()
        {
            const int signalAIndex = 0;
            const int signalBIndex = 1;
            const int signalCIndex = 2;
            
            _formulaVariables["A"] = signalAIndex;
            _formulaVariables["B"] = signalBIndex;
            _formulaVariables["C"] = signalCIndex;

            _formulaVariables["sig"] = (Func<int, float>)((indexOfSignal) => _signalsInputData[_currentSampleIndex].ChannelValues[indexOfSignal]);
            
            _formulaVariables["sqrt"] = (Func<float, float>)((v) => (float)Math.Sqrt(v));
            _formulaVariables["integrate"] = (Func<float, float>)((v) => v / _samplingRate);
            _formulaVariables["sigIntegrate"] = (Func<int, float>)((indexOfSignal) => _signalsInputData[_currentSampleIndex].ChannelValues[indexOfSignal] / _samplingRate);
            _formulaVariables["sigDifferentiate"] = (Func<int, float>)((indexOfSignal)
                => (
                       _signalsInputData[_currentSampleIndex].ChannelValues[indexOfSignal]
                       - (_currentSampleIndex == 0 ? 0f : _signalsInputData[_currentSampleIndex - 1].ChannelValues[indexOfSignal])
                   )
                   * _samplingRate);
        }

        public IEnumerable<float> Calculate(string formulaDefinition, int samplingRate, DecodedScan[] signalsInputData)
        {
            _formulaDefinition = formulaDefinition;
            _samplingRate = samplingRate;
            _signalsInputData = signalsInputData;
            _currentSampleIndex = -1;

            for (var sampleIndex = 0; sampleIndex < _signalsInputData.Length; sampleIndex++)
            {
                _currentSampleIndex = sampleIndex;
                
                var resultObject = _lambdaParser.Eval(_formulaDefinition, _formulaVariables);
                var resultValue = Convert.ToSingle(resultObject);
                yield return resultValue;
            }
        }

    }
}