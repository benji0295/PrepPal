# PrepPal App Documentation
## Overview
PrepPal is a meal planning and grocery tracking application built using .NET MAUI with an MVVM architecture. The app allows users to manage recipes, create meal plans, track fridge items, and organize grocery lists. PrepPal is connected to a PostgreSQL database using Entity Framework Core.

## Key Features
- **Recipe Management**: Add, favorite, and view detailed recipes.
- **Grocery List Management**: Organize grocery items, categorize by aisle, and track which items have been bought.
- **Fridge Management**: Keep track of items in the fridge and pantry, and move used items back to the grocery list.
- **Meal Planning**: Assign recipes to meal plans for specific days.

## Technology Stack
- **Frontend**: .NET MAUI (Cross-platform development)
- **Backend**: PostgreSQL with Entity Framework Core (EF Core) for database management
- **Architecture**: MVVM (Model-View-ViewModel)

## Future Improvements
* User Accounts: Implement user accounts so that recipes, grocery lists, and fridge items can be associated with individual users.
* Cloud Sync: Add functionality to sync recipes, grocery lists, and fridge items to a cloud database.
* Push Notifications: Notify users about expiring fridge items or upcoming grocery shopping tasks.

# Project Structure

PrepPal follows a modular structure for better maintainability. Below is the breakdown of the main folders and their responsibilities.

## 1. PrepPal.Shared
This project contains shared components such as:
- **Models**: Classes representing data entities (e.g., `Recipe`, `FridgeItem`, `GroceryItem`).
- **Data Access**: `PrepPalDbContext` and database connection handling using EF Core.
- **Compiled Models**: Precompiled model `PrepPalDbContextModel` for Native AOT compliance.

## 2. PrepPal.Maui
This project contains the UI components and logic including:
- **Views**: Pages such as `RecipePage`, `GroceryListPage`, and `FridgeListPage`.
- **ViewModels**: Logic handling for views, utilizing the MVVM pattern.
- **AppShell**: The entry point for navigation between pages.
- **MauiProgram.cs**: Service registration, database connection, and logging configuration.

## 3. PrepPal.Migration
A separate project dedicated to handling database migrations.
- **DbContext Migrations**: Stores migration files generated using Entity Framework Core.
- **Usage**: `dotnet ef migrations add <MigrationName>` to add a new migration and `dotnet ef database update` to apply migrations.

# MVVM Architecture

PrepPal follows the **Model-View-ViewModel (MVVM)** pattern to separate concerns and maintain a clean architecture.

## 1. Models
Data models represent the entities in the database. Key models include:
- `Recipe`
- `FridgeItem`
- `GroceryItem`

## 2. Views
UI pages that the user interacts with, like:
- `RecipePage`
- `GroceryListPage`
- `FridgeListPage`

## 3. ViewModels
ViewModels handle the business logic and interact with the models. They are responsible for data-binding to Views:
- **RecipeViewModel**: Manages recipe operations.
- **GroceryListViewModel**: Handles the grocery list functionality.
- **FridgeListViewModel**: Tracks fridge and pantry items.

## 4. SharedService
This service links the `GroceryListViewModel` and `FridgeListViewModel` to avoid circular dependencies. It enables shared operations between grocery and fridge items.

#### 5. **Recipes and Favorite Recipes**
- **Description**: Overview of how recipes are handled, favorited, and filtered.
- **Contents**:
    - **RecipePage**: Viewing and favoriting recipes.
    - **FavoriteRecipePage**: Displaying only favorited recipes.

Future Improvements
User Accounts: Implement user accounts so that recipes, grocery lists, and fridge items can be associated with individual users.
Cloud Sync: Add functionality to sync recipes, grocery lists, and fridge items to a cloud database.
Push Notifications: Notify users about expiring fridge items or upcoming grocery shopping tasks.
