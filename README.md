PrepPal App Documentation

Overview

PrepPal is a .NET MAUI-based application designed to help users track their recipes, create a meal plan, manage grocery lists, and monitor fridge items. 
The app uses a MVVM (Model-View-ViewModel) architecture to separate concerns between the user interface and the business logic. 
Users can add, view, and delete recipes, add ingredients to their grocery list, and mark fridge items as used. 
The app utilizes ObservableCollection to automatically notify and update the UI when data changes.

Features

Recipe Management

View a list of recipes with details such as name, category, servings, and instructions.
Add new recipes to the list.
Delete recipes.
Adjust the serving size for each recipe with + and - buttons.
Ingredients can be added to the grocery list with a single button click.

Meal Plan Management

Add recipes to a weekly meal plan.

Grocery List Management

Add ingredients to the grocery list directly from a recipe.
Mark grocery items as bought or not bought.
Swipe to delete items from the grocery list.

Fridge Item Management

Monitor items in the fridge with an expiration date and whether they have been used.
Strike-through effect on used items.
Items are listed with a checkbox to indicate usage.


Architecture

MVVM (Model-View-ViewModel)

The app is structured using the MVVM pattern, which promotes clean separation between UI (View), business logic (ViewModel), and data (Model).


Future Improvements
User Accounts: Implement user accounts so that recipes, grocery lists, and fridge items can be associated with individual users.
Cloud Sync: Add functionality to sync recipes, grocery lists, and fridge items to a cloud database.
Push Notifications: Notify users about expiring fridge items or upcoming grocery shopping tasks.
