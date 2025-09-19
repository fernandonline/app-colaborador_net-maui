using crud_maui.ViewModels;

namespace crud_maui.Views;

public partial class ColaboradorPage : ContentPage
{
	public ColaboradorPage(ColaboradorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}