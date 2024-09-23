using System.ComponentModel;

namespace PrepPal.Models;

public class Ingredient : INotifyPropertyChanged
{
    private bool _isSelected;
    
    public string IngredientId { get; set; }
    public string Name { get; set; }
    public string Aisle { get; set; }
    public string StorageLocation { get; set; }

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