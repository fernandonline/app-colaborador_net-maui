using CommunityToolkit.Mvvm.ComponentModel;

namespace crud_maui.DTOs
{
    public partial class ColaboradorDTO : ObservableObject
    {
        [ObservableProperty]
        public int idColaborador;
        [ObservableProperty]
        public string nomeCompleto;
        [ObservableProperty]
        public string email;
        [ObservableProperty]
        public decimal salario;
        [ObservableProperty]
        public DateTime dataContratacao;
    }
}
