namespace EM.Web.DTOs
{
    public class RelatorioAlunoFiltrosDTO
    {
        public bool EhParaEmitirCidade { get; set; }
        public bool EhParaEmitirSexo { get; set; }
        public bool EhParaEmitirCpf { get; set; }
        public bool EhParaEmitirDataNascimento { get; set; }

        public string? Sexo { get; set; }
        public int? CidadeId { get; set; }
        public string? NomeContendo { get; set; }
    }
}
