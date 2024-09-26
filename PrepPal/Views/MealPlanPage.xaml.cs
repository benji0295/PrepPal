using PrepPal.Views;
using Microsoft.Maui.Controls;

namespace PrepPal.Views;

[QueryProperty(nameof(RecipeName), "recipe")]
[QueryProperty(nameof(Day), "day")]
public partial class MealPlanPage : ContentPage
{
	public string RecipeName { get; set; }
	public string Day { get; set; }
	
	public MealPlanPage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (!string.IsNullOrEmpty(RecipeName) && !string.IsNullOrEmpty(Day))
		{
			AddRecipeToMealPlan(RecipeName, Day);
		}
		
	}
	
	private async void OnAddMealClicked(object sender, EventArgs e)
	{
		try
		{
			var button = sender as Button;
			string day = button?.CommandParameter as string;

			if (!string.IsNullOrEmpty(day))
			{
				Console.WriteLine($"Navigate to RecipeSelectionPage with day: {day}");
				
				await Shell.Current.GoToAsync($"//RecipeSelectionPage?day={day}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error navigating to RecipeSelectionPage: {ex.Message}");
			await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
		}
	}
	
	private void AddRecipeToMealPlan(string recipeName, string day)
	{
		try
		{
			var recipeLabel = new Label
			{
				Text = recipeName,
				FontSize = 16,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Center
			};

			switch (day)
			{
				case "Monday":
					InsertRecipeAboveButton(MondayStack, recipeLabel);
					break;
				case "Tuesday":
					InsertRecipeAboveButton(TuesdayStack, recipeLabel);
					break;
				case "Wednesday":
					InsertRecipeAboveButton(WednesdayStack, recipeLabel);
					break;
				case "Thursday":
					InsertRecipeAboveButton(ThursdayStack, recipeLabel);
					break;
				case "Friday":
					InsertRecipeAboveButton(FridayStack, recipeLabel);
					break;
				case "Saturday":
					InsertRecipeAboveButton(SaturdayStack, recipeLabel);
					break;
				case "Sunday":
					InsertRecipeAboveButton(SundayStack, recipeLabel);
					break;
				default:
					Console.WriteLine("Unknown day selected.");
					break;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error adding recipe to meal plan: {ex.Message}");
			DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
		}
	}

	private void InsertRecipeAboveButton(VerticalStackLayout stack, Label recipeLabel)
	{
		var buttonIndex = stack.Children.Count - 1;
		
		stack.Children.Insert(buttonIndex, recipeLabel);
	}
}