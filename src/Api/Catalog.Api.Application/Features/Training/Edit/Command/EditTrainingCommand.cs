using Catalog.Api.Application.Common.Exceptions;
using Catalog.Api.Application.Features.Training.Common.CreateEdit;
using Catalog.Api.EfCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Application.Features.Training.Edit.Command;

public class EditTrainingCommand : CreateEditTrainingCommonCommand<EditedTrainingDto>
{
    public Guid Id { get; set; }
}

public class EditTrainingCommandHandler : CreateEditTrainingCommonCommandHandler<EditTrainingCommand, EditedTrainingDto>
{
    public EditTrainingCommandHandler(CatalogContext catalogContext) : base(catalogContext)
    {
    }

    protected override async Task<Domain.Entities.TrainingAggregate.Training> GetTrainingAccordinglyToCommandAsync(EditTrainingCommand command)
    {
        return await CatalogContext.Training
                   .Include(training => training.Topics)
                   .Include(training => training.VatJustifications)
                   .Include(training => training.Attendances)
                   .Include(training => training.Audiences)
                   .FirstOrDefaultAsync(x => x.Id == command.Id)
                       ?? throw new EntityNotFoundException(command.Id, typeof(Domain.Entities.TrainingAggregate.Training));
    }

    protected override EditedTrainingDto MakeResult(Domain.Entities.TrainingAggregate.Training training)
    {
        return new EditedTrainingDto()
        {
            Id                      = training.Id,
            Title                   = training.Title,
            Description             = training.Description,
            Goal                    = training.Goal
        };
    }
}
