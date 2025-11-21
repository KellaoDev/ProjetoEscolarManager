using EM.Domain.Enums;
using EM.Domain.Interfaces;

namespace EM.Domain
{
    public class Cidade : IEntidade
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public EnumeradorUF EnumeradorUF { get; set; }

        public override bool Equals(object? obj) => obj is Cidade model && Codigo == model.Codigo;

        public override int GetHashCode() => HashCode.Combine(Codigo);

        public override string ToString() => $"Código: {Codigo}, Descrição: {Descricao}, UF: {EnumeradorUF}";
    }
}
