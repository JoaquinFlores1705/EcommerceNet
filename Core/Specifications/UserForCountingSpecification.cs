using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class UserForCountingSpecification : BaseSpecification<User>
    {
        public UserForCountingSpecification(UserSpecificationParams userParams)
            : base (x =>
                (string.IsNullOrEmpty(userParams.Search) || x.Name.Contains(userParams.Search)) &&
                (string.IsNullOrEmpty(userParams.Name) || x.Name.Contains(userParams.Name)) &&
                (string.IsNullOrEmpty(userParams.Lastname) || x.Lastname.Contains(userParams.Lastname))
            )
        {
            
        }
    }
}
