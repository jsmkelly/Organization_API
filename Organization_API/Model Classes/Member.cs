namespace Organization_API
{
    public class Member
    {
        private string _firstName = "";
        private string _lastName = "";
        private string _id = "";
        private DateTime _dateOfBirth;
        private Address _address = null!;
        private string _role = "";
        private List<Member> _subordinates = new List<Member>();

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - _dateOfBirth.Year;
                if (_dateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}
