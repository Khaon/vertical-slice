﻿using Catalog.Api.Domain.Entities;
using Catalog.Api.Domain.Enumerations.Trainer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Api.EfCore.Configurations;

public class TrainerConfiguration : EntityConfiguration<Trainer>
{
    public override void Configure(EntityTypeBuilder<Trainer> entityTypeBuilder)
    {
        base.Configure(entityTypeBuilder);

        entityTypeBuilder
            .Property(trainer => trainer.Firstname)
            .IsRequired();

        entityTypeBuilder
            .Property(trainer => trainer.Lastname)
            .IsRequired();

        entityTypeBuilder.Property(trainer => trainer.Bio)
            .HasMaxLength(500)
            .IsRequired();

        entityTypeBuilder.Property(trainer => trainer.SkillLevel)
            .HasConversion(
                trainerSkillLevel => trainerSkillLevel.Value,
                id => TrainerSkillLevel.FromValue(id));
    }
}