namespace PrepPal.Views;

public partial class GroceryListPage : ContentPage
{
	private GroceryListViewModel _viewModel;
	
	public GroceryListPage()
	{
		InitializeComponent();

		_viewModel = App.GroceryListViewModel;
		BindingContext = _viewModel;
		
		// BindingContext = new GroceryListViewModel(App.FridgeListViewModel);
	}
	private async void OnAddButtonClicked(object sender, EventArgs e)
	{
	    string result = await DisplayPromptAsync("Add Item", "Enter the name of the grocery item:");

	    if (!string.IsNullOrWhiteSpace(result))
	    {
	        string aisle = await DisplayPromptAsync("Aisle", "Enter the aisle:");

	        if (!string.IsNullOrWhiteSpace(aisle))
	        {
	            var viewModel = BindingContext as GroceryListViewModel;
	            var fridgeViewModel = App.FridgeListViewModel;
	            
	            var existingFridgeItem = fridgeViewModel.FridgeItems
	                .FirstOrDefault(f => f.Name.Equals(result, StringComparison.OrdinalIgnoreCase));

	            if (existingFridgeItem != null)
	            {
	                var groceryItem = new GroceryItem
	                {
	                    Name = existingFridgeItem.Name,
	                    LastBoughtDate = existingFridgeItem.LastBoughtDate, 
	                    IsBought = false,
	                    Aisle = aisle,
	                    IsInGroceryList = true,
	                    StorageLocation = "Pantry"
	                };

	                viewModel?.GroceryItems.Add(groceryItem);

	            }
	            else
	            {
	                var newItem = new GroceryItem
	                {
	                    Name = result,
	                    LastBoughtDate = DateTime.Now,
	                    IsBought = false,
	                    Aisle = aisle,
	                    IsInGroceryList = false,
	                    StorageLocation = "Pantry"
	                };
	                
	                viewModel?.GroceryItems.Add(newItem);
	            }
	            
	            viewModel?.GroupItems();
	            viewModel?.OnPropertyChanged(nameof(viewModel.GroceryItems));
	        }
	    }
	}

	private async void OnDeleteButtonClicked(object sender, EventArgs e)
	{
		var action = await DisplayActionSheet("Option", "Cancel", null, "Clear Grocery List",
			"Clear Selected Groceries");
		var viewModel = BindingContext as GroceryListViewModel;
		
		switch (action)
		{
			case "Clear Grocery List":
				viewModel?.ClearListCommand.Execute(null);
				break;
			case "Clear Selected Groceries":
				viewModel?.DeleteSelectedCommand.Execute(null);
				break;
		}
	}
	private async Task OnDeleteSelectedClicked()
	{
		bool confirm = await DisplayAlert("Delete Selected", "Are you sure you want to delete the selected items?", "Yes", "No");

		if (confirm)
		{
			var viewModel = BindingContext as GroceryListViewModel;
			var itemsToRemove = viewModel?.GroceryItems.Where(item => item.IsBought).ToList();
			foreach (var item in itemsToRemove)
			{
				viewModel.GroceryItems.Remove(item);
			}
		}
	}
	private async Task OnClearListClicked()
	{
		bool confirm = await DisplayAlert("Clear List", "Are you sure you want to clear the list?", "Yes", "No");
		if (confirm)
		{
			var viewModel = BindingContext as GroceryListViewModel;
			viewModel?.GroceryItems.Clear();
		}
	}
}