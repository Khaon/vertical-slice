﻿using Catalog.Api.Application.Extensions.FluentValidationExtensions;
using Catalog.Api.Domain.Enumerations.Trainer;
using FluentValidation;

namespace Catalog.Api.Application.Features.Trainer.CreateTrainer.Command;

public class CreateTrainerCommandValidator : AbstractValidator<CreateTrainerCommand>
{
    public CreateTrainerCommandValidator()
    {
        RuleFor(command => command.Firstname)
            .NotEmpty()
            .WithMessage("Firstname is required");

        RuleFor(command => command.Lastname)
            .NotEmpty()
            .WithMessage("Lastname is required");

        RuleFor(command => command.Bio)
            .MinimumLength(30)
            .WithMessage("Bio must be at least 30 characters long")
            .MaximumLength(500)
            .WithMessage("Bio must not exceed 500 characters");

        RuleFor(command => command.TrainerSkillLevelId)
            .EnumerationExists<CreateTrainerCommand, TrainerSkillLevel>()
            .WithMessage((_, level) => $"Trainer level skill id `{level}` doesn't exist");
    }
}