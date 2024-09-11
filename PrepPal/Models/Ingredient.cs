using System.ComponentModel;

namespace PrepPal.Models;

public class Ingredient : INotifyPropertyChanged
{
    public int IngredientId { get; set; }
    private bool _isSelected;
    public string Name { get; set; }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            OnPropertyChanged(nameof(IsSelected));
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}