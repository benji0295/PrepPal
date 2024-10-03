using System.ComponentModel;

namespace PrepPal.Models;

public class RecipeIngredient : INotifyPropertyChanged
{
    private bool _isSelected;
    
    public int RecipeIngredientId { get; set; } 
    public int RecipeId { get; set; } 
    public Recipe Recipe { get; set; } 
    public int IngredientId { get; set; }
    public decimal Quantity { get; set; }
    public string Unit { get; set; }
    public string Aisle { get; set; }
    public string StorageLocation { get; set; }
    public string IngredientName { get; set; }
    
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}