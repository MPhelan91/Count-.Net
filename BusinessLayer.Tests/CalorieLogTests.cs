using Common;
using DatabaseAccessLayer;
using NUnit.Framework;
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

    [OneTimeSetUp]
    public void Setup()
    {
      var foods = new List<SavedFood>() {
        new SavedFood { FoodName = "Chicken Breast", Calories = 110, Protien = 26, ServingSize = 4, ServingUnit = Unit.Ounces},
      };

      var foodEntries = new List<FoodEntry> { 
        new FoodEntry {FoodForEntry=foods[0], Calories = 100, Protien = 2, EntryDate = DateTime.Today},
        new FoodEntry {Calories = 200.1, Protien = 1, EntryDate = DateTime.Now},
        new FoodEntry {Calories = 100.91, Protien = 3, EntryDate = DateTime.Now},
        new FoodEntry {Calories = 1000.80, Protien = 500.42, EntryDate = DateTime.Today + TimeSpan.FromDays(1)},
        new FoodEntry {Calories = 2000, Protien = 800, EntryDate = DateTime.Today - TimeSpan.FromDays(1)},
        new FoodEntry {Calories = 3000, Protien = 300, EntryDate = DateTime.Today - TimeSpan.FromDays(2)},
      };

      var meals = new List<SavedMeal>() {
        new SavedMeal { MealName = "Chipotle", Calories = 770, Protien = 4},
        new SavedMeal { MealName = "Cliff Bar", Calories = 280, Protien = 5},
        new SavedMeal { MealName = "Fuel Wrap", Calories = 650, Protien = 6},
      };

      var mealEntries = new List<MealEntry>() {
        new MealEntry {MealForEntry = meals[1], EntryDate = DateTime.Today + TimeSpan.FromDays(1)},
        new MealEntry {MealForEntry = meals[0], EntryDate = DateTime.Today},
        new MealEntry {MealForEntry = meals[1], EntryDate = DateTime.Now},
        new MealEntry {MealForEntry = meals[2], EntryDate = DateTime.Now},
        new MealEntry {MealForEntry = meals[1], EntryDate = DateTime.Today - TimeSpan.FromDays(1)},
        new MealEntry {MealForEntry = meals[2], EntryDate = DateTime.Today - TimeSpan.FromDays(3)},
      };

      inMemoryDb = Common.CreateInMemoryDatabase("Test Database",foods:foods, meals: meals, mealEntries: mealEntries, foodEntries: foodEntries);
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
    public void GetCurrentEntries() {
      var currentEntries = sut.GetCurrentEntries();

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
    public void Constructor_Fails_With_Invalid_Dates() {
      var badStartDate = DateTime.Today + TimeSpan.FromDays(1);
      var badEndDate = DateTime.Today;

      Assert.Throws<ArgumentException>(() => new CalorieLog(inMemoryDb, badStartDate, badEndDate), "The calorie log's start date must be before the end date");
    }

    [Test]
    public void GetCurrentCounts() {
      var currentCounts = sut.GetCurrentCount();

      Assert.AreEqual(21, currentCounts.Protien);
      Assert.AreEqual(2101.01, currentCounts.Calories);
    }

  }
}
