using System;
using System.Collections.Generic;
using System.Data;
using Common;

namespace MathLibrary
{
  public static class Conversions
  {
    //From Key1 to Key2
    static Dictionary<Tuple<Unit, Unit>, double> _unitConversionDictionary = new Dictionary<Tuple<Unit, Unit>, double> {
      {Tuple.Create(Unit.Liters, Unit.Millilitres), 0},
      {Tuple.Create(Unit.Millilitres, Unit.Liters), 0},

      {Tuple.Create(Unit.Liters, Unit.Cups), 0},
      {Tuple.Create(Unit.Cups, Unit.Liters), 0},

      {Tuple.Create(Unit.Cups, Unit.Millilitres), 0},
      {Tuple.Create(Unit.Millilitres, Unit.Cups), 0},

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

      var portionInfo = new NutritionalInfo(Math.Round(servingInfo.Calories * ratio), Math.Round(servingInfo.Protien * ratio));

      return portionInfo;
    }

    public static double ConvertPortion(ServingInfo portion, Unit servingUnit)
    {
      //If Volume to Volume or Weight to Weight but if other... do something else
      var ratio = _unitConversionDictionary[Tuple.Create(portion.ServingUnit, servingUnit)];
      return ratio * portion.Serving;
    }
  }
}
