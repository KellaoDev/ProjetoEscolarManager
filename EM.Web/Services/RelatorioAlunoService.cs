using EM.Domain;
using EM.Repository.Interfaces;
using EM.Web.DTOs;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace EM.Web.Services
{
    public class RelatorioAlunoService(IRepositorioAluno repositorioAluno) : IRelatorioAlunoService
    {
        private readonly IRepositorioAluno _repositorioAluno = repositorioAluno;
        public byte[] Emita(RelatorioAlunoFiltrosDTO parametros)
        {
            IEnumerable<Aluno> alunos = _repositorioAluno.GetAll();

            using MemoryStream memoryStream = new();
            Document doc = new(PageSize.A4);
            PdfWriter.GetInstance(doc, memoryStream);

            doc.Open();

            Font fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            Paragraph titulo = new("Relatório de Alunos", fontTitulo)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20f
            };
            doc.Add(titulo);

            int qtdColunas = CalcularQuantidadeDeColunas(parametros);
            PdfPTable tabela = new(qtdColunas)
            {
                WidthPercentage = 100
            };

            AdicionarCabecalhos(tabela, parametros);

            foreach (var aluno in alunos)
            {
                AdicionarCelulasDaLinha(tabela, aluno, parametros);
            }

            doc.Add(tabela);

            doc.Close();
            return memoryStream.ToArray();
        }

        private int CalcularQuantidadeDeColunas(RelatorioAlunoFiltrosDTO parametros)
        {
            int qtdColunas = 1;

            if (parametros.EhParaEmitirCpf) qtdColunas++;
            if (parametros.EhParaEmitirDataNascimento) qtdColunas++;
            if (parametros.EhParaEmitirSexo) qtdColunas++;
            if (parametros.EhParaEmitirCidade) qtdColunas++;

            return qtdColunas;
        }

        private void AdicionarCabecalhos(PdfPTable tabela, RelatorioAlunoFiltrosDTO parametros)
        {
            AdicionarHeader(tabela, "Nome Completo");

            if (parametros.EhParaEmitirCpf)
            {
                AdicionarHeader(tabela, "CPF");
            }
            if (parametros.EhParaEmitirDataNascimento)
            {
                AdicionarHeader(tabela, "Data de Nascimento");
            }
                
            if (parametros.EhParaEmitirSexo)
            {
                AdicionarHeader(tabela, "Sexo");
            }
                
            if (parametros.EhParaEmitirCidade)
            {
                AdicionarHeader(tabela, "Cidade");
            }
        }

        private void AdicionarCelulasDaLinha(PdfPTable tabela, Aluno aluno, RelatorioAlunoFiltrosDTO parametros)
        {
            tabela.AddCell(aluno.Nome);
            if (parametros.EhParaEmitirCpf)
            {
                tabela.AddCell(aluno.Cpf ?? "-");
            }
            if (parametros.EhParaEmitirDataNascimento)
            {
                tabela.AddCell(aluno.DataNascimento.ToShortDateString());
            }
            if (parametros.EhParaEmitirSexo)
            {
                tabela.AddCell(aluno.EnumeradorSexo.ToString());
            }
            if (parametros.EhParaEmitirCidade)
            {
                tabela.AddCell(aluno.Cidade?.Descricao ?? "-");
            }
        }
        
        private void AdicionarHeader(PdfPTable tabela, string texto)
        {
            Font font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            PdfPCell celula = new(new Phrase(texto, font))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                Padding = 5
            };
            tabela.AddCell(celula);
        }
    }
}
