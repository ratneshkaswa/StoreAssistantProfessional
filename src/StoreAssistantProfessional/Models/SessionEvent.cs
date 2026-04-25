using System.ComponentModel.DataAnnotations;

namespace StoreAssistantProfessional.Models;

public class SessionEvent
{
    [Key] public int Id { get; set; }

    public DateTime At { get; set; } = DateTime.UtcNow;

    [MaxLength(20)] public string Kind { get; set; } = "";    // RoleEnter / RoleExit / FailedUnlock / AutoDrop / AppStart / AppExit
    [MaxLength(20)] public string? Role { get; set; }
    [MaxLength(100)] public string? Reason { get; set; }
    [MaxLength(100)] public string? Detail { get; set; }
}
