using DatabaseAccessLayer;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections;
using Common;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Tests
{
  [TestFixture]
  public class DictionaryTests
  {
    CountDictionary sut;
    DatabaseContext inMemoryDb;

    [OneTimeSetUp]
    public void Setup()
    {
      var foods = new List<SavedFood>() {
        new SavedFood { FoodName = "ExistingFood", Calories = 100, Protien = 5, ServingSize = 30, ServingUnit = Unit.Ounces },
      };
      var meals = new List<SavedMeal>() {
        new SavedMeal { MealName = "ExistingMeal", Calories = 100, Protien = 5},
      };

      inMemoryDb = Common.CreateInMemoryDatabase("Test Database", foods, meals);
      sut = new CountDictionary(inMemoryDb);
    }

    #region Constructor
    [Test]
    public void Dictionary_Constructor() {
      var foods = new List<SavedFood>() {
        new SavedFood { FoodName = "ExistingFood", Calories = 100, Protien = 5, ServingSize = 30, ServingUnit = Unit.Ounces },
        new SavedFood { FoodName = "ExistingFood2", Calories = 100, Protien = 5, ServingSize = 30, ServingUnit = Unit.Ounces },
      };
      var meals = new List<SavedMeal>() {
        new SavedMeal { MealName = "ExistingMeal", Calories = 100, Protien = 5},
        new SavedMeal { MealName = "ExistingMeal2", Calories = 100, Protien = 5},
      };

      var testDb = Common.CreateInMemoryDatabase("Constructor Test", foods, meals);

      var sutForConstrutcorTest = new CountDictionary(testDb);

      CollectionAssert.AreEquivalent(foods, sutForConstrutcorTest.GetFoods());
      CollectionAssert.AreEquivalent(meals, sutForConstrutcorTest.GetMeals());
    }
    #endregion

    [TestCase("ExistingFood", "Food Name must be unique")]
    [TestCase("","Food Name cannot be null or whitespace")]
    [TestCase(" ","Food Name cannot be null or whitespace")]
    [TestCase(null,"Food Name cannot be null or whitespace")]
    public void AddFood_FailsWithNameException(string name, string exceptionMessage) {
      var newFood = new SavedFood { FoodName = name, Calories = 1, Protien = 1, ServingSize = 1, ServingUnit = Unit.Ounces };
      Assert.Throws<ArgumentException>(() => sut.AddFood(newFood), exceptionMessage);
    }

    [Test]
    public void AddFood_FailsWithInvalidCalorie() {
      var newFood = new SavedFood { FoodName = "InvalidCalorie", Calories = 0, Protien = 1, ServingSize = 1, ServingUnit = Unit.Ounces };
      Assert.Throws<ArgumentException>(() => sut.AddFood(newFood), "Calorie value must be greater than zero");
    }

    [Test]
    public void AddFood_FailsWithInvalidServingSize() {
      var newFood = new SavedFood { FoodName = "InvalidServing", Calories = 1, Protien = 1, ServingSize = 0, ServingUnit = Unit.Ounces };
      Assert.Throws<ArgumentException>(() => sut.AddFood(newFood),  "Serving Size must be greater than zero");
    }
    
    [Test]
    public void AddFood_Successful() {
      var newFood = new SavedFood { FoodName = "NewFood", Calories = 1, Protien = 1, ServingSize = 1, ServingUnit = Unit.Ounces };
      sut.AddFood(newFood);

      CollectionAssert.Contains(sut.GetFoods(), newFood);
    }

    [TestCase("ExistingMeal", "Meal Name must be unique")]
    [TestCase("","Meal Name cannot be null or whitespace")]
    [TestCase(" ","Meal Name cannot be null or whitespace")]
    [TestCase(null,"Meal Name cannot be null or whitespace")]
    public void AddMeal_FailsWithNameException(string name, string exceptionMessage) {
      var newMeal = new SavedMeal { MealName = name, Calories = 1, Protien = 1};
      Assert.Throws<ArgumentException>(() => sut.AddMeal(newMeal), exceptionMessage);
    }

    [Test]
    public void AddMeal_FailsWithInvalidCalorie() {
      var newMeal = new SavedMeal { MealName =  "InvalidCalorie", Calories = 0, Protien = 1};
      Assert.Throws<ArgumentException>(() => sut.AddMeal(newMeal),  "Calorie value must be greater than zero");
    }

    [Test]
    public void AddMeal_Successful() {
      var newMeal = new SavedMeal { MealName = "NewMeal", Calories = 1, Protien = 1};
      sut.AddMeal(newMeal);

      CollectionAssert.Contains(sut.GetMeals(), newMeal);
    }

    [Test]
    public void RemoveFood_Successful() {
      var food = new SavedFood { FoodName = "ToBeDeleted", Calories = 100, Protien = 5, ServingSize = 30, ServingUnit = Unit.Ounces };
      sut.AddFood(food);

      CollectionAssert.Contains(sut.GetFoods(), food);

      sut.RemoveFood(food.Id);

      CollectionAssert.DoesNotContain(sut.GetFoods(), food);
    }

    [Test]
    public void RemoveFood_FailsWhenFoodDoesntExist() {
      Assert.Throws<ArgumentException>(() => sut.RemoveFood(150),  "No Food With Id 150 exists");
    }
    [Test]
    public void RemoveMeal_Successful() {
      var meal = new SavedMeal { MealName = "ToBeDeleted", Calories = 100, Protien = 5};
      sut.AddMeal(meal);

      CollectionAssert.Contains(sut.GetMeals(), meal);

      sut.RemoveMeal(meal.Id);

      CollectionAssert.DoesNotContain(sut.GetMeals(), meal);
    }

    [Test]
    public void RemoveMeal_FailsWhenFoodDoesntExist() {
      Assert.Throws<ArgumentException>(() => sut.RemoveMeal(150),  "No Meal With Id 150 exists");
    }

    [Test]
    public void EditFood_Successful() {
      var food = new SavedFood { FoodName = "ToBeEdited", Calories = 1, Protien = 1, ServingSize = 1, ServingUnit = Unit.Ounces };
      sut.AddFood(food);

      var editedFood = new SavedFood { Id = food.Id, FoodName = "Edited", Calories = 3, Protien = 3, ServingSize = 3, ServingUnit = Unit.Cups };
      sut.EditFood(editedFood);
      var foodInDb = sut.GetFoods().SingleOrDefault(f => f.Id == food.Id);

      Assert.AreEqual(editedFood.FoodName, foodInDb.FoodName);
      Assert.AreEqual(editedFood.Calories, foodInDb.Calories);
      Assert.AreEqual(editedFood.Protien, foodInDb.Protien);
      Assert.AreEqual(editedFood.ServingSize, foodInDb.ServingSize);
      Assert.AreEqual(editedFood.ServingUnit, foodInDb.ServingUnit);
    }
    [Test]
    public void EditFood_FailsWhenNameIsEmptyString() {
      var food = new SavedFood { FoodName = "FailEditWithEmptyStringName", Calories = 1, Protien = 1, ServingSize = 1, ServingUnit = Unit.Ounces };
      sut.AddFood(food);

      var editedFood = new SavedFood { Id = food.Id, FoodName = "", Calories = 3, Protien = 3, ServingSize = 3, ServingUnit = Unit.Cups };
      Assert.Throws<ArgumentException>(() => sut.EditFood(editedFood),  "Food Name cannot be null or whitespace");
    }
    [Test]
    public void EditFood_FailsWithInvalidCalorie() {
      var food = new SavedFood { FoodName = "FailEditWithInvalidCalorie", Calories = 1, Protien = 1, ServingSize = 1, ServingUnit = Unit.Ounces };
      sut.AddFood(food);

      var editedFood = new SavedFood { Id = food.Id, FoodName = "EditedFood", Calories = 0, Protien = 3, ServingSize = 3, ServingUnit = Unit.Cups };
      Assert.Throws<ArgumentException>(() => sut.EditFood(editedFood),  "Calorie value must be greater than zero");
    }

    [Test]
    public void EditFood_FailsWithInvalidServingSize() {
      var food = new SavedFood { FoodName = "FailEditWithInvalidServing", Calories = 1, Protien = 1, ServingSize = 1, ServingUnit = Unit.Ounces };
      sut.AddFood(food);

      var editedFood = new SavedFood { Id = food.Id, FoodName = "EditedFood", Calories = 1, Protien = 3, ServingSize = 0, ServingUnit = Unit.Cups };
      Assert.Throws<ArgumentException>(() => sut.EditFood(editedFood),  "Serving Size must be greater than zero");
      var newFood = new SavedFood { FoodName = "InvalidServing", Calories = 1, Protien = 1, ServingSize = 0, ServingUnit = Unit.Ounces };
    }
    [Test]
    public void EditMeal_Successful() {
      var meal = new SavedMeal { MealName = "ToBeEdited", Calories = 1, Protien = 1};
      sut.AddMeal(meal);

      var editedMeal = new SavedMeal { Id = meal.Id, MealName = "Edited", Calories = 3, Protien = 3};
      sut.EditMeal(editedMeal);
      var mealInDb = sut.GetMeals().SingleOrDefault(f => f.Id == meal.Id);

      Assert.AreEqual(editedMeal.MealName, mealInDb.MealName);
      Assert.AreEqual(editedMeal.Protien, mealInDb.Protien);
      Assert.AreEqual(editedMeal.Calories, mealInDb.Calories);
    }
    [Test]
    public void EditMeal_FailsWhenNameIsEmptyString() {
      var meal = new SavedMeal { MealName = "FailEditWithEmptyStringName", Calories = 1, Protien = 1};
      sut.AddMeal(meal);

      var editedMeal = new SavedMeal { Id = meal.Id, MealName = "", Calories = 3, Protien = 3};
      Assert.Throws<ArgumentException>(() => sut.EditMeal(editedMeal),  "Meal Name cannot be null or whitespace");
    }
    [Test]
    public void EditMeal_FailsWithInvalidCalorie() {
      var meal = new SavedMeal { MealName = "FailEditWithInvalidCalorie", Calories = 1, Protien = 1};
      sut.AddMeal(meal);

      var editedMeal = new SavedMeal { Id = meal.Id, MealName = "EditedFood", Calories = 0, Protien = 3};
      Assert.Throws<ArgumentException>(() => sut.EditMeal(editedMeal),  "Calorie value must be greater than zero");
    }
  }
}