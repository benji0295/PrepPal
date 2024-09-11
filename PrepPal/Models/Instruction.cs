using System.ComponentModel;

namespace PrepPal.Models;

public class Instruction : INotifyPropertyChanged
{
    public int InstructionId { get; set; }
    public int RecipeId { get; set; }
    private bool _isCompleted;

    public int Step { get; set; }
    public string Description { get; set; }
    public Recipe Recipe { get; set; }

    public bool IsCompleted
    {
        get => _isCompleted;
        set
        {
            _isCompleted = value;
            OnPropertyChanged(nameof(IsCompleted));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}