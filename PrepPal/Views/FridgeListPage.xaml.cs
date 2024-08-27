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
    
    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var action = await DisplayActionSheet("Option", "Cancel", null, "Clear Fridge",
            "Clear Selected Items");
        switch (action)
        {
            case "Clear Fridge":
                OnClearListClicked();
                break;
            case "Clear Selected Items":
                OnDeleteSelectedClicked();
                break;
        }
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        OnAddItemClicked();
    }

    private async Task OnAddItemClicked()
    {
        string result = await DisplayPromptAsync("Add Item", "Enter the name of the item:");

        if (!string.IsNullOrWhiteSpace(result))
        {
            _viewModel.FridgeItems.Add(new FridgeItem { Name = result, IsUsed = false });
        }
    }

    private async Task OnClearListClicked()
    {
        bool confirm = await DisplayAlert("Clear List", "Are you sure you want to clear your fridge?", "Yes", "No");
        if (confirm)
        {
            _viewModel.FridgeItems.Clear();
        }
    }
    private async Task OnDeleteSelectedClicked()
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