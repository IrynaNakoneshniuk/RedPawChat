using Microsoft.AspNetCore.Identity;

public class ExtendedIdentityRole : IdentityRole<string>
{
    public ExtendedIdentityRole() { }

    public string Description { get; set; }
}