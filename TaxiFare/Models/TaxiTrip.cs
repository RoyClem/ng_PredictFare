using Microsoft.ML.Data;

namespace TaxiFare
{
    public class TaxiTrip
    {
        public string VendorId;

        public string RateCode;

        public float PassengerCount;

        public float TripTime;

        public float TripDistance;

        public string PaymentType;

        public float FareAmount;

    }

    public class TaxiTripFarePrediction
    {

        // TaxiTripFarePrediction class represents predicted results.
        // In case of the regression task the Score column contains predicted label values.

        [ColumnName("Score")]
        public float FareAmount;
    }
}