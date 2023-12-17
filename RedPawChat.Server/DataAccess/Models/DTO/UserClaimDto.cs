namespace RedPawChat.Server.DataAccess.Models.DTO
{
    public class UserClaimDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }    
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public UserClaimDto() { }

        public UserClaimDto(Guid id, Guid userId, string claimType, string claimValue)
        {
            Id = id;
            UserId = userId;
            ClaimType = claimType;
            ClaimValue = claimValue;
        }
    }
}
