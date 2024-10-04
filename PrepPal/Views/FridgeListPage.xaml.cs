namespace PrepPal.Views;

public partial class FridgeListPage : ContentPage
{
    private FridgeListViewModel _viewModel;
    
	public FridgeListPage()
	{
		InitializeComponent();
        _viewModel = App.FridgeListViewModel;
        BindingContext = _viewModel;
    }

    private async void OnFilterButtonClicked(object sender, EventArgs e)
    {
        var action = await DisplayActionSheet("Filter By", "Cancel", null, "Storage Location", "Date Bought");
            
        if (action == "Storage Location")
        {
            Console.WriteLine("Filter by Storage Location selected.");
            _viewModel.CurrentFilter = FilterType.ByStorageLocation;
        }
        else if (action == "Date Bought")
        {
            Console.WriteLine("Filter by Date Bought selected.");
            _viewModel.CurrentFilter = FilterType.ByDateBought;
        }
        
        Console.WriteLine($"Current filter set to: {_viewModel.CurrentFilter}");
        _viewModel.OnPropertyChanged(nameof(_viewModel.CurrentFilter));
        
        Console.WriteLine("Applying filter to the items.");
        _viewModel.ApplyFilter();
        
        Console.WriteLine("Filter application complete.");
    }
    
    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var action = await DisplayActionSheet("Option", "Cancel", null, "Clear Fridge",
            "Clear Selected Items");
        var viewModel = BindingContext as FridgeListViewModel;
        
        switch (action)
        {
            case "Clear Fridge":
                viewModel?.ClearListCommand.Execute(null);
                break;
            case "Clear Selected Items":
                viewModel?.DeleteSelectedCommand.Execute(null);
                break;
        }
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        OnAddItemClicked();
    }

    private async Task OnAddItemClicked()
    {
        string itemName = await DisplayPromptAsync("Add Item", "Enter the name of the item:");

        if (!string.IsNullOrWhiteSpace(itemName))
        {
            // Ask for the storage location
            string storageLocation = await DisplayPromptAsync("Storage Location", "Enter the storage location:");

            if (!string.IsNullOrWhiteSpace(storageLocation))
            {
                var viewModel = BindingContext as FridgeListViewModel;
                viewModel?.FridgeItems.Add(new FridgeItem 
                { 
                    Name = itemName, 
                    LastBoughtDate = DateTime.Now, 
                    IsUsed = false, 
                    StorageLocation = storageLocation 
                });
                
                viewModel?.GroupItems();  
                viewModel?.OnPropertyChanged(nameof(viewModel.FridgeItems));
                viewModel?.OnPropertyChanged(nameof(viewModel.GroupedFridgeItems));
            }
        }
    }

    private async Task OnClearListClicked()
    {
        bool confirm = await DisplayAlert("Clear List", "Are you sure you want to clear your fridge?", "Yes", "No");
        if (confirm)
        {
            var viewModel = BindingContext as FridgeListViewModel;
            viewModel?.FridgeItems.Clear();
            
            viewModel?.GroupItems();  
            viewModel?.OnPropertyChanged(nameof(viewModel.FridgeItems));
            viewModel?.OnPropertyChanged(nameof(viewModel.GroupedFridgeItems));
        }
    }
    private async Task OnDeleteSelectedClicked()
    {
        bool confirm = await DisplayAlert("Delete Selected", "Are you sure you want to delete the selected items?", "Yes", "No");

        if (confirm)
        {
            var viewModel = BindingContext as FridgeListViewModel;
            var itemsToRemove = viewModel?.FridgeItems.Where(item => item.IsUsed).ToList();
            foreach (var item in itemsToRemove)
            {
                viewModel?.FridgeItems.Remove(item);
                
                viewModel?.GroupItems();  
                viewModel?.OnPropertyChanged(nameof(viewModel.FridgeItems));
                viewModel?.OnPropertyChanged(nameof(viewModel.GroupedFridgeItems));
            }
        }
        
    }
}