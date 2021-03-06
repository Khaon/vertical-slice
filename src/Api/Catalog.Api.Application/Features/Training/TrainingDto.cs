namespace Catalog.Api.Application.Features.Training;

public record TrainingDto
{
    public Guid Id { get; set; }

    public string Title { get; init; } = null!;

    public string Description { get; init; } = null!;

    public string Goal { get; init; } = null!;
}
