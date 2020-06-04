using Common;
using DatabaseAccessLayer;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer.Tests
{
  [TestFixture]
  public class CalorieLogTests {
    CalorieLog sut;
    DatabaseContext inMemoryDb;
    SavedFood[] foods;
    SavedMeal[] meals;
    FoodEntry[] foodEntries;
    MealEntry[] mealEntries;

    [SetUp]
    public void Setup()
    {
      foods = new SavedFood[] {
        new SavedFood { FoodName = "Chicken Breast", Calories = 110, Protien = 26, ServingSize = 4, ServingUnit = Unit.Ounces},
      };

      foodEntries = new FoodEntry[] { 
        new FoodEntry {FoodForEntry=foods[0], Calories = 100, Protien = 2, EntryDate = DateTime.Today},
        new FoodEntry {Calories = 200.1, Protien = 1, EntryDate = DateTime.Now},
        new FoodEntry {Calories = 100.91, Protien = 3, EntryDate = DateTime.Now},
        new FoodEntry {Calories = 1000.80, Protien = 500.42, EntryDate = DateTime.Today + TimeSpan.FromDays(1)},
        new FoodEntry {Calories = 2000, Protien = 800, EntryDate = DateTime.Today - TimeSpan.FromDays(1)},
        new FoodEntry {Calories = 3000, Protien = 300, EntryDate = DateTime.Today - TimeSpan.FromDays(2)},
      };

      meals = new SavedMeal[] {
        new SavedMeal { MealName = "Chipotle", Calories = 770, Protien = 4},
        new SavedMeal { MealName = "Cliff Bar", Calories = 280, Protien = 5},
        new SavedMeal { MealName = "Fuel Wrap", Calories = 650, Protien = 6},
      };

      mealEntries = new MealEntry[] {
        new MealEntry {MealForEntry = meals[1], EntryDate = DateTime.Today + TimeSpan.FromDays(1)},
        new MealEntry {MealForEntry = meals[0], EntryDate = DateTime.Today},
        new MealEntry {MealForEntry = meals[1], EntryDate = DateTime.Now},
        new MealEntry {MealForEntry = meals[2], EntryDate = DateTime.Now},
        new MealEntry {MealForEntry = meals[1], EntryDate = DateTime.Today - TimeSpan.FromDays(1)},
        new MealEntry {MealForEntry = meals[2], EntryDate = DateTime.Today - TimeSpan.FromDays(3)},
      };

      inMemoryDb = Common.CreateInMemoryDatabase("Calorie Log Database",foods:foods, meals: meals, mealEntries: mealEntries, foodEntries: foodEntries);
      sut = new CalorieLog(inMemoryDb);
    }

    private class EntryComparer : Comparer<CalorieEntry>
    {
        public override int Compare(CalorieEntry x, CalorieEntry y)
        {
            return x.Name.CompareTo(y.Name) + x.Calories.CompareTo(y.Calories) + x.Protien.CompareTo(y.Protien);
        }
    }
    [Test]
    public void GetCurrentOnEmptyDatabase()
    { 
      var testDb = Common.CreateInMemoryDatabase("Empty DB Test");

      var sutForEmptyDbTest = new CalorieLog(testDb);

      var info = sutForEmptyDbTest.GetCount(DateTime.Today);
      var entries = sutForEmptyDbTest.GetEntries(DateTime.Today);

      Assert.IsTrue(entries.Length == 0);
      Assert.AreEqual(0, info.Calories);
      Assert.AreEqual(0, info.Protien);
    }

    [Test]
    public void GetYesterdaysEntries() {
      var currentEntries = sut.GetEntries(DateTime.Today - TimeSpan.FromDays(1));

      var expectedEntries = new CalorieEntry[] {
        new CalorieEntry{Name="Manual Entry", Calories=2000, Protien=800},
        new CalorieEntry{Name="Cliff Bar", Calories=280, Protien=5},
      };

      Assert.That(currentEntries, Is.EquivalentTo(expectedEntries).Using<CalorieEntry>(new EntryComparer()));
      Assert.That(currentEntries, Is.Ordered.Descending.By("EntryDate"));
    }

    [Test]
    public void GetYesterdaysCounts() {
      var currentCounts = sut.GetCount(DateTime.Today - TimeSpan.FromDays(1));

      Assert.AreEqual(805, currentCounts.Protien);
      Assert.AreEqual(2280, currentCounts.Calories);
    }

    [Test]
    public void GetCurrentEntries() {
      var currentEntries = sut.GetEntries(DateTime.Today);

      var expectedEntries = new CalorieEntry[] {
        new CalorieEntry{Name="Chicken Breast", Calories=100, Protien=2},
        new CalorieEntry{Name="Manual Entry", Calories=200.1, Protien=1},
        new CalorieEntry{Name="Manual Entry", Calories=100.91, Protien=3},
        new CalorieEntry{Name="Chipotle", Calories=770, Protien=4},
        new CalorieEntry{Name="Cliff Bar", Calories=280, Protien=5},
        new CalorieEntry{Name="Fuel Wrap", Calories=650, Protien=6},
      };

      Assert.That(currentEntries, Is.EquivalentTo(expectedEntries).Using<CalorieEntry>(new EntryComparer()));
      Assert.That(currentEntries, Is.Ordered.Descending.By("EntryDate"));
    }

    [Test]
    public void GetCurrentCounts() {
      var currentCounts = sut.GetCount(DateTime.Today);

      Assert.AreEqual(21, currentCounts.Protien);
      Assert.AreEqual(2101.01, currentCounts.Calories);
    }

    [Test]
    public void AddEntries_Succesful()
    {
      sut.AddFoodEntry(foods[0].Id, new NutritionalInfo(3, 3));
      sut.AddMealEntry(meals[0].Id);
      sut.AddManualEntry(new NutritionalInfo(3, 3));

      var currentEntries = sut.GetEntries(DateTime.Today);
      Assert.IsTrue(currentEntries.Any(o => o.Name.Equals("Manual Entry") && o.Calories == 3 && o.Protien == 3));
      Assert.IsTrue(currentEntries.Any(o => o.Name.Equals("Chicken Breast") && o.Calories == 3 && o.Protien == 3));
      Assert.AreEqual(currentEntries.Count(o => o.Name.Equals("Chipotle") && o.Calories == 770 && o.Protien == 4), 2);
    }


    [Test]
    public void AddRemoveEntries_Successful()
    {
      foreach (var f in foodEntries)
      {
        sut.RemoveFoodEntry(f.Id);
      }
      foreach (var m in mealEntries)
      {
        sut.RemoveMealEntry(m.Id);
      }
      var currentEntries = sut.GetEntries(DateTime.Today);
      CollectionAssert.IsEmpty(currentEntries);
    }

    [Test]
    public void Validate_Throws_Exception() {
      Assert.Throws<ArgumentException>(() => sut.ValidateEntry(new NutritionalInfo(0,0)), "Calorie for entry must be greater than 0");
    }
    [Test]
    public void Remove_Fails_With_Invalid_Ids() {
      Assert.Throws<ArgumentException>(() => sut.RemoveFoodEntry(1234), "No FoodEntry exists with id 1234");
      Assert.Throws<ArgumentException>(() => sut.RemoveMealEntry(1234), "No MealEntry exists with id 1234");
    }
  }
}
