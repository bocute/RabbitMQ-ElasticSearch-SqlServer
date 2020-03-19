using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceRabbitMQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceRabbitMQ.Data.SqlServer.Mappings
{
    public class NotasFiscaisMap : IEntityTypeConfiguration<NotaFiscal>
    {
        public void Configure(EntityTypeBuilder<NotaFiscal> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Numero)
                .IsRequired();

            builder.Property(n => n.Serie)
                .HasColumnType("varchar(3)")
                .IsRequired();

            builder.Property(n => n.SerieEcf)
                .HasColumnType("varchar(20)")
                .IsRequired();

            builder.Property(n => n.NroEmpresa)
                .IsRequired();

            builder.Ignore(n => n.WebRootPath);

            builder.ToTable("notasfiscais");

        }
    }
}
