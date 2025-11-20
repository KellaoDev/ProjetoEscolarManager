using EM.Domain.Enums;
using EM.Domain.Interfaces;

namespace EM.Domain
{
    public  class Aluno : IEntidade
    {
        public int Matricula {  get; set; }

        public string Nome { get; set; } = string.Empty;

        public string? Cpf { get; set; }

        public DateTime DataNascimento { get; set; }

        public EnumeradorSexo EnumeradorSexo { get; set; }

        public int CidadeId { get; set; }

        public Cidade? Cidade { get; set; }

        public override bool Equals(object? obj) => obj is Aluno aluno && Matricula == aluno.Matricula;

        public override int GetHashCode() => HashCode.Combine(Matricula);
        
        public override string ToString() => $"Matrícula: {Matricula}, Nome: {Nome}, CPF: {Cpf}, Sexo: {EnumeradorSexo}, Data de Nascimento: {DataNascimento}, Cidade: {Cidade}";
    }
}
