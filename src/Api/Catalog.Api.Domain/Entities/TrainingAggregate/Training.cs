using Ardalis.GuardClauses;
using Catalog.Api.Domain.Entities.Base;
using Catalog.Api.Domain.Entities.TrainerAggregate;
using Catalog.Api.Domain.Entities.TrainingAggregate.Events;
using Catalog.Api.Domain.Entities.TrainingAggregate.Message;
using Catalog.Api.Domain.Extensions;
using Catalog.Shared.Enumerations.Training;

namespace Catalog.Api.Domain.Entities.TrainingAggregate;

public class Training : Entity, IEntity
{
    private string _title = null!;

    public string Title
    {
        get  => _title;
        init => ChangeTitle(value);
    }

    private string _description = null!;

    public string Description
    {
        get  => _description;
        init => ChangeDescription(value);
    }

    private string _goal = null!;

    public string Goal
    {
        get  => _goal;
        init => ChangeGoal(value);
    }

    private readonly HashSet<TrainingAssignment> _assignments;
    private readonly HashSet<TrainingAudience> _audiences;
    private readonly HashSet<TrainingTopic> _topics;
    private readonly HashSet<TrainingAttendance> _attendances;
    private readonly HashSet<TrainingVatJustification> _vatJustifications;

    public IReadOnlySet<TrainingAudience> Audiences => _audiences;

    public IReadOnlySet<TrainingAttendance> Attendances => _attendances;

    public IReadOnlySet<TrainingAssignment> Assignments => _assignments;

    public IReadOnlySet<TrainingTopic> Topics => _topics;

    public IReadOnlySet<TrainingVatJustification> VatJustifications => _vatJustifications;

    private Training()
    {
        _assignments      = new HashSet<TrainingAssignment>();
        _topics           = new HashSet<TrainingTopic>();
        _attendances      = new HashSet<TrainingAttendance>();
        _vatJustifications = new HashSet<TrainingVatJustification>();
        _audiences        = new HashSet<TrainingAudience>();
    }

    public Training(string title) : this()
    {
        Title = title;
    }

    public TrainingAssignment Assign(Trainer trainer)
    {
        Guard.Against.Default(trainer.Id, nameof(trainer));
        var assignment = new TrainingAssignment()
        {
            Trainer = trainer
        };
        _assignments.Add(assignment);
        return assignment;
    }

    internal void ChangeTitle(string title)
    {
        _title = Guard.Against.NullOrWhiteSpace(title, nameof(title));
        _title = Guard.Against.MinLength(title, 5, nameof(title));
    }

    internal void ChangeDescription(string description)
    {
        _description = Guard.Against.NullOrWhiteSpace(description, nameof(description));
        _description = Guard.Against.Between(description, 30, 500, nameof(description));
    }

    internal void ChangeGoal(string goal)
    {
        _goal = Guard.Against.NullOrWhiteSpace(goal, nameof(goal));
        _goal = Guard.Against.Between(goal, 30, 500, nameof(goal));
    }

    internal void SetAttendance(IEnumerable<int>? attendances)
    {
        SetAttendance(attendances?.Select(Attendance.FromValue));
    }

    internal void SetAttendance(IEnumerable<Attendance>? attendances)
    {
        _attendances.ReplaceWith(attendances, attendance => new TrainingAttendance(this, attendance));
    }

    internal void SetAudience(List<int>? requestAudiences)
    {
        SetAudience(requestAudiences?.Select(Audience.FromValue));
    }

    internal void SetAudience(IEnumerable<Audience>? audiences)
    {
       _audiences.ReplaceWith(audiences, audience => new TrainingAudience(this, audience));
    }

    internal void SetTopics(IEnumerable<int>? topics)
    {
        SetTopics(topics?.Select(Topic.FromValue));
    }

    internal void SetTopics(IEnumerable<Topic>? topics)
    {
        _topics.ReplaceWith(topics, topic => new TrainingTopic(this, topic));
    }

    internal void SetVatJustifications(IEnumerable<int>? vatJustificationIds)
    {
        SetVatJustifications(vatJustificationIds?.Select(VatJustification.FromValue));
    }

    internal void SetVatJustifications(IEnumerable<VatJustification>? vatJustifications)
    {
        _vatJustifications.ReplaceWith(vatJustifications, vat => new TrainingVatJustification(this, vat));
    }

    public void Edit(TrainingEditMessage message)
    {
        // Those fields cannot be null, the validation asserts that.
        ChangeTitle(message.Title!);
        ChangeDescription(message.Description!);
        ChangeGoal(message.Goal!);

        SetTopics(message.Topics);
        SetAttendance(message.Attendances);
        SetVatJustifications(message.VatJustifications);
        SetAudience(message.Audiences);

        if (!IsTransient)
        {
            DomainEvents.Add(new TrainingEditedEvent(this));
        }
    }
}
