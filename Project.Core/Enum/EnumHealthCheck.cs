using System.ComponentModel;

namespace Project.Core.Enum
{
    /// <summary>
    /// Species covered by the ill-health / early issue detection feature.
    /// Int-backed (stored as int) to match the repo enum convention.
    /// </summary>
    public enum EnumHealthCheckSpecies
    {
        [Description("Dog")]
        Dog = 1,
        [Description("Cat")]
        Cat = 2,
    }

    /// <summary>
    /// Review lifecycle of a health-check event.
    /// </summary>
    public enum EnumHealthCheckStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Reviewed")]
        Reviewed = 2,
        [Description("Closed")]
        Closed = 3,
    }
}
