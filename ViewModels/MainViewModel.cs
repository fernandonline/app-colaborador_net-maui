using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using crud_maui.Views;
using crud_maui.Models;
using crud_maui.Utils;
using crud_maui.DTOs;
using crud_maui.DataAcess;
using System.Collections.ObjectModel;

namespace crud_maui.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ColaboradorDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<ColaboradorDTO> listaColaborador = new ObservableCollection<ColaboradorDTO>();
    
        public MainViewModel(ColaboradorDbContext context)
        {
            _dbContext = context;

            MainThread.BeginInvokeOnMainThread(new Action(async () => await Obter()));

            WeakReferenceMessenger.Default.Register<ColaboradorMensageria>(this, (r, m) => ColaboradorMensagemRecebida(m.Value));
        }

        public async Task Obter()
        {
            var lista = await _dbContext.Colaboradores.ToListAsync();
            if(lista.Count != 0)
            {
                ListaColaborador.Clear();
                foreach (var item in lista)
                {
                    ListaColaborador.Add(new ColaboradorDTO
                    {
                        IdColaborador = item.IdColaborador,
                        NomeCompleto = item.NomeCompleto,
                        Email = item.Email,
                        Salario = item.Salario,
                        DataContratacao = item.DataContratacao
                    });
                }
            }
        }

        private void ColaboradorMensagemRecebida(ColaboradorMensagem colaboradorMensagem)
        {
            var colaboradorDto = colaboradorMensagem.ColaboradorDto;
            if(colaboradorMensagem.Criando)
            {
                ListaColaborador.Add(colaboradorDto);
            }
            else
            {
                var encontrado = ListaColaborador
                    .First(e => e.IdColaborador == colaboradorDto.IdColaborador);

                encontrado.NomeCompleto = colaboradorDto.NomeCompleto;
                encontrado.Email = colaboradorDto.Email;
                encontrado.Salario = colaboradorDto.Salario;
                encontrado.DataContratacao = colaboradorDto.DataContratacao;
            }
        }

        [RelayCommand]
        private async Task Criar()
        {
            var uri = $"{nameof(ColaboradorPage)}?id=0";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Editar(ColaboradorDTO colaboradorDto)
        {
            var uri = $"{nameof(ColaboradorPage)}?id={colaboradorDto.IdColaborador}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Deletar(ColaboradorDTO colaboradorDto)
        {
            bool confirmacao = await Shell.Current.DisplayAlert("Confirmação", "Deseja realmente excluir?", "Sim", "Não");
            if (confirmacao)
            {
                var encontrado = await _dbContext.Colaboradores.FirstAsync(e => e.IdColaborador == colaboradorDto.IdColaborador);
                _dbContext.Colaboradores.Remove(encontrado);
                await _dbContext.SaveChangesAsync();
                ListaColaborador.Remove(colaboradorDto);

            }
        }

    }
}
