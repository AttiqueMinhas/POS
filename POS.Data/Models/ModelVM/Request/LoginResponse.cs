namespace POS.Data.Models.ModelVM.Request
{
    public class LoginResponse
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public int TaskID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsCompleted { get; set;}
        public DateTime TargetDate { get; set; }
        public string? Photo { get; set; }
    }
}
