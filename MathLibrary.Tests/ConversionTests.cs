using Common;
using NUnit.Framework;

namespace MathLibrary.Tests
{
  public class ConversionTests
  {
    [TestCase(4 , Unit.Ounces, 110, 26,   
              12, Unit.Ounces, 330, 78)]
    [TestCase(4 , Unit.Ounces, 110, 26,   
              1.25, Unit.Pounds, 550, 130)]
    public void Convert(double servingSize, Unit servingUnit, double servingCalorie, double servingProtien,
                        double portion, Unit portionUnit, double portionCalorie, double portionProtien)
    {
      var portionInfo = Conversions.Convert(new ServingInfo(servingSize, servingUnit), new NutritionalInfo(servingCalorie, servingProtien), new ServingInfo(portion, portionUnit));
      Assert.AreEqual(portionCalorie, portionInfo.Calories);
      Assert.AreEqual(portionProtien, portionInfo.Protien);
    }
   
    [TestCase(Unit.Pounds, Unit.Ounces, 16)]
    [TestCase(Unit.Ounces, Unit.Pounds, 0.0625)]
    public void ConvertPortionToAndFrom(Unit from, Unit to, double expected) {
      var result = Conversions.ConvertPortion(new ServingInfo(1, from), to);
      Assert.AreEqual(expected, result);
    }
  }
}