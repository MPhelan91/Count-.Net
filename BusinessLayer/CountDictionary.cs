using DatabaseAccessLayer;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;

namespace BusinessLayer
{
  public class CountDictionary {
    private DatabaseContext _context;
    public CountDictionary(DatabaseContext context){
      _context = context;
    }
    public IList<SavedFood> GetFoods() {
      return _context.SavedFoods.ToList();
    }
    public IList<SavedMeal> GetMeals() {
      return _context.SavedMeals.ToList();
    }
    private void validateFood(SavedFood newFood) {
      if(_context.SavedFoods.Any(f => f.FoodName.Equals(newFood.FoodName) && f.Id != newFood.Id ))
        throw new ArgumentException("Food Name must be unique");

      if(string.IsNullOrWhiteSpace(newFood.FoodName))
        throw new ArgumentException("Food Name cannot be null or whitespace");

      if(newFood.Calories <= 0)
        throw new ArgumentException("Calorie value must be greater than zero");

      if(newFood.ServingSize <= 0)
        throw new ArgumentException("Serving Size must be greater than zero");
    }
    public void AddFood(SavedFood newFood){
      validateFood(newFood);

      _context.SavedFoods.Add(newFood);
      _context.SaveChanges();
    }
    private void validateMeal(SavedMeal newMeal) {
      if(_context.SavedMeals.Any(m => m.MealName.Equals(newMeal.MealName) && m.Id != newMeal.Id ))
        throw new ArgumentException("Meal Name must be unique");

      if(string.IsNullOrWhiteSpace(newMeal.MealName))
        throw new ArgumentException("Meal Name cannot be null or whitespace");

      if(newMeal.Calories <= 0)
        throw new ArgumentException("Calorie value must be greater than zero");
    }
    public void AddMeal(SavedMeal newMeal){
      validateMeal(newMeal);
      _context.SavedMeals.Add(newMeal);
      _context.SaveChanges();
    }
    public void RemoveFood(int foodId) {
      var existingFood = getFood(foodId);
      _context.SavedFoods.Remove(existingFood);
      _context.SaveChanges();
    }
    public void RemoveMeal(int mealId) {
      var existingMeal = getMeal(mealId);
      _context.SavedMeals.Remove(existingMeal);
      _context.SaveChanges();
    }
    public SavedMeal getMeal(int mealId) {
      var existingMeal = _context.SavedMeals.SingleOrDefault(o => o.Id == mealId);
      if(existingMeal == null) {
        throw new ArgumentException(string.Format("No Meal With Id {0} exists", mealId));
      }
      return existingMeal;
    }
    public SavedFood getFood(int foodId) {
      var existingFood = _context.SavedFoods.SingleOrDefault(o => o.Id == foodId);
      if(existingFood == null) {
        throw new ArgumentException(string.Format("No Food With Id {0} exists", foodId));
      }
      return existingFood;
    }
    public void EditFood(SavedFood food) {
      validateFood(food);

      var existingFood = getFood(food.Id);
      existingFood.FoodName = food.FoodName;
      existingFood.Calories = food.Calories;
      existingFood.Protien = food.Protien;
      existingFood.ServingSize = food.ServingSize;
      existingFood.ServingUnit = food.ServingUnit;
      _context.SaveChanges();
    }
    public void EditMeal(SavedMeal meal) {
      validateMeal(meal);

      var existingMeal = getMeal(meal.Id);
      existingMeal.MealName = meal.MealName;
      existingMeal.Calories = meal.Calories;
      existingMeal.Protien = meal.Protien;
      _context.SaveChanges();
    }
  }
}
