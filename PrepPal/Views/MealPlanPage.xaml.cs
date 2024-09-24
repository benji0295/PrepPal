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
	
	private async void OnAddMealClicked(object sender, EventArgs e)
	{
		try
		{
			var button = sender as Button;
			string day = button?.CommandParameter as string;

			if (!string.IsNullOrEmpty(day))
			{
				// Pass the day to the RecipeSelectionPage
				await Shell.Current.GoToAsync($"///RecipeSelectionPage?day={day}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error navigating to RecipeSelectionPage: {ex.Message}");
			await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
		}
	}
	
	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		if (!string.IsNullOrEmpty(RecipeName) && !string.IsNullOrEmpty(Day))
		{
			AddRecipeToMealPlan(RecipeName, Day);
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
					MondayStack?.Children.Insert(1, recipeLabel);
					break;
				case "Tuesday":
					TuesdayStack?.Children.Insert(1, recipeLabel);
					break;
				case "Wednesday":
					WednesdayStack?.Children.Insert(1, recipeLabel);
					break;
				case "Thursday":
					ThursdayStack?.Children.Insert(1, recipeLabel);
					break;
				case "Friday":
					FridayStack?.Children.Insert(1, recipeLabel);
					break;
				case "Saturday":
					SaturdayStack?.Children.Insert(1, recipeLabel);
					break;
				case "Sunday":
					SundayStack?.Children.Insert(1, recipeLabel);
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
}