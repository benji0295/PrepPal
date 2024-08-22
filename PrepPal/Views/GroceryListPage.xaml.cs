using PrepPal.Models;
using PrepPal.ViewModels;
using System;
using System.Linq;
using Microsoft.Maui.Controls;	

namespace PrepPal.Views;

public partial class GroceryListPage : ContentPage
{
	private GroceryListViewModel _viewModel;
	public GroceryListPage()
	{
		InitializeComponent();
		_viewModel = BindingContext as GroceryListViewModel; 
	}

	private async void OnAddItemClicked(object sender, EventArgs e)
	{
		string result = await DisplayPromptAsync("Add Item", "Enter the name of the grocery item:");

		if (!string.IsNullOrWhiteSpace(result))
		{
			_viewModel.GroceryItems.Add(new GroceryItem { Name = result, IsBought = false });
		}
	}

	private async void OnClearListClicked(object sender, EventArgs e)
	{
		bool confirm = await DisplayAlert("Clear List", "Are you sure you want to clear the list?", "Yes", "No");
		if (confirm)
		{
			_viewModel.GroceryItems.Clear();
		}
	}
	private async void OnDeleteSelectedClicked(object sender, EventArgs e)
	{
		bool confirm = await DisplayAlert("Delete Selected", "Are you sure you want to delete the selected items?", "Yes", "No");

		if (confirm)
		{
			var itemsToRemove = _viewModel.GroceryItems.Where(item => item.IsBought).ToList();
			foreach (var item in itemsToRemove)
			{
				_viewModel.GroceryItems.Remove(item);
			}
		}
	}
}