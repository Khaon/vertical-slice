﻿namespace Catalog.Api.Application.Features.Training.GetById;

public class TrainingByIdDto
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public string? Title { get; set; }

    public string? TrainingTypeDescription { get; set; }

    public int TrainingTypeId { get; set; }
}