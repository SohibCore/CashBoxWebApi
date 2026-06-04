    namespace CashBox.Service.Services.AccountServices
    {
        public class AccountService
        {
            public int UserId
            {
                get { return _userId; }
                set { _userId = value; }
            }
            public int OrganizationId
            {
                get { return _organizationId; }
                set { _organizationId = value; }
            }
            private int _userId;
            private int _organizationId;
        }

    }
