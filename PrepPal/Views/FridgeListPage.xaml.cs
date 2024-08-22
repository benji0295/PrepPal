using PrepPal.Models;
using PrepPal.ViewModels;
using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace PrepPal.Views;

public partial class FridgeListPage : ContentPage
{
    private FridgeListViewModel _viewModel;
	public FridgeListPage()
	{
		InitializeComponent();
        _viewModel = BindingContext as FridgeListViewModel;
	}

    private async void OnAddItemClicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("Add Item", "Enter the name of the grocery item:");

        if (!string.IsNullOrWhiteSpace(result))
        {
            _viewModel.FridgeItems.Add(new FridgeItem { Name = result, IsUsed = false });
        }
    }

    private async void OnClearListClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Clear List", "Are you sure you want to clear the list?", "Yes", "No");
        if (confirm)
        {
            _viewModel.FridgeItems.Clear();
        }
    }
    private async void OnDeleteSelectedClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Delete Selected", "Are you sure you want to delete the selected items?", "Yes", "No");

        if (confirm)
        {
            var itemsToRemove = _viewModel.FridgeItems.Where(item => item.IsUsed).ToList();
            foreach (var item in itemsToRemove)
            {
                _viewModel.FridgeItems.Remove(item);
            }
        }
    }
}