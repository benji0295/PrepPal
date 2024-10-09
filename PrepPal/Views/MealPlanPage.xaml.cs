namespace PrepPal.Views;

[QueryProperty(nameof(RecipeName), "recipe")]
[QueryProperty(nameof(Day), "day")]
[QueryProperty(nameof(RecipeImage), "image")]
public partial class MealPlanPage : ContentPage
{
	public string RecipeName { get; set; }
	public string Day { get; set; }
	public string RecipeImage { get; set; }

	private bool isRecipeAdded = false;
	
	public MealPlanPage()
	{
		InitializeComponent();
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (!string.IsNullOrEmpty(RecipeName) && !string.IsNullOrEmpty(Day) && !string.IsNullOrEmpty(RecipeImage) && !isRecipeAdded)
		{
			AddRecipeToMealPlan(RecipeName, RecipeImage, Day);

			ClearRecipeSelection();
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
				await Shell.Current.GoToAsync($"//RecipeSelectionPage?day={day}");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error navigating to RecipeSelectionPage: {ex.Message}");
			await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
		}
	}
	
	private void AddRecipeToMealPlan(string recipeName, string recipeImageSource, string day)
	{
		try
		{
			var targetStack = GetDayStack(day);
			
			if (targetStack != null && targetStack.Children.Any(c => ((Label)((HorizontalStackLayout)((SwipeView)c).Content).Children[1]).Text == recipeName))
			{
				return; 
			}
			
			var swipeView = new SwipeView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

			var deleteAction = new SwipeItem
			{
				Text = "Delete",
				BackgroundColor = Colors.LightCoral
			};

			deleteAction.Invoked += (sender, e) =>
			{
				RemoveRecipeFromMealPlan(swipeView, day);
			};

			var swipeItems = new SwipeItems { deleteAction };
			swipeItems.SwipeBehaviorOnInvoked = SwipeBehaviorOnInvoked.Close;

			swipeView.RightItems = swipeItems;

			var contentLayout = new HorizontalStackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Spacing = 10,
				VerticalOptions = LayoutOptions.Center
			};

			var recipeImage = new Image
			{
				Source = recipeImageSource,
				WidthRequest = 50,
				HeightRequest = 50,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Center,
				Aspect = Aspect.AspectFill
			};
			
			
			var recipeLabel = new Label
			{
				Text = recipeName,
				FontSize = 16,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.Center,
				Padding = new Thickness(10,5,0,5),
				Margin = new Thickness(0,10,0,0),
			};
			
			contentLayout.Children.Add(recipeImage);
			contentLayout.Children.Add(recipeLabel);

			swipeView.Content = contentLayout;

			targetStack?.Children.Add(swipeView);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error adding recipe to meal plan: {ex.Message}");
			DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
		}
	}
	private VerticalStackLayout GetDayStack(string day)
	{
		return day switch
		{
			"Monday" => MondayStack,
			"Tuesday" => TuesdayStack,
			"Wednesday" => WednesdayStack,
			"Thursday" => ThursdayStack,
			"Friday" => FridayStack,
			"Saturday" => SaturdayStack,
			"Sunday" => SundayStack,
			_ => null,
		};
	}
	
	private void RemoveRecipeFromMealPlan(View recipeView, string day)
	{
		switch (day)
		{
			case "Monday":
				MondayStack.Children.Remove(recipeView);
				break;
			case "Tuesday":
				TuesdayStack.Children.Remove(recipeView);
				break;
			case "Wednesday":
				WednesdayStack.Children.Remove(recipeView);
				break;
			case "Thursday":
				ThursdayStack.Children.Remove(recipeView);
				break;
			case "Friday":
				FridayStack.Children.Remove(recipeView);
				break;
			case "Saturday":
				SaturdayStack.Children.Remove(recipeView);
				break;
			case "Sunday":
				SundayStack.Children.Remove(recipeView);
				break;
		}
	}

	private void ClearRecipeSelection()
	{
		RecipeName = null;
		Day = null;
		RecipeImage = null;
	}
}