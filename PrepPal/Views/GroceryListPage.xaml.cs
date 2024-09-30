using PrepPal.Models;
using PrepPal.ViewModels;
using System;
using System.Linq;
using Microsoft.Maui.Controls;	

namespace PrepPal.Views;

public partial class GroceryListPage : ContentPage
{
	public GroceryListPage()
	{
		InitializeComponent();
		
		BindingContext = new GroceryListViewModel(App.FridgeListViewModel);
	}

	private async void OnDeleteButtonClicked(object sender, EventArgs e)
	{
		var action = await DisplayActionSheet("Option", "Cancel", null, "Clear Grocery List",
			"Clear Selected Groceries");
		switch (action)
		{
			case "Clear Grocery List":
				OnClearListClicked();
				break;
			case "Clear Selected Groceries":
				OnDeleteSelectedClicked();
				break;
		}
	}

	private async void OnAddButtonClicked(object sender, EventArgs e)
	{
		string result = await DisplayPromptAsync("Add Item", "Enter the name of the grocery item:");

		if (!string.IsNullOrWhiteSpace(result))
		{
			var viewModel = BindingContext as GroceryListViewModel;
			viewModel?.AddItemToGroceryList(result); // Use the new method to add the item
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
}