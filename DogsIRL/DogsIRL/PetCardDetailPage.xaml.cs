using DogsIRL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DogsIRL
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PetCardDetailPage : ContentPage
    {
        PetCard PetCard { get; set; }
        public PetCardDetailPage(PetCard petCard)
        {
            InitializeComponent();
            PetCard = petCard;
        }

        protected override void OnAppearing()
        {
            LabelName.Text = PetCard.Name;
            LabelCollections.Text = PetCard.Collections.ToString();
            ImageElement.Source = PetCard.ImageURL;
            LabelGoodDog.Text = PetCard.GoodDog.ToString();
            LabelFloofiness.Text = PetCard.Floofiness.ToString();
            LabelEnergy.Text = PetCard.Energy.ToString();
            LabelSnuggles.Text = PetCard.Snuggles.ToString();
            LabelAppetite.Text = PetCard.Appetite.ToString();
            LabelBravery.Text = PetCard.Bravery.ToString();
        }
    }
}