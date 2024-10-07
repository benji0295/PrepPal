using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrepPal.Models
{
    public class Recipe : INotifyPropertyChanged
    {
        private bool _isFavorite;
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Servings { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public int TotalTime { get; set; }
        public string Source { get; set; }
        public string SourceURL { get; set; }
        public string ImageURL { get; set; }

        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                _isFavorite = value;
                OnPropertyChanged();
            }
        }
        public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
        public List<Instruction> Instructions { get; set; } = new List<Instruction>();
        
        public string ShortenedSourceUrl
        {
            get
            {
                if (Uri.TryCreate(SourceURL, UriKind.Absolute, out Uri uri))
                {
                    return uri.Host;
                }
                return string.Empty;
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
