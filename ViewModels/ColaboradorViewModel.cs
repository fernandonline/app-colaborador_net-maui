using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using crud_maui.Models;
using crud_maui.Utils;
using crud_maui.DTOs;
using crud_maui.DataAcess;

namespace crud_maui.ViewModels
{
    public partial class ColaboradorViewModel : ObservableObject, IQueryAttributable
    {
        private readonly ColaboradorDbContext _dbContext;

        [ObservableProperty]
        private ColaboradorDTO colaboradorDto = new ColaboradorDTO();

        [ObservableProperty]
        private string tituloPagina;
        
        private int IdColaborador;

        [ObservableProperty]
        private bool loadingVisivel = false;

        public ColaboradorViewModel(ColaboradorDbContext context)
        {
            _dbContext = context;
            ColaboradorDto.DataContratacao = DateTime.Now;

        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id = int.Parse(query["id"].ToString());
            IdColaborador = id;

            if (IdColaborador == 0)
            {
                TituloPagina = "Novo Colaborador";
            }
            else
            {
                TituloPagina = "Editar Colaborador";
                loadingVisivel = true;

                await Task.Run(async () =>
                {
                    var encontrado = await _dbContext.Colaboradores.FirstAsync(e => e.IdColaborador == IdColaborador);
                    ColaboradorDto.IdColaborador = encontrado.IdColaborador;
                    ColaboradorDto.NomeCompleto = encontrado.NomeCompleto;
                    ColaboradorDto.Email = encontrado.Email;
                    ColaboradorDto.Salario = encontrado.Salario;
                    ColaboradorDto.DataContratacao = encontrado.DataContratacao;

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        loadingVisivel = false;
                    });
                });
            }
        }

        [RelayCommand]
        private async Task Salvar()
        {
            if (string.IsNullOrWhiteSpace(ColaboradorDto.NomeCompleto))
            {
                await Shell.Current.DisplayAlert("Erro", "O campo Nome é obrigatório.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(ColaboradorDto.Email))
            {
                await Shell.Current.DisplayAlert("Erro", "O campo Email é obrigatório.", "OK");
                return;
            }

            LoadingVisivel = true;
            ColaboradorMensagem mensagem = new ColaboradorMensagem();

            await Task.Run(async () =>
            {
                if (IdColaborador == 0)
                {
                    var novoColaborador = new Colaborador
                    {
                        NomeCompleto = ColaboradorDto.NomeCompleto,
                        Email = ColaboradorDto.Email,
                        Salario = ColaboradorDto.Salario,
                        DataContratacao = ColaboradorDto.DataContratacao
                    };

                    _dbContext.Colaboradores.Add(novoColaborador);
                    await _dbContext.SaveChangesAsync();
                    colaboradorDto.IdColaborador = novoColaborador.IdColaborador;

                    mensagem = new ColaboradorMensagem()
                    {
                        Criando = true,
                        ColaboradorDto = ColaboradorDto
                    };
                }
                else
                {
                    var colaboradorExistente = await _dbContext.Colaboradores.FirstAsync(e => e.IdColaborador == IdColaborador);
                    colaboradorExistente.NomeCompleto = ColaboradorDto.NomeCompleto;
                    colaboradorExistente.Email = ColaboradorDto.Email;
                    colaboradorExistente.Salario = ColaboradorDto.Salario;
                    colaboradorExistente.DataContratacao = ColaboradorDto.DataContratacao;

                    await _dbContext.SaveChangesAsync();
                    mensagem = new ColaboradorMensagem()
                    {
                        Criando = false,
                        ColaboradorDto = ColaboradorDto
                    };
                }

                MainThread.BeginInvokeOnMainThread(async() =>
                {
                    LoadingVisivel = false;
                    WeakReferenceMessenger.Default.Send(new ColaboradorMensageria(mensagem));
                    await Shell.Current.Navigation.PopAsync();
                });

            });
        }
    }
}
