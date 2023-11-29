using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RedPaw.Models;

public partial class GroupMember
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid ConversationId { get; set; }

    public DateTime ?Joined { get; set; }

    public DateTime ?Left { get; set; }

    public int IsBlocked { get; set; }
}
