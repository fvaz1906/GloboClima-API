using GloboClima.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GloboClima.Infra.Data.Mapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Nome da tabela (opcional)
            builder.ToTable("Users");

            // Chave primária (herdada de BaseEntity)
            builder.HasKey(u => u.Id);

            // Name - Opcional
            builder.Property(u => u.Name)
                .HasMaxLength(150)   // Tamanho máximo de 150 caracteres
                .IsUnicode(false)    // Sem suporte a Unicode
                .IsRequired(false);  // Opcional

            // Email - Opcional
            builder.Property(u => u.Email)
                .HasMaxLength(150)   // Tamanho máximo de 150 caracteres
                .IsUnicode(false)    // Sem suporte a Unicode
                .IsRequired(false);  // Opcional

            // Photo - Opcional
            builder.Property(u => u.Photo)
                .HasMaxLength(250)   // Tamanho máximo de 250 caracteres (ex: URL da foto)
                .IsUnicode(false)    // Sem suporte a Unicode
                .IsRequired(false);  // Opcional

            // Password - Opcional
            builder.Property(u => u.Password)
                .HasMaxLength(100)   // Tamanho máximo de 100 caracteres
                .IsUnicode(false)    // Sem suporte a Unicode
                .IsRequired(false);  // Opcional

            // Campo não mapeado (IFormFile)
            builder.Ignore(u => u.File);

        }
    }
}
