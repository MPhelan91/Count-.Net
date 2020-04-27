using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Common;

namespace MathLibrary
{
  public static class Conversions
  {
    static Unit[] volumeUnits = new Unit[] { Unit.Cups, Unit.Liters, Unit.Millilitres};
    static Unit[] massAndWeightUnits = new Unit[] { Unit.Grams, Unit.Ounces, Unit.Pounds };

    //From Key1 to Key2
    static Dictionary<Tuple<Unit, Unit>, double> _unitConversionDictionary = new Dictionary<Tuple<Unit, Unit>, double> {
      {Tuple.Create(Unit.Liters, Unit.Millilitres), 1000},
      {Tuple.Create(Unit.Millilitres, Unit.Liters), 0.001},

      {Tuple.Create(Unit.Liters, Unit.Cups), 4.22675},
      {Tuple.Create(Unit.Cups, Unit.Liters), 0.236588},

      {Tuple.Create(Unit.Cups, Unit.Millilitres), 236.588},
      {Tuple.Create(Unit.Millilitres, Unit.Cups), 0.00422675},

      {Tuple.Create(Unit.Grams, Unit.Ounces), 0.035274},
      {Tuple.Create(Unit.Ounces, Unit.Grams), 28.3495},

      {Tuple.Create(Unit.Grams, Unit.Pounds), 0.00220462},
      {Tuple.Create(Unit.Pounds, Unit.Grams), 453.592},

      {Tuple.Create(Unit.Pounds, Unit.Ounces), 16},
      {Tuple.Create(Unit.Ounces, Unit.Pounds), 0.0625}
    };

    public static NutritionalInfo Convert(ServingInfo servingSize, NutritionalInfo servingInfo, ServingInfo portion) {
      var ratio = servingSize.ServingUnit == portion.ServingUnit
          ? portion.Serving / servingSize.Serving
          : ConvertPortion(portion, servingSize.ServingUnit) / servingSize.Serving;

      var portionInfo = new NutritionalInfo(servingInfo.Calories * ratio, servingInfo.Protien * ratio);

      return portionInfo;
    }

    public static double ConvertPortion(ServingInfo portion, Unit servingUnit)
    {
      if((volumeUnits.Contains(portion.ServingUnit) && massAndWeightUnits.Contains(servingUnit)) ||
         (volumeUnits.Contains(servingUnit) && massAndWeightUnits.Contains(portion.ServingUnit))) {
          throw new ArgumentException("Cannot convert between Volume and Mass/Weight without a supplied density, which is not currently a feature");
      }

      var ratio = _unitConversionDictionary[Tuple.Create(portion.ServingUnit, servingUnit)];
      return ratio * portion.Serving;
    }
  }
}
