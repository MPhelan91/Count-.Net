using Common;
using NUnit.Framework;
using System;

namespace MathLibrary.Tests
{
  public class ConversionTests
  {
    double _precisionDelta = 0.01;

    [TestCase(4 , Unit.Ounces, 110, 26,   
              12, Unit.Ounces, 330, 78)]
    [TestCase(4 , Unit.Ounces, 110, 26,   
              1.25, Unit.Pounds, 550, 130)]
    [TestCase(0.75 , Unit.Pounds, 100, 5,   
              400, Unit.Grams, 117.58, 5.88)]
    [TestCase(1 , Unit.Liters, 30, 3,   
              2500, Unit.Millilitres, 75, 7.5)]
    [TestCase(1 , Unit.Cups, 30, 3,   
              1, Unit.Liters, 126.80, 12.68)]
    [TestCase(1 , Unit.Cups, 30, 3,   
              1000, Unit.Millilitres, 126.80, 12.68)]
    public void Convert(double fromSize, Unit fromUnit, double fromCalorie, double fromProtien,
                        double toSize, Unit toUnit, double toCalorie, double toProtien) {
      var fromServing = new ServingInfo(fromSize, fromUnit);
      var fromInfo = new NutritionalInfo(fromCalorie, fromProtien);
      var toServing = new ServingInfo(toSize, toUnit);

      var toInfo = Conversions.Convert(fromServing, fromInfo, toServing);
      Assert.AreEqual(toCalorie, toInfo.Calories, _precisionDelta);
      Assert.AreEqual(toProtien, toInfo.Protien, _precisionDelta);

      var fromInfo2 = Conversions.Convert(toServing, toInfo, fromServing);
      Assert.AreEqual(fromInfo.Calories, fromInfo2.Calories, _precisionDelta);
      Assert.AreEqual(fromInfo.Protien, fromInfo2.Protien, _precisionDelta);
    }
   
    [TestCase(Unit.Pounds, Unit.Ounces, 16)]
    [TestCase(Unit.Ounces, Unit.Pounds, 0.0625)]
    public void ConvertPortionToAndFrom(Unit from, Unit to, double expected) {
      var result = Conversions.ConvertPortion(new ServingInfo(1, from), to);
      Assert.AreEqual(expected, result);
    }

    [TestCase(Unit.Pounds, Unit.Cups)]
    [TestCase(Unit.Pounds, Unit.Liters)]
    [TestCase(Unit.Pounds, Unit.Millilitres)]
    [TestCase(Unit.Grams, Unit.Cups)]
    [TestCase(Unit.Grams, Unit.Liters)]
    [TestCase(Unit.Grams, Unit.Millilitres)]
    [TestCase(Unit.Ounces, Unit.Cups)]
    [TestCase(Unit.Ounces, Unit.Liters)]
    [TestCase(Unit.Ounces, Unit.Millilitres)]
    public void ConvertPortionToAndFrom_Exception(Unit massOrWeight, Unit volume) {
      Assert.Throws<ArgumentException>(() => Conversions.ConvertPortion(new ServingInfo(1, massOrWeight),volume), 
        "Cannot convert between Volume and Mass/Weight without a supplied density, which is not currently a feature");
      Assert.Throws<ArgumentException>(() => Conversions.ConvertPortion(new ServingInfo(1, volume), massOrWeight), 
        "Cannot convert between Volume and Mass/Weight without a supplied density, which is not currently a feature");
    }
  }
}