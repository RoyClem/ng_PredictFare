using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Core.Data;

namespace TaxiFare.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {

        static readonly string _TaxiFarePath    = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare.csv");
        static readonly string _ModelPath       = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");

        [HttpGet("[action]")]
        public JsonResult LoadData()
        {
            List<string> vendors = new List<string>();
            List<string> rates = new List<string>();
            List<string> payTypes = new List<string>();
    
            using (var reader = new StreamReader(_TaxiFarePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    if(!vendors.Contains(values[0]))
                        vendors.Add(values[0]);

                    if (!rates.Contains(values[1]))
                        rates.Add(values[1]);

                    if (!payTypes.Contains(values[5]))
                        payTypes.Add(values[5]);
                }
            }

            vendors.Sort((x,y)      => string.Compare(x, y));
            rates.Sort((x, y)       => string.Compare(x, y));
            payTypes.Sort((x, y)    => string.Compare(x, y));
            var numPass = new List<string>(new string[] { "1", "2", "3", "4", "5" });
    
            return new JsonResult(new { vendors, rates, payTypes, numPass });
        }

        [HttpPost("[action]")]
        public JsonResult GetPrediction([FromBody]TaxiTrip trip)
        {
            MLContext mlContext = new MLContext(seed: 0);
            ITransformer loadedModel;

            using (var stream = new FileStream(_ModelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                loadedModel = mlContext.Model.Load(stream);
            }

            var predictionFunction = loadedModel.CreatePredictionEngine<TaxiTrip, TaxiTripFarePrediction>(mlContext);
           
            var prediction = predictionFunction.Predict(trip);
            
            return new JsonResult(Math.Round(prediction.FareAmount, 2));
        }

    }
}
