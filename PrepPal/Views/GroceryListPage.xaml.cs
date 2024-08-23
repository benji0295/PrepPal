using PrepPal.Models;
using PrepPal.ViewModels;
using System;
using System.Linq;
using Microsoft.Maui.Controls;	

namespace PrepPal.Views;

public partial class GroceryListPage : ContentPage
{
	private GroceryListViewModel _groceryListViewModel;
	public GroceryListPage(GroceryListViewModel groceryListViewModel)
	{
		InitializeComponent();
		_groceryListViewModel = groceryListViewModel;
		BindingContext = _groceryListViewModel;
	}

	private async void OnMenuButtonClicked(object sender, EventArgs e)
	{
		var action = await DisplayActionSheet("Option", "Cancel", null, "Add Item", "Clear Grocery List",
			"Clear Selected Groceries");
		switch (action)
		{
			case "Add Item":
				OnAddItemClicked();
				break;
			case "Clear Grocery List":
				OnClearListClicked();
				break;
			case "Clear Selected Groceries":
				OnDeleteSelectedClicked();
				break;
		}
	}

	private async Task OnAddItemClicked()
	{
		string result = await DisplayPromptAsync("Add Item", "Enter the name of the grocery item:");

		if (!string.IsNullOrWhiteSpace(result))
		{
			_groceryListViewModel.GroceryItems.Add(new GroceryItem { Name = result, IsBought = false });
		}
	}

	private async Task OnClearListClicked()
	{
		bool confirm = await DisplayAlert("Clear List", "Are you sure you want to clear the list?", "Yes", "No");
		if (confirm)
		{
			_groceryListViewModel.GroceryItems.Clear();
		}
	}
	private async Task OnDeleteSelectedClicked()
	{
		bool confirm = await DisplayAlert("Delete Selected", "Are you sure you want to delete the selected items?", "Yes", "No");

		if (confirm)
		{
			var itemsToRemove = _groceryListViewModel.GroceryItems.Where(item => item.IsBought).ToList();
			foreach (var item in itemsToRemove)
			{
				_groceryListViewModel.GroceryItems.Remove(item);
			}
		}
	}
}